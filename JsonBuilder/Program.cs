using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DartVS.DartAnalysis.JsonBuilder
{
	class Program
	{
		const string ApiDoc = @"..\..\..\AnalysisServer\spec_input.html";
		const string OutputJsonFile = @"..\..\..\DartAnalysis\Json.cs";
		const string OutputRequestFile = @"..\..\..\DartAnalysis\Requests.cs";

		static void Main(string[] args)
		{
			var builder = new ApiClassVisitor(new FileInfo(ApiDoc));

			var output = builder.GetContents();
			Debugger.Break();
		}
	}
}
