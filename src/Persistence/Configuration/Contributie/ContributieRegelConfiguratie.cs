using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Contributie.NotaOpdrachtRegels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Contributie;

public class ContributieRegelConfiguratie : IEntityTypeConfiguration<ContributieRegel>
{
    public void Configure(EntityTypeBuilder<ContributieRegel> builder)
    {
        builder.HasOne(cr => cr.Lid)
            .WithMany()
            .HasForeignKey(nameof(LidId))
            .OnDelete(DeleteBehavior.Cascade);
    }
}