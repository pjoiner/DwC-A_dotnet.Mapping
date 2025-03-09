// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "Different eol causes verify to faile on unix vs windows", Scope = "member", Target = "~M:DwC_A.Generator.ClassGenerator.FormatDeclarationSyntax(Microsoft.CodeAnalysis.SyntaxNode)~System.String")]
[assembly: SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "Different eol causes verify to faile on unix vs windows", Scope = "member", Target = "~M:DwC_A.Generator.RoslynGeneratorUtils.FormatSyntax(Microsoft.CodeAnalysis.SyntaxNode)~System.String")]
[assembly: SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1035:Do not use APIs banned for analyzers", Justification = "Different eol causes verify to faile on unix vs windows", Scope = "member", Target = "~M:DwC_A.Mapping.MappingSourceGenerator.Execute(Microsoft.CodeAnalysis.SourceProductionContext,Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax)")]
