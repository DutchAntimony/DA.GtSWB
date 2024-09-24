using DA.GtSWB.Application.Ledenadministratie.Adressen;

namespace DA.GtSWB.Tests.UnitTests.Application.Ledenadminstratie;

public class AdresDtoTests
{
    [Fact]
    public void Construction_Should_SanitizeInput()
    {
        var _valid = ValidDtoProvider.AdresDto;
        _valid.Straat.ShouldBe("Straat");
        _valid.Huisnummer.ShouldBe("1A");
        _valid.Postcode.ShouldBe("1234AB");
        _valid.Woonplaats.ShouldBe("Woonplaats");
        _valid.Land.ShouldBe(string.Empty);
    }

    [Fact]
    public void Construction_Should_BePossible_WithNullValues()
    {
        var dto = new AdresDto()
        {
            Straat = null,
            Huisnummer = null,
            Postcode = null,
            Woonplaats = null,
            Land = null
        };

        dto.ShouldNotBeNull();
    }

    [Fact]
    public void ToDomainModel_Should_ReturnSuccess_WhenInputIsValid()
    {
        var result = ValidDtoProvider.AdresDto.ToDomainModel();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void ToDomainModel_Should_ReturnSuccess_WhenInputIsInvalid()
    {
        var invalidDto = ValidDtoProvider.AdresDto with { Straat = null };
        var result = invalidDto.ToDomainModel();
        result.HasIssue.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("NL")]
    [InlineData("Nederland")]
    public void SetLandCode_Should_SetNLInputs_ToStringEmpty(string? land)
    {
        var dto = new AdresDto() { Land = land };
        dto.Land.ShouldBe(string.Empty);
    }

    [Theory]
    [InlineData("B")]
    [InlineData("BE")]
    [InlineData("Belgie")]
    [InlineData("België")]
    [InlineData("Belgique")]
    [InlineData("Belgium")]
    public void SetLandCode_Should_SetBInputs_ToStringEmpty(string? land)
    {
        var dto = new AdresDto() { Land = land };
        dto.Land.ShouldBe("B");
    }

    [Theory]
    [InlineData("D")]
    [InlineData("DE")]
    [InlineData("Deutschland")]
    [InlineData("Germany")]
    public void SetLandCode_Should_SetDInputs_ToStringEmpty(string? land)
    {
        var dto = new AdresDto() { Land = land };
        dto.Land.ShouldBe("D");
    }
}
