using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Application.Ledenadministratie.Services;
using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Domain.Specifications;

namespace DA.GtSWB.Application.Ledenadministratie.Commands;

public static class CreateNewBetaalwijze
{
    public record Command(LidId LidId, Option<BankrekeningDto> BankrekeningDto, RequestMetadata Metadata) : ICommand;

    internal sealed class Handler(IUnitOfWork unitOfWork,
        ISpecification<Lid> alleLeden,
        IBetaalwijzeCreationService betaalwijzeCreationService) : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await
                unitOfWork.Leden.SingleOrFailureAsync(alleLeden.ById(request.LidId), cancellationToken)
                .Combine(_ => betaalwijzeCreationService.CreateBetaalwijze(request.BankrekeningDto))
                .Check(request.Metadata.Validate())
                .Tap((lid, betaalwijze) => lid.AssignBetaalwijze(betaalwijze, request.Metadata.Timestamp, request.Metadata.Gebruiker));
        }
    }
}