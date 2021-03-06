﻿using System;
using System.IO;
using System.Threading.Tasks;
using DartVS.DartAnalysis;
using Xunit;

namespace DartVS.DartAnalysis.Tests
{
    public class ServerTests
    {
		[Fact]
		public async Task ServerVersion()
		{
			using (var service = new DartAnalysisService(new DirectoryInfo(@"M:\Apps\dart-sdk")))
			{
				var version = await service.GetVersion();
				Assert.Equal(new Version("1.2.3.4"), version);
			}
		}
    }
}
