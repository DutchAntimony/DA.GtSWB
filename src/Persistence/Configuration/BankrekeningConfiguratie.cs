using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration;

public class BankrekeningConfiguratie : IEntityTypeConfiguration<Bankrekening>
{
    public void Configure(EntityTypeBuilder<Bankrekening> builder)
    {
        builder.ToTable("Bankrekeningen").HasKey(br => br.Id);

        builder.Property(br => br.Id)
            .HasConversion(new IdConverter<BankrekeningId>())
            .HasColumnOrder(0)
            .ValueGeneratedNever();

        builder.Property(br => br.Iban)
            .HasConversion(new IbanDataConverter())
            .HasColumnOrder(1);

        builder.Property(br => br.Bic).HasMaxLength(11).HasColumnOrder(2);

        builder.Property(br => br.TenNameVan).HasMaxLength(127).HasColumnOrder(3);
    }
}
