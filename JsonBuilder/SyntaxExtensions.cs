using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;

namespace DartVS.DartAnalysis.JsonBuilder
{
	static class SyntaxExtensions
	{
		public static T WithXmlComment<T>(this T @class, string doc) where T : MemberDeclarationSyntax
		{
			var cleanComment = Regex.Replace(doc, @"\s+", " ");

			// TODO: Tidy up, wrap at suitable points, etc.
			return @class.WithLeadingTrivia(
				SyntaxFactory.TriviaList(
					SyntaxFactory.Trivia(
						SyntaxFactory.DocumentationCommentTrivia(
							SyntaxKind.SingleLineDocumentationCommentTrivia,
							SyntaxFactory.List(
								new XmlNodeSyntax[] {
									SyntaxFactory.XmlText()
									.WithTextTokens(
										SyntaxFactory.TokenList(SyntaxFactory.XmlTextLiteral(SyntaxFactory.TriviaList(SyntaxFactory.DocumentationCommentExterior(@"///")), " ", "", SyntaxFactory.TriviaList()))),
										SyntaxFactory.XmlElement(
											SyntaxFactory.XmlElementStartTag(SyntaxFactory.XmlName(SyntaxFactory.Identifier(@"summary"))),
											SyntaxFactory.XmlElementEndTag(SyntaxFactory.XmlName(SyntaxFactory.Identifier(@"summary")))
										)
									.WithContent(
										SyntaxFactory.SingletonList<XmlNodeSyntax>(
											SyntaxFactory.XmlText()
											.WithTextTokens(SyntaxFactory.TokenList(SyntaxFactory.XmlTextLiteral(SyntaxFactory.TriviaList(), cleanComment, "", SyntaxFactory.TriviaList())))
										)
									)
								}
							)
						)
					)
				)
			);
		}
	}
}
