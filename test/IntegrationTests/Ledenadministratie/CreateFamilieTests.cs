using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Ledenadministratie.Adressen;
using DA.GtSWB.Application.Ledenadministratie.Commands;
using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Common.Extensions;
using Shouldly;

namespace DA.GtSWB.Tests.IntegrationTests.Ledenadministratie;

[Collection("IntegrationTestCollection")]
public class CreateFamilieTests(IntegrationTestFixture testContext)
{
    [Fact]
    public async Task CreateFamilie_Should_ReturnAdresId_WhenOneLidIsAdded()
    {
        var personaliaDto = new PersonaliaDto() { Voorletters = "A", Achternaam = "Achternaam", GeslachtInput = "M", Geboortedatum = new DateOnly(2000, 1, 1) };
        var adresDto = new AdresDto() { Straat = " Straat", Huisnummer = " 2A ", Postcode = "1234 AB", Woonplaats = "   Plaats " };
        var metadata = new RequestMetadata(DateTime.Now, nameof(CreateFamilieTests));

        var request = new CreateFamilie.Command(adresDto, [personaliaDto], metadata);

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

        var request = new CreateFamilie.Command(adresDto, [personaliaDto1, personaliaDto2], metadata);

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

        var request = new CreateFamilie.Command(adresDto, [personaliaDto], metadata);

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

        var request = new CreateFamilie.Command(adresDto, [personaliaDto1, personaliaDto2], metadata);

        var result = await testContext.Sender.Send(request);

        result.IsSuccess.ShouldBeFalse();
    }
}
