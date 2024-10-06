using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DA.PdfGeneration.Letters;

public class LetterDocument<T>(LetterDocumentDataSource<T> dataSource) : IDocument
    where T : LetterComponent
{
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);

            page.Header()
                .AlignCenter()
                .Text(dataSource.Koptekst)
                .SemiBold().FontSize(18);

            page.Content().DefaultTextStyle(x => x.FontSize(11)).Element(ComposeContent);

        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Column(column =>
        {
            foreach (var item in dataSource.DataCollection)
            {
                column.Item().Component(item);
                if (item != dataSource.DataCollection.Last())
                {
                    column.Item().PageBreak();
                }
            }
        });
    }
}