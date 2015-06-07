using System;
using System.Linq;
using System.IO;
using System.Xml.Linq;

namespace DartVS.DartAnalysis.JsonBuilder
{
	class ApiClassVisitor
	{
		ApiClassBuilder classBuilder = new ApiClassBuilder();

		public ApiClassVisitor(FileInfo apiDoc)
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
			classBuilder.BuildRequestClass(element);
			classBuilder.BuildResponseClass(element);			
		}		

		public string GetContents()
		{
			return classBuilder.GetContents();
		}
	}
}
