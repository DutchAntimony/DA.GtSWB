using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;

namespace DA.GtSWB.Domain.Models.Ledenadministratie;

public abstract class Betaalwijze
{
    public required BetaalwijzeId Id { get; init; }

    private ICollection<Lid> LidCollection { get; } = [];

    public IEnumerable<Lid> Leden => LidCollection;

    public LidId? VerantwoordelijkLidId { get; set; }
    public Lid? VerantwoordelijkLid { get; set; }
}

public static class BetaalwijzeExtensions
{
    public static T Match<T>(this Betaalwijze betaalwijze,
        Func<NotaBetaalwijze, T> matchNota,
        Func<IncassoBetaalwijze, T> matchIncasso)
    {
        return betaalwijze switch
        {
            NotaBetaalwijze nota => matchNota(nota),
            IncassoBetaalwijze inc => matchIncasso(inc),
            _ => throw new NotImplementedException()
        };
    }

    public static Task<T> MatchAsync<T>(this Betaalwijze betaalwijze,
    Func<NotaBetaalwijze, Task<T>> matchNota,
    Func<IncassoBetaalwijze, Task<T>> matchIncasso)
    {
        return betaalwijze switch
        {
            NotaBetaalwijze nota => matchNota(nota),
            IncassoBetaalwijze inc => matchIncasso(inc),
            _ => throw new NotImplementedException()
        };
    }
}