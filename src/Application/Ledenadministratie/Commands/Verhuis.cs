using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Application.Ledenadministratie.Adressen;
using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Domain.Specifications;

namespace DA.GtSWB.Application.Ledenadministratie.Commands;

public static class Verhuis
{
    public sealed record Command(
        AdresDto Adres,
        IEnumerable<LidId> Personen,
        RequestMetadata Metadata) : ICreateEntityCommand<AdresId>;

    internal sealed class Handler(IUnitOfWork unitOfWork, ISpecification<Lid> alleLeden) : ICreateEntityCommandHandler<Command, AdresId>
    {
        public async Task<Result<AdresId>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await request.Metadata.Validate()
                .Bind(request.Adres.ToDomainModel())
                .CheckForEachAsync(request.Personen, (lidId, adres) => 
                    VerhuisLid(lidId, adres, cancellationToken))
                .Map(adres => adres.Id);
        }

        private async Task<Result> VerhuisLid(LidId lidId, Adres adres, CancellationToken cancellationToken)
        {
            return await
                unitOfWork.Leden.SingleOrFailureAsync(alleLeden.ById(lidId), cancellationToken)
                .Tap(lid => lid.Verhuis(adres.AsOption()));
        }
    }
}