using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration;

public class NotaBetaalwijzeConfiguratie : IEntityTypeConfiguration<NotaBetaalwijze>
{
    public void Configure(EntityTypeBuilder<NotaBetaalwijze> builder)
    {
        builder.Property(nbw => nbw.PreferEmail)
            .HasDefaultValue(false)
            .HasColumnOrder(4);
    }
}
