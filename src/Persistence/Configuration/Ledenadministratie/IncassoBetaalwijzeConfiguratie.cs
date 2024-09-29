using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Ledenadministratie;

public class IncassoBetaalwijzeConfiguratie : IEntityTypeConfiguration<IncassoBetaalwijze>
{
    public void Configure(EntityTypeBuilder<IncassoBetaalwijze> builder)
    {
        builder.HasOne(ibw => ibw.Bankrekening)
            .WithMany()
            .HasForeignKey(nameof(BankrekeningId))
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(nameof(BankrekeningId))
            .HasColumnOrder(3);
    }
}
