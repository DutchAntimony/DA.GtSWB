using DA.GtSWB.Common.Data.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration;

public class PersonaliaConfiguratie : IEntityTypeConfiguration<Personalia>
{
    public void Configure(EntityTypeBuilder<Personalia> builder)
    {
        builder.ToTable("Personen").HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(new IdConverter<PersonaliaId>())
            .HasColumnOrder(0)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(p => p.Voorletters).HasMaxLength(15).HasColumnOrder(1).IsRequired();

        builder.Property<string?>("_tussenvoegsel")
            .HasColumnName("Tussenvoegsel")
            .HasColumnOrder(2)
            .HasMaxLength(15);
        builder.Ignore(p => p.Tussenvoegsel);

        builder.Property(p => p.Achternaam).HasMaxLength(127).HasColumnOrder(3).IsRequired();

        builder.Property(p => p.Geslacht).HasConversion<int>().HasColumnOrder(4).IsRequired();

        builder.Property(p => p.Geboortedatum).HasColumnType("date").HasColumnOrder(5).IsRequired();
    }
}

