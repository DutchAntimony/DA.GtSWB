using System.Globalization;
using System.Xml;

namespace DA.Sepa.Extensions;

internal static class XmlWriterExtensions
{
    internal static XmlElement NewElement(this XmlElement parent, string name) => parent.NewElement(name, null);

    internal static XmlElement NewElement(this XmlElement parent, string name, object? value)
    {
        var e = parent.OwnerDocument.CreateElement(name);
        e.InnerText = $"{value ?? ""}";
        parent.AppendChild(e);
        return e;
    }

    internal static string FormatDateTime(this DateTime dateTime) => string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd\\THH:mm:ss}", dateTime);
    internal static string FormatDate(this DateOnly date) => string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-dd}", date);
    internal static string FormatAmount(this decimal amount) => string.Format(CultureInfo.InvariantCulture, "{0:0.00}", amount);
}