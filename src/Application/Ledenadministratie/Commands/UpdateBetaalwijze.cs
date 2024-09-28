using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Domain.Specifications;

namespace DA.GtSWB.Application.Ledenadministratie.Commands;

public static class UpdateBetaalwijze
{
    public record Command(LidId LidId, BetaalwijzeId BetaalwijzeId, RequestMetadata Metadata) : ICommand;

    internal sealed class Handler(IUnitOfWork unitOfWork, 
        ISpecification<Lid> alleLeden, 
        ISpecification<Betaalwijze> alleBetaalwijzes) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await
                unitOfWork.Leden.SingleOrFailureAsync(alleLeden.ById(request.LidId), cancellationToken)
                .CombineAsync(_ => unitOfWork.Betaalwijzes.SingleOrFailureAsync(alleBetaalwijzes.ById(request.BetaalwijzeId), cancellationToken))
                .Check(request.Metadata.Validate())
                .Tap((lid, betaalwijze) => lid.AssignBetaalwijze(betaalwijze));
        }
    }
}
