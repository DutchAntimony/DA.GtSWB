using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Ledenadministratie.Betaalwijzes;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration;

public class BetaalwijzeConfiguratie : IEntityTypeConfiguration<Betaalwijze>
{
    public void Configure(EntityTypeBuilder<Betaalwijze> builder)
    {
        builder.ToTable("Betaalwijzes").HasKey(bw => bw.Id);

        builder.Property(bw => bw.Id)
            .HasConversion(new IdConverter<BetaalwijzeId>())
            .HasColumnOrder(0)
            .ValueGeneratedNever();

        builder.HasDiscriminator<string>("Betaalmethode")
            .HasValue<NotaBetaalwijze>("Nota")
            .HasValue<IncassoBetaalwijze>("Incasso");

        builder
            .HasOne(p => p.VerantwoordelijkLid)      
            .WithMany()                            
            .HasForeignKey(p => p.VerantwoordelijkLidId)  
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property("Betaalmethode")
            .HasMaxLength(50)
            .HasColumnOrder(1);
    }
}
