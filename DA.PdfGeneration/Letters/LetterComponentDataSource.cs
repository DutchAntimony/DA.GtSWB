namespace DA.PdfGeneration.Letters;

public abstract class LetterComponentDataSource
{
    public string Geadresseerde { get; set; } = "";
    public string Adres { get; set; } = "";
    public string Datum { get; set; } = "";
    public string Aanhef { get; set; } = "";
    public string Groet { get; set; } = "";
    public string Afzender { get; set; } = "";
}