using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DA.PdfGeneration.Letters;

public class NotaDocument(NotaDocumentDataSource notaDataSource) : LetterComponent(notaDataSource)
{
    protected override internal void ComposeLetterBody(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(8);
            foreach (var text in notaDataSource.Inleiding)
            {
                column.Item().Text(text);
            }

            column.Item().Element(ComposeNotaTable);
            foreach (var text in notaDataSource.Afsluiting)
            {
                column.Item().Text(text);
            }
        });
    }

    private void ComposeNotaTable(IContainer container)
    {
        container
            .PaddingLeft(15)
            .PaddingTop(10)
            .PaddingBottom(10)
            .Table(table =>
        {
            table.ColumnsDefinition(column =>
            {
                column.RelativeColumn();
                column.ConstantColumn(50);
            });

            foreach (var entry in notaDataSource.NotaRegels)
            {
                table.Cell().Element(CellStyle).Text(entry.Omschrijving);
                table.Cell().Element(CellStyle).AlignRight().Text($"{entry.Bedrag:c}");
            }

            table.Cell().Text(notaDataSource.TotaalOmschrijving).Bold();
            table.Cell().AlignRight().Text($"{notaDataSource.Totaalbedrag:c}").Bold();
        });

        static IContainer CellStyle(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(2);
        }
    }
}