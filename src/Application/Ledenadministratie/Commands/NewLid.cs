using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Application.Ledenadministratie.Services;
using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Domain.Specifications;

namespace DA.GtSWB.Application.Ledenadministratie.Commands;
public static class NewLid
{
    public record Command(PersonaliaDto Personalia,
        AdresId AdresId,
        Option<BetaalwijzeId> BetaalwijzeOption,
        RequestMetadata MetaData) : ICreateEntityCommand<LidId>;

    internal class Handler(
        ILidCreationService lidCreationService,
        ISpecification<Lid> alleLeden,
        ISpecification<Betaalwijze> allBetaalwijzes,
        IUnitOfWork unitOfWork)
        : ICreateEntityCommandHandler<Command, LidId>
    {
        public async Task<Result<LidId>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await request.MetaData.Validate()
                .MapAsync(unitOfWork.Leden.QueryAsync(alleLeden.ByAdresId(request.AdresId), cancellationToken))
                .CheckIf(leden => leden.Count <= 0, new InternalServerError(new InvalidOperationException("Geen leden met meegegeven adresId")))
                .Map(leden => leden.First().AdresDataDatabase)
                .BindAsync(adres => CreateFromBetaalwijze(request.Personalia, adres!, request.BetaalwijzeOption, request.MetaData, cancellationToken))
                .Map(lid => lid.Id);
        }

        private async Task<Result<Lid>> CreateFromBetaalwijze(PersonaliaDto personalia, Adres adres, Option<BetaalwijzeId> betaalwijzeOption, RequestMetadata metadata, CancellationToken cancellationToken)
        {
            //todo: moet hier nog een check of de betaalwijze wel bij de leeftijd van het personalia past?
            return await betaalwijzeOption
                .MapAsync(id => unitOfWork.Betaalwijzes.SingleOrFailureAsync(allBetaalwijzes.ById(id))
                    .BindAsync(bw => lidCreationService.CreateBetalend(personalia, adres, bw, metadata, cancellationToken)))
                .ReduceAsync(lidCreationService.CreateGratis(personalia, adres, metadata, cancellationToken));
        }
    }
}

public static class SchrijfUit
{
    public record Command(LidId LidId, RequestMetadata Metadata) : ICommand;

    internal sealed class Handler(IUnitOfWork unitOfWork,
        ISpecification<Lid> alleLeden,
        ILidUitschrijfService lidUitschrijfService) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await
                unitOfWork.Leden.SingleOrFailureAsync(alleLeden.ById(request.LidId), cancellationToken)
                .Tap(lidUitschrijfService.SchrijfUit);
        }
    }
}