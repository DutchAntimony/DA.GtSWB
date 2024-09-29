using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Application.Ledenadministratie.Adressen;
using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Application.Ledenadministratie.Services;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Application.Ledenadministratie.Commands;
public static class CreateFamilie
{
    public sealed record Command(
        AdresDto Adres,
        IEnumerable<PersonaliaDto> Personen,
        Option<BankrekeningDto> BetaalwijzeDto,
        RequestMetadata Metadata) : ICreateEntityCommand<AdresId>;

    internal sealed class Handler(ILidCreationService lidCreationService, IBetaalwijzeCreationService betaalwijzeCreationService)
        : ICreateEntityCommandHandler<Command, AdresId>
    {
        public async Task<Result<AdresId>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await Result.IgnoreWarnings(false)
                .Bind(request.Metadata.Validate())
                .Bind(request.Adres.ToDomainModel())
                .Combine(betaalwijzeCreationService.CreateBetaalwijze(request.BetaalwijzeDto))
                .CheckForEachAsync(request.Personen, (lid, tuple) => TryCreateLid(lid, tuple.Item1, tuple.Item2, request, cancellationToken))
                .Map((adres, _) => adres.Id);
        }

        private async Task<Result> TryCreateLid(PersonaliaDto personalia, Adres adres, Betaalwijze betaalwijze, Command request, CancellationToken cancellationToken)
        {
            return await Result.Create()
                .Map(personalia.Geboortedatum.GetAge(request.Metadata.Timestamp))
                .CheckIfAsync(leeftijd => leeftijd < 18, _ => lidCreationService.CreateGratis(personalia, adres, request.Metadata, cancellationToken).Flatten())
                .CheckIfAsync(leeftijd => leeftijd >= 18, _ => lidCreationService.CreateBetalend(personalia, adres, betaalwijze, request.Metadata, cancellationToken).Flatten());
        }
    }
}