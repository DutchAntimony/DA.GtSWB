using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using System.Linq.Expressions;

namespace DA.GtSWB.Domain.Specifications;

public static class LedenSpecs
{
    public static Expression<Func<Lid, bool>> ByAdresId(AdresId id) => lid => lid.AdresDataDatabase != null && lid.AdresDataDatabase.Id == id;
    public static Expression<Func<Lid, bool>> ById(LidId id) => lid=> lid.Id == id;

    public static ISpecification<Lid> ByAdresId(this ISpecification<Lid> spec, AdresId id) =>
        spec.And(ByAdresId(id));

    public static ISpecification<Lid> ById(this ISpecification<Lid> spec, LidId id) =>
        spec.And(ById(id));
}