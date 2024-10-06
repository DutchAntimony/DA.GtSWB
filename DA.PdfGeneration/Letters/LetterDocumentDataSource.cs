namespace DA.PdfGeneration.Letters;

public class LetterDocumentDataSource<T> where T : LetterComponent
{

    public LetterDocumentDataSource(List<T> dataCollection)
    {
        DataCollection = dataCollection;
    }

    public List<T> DataCollection { get; }
    public string Koptekst { get; set; } = "";
}
