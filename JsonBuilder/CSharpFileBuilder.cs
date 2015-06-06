using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartVS.DartAnalysis.JsonBuilder
{
	public class CSharpFileBuilder
	{
		NamespaceDeclarationSyntax @namespace;

		public CSharpFileBuilder(string @namespace) {
			this.@namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName(@namespace));
        }

		public CSharpClass AddClass(string name)
		{
			var @class = SyntaxFactory.ClassDeclaration(name);
            this.@namespace = this.@namespace.AddMembers(@class);
			return new CSharpClass(@class);
        }

		public string GetContents()
		{
			var workspace = MSBuildWorkspace.Create();
			var formattedResult = Formatter.Format(@namespace, workspace);

			var output = new StringBuilder();
			using (var sw = new StringWriter(output))
				formattedResult.WriteTo(sw);

			return output.ToString();
		}
	}

	public class CSharpClass
	{
		ClassDeclarationSyntax @class;

		public CSharpClass(ClassDeclarationSyntax @class)
		{
			this.@class = @class;
		}

		public void AddProperty<T>(string name)
		{
			var property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(typeof(T).FullName), name);

			@class = @class.AddMembers(property);
		}
	}
}
