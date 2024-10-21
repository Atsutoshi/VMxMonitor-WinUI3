using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM_app_221_WAS.services;
using VMxMonitor.dialogs;
using VMxMonitor.models;

namespace VMxMonitor.views.pages
{
	public sealed partial class MonitorPage : UserControl
	{
		/* =====[ constants ]===== */

		private const int ASYNC_SLEEP1 = 330;
		private const int ASYNC_SLEEP2 = 250;

		/* =====[ private methods ]===== */

		private async void analyze()
		{
			// directory and file.
			DateTime now = DateTime.Now;
			string dirname = ( now.Year * 10000 + now.Month * 100 + now.Day ).ToString();
			string root = Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData );
			string dir = System.IO.Path.Combine( new string[] { root, "FMCC", "VMxMonitor1", "data", dirname } );
			if( !System.IO.Directory.Exists( dir ) )
			{
				System.IO.Directory.CreateDirectory( dir );
			}
			String name = DateTime.Now.Ticks.ToString();
			string fileW = System.IO.Path.Combine( dir, name + "_W.csv" );
			string fileP = System.IO.Path.Combine( dir, name + "_P.csv" );

			// wave.
			using( System.IO.StreamWriter output = new System.IO.StreamWriter( fileW ) )
			{
				output.WriteLine( "ECG,PPG,APG" );
				foreach( WaveModel wave in mWaves )
				{
					output.WriteLine( wave.ECG + "," + wave.PPG + "," + wave.APG );
				}
			}

			// peak.
			using( System.IO.StreamWriter output = new System.IO.StreamWriter( fileP ) )
			{
				String dateS = String.Format( "{0:D4}/{1:D2}/{2:D2}", now.Year, now.Month, now.Day );
				String timeS = String.Format( "{0:D2}:{1:D2}:{2:D2}", now.Hour, now.Minute, now.Second );
				output.Write( "{0} {1},", dateS, timeS );
				output.Write( "{0},{1},", mLength, mUser.Age );
				if( mUser.Sex == UserModel.SEX_MALE )
				{
					output.WriteLine( "M" );
				}
				else if( mUser.Sex == UserModel.SEX_FEMALE )
				{
					output.WriteLine( "F" );
				}
				else
				{
					output.WriteLine( "-" );
				}
				output.WriteLine( "RR,AA,RA,RT,AT" );
				foreach( PeakModel peak in PeakModel.buildList( mUnits ) )
				{
					output.WriteLine( peak.RR + "," + peak.AA + "," + peak.RA + "," + peak.RT + "," + peak.AT );
				}
			}

			await Task.Delay( ASYNC_SLEEP1 );

			// analyze.
			string[] args = Environment.GetCommandLineArgs();
			string app = System.IO.Path.GetDirectoryName( args[0] );
			string exe;
			if( IntPtr.Size == 8 )
			{
				exe = System.IO.Path.Combine( new string[] { app, "x64", "Memfmcc.exe" } );
			}
			else
			{
				exe = System.IO.Path.Combine( new string[] { app, "x86", "Memfmcc.exe" } );
			}
			string basename = System.IO.Path.Combine( dir, name );
			string arguments = String.Format( "-len 30 {0}", basename );
			foreach( string arg in Environment.GetCommandLineArgs() )
			{
				if( arg == "--once" )
				{
					arguments = basename;
				}
			}
			Process proc = new Process();
			proc.StartInfo.FileName = exe;
			proc.StartInfo.Arguments = arguments;
			proc.StartInfo.CreateNoWindow = true;
			proc.StartInfo.UseShellExecute = false;
			proc.Start();
			proc.WaitForExit();

			string error = null;
			if( proc.ExitCode == 3 )
			{
				error = resourceLoader.GetString( "errorAnalyzerNoData" );
			}
			else if( proc.ExitCode == 91 )
			{
				error = resourceLoader.GetString( "errorAnalyzerAuthentication" );
			}
			else if( proc.ExitCode == 92 )
			{
				error = resourceLoader.GetString( "errorAnalyzerExecution" );
			}
			else if( proc.ExitCode == 93 )
			{
				error = resourceLoader.GetString( "errorAnalyzerReading" );
			}
			else if( proc.ExitCode == 94 )
			{
				error = resourceLoader.GetString( "errorAnalyzerWriteing" );
			}
			else if( proc.ExitCode == 99 )
			{
				error = resourceLoader.GetString( "errorAnalyzerAnalyzing" );
			}
			if( error != null )
			{
				// error.
				AlertDialog.ShowAsync( error );
				if( Cancel != null )
				{
					Cancel( this, null );
				}
				return;
			}

			// read result.
			using( System.IO.StreamReader reader = new System.IO.StreamReader( basename + "_A.csv" ) )
			{
				SequenceModel sequence = mDataModel.getSequence( MonitorModel.TABLE_NAME );
				MonitorModel monitorVSM = null;
				string header = reader.ReadLine();
				string line;
				char[] delimiters = { ',' };
				while( ( line = reader.ReadLine() ) != null )
				{
					// DATETIME,LENGTH,AGE,SEX,TYPE,ERROR,AVGHR,MINHR,MAXHR,RR,RA,LF,HF,TP,LF/HF,SDLH,CVRR,CCVTP,A-AGE,D-SCORE
					string[] cells = line.Trim().Split( delimiters );
					if( cells.Length >= 20 )
					{
						// save into database.
						MonitorModel monitor = new MonitorModel();
						monitor.Id = sequence.Id;
						monitor.CommunityId = mUser.CommunityId;
						monitor.UserId = mUser.Id;
						monitor.File = basename;
						monitor.Length = int.Parse( cells[1] );
						monitor.Mode = cells[4];
						monitor.Error = int.Parse( cells[5] );
						monitor.AvgHR = int.Parse( cells[6] );
						monitor.MinHR = int.Parse( cells[7] );
						monitor.MaxHR = int.Parse( cells[8] );
						monitor.RR = double.Parse( cells[9] );
						monitor.RA = double.Parse( cells[10] );
						monitor.LF = double.Parse( cells[11] );
						monitor.HF = double.Parse( cells[12] );
						monitor.TP = double.Parse( cells[13] );
						monitor.LH = double.Parse( cells[14] );
						monitor.SDLH = double.Parse( cells[15] );
						monitor.CVRR = double.Parse( cells[16] );
						monitor.CCVTP = double.Parse( cells[17] );
						monitor.Age = int.Parse( cells[18] );
						monitor.Debiation = int.Parse( cells[19] );
						monitor.FinishedAt = now;
						mDataModel.MonitorModel.Add( monitor );  // 修正箇所
						++sequence.Id;
						//
						if( monitor.Mode.Equals( "VSM" ) )
						{
							monitorVSM = monitor;
						}
					}
				}
				reader.Close();
				//
				mDataModel.SaveChanges();  // 修正箇所

				// finish.
				await Task.Delay( ASYNC_SLEEP2 );
				if( Finish != null && monitorVSM != null )
				{
					Finish( this, new MonitorEventArgs( monitorVSM ) );
					return;
				}
			}

			// error.
			AlertDialog.ShowAsync( resourceLoader.GetString( "errorAnalysisFileFailed" ) );
			if( Cancel != null )
			{
				Cancel( this, null );
			}
		}
	}
}
