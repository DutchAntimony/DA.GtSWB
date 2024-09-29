using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Contributie;

public class ContributieOpdrachtConfiguratie : IEntityTypeConfiguration<ContributieOpdracht>
{
    public void Configure(EntityTypeBuilder<ContributieOpdracht> builder)
    {
        builder.ToTable("ContributieOpdrachten").HasKey(co => co.Id);

        builder.Property(co => co.Id)
            .HasConversion(new IdConverter<ContributieOpdrachtId>())
            .ValueGeneratedNever()
            .HasColumnOrder(0);

        builder.Property(co => co.Jaargang).HasColumnOrder(1);
        builder.HasIndex(co => co.Jaargang).IsUnique();

        builder.HasMany(co => co.BetaalOpdrachtCollectie)
            .WithOne()
            .HasForeignKey(bo => bo.ContributieOpdrachtId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(co => co.BetaalOpdrachten);
    }
}
