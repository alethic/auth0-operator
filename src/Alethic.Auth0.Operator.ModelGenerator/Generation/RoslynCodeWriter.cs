using Alethic.Auth0.Operator.ModelGenerator.Configuration;
using Alethic.Auth0.Operator.ModelGenerator.Discovery;
using Alethic.Auth0.Operator.ModelGenerator.Transformation;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Alethic.Auth0.Operator.ModelGenerator.Generation;

public sealed class RoslynCodeWriter
{
    public GenerationResult WriteFiles(IEnumerable<GeneratedType> generatedTypes, string outputDirectory, GeneratorConfiguration configuration)
    {
        var fullOutputDirectory = Path.GetFullPath(outputDirectory);
        Directory.CreateDirectory(fullOutputDirectory);

        var generatedFiles = new List<string>();
        foreach (var generatedType in generatedTypes)
        {
            var relativePath = configuration.UseNamespaceSubdirectories
                ? Path.Combine(generatedType.Namespace.Replace('.', Path.DirectorySeparatorChar), generatedType.Name + ".cs")
                : generatedType.Name + ".cs";
            var filePath = Path.Combine(fullOutputDirectory, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            if (!configuration.OverwriteExistingFiles && File.Exists(filePath))
            {
                continue;
            }

            var compilationUnit = CompilationUnit()
                .WithUsings(List(generatedType.UsingDirectives.Select(@namespace => UsingDirective(ParseName(@namespace)))))
                .WithMembers(List<MemberDeclarationSyntax>(
                [
                    FileScopedNamespaceDeclaration(ParseName(generatedType.Namespace))
                        .WithMembers(List<MemberDeclarationSyntax>([CreateTypeDeclaration(generatedType, configuration)])),
                ]))
                .NormalizeWhitespace();

            File.WriteAllText(filePath, compilationUnit.ToFullString() + Environment.NewLine);
            generatedFiles.Add(filePath);
        }

        return new GenerationResult
        {
            OutputDirectory = fullOutputDirectory,
            GeneratedFiles = generatedFiles,
        };
    }

    private static MemberDeclarationSyntax CreateTypeDeclaration(GeneratedType generatedType, GeneratorConfiguration configuration)
    {
        if (generatedType.IsEnumLike)
        {
            return CreateEnumDeclaration(generatedType);
        }

        TypeDeclarationSyntax declaration;
        if (configuration.EmitRecords)
        {
            declaration = RecordDeclaration(
                    kind: SyntaxKind.RecordDeclaration,
                    attributeLists: default,
                    modifiers: TokenList(Token(SyntaxKind.PublicKeyword)),
                    keyword: Token(SyntaxKind.RecordKeyword),
                    classOrStructKeyword: default,
                    identifier: Identifier(generatedType.Name),
                    typeParameterList: null,
                    parameterList: null,
                    baseList: null,
                    constraintClauses: default,
                    openBraceToken: Token(SyntaxKind.OpenBraceToken),
                    members: List(generatedType.Properties.Select(CreatePropertyDeclaration)),
                    closeBraceToken: Token(SyntaxKind.CloseBraceToken),
                    semicolonToken: default)
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)));
        }
        else
        {
            declaration = ClassDeclaration(generatedType.Name)
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithMembers(List(generatedType.Properties.Select(CreatePropertyDeclaration)));
        }

        if (generatedType.Attributes.Count > 0)
        {
            declaration = declaration.WithAttributeLists(List(generatedType.Attributes.Select(CreateAttributeList)));
        }

        return declaration;
    }

    private static MemberDeclarationSyntax CreateEnumDeclaration(GeneratedType generatedType)
    {
        var declaration = EnumDeclaration(generatedType.Name)
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithMembers(SeparatedList(generatedType.EnumMembers.Select(CreateEnumMemberDeclaration)));

        return declaration.WithAttributeLists(List(new[]
        {
            AttributeList(SingletonSeparatedList(Attribute(IdentifierName("JsonConverter"))
                .WithArgumentList(AttributeArgumentList(SingletonSeparatedList(AttributeArgument(ParseExpression("typeof(JsonStringEnumConverter)"))))))),
        }));
    }

    private static EnumMemberDeclarationSyntax CreateEnumMemberDeclaration((string Name, string SerializedName) member)
    {
        return EnumMemberDeclaration(member.Name)
            .WithAttributeLists(List(new[]
            {
                AttributeList(SingletonSeparatedList(Attribute(IdentifierName("JsonStringEnumMemberName"))
                    .WithArgumentList(AttributeArgumentList(SingletonSeparatedList(AttributeArgument(ParseExpression("\"" + member.SerializedName + "\""))))))),
            }));
    }

    private static MemberDeclarationSyntax CreatePropertyDeclaration(GeneratedProperty property)
    {
        var accessors = property.HasSetter
            ? new[]
            {
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
            }
            : new[]
            {
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
            };

        var declaration = PropertyDeclaration(ParseTypeName(RenderType(property.Type)), property.Name)
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithAccessorList(AccessorList(List(accessors)));

        if (property.Attributes.Count > 0)
        {
            declaration = declaration.WithAttributeLists(List(property.Attributes.Select(CreateAttributeList)));
        }

        if (!string.IsNullOrWhiteSpace(property.Summary))
        {
            declaration = declaration.WithLeadingTrivia(TriviaList(CreateSummaryTrivia(property.Summary!)));
        }

        return declaration;
    }

    private static AttributeListSyntax CreateAttributeList(GeneratedAttribute attribute)
    {
        var arguments = new List<AttributeArgumentSyntax>();
        arguments.AddRange(attribute.ConstructorArguments.Select(argument => AttributeArgument(ParseExpression(argument))));
        arguments.AddRange(attribute.NamedArguments.Select(argument => AttributeArgument(ParseExpression(argument.Value))
            .WithNameEquals(NameEquals(IdentifierName(argument.Key)))));

        var attributeName = attribute.TypeName.EndsWith("Attribute", StringComparison.Ordinal)
            ? attribute.TypeName[..^9]
            : attribute.TypeName;

        return AttributeList(SingletonSeparatedList(Attribute(ParseName(attributeName))
            .WithArgumentList(arguments.Count == 0 ? null : AttributeArgumentList(SeparatedList(arguments)))));
    }

    private static IEnumerable<SyntaxTrivia> CreateSummaryTrivia(string summary)
    {
        yield return Comment("/// <summary>");
        yield return ElasticCarriageReturnLineFeed;
        yield return Comment($"/// {summary}");
        yield return ElasticCarriageReturnLineFeed;
        yield return Comment("/// </summary>");
        yield return ElasticCarriageReturnLineFeed;
    }

    private static string RenderType(DiscoveredTypeReference type)
    {
        if (type.IsArray && type.ElementType is not null)
        {
            var arrayType = RenderType(type.ElementType) + string.Concat(Enumerable.Repeat("[]", Math.Max(1, type.ArrayRank)));
            return type.IsNullableReferenceType
                ? arrayType + "?"
                : arrayType;
        }

        var baseType = type.Alias ?? type.Name;
        if (type.GenericArguments.Count > 0)
        {
            baseType += "<" + string.Join(", ", type.GenericArguments.Select(RenderType)) + ">";
        }

        if (type.IsNullableValueType || type.IsNullableReferenceType)
        {
            return baseType + "?";
        }

        return baseType;
    }
}