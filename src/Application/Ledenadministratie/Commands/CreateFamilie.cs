using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Ledenadministratie.Adressen;
using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Common.Data.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using MediatR;

namespace DA.GtSWB.Application.Ledenadministratie.Commands;
public static class CreateFamilie
{
    public sealed record Command(
        AdresDto Adres,
        IEnumerable<PersonaliaDto> Personen,
        RequestMetadata Metadata) : IRequest<Result<AdresId>> 
    { }

    internal sealed class Handler(IUnitOfWork unitOfWork, ILidnummerProvider lidnummerProvider)
        : IRequestHandler<Command, Result<AdresId>>
    {
        private DateTime _referenceDateTime;
        public async Task<Result<AdresId>> Handle(Command request, CancellationToken cancellationToken)
        {
            _referenceDateTime = request.Metadata.Timestamp;

            return await Result.IgnoreWarnings(false)
                .Bind(request.Metadata.Validate())
                .Bind(request.Adres.ToDomainModel())
                .CheckForEachAsync(request.Personen, (persoon, adres) => CreateAndInsertLid(persoon, adres, cancellationToken))
                .Map(adres => adres.Id);
        }

        private async Task<Result> CreateAndInsertLid(PersonaliaDto persoondto, Adres adres, CancellationToken cancellationToken)
        {
            return await Result.Create()
                .Bind(persoondto.ToDomainModel(_referenceDateTime))
                .MapAsync(persoon => Lid.Create(lidnummerProvider, persoon, adres, cancellationToken))
                .Tap(unitOfWork.Leden.Add);
        }
    }
}
