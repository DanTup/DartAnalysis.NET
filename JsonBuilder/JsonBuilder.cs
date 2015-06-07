using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace DartVS.DartAnalysis.JsonBuilder
{
	class JsonBuilder
	{
		CSharpFileBuilder builder = new CSharpFileBuilder("DartVS.DartAnalysis");

		public JsonBuilder(FileInfo apiDoc)
		{
			VisitChildren(XDocument.Load(apiDoc.FullName));
		}

		void VisitChildren(XContainer parent)
		{
			foreach (var element in parent.Elements())
				Visit(element);
		}

		void Visit(XElement element)
		{
			if (element.Name == "request")
				VisitRequest(element);
			else
				VisitChildren(element);
		}

		void VisitRequest(XElement element)
		{
			var domainName = element.Ancestors("domain").First().Attribute("name").Value;
			var methodName = element.Attribute("method").Value;
			var className = GenerateName(domainName, methodName);
			var doc = element.Element("p").Value;
			builder.AddClass(className, doc);
		}

		string GenerateName(params string[] parts)
		{
			return string.Join("", parts.Select(p => char.ToUpper(p[0]) + p.Substring(1)));
		}

		public string GetContents()
		{
			return builder.GetContents();
		}
	}
}
