using DA.GtSWB.Application.Ledenadministratie.Personen;

namespace DA.GtSWB.Tests.UnitTests.Application.Ledenadminstratie;

public class PersonaliaDtoTests
{
    [Theory]
    [InlineData("a", "A.")]
    [InlineData("ab", "A.B.")]
    [InlineData("SJ", "S.J.")]
    [InlineData("Sj", "Sj.")]
    [InlineData("SjA", "Sj.A.")]
    [InlineData("ASj", "A.Sj.")]
    public void Construct_Should_ProperlySetVoorletters_WhenVoorlettersAreValid(string input, string expected)
    {
        var dto = new PersonaliaDto() with { Voorletters = input };
        dto.Voorletters.ShouldBe(expected);
    }

    //[Theory]
    //[InlineData("M")]
    //[InlineData("Man")]
    //[InlineData("Dhr")]
    //[InlineData("Dhr.")]
    //public void ParseGeslacht_Should_ReturnMan(string geslacht)
    //{
    //    var dto = new PersonaliaDto() with { GeslachtInput = geslacht };

    //}
}