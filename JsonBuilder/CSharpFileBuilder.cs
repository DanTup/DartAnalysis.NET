using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DartVS.DartAnalysis.JsonBuilder
{
	public class CSharpFileBuilder
	{
		readonly string @namespace;
		readonly List<CSharpClass> classes = new List<CSharpClass>();

		public CSharpFileBuilder(string @namespace) {
			this.@namespace = @namespace;
		}

		public CSharpClass AddClass(string name, string docComment)
		{
			var @class = new CSharpClass(name, docComment);
			classes.Add(@class);
			return @class;
		}

		public string GetContents()
		{
			var document =
				SyntaxFactory
					.NamespaceDeclaration(SyntaxFactory.IdentifierName(@namespace))
					.WithMembers(GetClasses());

			var workspace = MSBuildWorkspace.Create();
			var options = workspace.Options.WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, true);
			var formattedResult = Formatter.Format(document, workspace, options);

			var output = new StringBuilder();
			using (var sw = new StringWriter(output))
				formattedResult.WriteTo(sw);

			return output.ToString();
		}

		SyntaxList<MemberDeclarationSyntax> GetClasses()
		{
			return SyntaxFactory.List(
				classes.Select(c => c.GetClass())
			);
		}
	}

	public class CSharpClass
	{
		readonly string name;
		readonly string docComment;
		readonly List<CSharpProperty> properties = new List<CSharpProperty>();

		public CSharpClass(string name, string docComment)
		{
			this.name = name;
			this.docComment = docComment;
		}

		public void AddProperty<T>(string name, string docComment)
		{
			properties.Add(new CSharpProperty(typeof(T), name, docComment));
		}

		public MemberDeclarationSyntax GetClass()
		{
			return SyntaxFactory
				.ClassDeclaration(name)
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
				// TODO: XML Docs...
				.WithMembers(GetProperties());
		}
		SyntaxList<MemberDeclarationSyntax> GetProperties()
		{
			return SyntaxFactory.List<MemberDeclarationSyntax>(
				properties.Select(p => p.GetProperty())
			);
		}
	}

	public class CSharpProperty {
		readonly Type type;
		readonly string name;
		readonly string docComment;

		static CSharpCodeProvider csharp = new CSharpCodeProvider();

		public CSharpProperty(Type type, string name, string docComment) {
			this.type = type;
			this.name = name;
			this.docComment = docComment;
		}

		public PropertyDeclarationSyntax GetProperty() {
            return
				SyntaxFactory.PropertyDeclaration(
					SyntaxFactory.ParseTypeName(csharp.GetTypeOutput(new CodeTypeReference(type))),
					name
				)
				// TODO: XML Docs...
				.AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
				.AddAccessorListAccessors(
					SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
					SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
				);
        }
	}
}
