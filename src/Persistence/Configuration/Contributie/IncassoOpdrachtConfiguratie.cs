using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Contributie;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Contributie;

public class IncassoOpdrachtConfiguratie : IEntityTypeConfiguration<IncassoOpdracht>
{
    public void Configure(EntityTypeBuilder<IncassoOpdracht> builder)
    {
        builder.HasOne(io => io.Bankrekening)
            .WithMany()
            .HasForeignKey(nameof(BankrekeningId))
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(nameof(BankrekeningId))
            .HasColumnOrder(6);

        builder.Property(io => io.Omschrijving)
            .HasColumnOrder(7);

        builder.Property(io => io.IncassoAankondigingsDagen)
            .HasColumnName("UitvoerDelay")
            .HasColumnOrder(8);
    }
}
