using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Extensions;
using DA.GtSWB.Common.Formatters;
using DA.GtSWB.Common.Types.Extensions;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Settings;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Domain.Specifications;
using DA.PdfGeneration;
using DA.PdfGeneration.Letters;
using DA.Sepa;
using DA.Sepa.DataTypes;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;

namespace DA.GtSWB.Infrastructure.Services;

public class ContributieFileCreator(
    ISepaXmlWriter xmlWriter,
    IUnitOfWork unitOfWork,
    ISpecification<Lid> alleLeden,
    NaamFormatter naamFormatter,
    AdresFormatter adresFormatter,
    Configuraties configs,
    ILogger<ContributieFileCreator> logger) : IContributieFileCreator
{
    public async Task CreateIncassoSepaFile(string path, IEnumerable<IncassoOpdracht> incassoOpdrachten)
    {
        logger.LogInformation("Start creating incasso file");
        var firstOpdracht = incassoOpdrachten.First();
        var aanmaakDatum = firstOpdracht.AanmaakDatum;
        var messageId = aanmaakDatum.Year - 2000 + 22;
        var uitvoerDatum = DateOnly.FromDateTime(aanmaakDatum.AddDays(firstOpdracht.IncassoAankondigingsDagen));

        var payment = new SepaPayment(messageId, 
            CreditorName: configs.VerenigingTenNameVan,
            CreditorIban: configs.VerenigingIban.ToFlatString(),
            CreditorBic: configs.VerenigingBic,
            PayeeId: configs.VerenigingIncassantId,
            aanmaakDatum, uitvoerDatum);

        incassoOpdrachten.ForEach(io => payment.AddDirectDebitTransaction(
            CreateTransaction(io, aanmaakDatum)));

        var file = Path.Join(path, $"Inc{messageId}_{aanmaakDatum.ToFileNameFormat()}.xml");
        await xmlWriter.WriteAsync(file, payment);
        logger.LogInformation("Created incasso file: {file}", file);
    }
    
    private DirectDebitTransaction CreateTransaction(IncassoOpdracht opdracht, DateTime aanmaakDatum)
    {
        var transactieId = opdracht.Ident;
        logger.LogInformation("Creating direct debit transaction for lid: {lidnummer}", transactieId);
        return new DirectDebitTransaction(
            Id: transactieId,
            DebtorIban: opdracht.Bankrekening.Iban.ToFlatString(),
            DebtorName: opdracht.Bankrekening.TenNameVan,
            Amount: opdracht.Bedrag.Amount,
            Currency: "EUR",
            RemittanceInformation: opdracht.Omschrijving,
            DateOfSignature: opdracht.Bankrekening.MandaadDatum ?? new DateOnly(1999, 5, 1),
            MandateIdentification: $"2121-{opdracht.Bankrekening.MandaadType.GetIdentifier()}-{transactieId}",
            EndToEndId: $"{aanmaakDatum:yyyyMMdd}-{transactieId}");
    }

    public async Task CreateEtiketten(string path, IEnumerable<NotaOpdracht> opdrachten)
    {
        var labels = await Task.WhenAll(opdrachten.Select(GenerateLabel));
        var dataSource = new LabelDocumentDataSource(labels, 8, 3, 3, 4);
        var document = new LabelDocument(dataSource);

        string filename = Path.Join(path, $"Etiketten notas {DateTime.Now.ToFileNameFormat()}.pdf");
        await Task.Run(() => document.GeneratePdf(filename));
    }

    private async Task<string> GenerateLabel(NotaOpdracht opdracht)
    {
        var verantwoordelijkLidResult = await unitOfWork.Leden.SingleOrFailureAsync(alleLeden.ByLidnummer(opdracht.Ident));
        if (!verantwoordelijkLidResult.TryGetValue(out var verantwoordelijkLid))
        {
            throw new InvalidDataException();
        }
        var adres = verantwoordelijkLid.AdresDataDatabase.EnsureNotNull();
        return naamFormatter(verantwoordelijkLid!.Personalia) + "\n" + adresFormatter(adres);
    }

    public async Task CreateNotas(string path, IEnumerable<NotaOpdracht> opdrachten)
    {
        var notaTekst = opdrachten.First().NotaTekst;
        var notaDocumenten = await Task.WhenAll(opdrachten.Select(ToNotaDocument));

        var collectionDataSource = new LetterDocumentDataSource<NotaDocument>([.. notaDocumenten])
        {
            Koptekst = notaTekst.Titel
        };

        var document = new LetterDocument<NotaDocument>(collectionDataSource);
        var filename = Path.Join(path, $"Notas {DateTime.Now.ToFileNameFormat()}.pdf");
        await Task.Run(() => document.GeneratePdf(filename));

    }

    private async Task<NotaDocument> ToNotaDocument(NotaOpdracht opdracht)
    {
        var verantwoordelijkLidResult = await unitOfWork.Leden.SingleOrFailureAsync(alleLeden.ByLidnummer(opdracht.Ident));
        if (!verantwoordelijkLidResult.TryGetValue(out var verantwoordelijkLid))
        {
            throw new InvalidDataException();
        }
        var adres = verantwoordelijkLid.AdresDataDatabase.EnsureNotNull();

        var notaRegels = opdracht.OpdrachtRegels.Select(regel =>
            new NotaDocumentEntry(regel.Omschrijving, regel.Bedrag.Amount));
        var notaDataSource = new NotaDocumentDataSource(notaRegels.OrderBy(nr => nr.Omschrijving))
        {
            Geadresseerde = naamFormatter(verantwoordelijkLid!.Personalia),
            Adres = adresFormatter(adres),
            Datum = opdracht.NotaTekst.DatumPlaats,
            Aanhef = $"{opdracht.NotaTekst.Aanhef}{naamFormatter(verantwoordelijkLid!.Personalia)}",
            Inleiding = opdracht.NotaTekst.Inleiding.Split(@"\n"),
            TotaalOmschrijving = "Totaal van deze nota:",
            Afsluiting = opdracht.NotaTekst.Afsluiting.Split(@"\n"),
            Groet = "Met vriendelijke groet,",
            Afzender = opdracht.NotaTekst.Afzender
        };

        return new NotaDocument(notaDataSource);
    }
}