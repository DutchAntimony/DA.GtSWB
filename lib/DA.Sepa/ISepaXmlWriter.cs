using DA.Sepa.DataTypes;

namespace DA.Sepa;

public interface ISepaXmlWriter
{
    Task WriteAsync(string file, SepaPayment payment);
}