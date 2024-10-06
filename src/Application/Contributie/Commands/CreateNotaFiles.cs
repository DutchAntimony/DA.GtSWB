using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Domain.ServiceDefinitions;

namespace DA.GtSWB.Application.Contributie.Commands;

public static class CreateNotaFiles
{
    public record Command(string Path, RequestMetadata Metadata) : ICreateFileCommand;

    internal class Handler(IContributieFileCreator generator, IUnitOfWork unitOfWork) : ICreateFileCommandHandler<Command>
    {
        public async Task<Result<DirectoryInfo>> Handle(Command request, CancellationToken cancellationToken)
        {
            var opdrachten = unitOfWork.NotaOpdrachten.All;
            var notaTask = generator.CreateNotas(request.Path, opdrachten);
            var etikettenTask = generator.CreateEtiketten(request.Path, opdrachten);
            await Task.WhenAll(notaTask, etikettenTask);
            return new DirectoryInfo(request.Path);
        }
    }
}