using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Myriad.ECS.SourceGeneration;

/// <summary>
/// Find all structs which implement `IQuery` and automatically add an `IQueryWWWRRR` interface based
/// on the `Execute` method which has been implemented.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class AddQueryInterface
    : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Filter for structs with IQuery interface
        var classDeclarations = context
            .SyntaxProvider
            .CreateSyntaxProvider(
                predicate: IsSyntaxTargetForGeneration,
                transform: GetSemanticTargetForGeneration)
            .Where(static m => m is not null);

        //// Combine the selected enums with the `Compilation`
        //var compilationAndMethods = context.CompilationProvider.Combine(structs.WithComparer(Comparer.Instance).Collect());
        //context.RegisterSourceOutput(compilationAndMethods, static (spc, source) => Generate(source.Item1, source.Item2, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken ct)
    {
        if (node is not StructDeclarationSyntax sds)
            return false;

        return true;
    }

    private static StructDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken ct)
    {
        var candidate = (StructDeclarationSyntax)context.Node;

        if (candidate.BaseList is null)
            return null;

        Debug.WriteLine(candidate.BaseList.ToFullString());
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