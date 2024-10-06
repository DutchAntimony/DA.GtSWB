using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DA.PdfGeneration;

public class LabelDocument(LabelDocumentDataSource dataSource) : IDocument
{
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.MarginTop(dataSource.Margins[0], Unit.Millimetre);
                page.MarginRight(dataSource.Margins[1], Unit.Millimetre);
                page.MarginBottom(dataSource.Margins[2], Unit.Millimetre);
                page.MarginLeft(dataSource.Margins[3], Unit.Millimetre);

                page.Content().Element(ComposeContent);
            });
    }

    private void ComposeContent(IContainer container)
    {
        container
            .Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    for (var i = 0; i < dataSource.Columns; i++)
                        columns.RelativeColumn();
                });

                foreach (var label in dataSource.Labels)
                {
                    table.Cell().Element(Label).Text(label).FontSize(12);
                }
            });
    }

    private IContainer Label(IContainer container)
    {
        return container
            .Height(dataSource.Height, Unit.Millimetre)
            .Width(dataSource.Width, Unit.Millimetre)
            .AlignCenter()
            .AlignMiddle()
            .Padding(5, Unit.Millimetre);
    }
}