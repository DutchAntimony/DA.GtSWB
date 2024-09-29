using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Contributie;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Contributie;

public class NotaOpdrachtConfiguratie : IEntityTypeConfiguration<NotaOpdracht>
{
    public void Configure(EntityTypeBuilder<NotaOpdracht> builder)
    {
        builder.HasOne(no => no.NotaTekst)
            .WithMany()
            .HasForeignKey(nameof(NotaTekstId))
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(nameof(NotaTekstId)).HasColumnOrder(9);
    }
}
