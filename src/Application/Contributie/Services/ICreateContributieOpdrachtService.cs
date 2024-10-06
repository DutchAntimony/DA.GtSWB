using DA.GtSWB.Domain.Models.Contributie;

namespace DA.GtSWB.Application.Contributie.Services;
internal interface ICreateContributieOpdrachtService
{
    Task<ContributieOpdracht> CreateOpdracht(DateTime timestamp, CancellationToken cancellationToken);
}