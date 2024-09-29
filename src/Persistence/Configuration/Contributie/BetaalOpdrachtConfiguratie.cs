using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Contributie;

public class BetaalOpdrachtConfiguratie : IEntityTypeConfiguration<BetaalOpdracht>
{
    public void Configure(EntityTypeBuilder<BetaalOpdracht> builder)
    {
        builder.ToTable("BetaalOpdrachten").HasKey(bo => bo.Id);

        builder.Property(bo => bo.Id)
            .HasConversion(new IdConverter<BetaalOpdrachtId>())
            .ValueGeneratedNever()
            .HasColumnOrder(0);

        builder.Property(nameof(ContributieOpdrachtId))
            .HasColumnOrder(1);

        builder.HasDiscriminator<string>("Type")
            .HasValue<IncassoOpdracht>("Incasso")
            .HasValue<NotaOpdracht>("Nota");

        builder.Property("Type")
            .HasMaxLength(50)
            .HasColumnOrder(2);

        builder.Property(bo => bo.AanmaakDatum)
            .HasColumnOrder(3);

        builder.Property(bo => bo.Status)
            .HasColumnOrder(4);

        builder.Property(bo => bo.Iteratie)
            .HasColumnOrder(5);

        builder.Ignore(bo => bo.OpdrachtRegels);
    }
}
