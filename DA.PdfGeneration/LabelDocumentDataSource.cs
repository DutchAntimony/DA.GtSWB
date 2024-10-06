namespace DA.PdfGeneration;

public class LabelDocumentDataSource
{
    private const int PAGE_HEIGHT = 297;
    private const int PAGE_WIDTH = 210;

    public LabelDocumentDataSource(IEnumerable<string> labels, int rows, int columns, int margin) :
        this(labels, rows, columns, margin, margin, margin, margin)
    { }

    public LabelDocumentDataSource(IEnumerable<string> labels, int rows, int columns, int marginNS, int marginEW) :
        this(labels, rows, columns, marginNS, marginEW, marginNS, marginEW)
    { }

    public LabelDocumentDataSource(IEnumerable<string> labels, int rows, int columns, int marginN, int marginE, int marginS, int marginW)
    {
        Labels = labels;
        Rows = rows;
        Columns = columns;
        Margins = new int[] { marginN, marginE, marginS, marginW };
    }


    public IEnumerable<string> Labels { get; set; }

    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public int[] Margins { get; private set; } // NESW

    internal int Height => (PAGE_HEIGHT - Margins[0] - Margins[2]) / Rows;
    internal int Width => (PAGE_WIDTH - Margins[1] - Margins[3]) / Columns;
}
