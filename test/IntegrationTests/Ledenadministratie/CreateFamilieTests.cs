using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Ledenadministratie.Adressen;
using DA.GtSWB.Application.Ledenadministratie.Commands;
using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Common.Extensions;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Persistence.Configuration.Tests;
using DA.Options;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace DA.GtSWB.Tests.IntegrationTests.Ledenadministratie;

[Collection("IntegrationTestCollection")]
public class CreateFamilieTests(IntegrationTestFixture testContext)
{
    [Fact(Skip ="")]
    public async Task CreateFamilie_Should_ReturnAdresId_WhenOneLidIsAdded()
    {
        var personaliaDto = new PersonaliaDto() { Voorletters = "A", Achternaam = "Achternaam", GeslachtInput = "M", Geboortedatum = new DateOnly(2000, 1, 1) };
        var adresDto = new AdresDto() { Straat = " Straat", Huisnummer = " 2A ", Postcode = "1234 AB", Woonplaats = "   Plaats " };
        var metadata = new RequestMetadata(DateTime.Now, nameof(CreateFamilieTests));

        var request = new CreateFamilie.Command(adresDto, [personaliaDto], Option.None, metadata);

        var result = await testContext.Sender.Send(request);

        result.IsSuccess.ShouldBeTrue();
        var adresId = result.ReduceOrThrow();
        adresId.IsEmpty().ShouldBeFalse();
    }

    [Fact]
    public async Task CreateFamilie_Should_ReturnAdresId_WhenMultipleLedenAreAdded()
    {
        var personaliaDto1 = new PersonaliaDto() { Voorletters = "A", Achternaam = "Achternaam", GeslachtInput = "M", Geboortedatum = new DateOnly(2000, 1, 1) };
        var personaliaDto2 = new PersonaliaDto() { Voorletters = "B", Achternaam = "Naamachter", GeslachtInput = "V", Geboortedatum = new DateOnly(1980, 2, 3) };
        var adresDto = new AdresDto() { Straat = "Straat", Huisnummer = "2", Postcode = "1234AB", Woonplaats = "Plaats" };
        var metadata = new RequestMetadata(DateTime.Now, nameof(CreateFamilieTests));

        var request = new CreateFamilie.Command(adresDto, [personaliaDto1, personaliaDto2], Option.None, metadata);

        var result = await testContext.Sender.Send(request);

        result.IsSuccess.ShouldBeTrue();
        var adresId = result.ReduceOrThrow();
        adresId.IsEmpty().ShouldBeFalse();
    }

    [Fact]
    public async Task CreateFamilie_Should_ReturnError_WhenAdresIsInvalid()
    {
        var personaliaDto = new PersonaliaDto() { Voorletters = "A", Achternaam = "Achternaam", GeslachtInput = "M", Geboortedatum = new DateOnly(2000, 1, 1) };
        var adresDto = new AdresDto() { Straat = "Straat", Huisnummer = "2", Postcode = "1234ABC", Woonplaats = "Plaats" };
        var metadata = new RequestMetadata(DateTime.Now, nameof(CreateFamilieTests));

        var request = new CreateFamilie.Command(adresDto, [personaliaDto], Option.None, metadata);

        var result = await testContext.Sender.Send(request);

        result.IsSuccess.ShouldBeFalse();
    }


    [Fact]
    public async Task CreateFamilie_Should_ReturnError_WhenVoorlettersAreInvalid()
    {
        var personaliaDto1 = new PersonaliaDto() { Voorletters = "A", Achternaam = "Achternaam", GeslachtInput = "M", Geboortedatum = new DateOnly(2000, 1, 1) };
        var personaliaDto2 = new PersonaliaDto() { Voorletters = string.Empty, Achternaam = "Naamachter", GeslachtInput = "V", Geboortedatum = new DateOnly(1980, 2, 3) };
        var adresDto = new AdresDto() { Straat = "Straat", Huisnummer = "2", Postcode = "1234AB", Woonplaats = "Plaats" };
        var metadata = new RequestMetadata(DateTime.Now, nameof(CreateFamilieTests));

        var request = new CreateFamilie.Command(adresDto, [personaliaDto1, personaliaDto2], Option.None, metadata);

        var result = await testContext.Sender.Send(request);

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task Test()
    {
        using var transaction = testContext.DbContext.Database.BeginTransaction();

        try
        {
            // Create a new pay method
            var payMethod = new PayMethod() { Id = BetaalwijzeId.Create() };

            // Create members and assign them to the pay method from the member side
            var member1 = new Member { Id = LidId.Create(), Name = "John" };
            var member2 = new Member { Id = LidId.Create(), Name = "Jane" };

            // Save everything via the Member context
            testContext.DbContext.Members.AddRange(member1, member2);
            await testContext.DbContext.SaveChangesAsync();

            // Member1 assigns itself to the pay method and becomes the responsible member
            member1.AssignPayMethod(payMethod, isResponsable: true);

            // Member2 assigns itself to the same pay method but is not responsible
            member2.AssignPayMethod(payMethod);

            testContext.DbContext.Set<PayMethod>().Add(payMethod);
            await testContext.DbContext.SaveChangesAsync();

            var member = testContext.DbContext.Members
                .Include(m => m.PayMethod)
                .ThenInclude(pm => pm.ResponsableMember)
                .FirstOrDefault(m => m.Id == member1.Id);

            if (member != null && member.PayMethod != null)
            {
                Console.WriteLine($"Member: {member.Name} is using PayMethod {member.PayMethod.Id}");

                if (member.PayMethod.ResponsableMember != null)
                {
                    Console.WriteLine($"Responsable Member: {member.PayMethod.ResponsableMember.Name}");
                }
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
        }
    }
}
