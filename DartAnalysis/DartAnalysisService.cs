using System.IO;
using System.Reflection;

namespace DartVS.DartAnalysis
{
    public partial class DartAnalysisService
    {
		readonly DirectoryInfo SdkLocation;
		readonly FileInfo DartVmLocation;
		static readonly FileInfo AnalysisServerSnapshotLocation;
		const string AnalysisServerFilename = "analysis_server.dart.snapshot";

		#region Static setup

		static DartAnalysisService()
		{
			AnalysisServerSnapshotLocation = WriteAnalysisServerSnapshot();
		}

		static FileInfo WriteAnalysisServerSnapshot()
		{
			var asm = Assembly.GetExecutingAssembly();
			var snapshotStream = asm.GetManifestResourceStream(string.Format("{0}.{1}", typeof(DartAnalysisService).Namespace, AnalysisServerFilename));

			var tempFolder = Path.Combine(Path.GetTempPath(), "DartAnalysis.NET", Path.GetRandomFileName());
			var file = Path.Combine(tempFolder, AnalysisServerFilename);

			Directory.CreateDirectory(tempFolder);
			using (var fileStream = File.Create(file))
				snapshotStream.CopyTo(fileStream);

			return new FileInfo(file);
		}

		#endregion

		public DartAnalysisService(DirectoryInfo sdkLocation)
		{
			this.SdkLocation = sdkLocation;
			this.DartVmLocation = new FileInfo(Path.Combine(sdkLocation.FullName, @"bin\dart.exe"));
		}
	}
}
