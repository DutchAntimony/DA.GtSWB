using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Contributie;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Contributie;

public class NotaTekstConfiguratie : IEntityTypeConfiguration<NotaTekst>
{
    public void Configure(EntityTypeBuilder<NotaTekst> builder)
    {
        builder.ToTable("NotaTeksten").HasKey(or => or.Id);

        builder.Property(nt => nt.Id)
            .HasConversion(new IdConverter<NotaTekstId>())
            .ValueGeneratedNever()
            .HasColumnOrder(0);
    }
}