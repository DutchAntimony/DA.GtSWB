namespace DA.PdfGeneration.Letters;

public class NotaDocumentDataSource(IEnumerable<NotaDocumentEntry> notaRegels) : LetterComponentDataSource()
{
    public string[] Inleiding { get; set; } = [];
    public string[] Afsluiting { get; set; } = [];

    public IEnumerable<NotaDocumentEntry> NotaRegels { get; } = notaRegels;
    public string TotaalOmschrijving { get; set; } = "Totaalbedrag van deze nota:";
    internal decimal Totaalbedrag => NotaRegels.Sum(n => n.Bedrag);
}

public record NotaDocumentEntry(string Omschrijving, decimal Bedrag);
