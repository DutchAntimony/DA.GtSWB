using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Ledenadministratie;

public class LidConfiguratie : IEntityTypeConfiguration<Lid>
{
    public void Configure(EntityTypeBuilder<Lid> builder)
    {
        builder.ToTable("Leden").HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(new IdConverter<LidId>())
            .ValueGeneratedNever()
            .HasColumnOrder(0);

        builder.Property(l => l.Lidnummer).HasColumnOrder(1);
        builder.HasIndex(l => l.Lidnummer).IsUnique();

        builder.HasOne(l => l.Personalia)
            .WithOne()
            .HasForeignKey<Lid>(nameof(PersonaliaId))
            .OnDelete(DeleteBehavior.Restrict);
        builder.Property(nameof(PersonaliaId)).HasColumnOrder(2);

        builder.HasOne(l => l.AdresDataDatabase)
            .WithMany()
            .HasForeignKey(nameof(AdresId))
            .OnDelete(DeleteBehavior.SetNull);
        builder.Property(nameof(AdresId)).HasColumnOrder(3);

        builder.HasOne(l => l.BetaalwijzeDataDatabase)
            .WithMany(bw => bw.Leden)
            .HasForeignKey(nameof(BetaalwijzeId))
            .OnDelete(DeleteBehavior.SetNull);
        builder.Property(nameof(BetaalwijzeId)).HasColumnOrder(4);

        builder.Property(l => l.IsUitgeschreven).HasColumnOrder(5);
    }
}
