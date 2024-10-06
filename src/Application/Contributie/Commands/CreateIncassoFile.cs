using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Domain.ServiceDefinitions;

namespace DA.GtSWB.Application.Contributie.Commands;

public static class CreateIncassoFile
{
    public record Command(string Path, RequestMetadata Metadata) : ICreateFileCommand;

    internal class Handler(IContributieFileCreator generator, IUnitOfWork unitOfWork) : ICreateFileCommandHandler<Command>
    {
        public async Task<Result<DirectoryInfo>> Handle(Command request, CancellationToken cancellationToken)
        {
            var opdrachten = unitOfWork.IncassoOpdrachten.All;
            await generator.CreateIncassoSepaFile(request.Path, opdrachten);
            return new DirectoryInfo(request.Path);
        }
    }
}
