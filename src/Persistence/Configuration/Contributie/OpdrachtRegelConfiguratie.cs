using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Domain.Models.Contributie.NotaOpdrachtRegels;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Contributie;

public class OpdrachtRegelConfiguratie : IEntityTypeConfiguration<OpdrachtRegel>
{
    public void Configure(EntityTypeBuilder<OpdrachtRegel> builder)
    {
        builder.ToTable("OpdrachtRegels").HasKey(or => or.Id);

        builder.Property(or => or.Id)
            .HasConversion(new IdConverter<OpdrachtRegelId>())
            .ValueGeneratedNever()
            .HasColumnOrder(0);

        builder.HasDiscriminator<string>("Type")
            .HasValue<KostenRegel>("Kosten")
            .HasValue<ContributieRegel>("Contributie")
            .HasValue<InformatieRegel>("Informatie");
        builder.Property("Type").HasMaxLength(50).HasColumnOrder(1);

        builder.HasOne(or => or.BetaalOpdracht)
            .WithMany(bo => bo.OpdrachtRegelCollectie)
            .HasForeignKey(nameof(BetaalOpdrachtId))
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(nameof(BetaalOpdrachtId))
            .HasColumnOrder(2);

        builder.Property(or => or.Omschrijving)
            .HasColumnOrder(3);

        builder.Property(or => or.Bedrag)
            .HasConversion(new MoneyConverter())
            .HasColumnOrder(4);
    }
}
