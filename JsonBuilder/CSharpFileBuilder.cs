using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using System;
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

		public CSharpClass AddClass(string name)
		{
			var @class = new CSharpClass(name);
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
			var formattedResult = Formatter.Format(document, workspace);

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
		readonly List<Tuple<Type, string>> properties = new List<Tuple<Type, string>>();

		public CSharpClass(string name)
		{
			this.name = name;
		}

		public void AddProperty<T>(string name)
		{
			properties.Add(Tuple.Create(typeof(T), name));
		}

		public MemberDeclarationSyntax GetClass()
		{
			return SyntaxFactory
				.ClassDeclaration(name)
				.WithMembers(GetProperties());
		}
		SyntaxList<MemberDeclarationSyntax> GetProperties()
		{
			return SyntaxFactory.List<MemberDeclarationSyntax>(
				properties.Select(p => SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(p.Item1.FullName), p.Item2))
			);
		}
	}
}
