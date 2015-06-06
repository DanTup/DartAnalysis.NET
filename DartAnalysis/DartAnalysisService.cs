using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace DartVS.DartAnalysis
{
    public partial class DartAnalysisService : IDisposable
    {
		readonly DirectoryInfo SdkLocation;
		readonly FileInfo DartVmLocation;
		readonly DirectoryInfo TempFolder;
		readonly FileInfo AnalysisServerSnapshotLocation;
		const string AnalysisServerFilename = "analysis_server.dart.snapshot";
		
		public DartAnalysisService(DirectoryInfo sdkLocation)
		{
			this.SdkLocation = sdkLocation;
			this.DartVmLocation = new FileInfo(Path.Combine(sdkLocation.FullName, @"bin\dart.exe"));

			// Create a temp folder to store the Analysis Server snapshot.
			TempFolder = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "DartAnalysis.NET", Path.GetRandomFileName()));
			Directory.CreateDirectory(TempFolder.FullName);

			// Write the Analysis Server snapshot from the embedded resource.
			AnalysisServerSnapshotLocation = new FileInfo(Path.Combine(TempFolder.FullName, AnalysisServerFilename));
			WriteAnalysisServerSnapshot(AnalysisServerSnapshotLocation);
		}

		static void WriteAnalysisServerSnapshot(FileInfo file)
		{
			var asm = Assembly.GetExecutingAssembly();
			var snapshotStream = asm.GetManifestResourceStream(string.Format("{0}.{1}", typeof(DartAnalysisService).Namespace, AnalysisServerFilename));

			using (var fileStream = File.Create(file.FullName))
				snapshotStream.CopyTo(fileStream);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			//if (disposing)
			//{
			//}
			try
			{
				TempFolder.Delete(true);
			}
			catch { }
		}
	}
}
