using DA.GtSWB.Common.Types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DA.GtSWB.Persistence.Converters;

public class IbanDataConverter : ValueConverter<Iban, string>
{
    public IbanDataConverter()
        : base(
            iban => iban.ToString(),
            str => new Iban(str))
    { }
}
