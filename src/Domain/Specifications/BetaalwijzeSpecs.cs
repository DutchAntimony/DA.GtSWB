using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using System.Linq.Expressions;

namespace DA.GtSWB.Domain.Specifications;
public static class BetaalwijzeSpecs
{
    public static Expression<Func<Betaalwijze, bool>> ById(BetaalwijzeId id) => betaalwijze => betaalwijze.Id == id;

    public static ISpecification<Betaalwijze> ById(this ISpecification<Betaalwijze> spec, BetaalwijzeId id) =>
        spec.And(ById(id));
}
