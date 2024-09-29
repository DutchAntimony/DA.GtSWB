using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.Models.Ledenadministratie.Mutaties;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Ledenadministratie;
public class LidMutatieConfiguratie : IEntityTypeConfiguration<LidMutatie>
{
    public void Configure(EntityTypeBuilder<LidMutatie> builder)
    {
        builder.ToTable("LidMutaties").HasKey(lm => lm.Id);

        builder.Property(lm => lm.Id)
            .HasConversion(new IdConverter<LidMutatieId>())
            .HasColumnOrder(0)
            .ValueGeneratedNever();

        builder.HasDiscriminator<string>("Mutatie_type")
            .HasValue<NaamWijzigingMutatie>("Naam wijziging")
            //.HasValue<UitschrijfMutatie>("Uitschrijving")
            .HasValue<VerhuisMutatie>("Verhuizing")
            .HasValue<NieuwLidMutatie>("Nieuw")
            .HasValue<BetaalwijzeMutatie>("Betaalwijze wijziging");

        builder.Property("Mutatie_type")
            .HasMaxLength(50)
            .HasColumnOrder(1);

        builder.HasOne(lm => lm.Lid)
            .WithMany(l => l.Mutaties)
            .HasForeignKey("LidId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property("LidId")
            .HasColumnOrder(2);

        builder.Property(lm => lm.Gebruiker)
            .HasMaxLength(50);
    }
}

public class BetaalwijzeMutatieConfiguratie : IEntityTypeConfiguration<BetaalwijzeMutatie>
{
    public void Configure(EntityTypeBuilder<BetaalwijzeMutatie> builder)
    {
        var vorigeBetaalwijzeId = "vorigeBetaalwijzeId";
        var betaalwijzeId = "betaalwijzeId";

        builder.HasOne(bwm => bwm.VorigeBetaalwijze)
            .WithMany()
            .HasForeignKey(vorigeBetaalwijzeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(vorigeBetaalwijzeId)
            .HasColumnName(vorigeBetaalwijzeId)
            .HasColumnOrder(7);

        builder.HasOne(bwm => bwm.NieuweBetaalwijze)
            .WithMany()
            .HasForeignKey(betaalwijzeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(betaalwijzeId)
            .HasColumnName(betaalwijzeId)
            .HasColumnOrder(8);
    }
}

public class NaamWijzigingMutatieConfiguratie : IEntityTypeConfiguration<NaamWijzigingMutatie>
{
    public void Configure(EntityTypeBuilder<NaamWijzigingMutatie> builder)
    {
        var vorigePersonaliaId = "vorigePersonaliaId";
        var PersonaliaId = "personaliaId";

        builder.HasOne(nwm => nwm.VorigePersonalia)
            .WithMany()
            .HasForeignKey(vorigePersonaliaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(vorigePersonaliaId)
            .HasColumnName(vorigePersonaliaId)
            .HasColumnOrder(3);

        builder.HasOne(nwm => nwm.NieuwPersonalia)
            .WithMany()
            .HasForeignKey(PersonaliaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(PersonaliaId)
            .HasColumnName(PersonaliaId)
            .HasColumnOrder(4);
    }
}

public class VerhuisMutatieConfiguratie : IEntityTypeConfiguration<VerhuisMutatie>
{
    public void Configure(EntityTypeBuilder<VerhuisMutatie> builder)
    {
        var vorigAdresId = "vorigAdresId";
        var adresId = "adresId";

        builder.HasOne(vm => vm.VorigAdres)
            .WithMany()
            .HasForeignKey(vorigAdresId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(vorigAdresId)
            .HasColumnName(vorigAdresId)
            .HasColumnOrder(5);

        builder.HasOne(vm => vm.NieuwAdres)
            .WithMany()
            .HasForeignKey(adresId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(adresId)
            .HasColumnName(adresId)
            .HasColumnOrder(6);

    }
}

public class NieuwLidMutatieConfiguratie : IEntityTypeConfiguration<NieuwLidMutatie>
{
    public void Configure(EntityTypeBuilder<NieuwLidMutatie> builder)
    {
        var personaliaId = "personaliaId";
        var adresId = "adresId";
        var betaalwijzeId = "betaalwijzeId";

        builder.HasOne(nlm => nlm.Personalia)
            .WithMany()
            .HasForeignKey(personaliaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(personaliaId)
            .HasColumnName(personaliaId)
            .HasColumnOrder(4);

        builder.HasOne(nlm => nlm.Adres)
            .WithMany()
            .HasForeignKey(adresId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(adresId)
            .HasColumnName(adresId)
            .HasColumnOrder(6);

        builder.HasOne(nlm => nlm.Betaalwijze)
            .WithMany()
            .HasForeignKey(betaalwijzeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(betaalwijzeId)
            .HasColumnName(betaalwijzeId)
            .HasColumnOrder(8);
    }
}