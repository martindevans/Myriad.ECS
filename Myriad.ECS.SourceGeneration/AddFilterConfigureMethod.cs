using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Myriad.ECS.SourceGeneration;

/// <summary>
/// Find all structs which implement `IQueryFilter` and automatically add a `ConfigureQueryBuilder` method
/// </summary>
[Generator(LanguageNames.CSharp)]
public class AddFilterConfigureMethod
    : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Register the attribute source
        context.RegisterPostInitializationOutput(i =>
        {
            var attributeSource = @"
            namespace HelloWorld
            {
                public class MyExampleAttribute: System.Attribute {} 
            }";
            i.AddSource("MyExampleAttribute.g.cs", attributeSource);
        });
        // Register the attribute source
        context.RegisterPostInitializationOutput(i =>
        {
            var attributeSource = @"
            namespace HelloWorld
            {
                public class MyExample3Attribute: System.Attribute {} 
            }";
            i.AddSource("MyExample3Attribute.g.cs", attributeSource);
        });

        // Filter for structs with IQueryFilter interface
        var classDeclarations = context
            .SyntaxProvider
            .CreateSyntaxProvider(
                predicate: IsStructDeclaration,
                transform: GetSemanticTargetForGeneration)
            .Where(static m => m is not null);

        //// Combine the selected enums with the `Compilation`
        //var compilationAndMethods = context.CompilationProvider.Combine(structs.WithComparer(Comparer.Instance).Collect());
        //context.RegisterSourceOutput(compilationAndMethods, static (spc, source) => Generate(source.Item1, source.Item2, spc));
    }

    private static bool IsStructDeclaration(SyntaxNode node, CancellationToken ct)
    {
        if (node is not StructDeclarationSyntax sds)
            return false;

        return true;
    }

    private static StructDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken ct)
    {
        var candidate = (StructDeclarationSyntax)context.Node;

        if (candidate.BaseList is null)
            return null;

        

        var symbol = context.SemanticModel.GetDeclaredSymbol(candidate.SyntaxTree.GetRoot());

        foreach (var @base in candidate.BaseList.Types)
        {
            
        }

        return null;
    }

    private static void Generate(Compilation compilation, ImmutableArray<StructDeclarationSyntax> structs, SourceProductionContext context)
    {
    }

    private class Comparer
        : IEqualityComparer<StructDeclarationSyntax>
    {
        public static readonly Comparer Instance = new Comparer();

        public bool Equals(StructDeclarationSyntax x, StructDeclarationSyntax y)
        {
            if (x == null && y == null)
                return true;
            if (x == null)
                return false;
            return x.Equals(y);
        }

        public int GetHashCode(StructDeclarationSyntax obj)
        {
            return obj.GetHashCode();
        }
    }
}