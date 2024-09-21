using System.Text;

namespace DA.Results.Issues;

public record ValidationError(List<ValidationFailure> Failures) : Error
{
    public Dictionary<string, IEnumerable<string>> GetFailuresGroupedByProperty() =>
        Failures.GroupBy(f => f.Property)
                 .ToDictionary(group => group.Key,
                               group => group.Select(f => f.Message));

    public IEnumerable<string> GetFailuresOfProperty(string property) =>
        Failures.Where(f => f.Property == property).Select(f => f.Message);

    public override string GetMessage() 
    {
        StringBuilder stringBuilder = new ();
        stringBuilder.AppendLine("Validation failures: ");
        Failures.ForEach(f => stringBuilder.AppendLine(
            $"{f.Property} : {f.Message}"));
        return stringBuilder.ToString();
    }

    public ValidationError(string property, string message) 
        : this([new ValidationFailure(property, message)]) { }
}

