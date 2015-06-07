using DartVS.DartAnalysis.JsonBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DartVS.DartAnalysis.JsonBuilder.Tests
{
	public class CSharpFileBuilderTests
	{
		[Fact]
		public void SimpleTest()
		{
			var builder = new CSharpFileBuilder("Danny.Test.Namespace");
			builder.AddClass("DannyClass", "This is the DannyClass class.").AddProperty<string>("Name", "This is the name property.");
			var contents = builder.GetContents();

			var expected = @"namespace Danny.Test.Namespace
{
	/// <summary>
	/// This is the DannyClass class.
	/// </summary>
	public class DannyClass
	{
		/// <summary>
		/// This is the name property.
		/// </summary>
		public string Name { get; set; }
	}
}";

			Assert.Equal(expected, contents);
		}
	}
}
