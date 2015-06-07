using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace DartVS.DartAnalysis.JsonBuilder
{
	class ApiClassBuilder
	{
		CSharpFileBuilder builder = new CSharpFileBuilder("DartVS.DartAnalysis");

		public ApiClassBuilder(FileInfo apiDoc)
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
			var doc = element.Element("p").Value;

			var requestClass = builder.AddClass(Name(domainName, methodName, "Request"), doc);
			if (element.Element("params") != null)
				BuildRequestClass(requestClass, element.Element("params"));

			if (element.Element("result") != null)
			{
				var responseClass = builder.AddClass(Name(domainName, methodName, "Response"), doc);
				BuildResultClass(responseClass, element.Element("result"));
			}
		}
		void BuildRequestClass(CSharpClass requestClass, XElement element)
		{
			foreach (var field in element.Elements("field"))
			{
				// TODO: Fix type
				requestClass.AddProperty<string>(Name(field.Attribute("name").Value), field.Element("p").Value);
			}
		}

		void BuildResultClass(CSharpClass responseClass, XElement element)
		{
			foreach (var field in element.Elements("field"))
			{
				// TODO: Fix type
				responseClass.AddProperty<string>(Name(field.Attribute("name").Value), field.Element("p").Value);
			}
		}

		string Name(params string[] parts)
		{
			return string.Join("", parts.Select(p => char.ToUpper(p[0]) + p.Substring(1)));
		}

		public string GetContents()
		{
			return builder.GetContents();
		}
	}
}
