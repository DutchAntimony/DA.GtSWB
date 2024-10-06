using DA.GtSWB.Domain.Models.Contributie;

namespace DA.GtSWB.Domain.ServiceDefinitions;

public interface IContributieFileCreator
{
    Task CreateEtiketten(string path, IEnumerable<NotaOpdracht> opdrachten);
    Task CreateIncassoSepaFile(string path, IEnumerable<IncassoOpdracht> incassoOpdrachten);
    Task CreateNotas(string path, IEnumerable<NotaOpdracht> opdrachten);
}