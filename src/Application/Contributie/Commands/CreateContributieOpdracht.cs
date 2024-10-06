using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Application.Contributie.Services;
using DA.GtSWB.Common.Types.IDs;

namespace DA.GtSWB.Application.Contributie.Commands;
public static class CreateContributieOpdracht
{
    public record Command(RequestMetadata Metadata) : ICreateEntityCommand<ContributieOpdrachtId>;

    internal class Handler(ICreateContributieOpdrachtService createContributieOpdrachtService)
        : ICreateEntityCommandHandler<Command, ContributieOpdrachtId>
    {
        public async Task<Result<ContributieOpdrachtId>> Handle(Command request, CancellationToken cancellationToken)
        {
            var opdracht = await createContributieOpdrachtService.CreateOpdracht(request.Metadata.Timestamp, cancellationToken);
            return opdracht.Id;
        }
    }
}
