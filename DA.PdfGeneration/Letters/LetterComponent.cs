using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace DA.PdfGeneration.Letters;

public abstract class LetterComponent(LetterComponentDataSource dataSource) : IComponent
{
    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            column.Item().PaddingTop(50).Text($"{dataSource.Geadresseerde}\n{dataSource.Adres}");
            column.Item().PaddingTop(20).Text(x =>
            {
                x.Span(dataSource.Datum);
                x.AlignRight();
            });
            column.Item().Text(dataSource.Aanhef);
            column.Item().Element(ComposeLetterBody);
            column.Item().Text(dataSource.Groet);
            column.Item().Text(dataSource.Afzender.ReplaceLineEndings());
        });
    }

    protected internal abstract void ComposeLetterBody(IContainer container);
}