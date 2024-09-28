using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA.GtSWB.Persistence.Configuration.Tests;

public class PayMethod
{
    public BetaalwijzeId Id { get; set; }

    // Collection of members using this payment method
    public ICollection<Member> Members { get; set; } = [];

    // The responsible member (a specific member from the Members collection)
    public LidId? ResponsableMemberId { get; set; }
    public Member ResponsableMember { get; set; } = null!;
}

public class Member
{
    public LidId Id { get; set; }
    public string Name { get; set; } = null!;

    // Each member may have a PayMethod (many members can share the same method)
    public BetaalwijzeId? PayMethodId { get; set; }
    public PayMethod PayMethod { get; set; } = null!;

    // Additional property to indicate whether the member is responsible
    public bool IsResponsable { get; set; }

    // Method to assign a PayMethod to this member
    public void AssignPayMethod(PayMethod payMethod, bool isResponsable = false)
    {
        this.PayMethod = payMethod;

        // If this member is responsible for the payment method, set it
        if (isResponsable)
        {
            payMethod.ResponsableMember = this;
        }
    }
}

public class PayMethodConfiguration : IEntityTypeConfiguration<PayMethod>
{
    public void Configure(EntityTypeBuilder<PayMethod> builder)
    {
        builder.Property(p => p.Id)
            .HasConversion(new IdConverter<BetaalwijzeId>())
            .ValueGeneratedNever();

        builder
            .HasOne(p => p.ResponsableMember)      // PayMethod has one ResponsableMember
            .WithMany()                            // No reverse navigation property
            .HasForeignKey(p => p.ResponsableMemberId)  // FK in PayMethod pointing to Member
            .OnDelete(DeleteBehavior.Restrict);    // Prevent cascade deletion
    }
}

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.Property(l => l.Id)
            .HasConversion(new IdConverter<LidId>())
            .ValueGeneratedNever();

        builder
            .HasOne(m => m.PayMethod)              // Each Member has one PayMethod
            .WithMany(p => p.Members)              // PayMethod has many Members
            .HasForeignKey(m => m.PayMethodId)     // FK in Member pointing to PayMethod
            .OnDelete(DeleteBehavior.Restrict);    // Prevent cascade deletion
    }
}

