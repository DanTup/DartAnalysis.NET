using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartAnalysis
{
    public class DartAnalysisService
    {
		readonly DirectoryInfo SdkLocation;
		readonly FileInfo DartVmLocation;
		readonly FileInfo AnalysisServerSnapshotLocation;

		public DartAnalysisService(DirectoryInfo sdkLocation)
		{
			this.SdkLocation = sdkLocation;
			this.DartVmLocation = new FileInfo(Path.Combine(sdkLocation.FullName, @"bin\dart.exe"));
			this.AnalysisServerSnapshotLocation = null;
		}

		public Task<Version> GetVersion()
		{
			throw new NotImplementedException();
		}
    }
}
