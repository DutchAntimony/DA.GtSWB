using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Configuration;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Settings;
internal class ConfiguratieItemConfiguratie : IEntityTypeConfiguration<ConfiguratieItem>
{
    public void Configure(EntityTypeBuilder<ConfiguratieItem> builder)
    {
        builder.ToTable("ConfiguratieItems").HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .HasConversion(new IdConverter<ConfiguratieItemId>())
            .ValueGeneratedNever();
    }
}
