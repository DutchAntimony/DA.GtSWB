using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace DA.GtSWB.SourceGenerators.InputSanitizer;

[Generator]
public class SanitizationGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Register a syntax receiver that will collect candidate classes and properties.
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        // Retrieve the syntax receiver to access the candidate nodes.
        if (context.SyntaxReceiver is not SyntaxReceiver receiver)
            return;

        // Loop over all candidate classes.
        foreach (var classDeclaration in receiver.CandidateClasses)
        {
            // Generate sanitized properties based on detected class
            var source = GenerateSanitizedProperties(classDeclaration);

            // Add the source to the compilation
            var className = classDeclaration.Identifier.Text;
            context.AddSource($"{className}_Sanitized.g.cs", SourceText.From(source, Encoding.UTF8));
        }
    }

    private string GenerateSanitizedProperties(ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.Text;
        var sb = new StringBuilder();
        sb.AppendLine($"public partial class {className}");
        sb.AppendLine("{");

        // Find properties marked with [Sanitize]
        var properties = classDeclaration.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(prop => prop.AttributeLists.Any(attr => attr.Attributes.Any(a => a.Name.ToString() == "Sanitize")));

        foreach (var prop in properties)
        {
            var propName = prop.Identifier.Text;
            var propType = prop.Type.ToString();

            // Generate a backing field and sanitized getter/setter for each property
            sb.AppendLine($"    private {propType} _{propName};");
            sb.AppendLine($"    public {propType} {propName}");
            sb.AppendLine("    {");
            sb.AppendLine($"        get => _{propName};");
            sb.AppendLine($"        init => _{propName} = value?.Sanitize();");
            sb.AppendLine("    }");
        }

        sb.AppendLine("}");
        return sb.ToString();
    }

    // This class identifies classes and properties marked with [Sanitize]
    private class SyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // Check if the node is a class declaration
            if (syntaxNode is ClassDeclarationSyntax classDeclaration)
            {
                // Check if the class or any of its properties has [Sanitize] attribute
                if (classDeclaration.AttributeLists
                    .Any(attr => attr.Attributes.Any(a => a.Name.ToString() == "Sanitize")) ||
                    classDeclaration.Members.OfType<PropertyDeclarationSyntax>()
                    .Any(prop => prop.AttributeLists.Any(attr => attr.Attributes.Any(a => a.Name.ToString() == "Sanitize"))))
                {
                    CandidateClasses.Add(classDeclaration);
                }
            }
        }
    }
}

