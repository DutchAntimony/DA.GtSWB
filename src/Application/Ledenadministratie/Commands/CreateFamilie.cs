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

//public static class CreateFamilieOld
//{
//    public sealed record Command(
//        AdresDto Adres,
//        IEnumerable<PersonaliaDto> Personen,
//        Option<BankrekeningDto> BetaalwijzeDto,
//        RequestMetadata Metadata) : IRequest<Result<AdresId>>
//    { }

//    internal sealed class Handler(IUnitOfWork unitOfWork, ILidnummerProvider lidnummerProvider, IBicProvider bicProvider)
//        : IRequestHandler<Command, Result<AdresId>>
//    {
//        private DateTime _referenceDateTime;
//        private Option<Betaalwijze> _convertedBetaalwijze = Option.None;

//        public async Task<Result<AdresId>> Handle(Command request, CancellationToken cancellationToken)
//        {
//            _referenceDateTime = request.Metadata.Timestamp;

//            return await Result.IgnoreWarnings(false)
//                .Bind(request.Metadata.Validate())
//                .Bind(request.Adres.ToDomainModel())
//                .CheckForEachAsync(request.Personen, (persoon, adres) => TryCreateAndInsertLid(persoon, adres, request.BetaalwijzeDto, cancellationToken))
//                .Map(adres => adres.Id);
//        }

//        private async Task<Result> TryCreateAndInsertLid(PersonaliaDto persoondto, Adres adres,
//            Option<BankrekeningDto> bankrekeningOption, CancellationToken cancellationToken)
//        {
//            var age = persoondto.Geboortedatum.GetAge(_referenceDateTime);

//            var lidResult = (age, bankrekeningOption.HasValue, _convertedBetaalwijze.HasValue) switch
//            {
//                ( >= 18, false, false) => await TryCreateNewNotaLid(persoondto, adres, cancellationToken),
//                ( >= 18, true, false) => await TryCreateNewIncassoLid(persoondto, adres, bankrekeningOption, cancellationToken),
//                ( >= 18, _, true) => await TryCreateNewBetalendLid(persoondto, adres, cancellationToken),
//                ( < 18, _, _) => await TryCreateGratisLid(persoondto, adres, cancellationToken)
//            };
//            return lidResult.Tap(unitOfWork.Leden.Add);
//        }

//        private async Task<Result<Lid>> TryCreateGratisLid(PersonaliaDto persoonDto, Adres adres,
//            CancellationToken cancellationToken = default)
//        {
//            return await persoonDto.ToDomainModel(_referenceDateTime)
//                .MapAsync(personalia => Lid.CreateLid(lidnummerProvider, personalia, adres, cancellationToken));
//        }

//        private async Task<Result<Lid>> TryCreateNewNotaLid(PersonaliaDto persoonDto, Adres adres,
//            CancellationToken cancellationToken = default)
//        {
//            return await persoonDto.ToDomainModel(_referenceDateTime)
//                .MapAsync(personalia => Lid.CreateBetalendLidMetNieuweNotaBetaalwijze(
//                    lidnummerProvider, personalia, adres, cancellationToken))
//                .Tap(lid => _convertedBetaalwijze = lid.Betaalwijze);
//        }

//        private async Task<Result<Lid>> TryCreateNewIncassoLid(PersonaliaDto persoonDto, Adres adres,
//            Option<BankrekeningDto> bankrekeningOption, CancellationToken cancellationToken = default)
//        {
//            return await persoonDto.ToDomainModel(_referenceDateTime)
//                .Combine(bankrekeningOption.Reduce(() => throw new Exception()).ToDomainModel(bicProvider))
//                .MapAsync((personalia, bankrekening) => Lid.CreateBetalendLidMetNieuweIncassoBetaalwijze(
//                    lidnummerProvider, personalia, adres, bankrekening, cancellationToken))
//                .Tap(lid => _convertedBetaalwijze = lid.Betaalwijze);
//        }

//        private async Task<Result<Lid>> TryCreateNewBetalendLid(PersonaliaDto persoonDto, Adres adres,
//            CancellationToken cancellationToken = default)
//        {
//            return await persoonDto.ToDomainModel(_referenceDateTime)
//                .MapAsync(personalia => Lid.CreateBetalendLid(lidnummerProvider, personalia, adres,
//                _convertedBetaalwijze.Reduce(() => throw new Exception()), cancellationToken));
//        }
//    }
//}