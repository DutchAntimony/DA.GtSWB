using DA.GtSWB.Common.Types;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DA.GtSWB.Persistence.Converters;

public class MoneyConverter : ValueConverter<Money, decimal>
{
    public MoneyConverter()
        : base(
            money => money.Amount,
            dec => new Money(dec))
    { }
}
