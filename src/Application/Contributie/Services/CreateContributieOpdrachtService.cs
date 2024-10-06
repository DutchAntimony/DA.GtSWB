using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Formatters;
using DA.GtSWB.Common.Types;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Domain.Models.Contributie.NotaOpdrachtRegels;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using DA.GtSWB.Domain.Models.Settings;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Domain.Specifications;
using System.Diagnostics;

namespace DA.GtSWB.Application.Contributie.Services;

internal class CreateContributieOpdrachtService(
    IUnitOfWork unitOfWork,
    NaamFormatter naamFormatter,
    ISpecification<Lid> alleLeden,
    ISpecification<IncassoBetaalwijze> alleIncassoBetaalwijzes,
    ISpecification<NotaBetaalwijze> alleNotaBetaalwijzes,
    INotaTekstProvider notaTekstProvider,
    Configuraties configuraties) : ICreateContributieOpdrachtService
{
    private readonly LidFormatter _lidFormatter =
        (lid) => $"{lid.Lidnummer} - {naamFormatter(lid.Personalia)} - ({lid.Personalia.Geboortedatum})";

    public async Task<ContributieOpdracht> CreateOpdracht(DateTime timestamp, CancellationToken cancellationToken)
    {
        var incassoBetaalwijzes = await unitOfWork.IncassoBetaalwijzes.QueryAsync(alleIncassoBetaalwijzes, cancellationToken);
        var incassoOpdrachten = incassoBetaalwijzes.Select(ib => CreateIncassoOpdracht(ib, timestamp));

        var notaBetaalwijzes = await unitOfWork.NotaBetaalwijzes.QueryAsync(alleNotaBetaalwijzes, cancellationToken);
        var tekst = notaTekstProvider.GetVoorContributieNota(DateOnly.FromDateTime(timestamp));
        //unitOfWork.NotaTeksten.Add(tekst);
        var notaOpdrachten = await Task.WhenAll(notaBetaalwijzes.Select(async nb => await CreateFirstIterationNotaOpdracht(nb, tekst, timestamp)));
        var result = ContributieOpdracht.Create(timestamp.Year, incassoOpdrachten.Concat(notaOpdrachten)); 
        unitOfWork.ContributieOpdrachten.Add(result);
        return result;
    }

    private async Task<BetaalOpdracht> CreateFirstIterationNotaOpdracht(NotaBetaalwijze betaalwijze, NotaTekst notaTekst, DateTime timestamp)
    {
        if (betaalwijze.VerantwoordelijkLid?.IsUitgeschreven ?? true || betaalwijze.VerstuurAdres is null)
        {
            throw new InvalidOperationException("Kan geen betaalwijze aanmaken voor betaalwijze zonder verantwoordelijk lid of adres.");
        }

        var regels = CreateContributieRegels(betaalwijze.Leden, configuraties.Contributie).ToList();
        var infoRegelLeden = await unitOfWork.Leden.QueryAsync(alleLeden.ByAdresId(betaalwijze.VerstuurAdres!.Id)
            .WithDifferentBetaalwijze(betaalwijze.VerantwoordelijkLid));
        regels.AddRange(CreateInformatieRegels(infoRegelLeden));
        regels.Add(KostenRegel.Create("Administratiekosten", configuraties.Administratiekosten, 1));
        return NotaOpdracht.Create(betaalwijze.VerantwoordelijkLid, regels, notaTekst, timestamp);
    }

    private IncassoOpdracht CreateIncassoOpdracht(IncassoBetaalwijze betaalwijze, DateTime timestamp)
    {
        if (betaalwijze.VerantwoordelijkLid?.IsUitgeschreven ?? true)
        {
            Debugger.Break();
            throw new InvalidOperationException("Kan geen betaalwijze aanmaken voor betaalwijze zonder verantwoordelijk lid.");
        }

        var regels = CreateContributieRegels(betaalwijze.Leden, configuraties.Contributie);
        var aantalLedenTekst = regels.Count == 1 ? "1 lid" : $"{regels.Count} leden";
        var omschrijving = $"Contributie {timestamp.Year} - {aantalLedenTekst} - Lidnr. {betaalwijze.VerantwoordelijkLid.Lidnummer}";
        return IncassoOpdracht.Create(betaalwijze, omschrijving, regels, timestamp);
    }

    private List<OpdrachtRegel> CreateContributieRegels(IEnumerable<Lid> leden, Money contributie)
    {
        return leden.Select(lid => ContributieRegel.Create(lid, _lidFormatter, contributie) as OpdrachtRegel).ToList();
    }

    private List<OpdrachtRegel> CreateInformatieRegels(IEnumerable<Lid> leden)
    {
        return leden.Select(lid => InformatieRegel.Create($"{_lidFormatter(lid)} " +
            $"{BetaalwijzeNaarOpmerking(lid.Betaalwijze)}") as OpdrachtRegel).ToList();

        static string BetaalwijzeNaarOpmerking(Option<Betaalwijze> betaalwijze)
        {
            return betaalwijze.Map(bw => bw.Match(
                _ => "Betaald per eigen nota",
                _ => "Betaald per incasso"))
            .Reduce("Onder 18 is gratis");
        }
    }
}
