using DA.GtSWB.Tests.UnitTests.Application.Extensions;

namespace DA.GtSWB.Tests.UnitTests.Application.Ledenadminstratie;

public class AdresDtoTests_Validation
{
    [Fact]
    public void Validate_Should_ReturnSuccess_WhenInputIsValid()
    {
        var result = ValidDtoProvider.AdresDto.Validate();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_WhenStraatIsEmpty()
    {
        var dto = ValidDtoProvider.AdresDto with { Straat = "" };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Straat", 1);
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_WhenStraatIsTooShort()
    {
        var dto = ValidDtoProvider.AdresDto with { Straat = "A" };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Straat", 1);
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_WhenStraatIsTooLong()
    {
        var dto = ValidDtoProvider.AdresDto with { Straat = new string('a', 101) };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Straat", 1);
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_WhenHuisnummerIsEmpty()
    {
        var dto = ValidDtoProvider.AdresDto with { Huisnummer = "" };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Huisnummer", 1);
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_WhenHuisnummerIsTooLong()
    {
        var dto = ValidDtoProvider.AdresDto with { Huisnummer = new string('a', 11) };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Huisnummer", 1);
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_WhenWoonplaatsIsEmpty()
    {
        var dto = ValidDtoProvider.AdresDto with { Woonplaats = "" };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Woonplaats", 1);
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_WhenWoonplaatsIsTooLong()
    {
        var dto = ValidDtoProvider.AdresDto with { Woonplaats = new string('a', 101) };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Woonplaats", 1);
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_WhenLandIsTooLong()
    {
        var dto = ValidDtoProvider.AdresDto with { Land = new string('a', 11) };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Land", 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123AB")]
    [InlineData("1234")]
    [InlineData("1234ABC")]
    [InlineData("1234SA")]
    public void Validate_Should_ReturnValidationFailure_WhenPostcodeIsIncorrect_ForNL(string? postcode)
    {
        var dto = ValidDtoProvider.AdresDto with { Land = "NL", Postcode = postcode };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Postcode", 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("1234AB")]
    [InlineData("12345")]
    public void Validate_Should_ReturnValidationFailure_WhenPostcodeIsIncorrect_ForB(string? postcode)
    {
        var dto = ValidDtoProvider.AdresDto with { Land = "B", Postcode = postcode };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Postcode", 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1234")]
    [InlineData("1234AB")]
    [InlineData("123456")]
    public void Validate_Should_ReturnValidationFailure_WhenPostcodeIsIncorrect_ForD(string? postcode)
    {
        var dto = ValidDtoProvider.AdresDto with { Land = "D", Postcode = postcode };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Postcode", 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1234567890AB")] // too many characters
    public void Validate_Should_ReturnValidationFailure_WhenPostcodeIsIncorrect_ForOtherCountry(string? postcode)
    {
        var dto = ValidDtoProvider.AdresDto with { Land = "F", Postcode = postcode };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Postcode", 1);
    }

    [Fact]
    public void Validate_Should_ReturnValidationFailure_ForEachWrongProperty()
    {
        var dto = ValidDtoProvider.AdresDto with { Huisnummer = "", Straat = "A", Woonplaats = new string('a', 101) };
        var result = dto.Validate();
        result.HasIssue.ShouldBeTrue();
        result.ShouldHaveValidationIssue("Huisnummer", 1);
        result.ShouldHaveValidationIssue("Straat", 1);
        result.ShouldHaveValidationIssue("Woonplaats", 1);
    }
}
