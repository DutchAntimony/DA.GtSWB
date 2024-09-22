using DA.GtSWB.Common.Data.IDs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DA.GtSWB.Persistence.Converters;

public class IdConverter<T> : ValueConverter<T, string> where T : IId
{
    public IdConverter()
        : base(
            id => id.Value.ToString(),
            str => (T)Activator.CreateInstance(typeof(T), Ulid.Parse(str))!)
    { }
}