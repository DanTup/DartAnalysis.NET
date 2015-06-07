using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DartVS.DartAnalysis.JsonBuilder
{
	class ApiClassBuilder
	{
		CSharpFileBuilder builder = new CSharpFileBuilder("DartVS.DartAnalysis");

		public void BuildRequestClass(XElement requestElement)
		{
			var domainName = requestElement.Ancestors("domain").First().Attribute("name").Value;
			var methodName = requestElement.Attribute("method").Value;
			var doc = requestElement.Element("p").Value;

			var requestClass = builder.AddClass(Name(domainName, methodName, "Request"), doc);
			if (requestElement.Element("params") != null)
			{
				foreach (var field in requestElement.Elements("field"))
					// TODO: Fix type
					requestClass.AddProperty<string>(Name(field.Attribute("name").Value), field.Element("p").Value);
			}
		}
		
		public void BuildResponseClass(XElement requestElement)
		{
			// If we don't return a result, we don't need to build a class.
			if (requestElement.Element("result") == null)
				return;

			var domainName = requestElement.Ancestors("domain").First().Attribute("name").Value;
			var methodName = requestElement.Attribute("method").Value;
			var doc = requestElement.Element("p").Value;

			var responseClass = builder.AddClass(Name(domainName, methodName, "Response"), doc);
			foreach (var field in requestElement.Element("result").Elements("field"))
				// TODO: Fix type
				responseClass.AddProperty<string>(Name(field.Attribute("name").Value), field.Element("p").Value);
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
