using System.Diagnostics.CodeAnalysis;

namespace DA.GtSWB.Tests.UnitTests.Application.Extensions;
public static class ResultExtensions
{
    [ExcludeFromCodeCoverage]
    public static void ShouldHaveValidationIssue(this Result result, string property, int expectedQuantity)
    {
        result.Act()
            .OnSuccess(() => result.IsSuccess.ShouldBeFalse())
            .OnError<ValidationError>(validationError =>
                validationError.Failures.Count(f => f.Property == property).ShouldBe(expectedQuantity))
            .OnDefaultError(error => throw new Exception(error.GetMessage()))
            .Execute();
    }
}
