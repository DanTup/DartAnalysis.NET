using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DartAnalysis.Tests
{
    public class ServerTests
    {
		[Fact]
		public async Task ServerVersion()
		{
			var service = new DartAnalysisService(new DirectoryInfo(@"M:\Apps\dart-sdk"));
			var version = await service.GetVersion();
			Assert.Equal(new Version("1.2.3.4"), version);
		}
    }
}
