using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Types;
using DA.GtSWB.Domain.Extensions;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using DA.GtSWB.Domain.Models.Ledenadministratie.Mutaties;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Domain.Specifications;

namespace DA.GtSWB.Application.Ledenadministratie.Services;

internal interface ILidCreationService
{
    Task<Result<Lid>> CreateBetalend(PersonaliaDto dto, Adres adres, Betaalwijze betaalwijze, RequestMetadata metadata, CancellationToken cancellationToken = default);
    Task<Result<Lid>> CreateGratis(PersonaliaDto dto, Adres adres, RequestMetadata metadata, CancellationToken cancellationToken = default);
}

internal class LidCreationService(
    IUnitOfWork unitOfWork,
    ILidnummerProvider lidnummerProvider,
    ISpecification<Betaalwijze> allBetaalwijzes) : ILidCreationService
{
    public async Task<Result<Lid>> CreateGratis(PersonaliaDto dto, Adres adres, RequestMetadata metadata,
        CancellationToken cancellationToken = default)
    {
        return await dto.ToDomainModel(metadata.Timestamp)
            .MapAsync(personalia => Lid.Create(lidnummerProvider, personalia, adres, cancellationToken))
            .Tap(lid => lid.Mutaties.Add(NieuwLidMutatie.Create(lid, metadata.Timestamp, metadata.Gebruiker)))
            .Tap(unitOfWork.Leden.Add);
    }

    public async Task<Result<Lid>> CreateBetalend(PersonaliaDto dto, Adres adres, Betaalwijze betaalwijze,
        RequestMetadata metadata, CancellationToken cancellationToken = default)
    {
        return await unitOfWork.Betaalwijzes.SingleOrFailureAsync(
                allBetaalwijzes.ById(betaalwijze.Id), cancellationToken)
            .BindAsync(_ => CreateBestaandeBetaalwijze(dto, adres, betaalwijze, metadata, cancellationToken))
            .CompensateAsync(() => CreateNieuweBetaalwijze(dto, adres, betaalwijze, metadata, cancellationToken));
    }

    private async Task<Result<Lid>> CreateBestaandeBetaalwijze(PersonaliaDto dto, Adres adres, Betaalwijze betaalwijze,
        RequestMetadata metadata, CancellationToken cancellationToken = default)
    {
        return await CreateGratis(dto, adres, metadata, cancellationToken)
            .Tap(lid => lid.AssignBetaalwijze(betaalwijze, metadata.Timestamp, metadata.Gebruiker, false));
    }

    private async Task<Result<Lid>> CreateNieuweBetaalwijze(PersonaliaDto dto, Adres adres, Betaalwijze betaalwijze,
        RequestMetadata metadata, CancellationToken cancellationToken = default)
    {
        return await CreateGratis(dto, adres, metadata, cancellationToken)
            .TapAsync(unitOfWork.CommitAsync(cancellationToken))
            .Tap(lid => lid.AssignBetaalwijze(betaalwijze, metadata.Timestamp, metadata.Gebruiker, true));
    }
}

internal interface IBetaalwijzeCreationService
{
    Result<Betaalwijze> CreateBetaalwijze(Option<BankrekeningDto> BetaalwijzeDto);
}

internal class BetaalwijzeCreationService(IBicProvider bicProvider) : IBetaalwijzeCreationService
{
    public Result<Betaalwijze> CreateBetaalwijze(Option<BankrekeningDto> BetaalwijzeDto)
    {
        return BetaalwijzeDto
            .Map(dto => dto.ToDomainModel(bicProvider))
            .Map(rekResult => rekResult
                .Map(rek => IncassoBetaalwijze.Create(rek)))
                .Map(ibw => ibw.Cast<IncassoBetaalwijze, Betaalwijze>())
            .Reduce(NotaBetaalwijze.Create());
    }
}

internal interface ILidUitschrijfService
{
    void SchrijfUit(Lid lid);
}