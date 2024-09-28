using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration;

public class AdresConfiguratie : IEntityTypeConfiguration<Adres>
{
    public void Configure(EntityTypeBuilder<Adres> builder)
    {
        builder.ToTable("Adressen").HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(new IdConverter<AdresId>())
            .ValueGeneratedNever();

        builder.Property(a => a.Straat).HasMaxLength(100);
        builder.Property(a => a.Huisnummer).HasMaxLength(10);
        builder.Property(a => a.Postcode).HasMaxLength(10);
        builder.Property(a => a.Woonplaats).HasMaxLength(100);

        builder.Property<string?>("_land")
            .HasColumnName("Land")
            .HasMaxLength(100);

        builder.Ignore(a => a.Land);
    }
}
