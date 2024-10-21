using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VMxMonitor.models;
using System.IO;
using System;

namespace VMxMonitor.pages
{
    public partial class EntrancePage : UserControl
	{
		/* =====[ private methods ]===== */

		private async void exportMonitors( UserModel user, DateTime? from, DateTime? to )
		{
			// select folder
			var picker = new FolderPicker();
			picker.SuggestedStartLocation = PickerLocationId.Desktop;
			picker.FileTypeFilter.Add( "*" );

			var hwnd = WinRT.Interop.WindowNative.GetWindowHandle( this );
			WinRT.Interop.InitializeWithWindow.Initialize( picker, hwnd );

			StorageFolder folder = await picker.PickSingleFolderAsync();
			if( folder == null )
			{
				return;
			}

			string dir = folder.Path;

			// models
			Int32 fromS = 0;
			if( from != null )
			{
				fromS = DataModel.date2unixtime( ( DateTime )from );
			}
			Int32 toS;
			if( to != null )
			{
				toS = DataModel.date2unixtime( ( DateTime )to );
			}
			else
			{
				toS = DataModel.date2unixtime( DateTime.Now ) + 86400;
			}
			List<MonitorModel> monitors = new List<MonitorModel>();
			IQueryable<MonitorModel> records = from t1 in mDataModel.MonitorModel
											   where t1.UserId == user.Id && t1._FinishedAt >= fromS && t1._FinishedAt <= toS
											   orderby t1._FinishedAt descending, t1.Id ascending
											   select t1;
			foreach( MonitorModel record in records )
			{
				monitors.Add( record );
			}

			// copy file
			foreach( MonitorModel monitor in monitors )
			{
				if( monitor.Mode.Equals( "VSM" ) )
				{
					string src;
					string csv = String.Format( "{0}_{1}", user.Name, monitor.FinishedAt.ToLocalTime().ToString( "yyyyMMdd_HHmmss" ) );
					foreach( string ext in new string[] { "P", "W", "ECG", "PPG", "VSM" } )
					{
						src = String.Format( "{0}_{1}.csv", monitor.File, ext );
						if( File.Exists( src ) )
						{
							File.Copy( src, Path.Combine( dir, String.Format( "{0}_{1}.csv", csv, ext ) ), true );
						}
					}
				}
			}

			// write list
			string allCSV = String.Format( "{0}_{1}_all.csv", user.Name, DateTime.Now.ToString( "yyyyMMdd_HHmmss" ) );
			string allPath = Path.Combine( dir, allCSV );
			using( StreamWriter writer = new StreamWriter( allPath, true, Encoding.GetEncoding( "Shift_JIS" ) ) )
			{
				writer.Write( "ユーザーID,年齢,性別,測定日時,測定時間,種別,警告/エラー," );
				writer.Write( "平均HR,最小HR,最大HR,平均RR,平均RA,LF,HF,TP,LF/HF,SDLH,CVRR,CCVTP,自律神経機能年齢,自律神経機能偏差値,総合評価\r\n" );
				foreach( MonitorModel monitor in monitors )
				{
					string sex = user.Sex == UserModel.SEX_MALE ? "男性" : "女性";
					string aage = ( user.Age >= 20 && user.Age <= 70 ) ? monitor.Age.ToString() : "-";
					writer.Write( String.Format( "\"{0}\",{1},\"{2}\",", user.Name, user.Age, sex ) );
					writer.Write( String.Format( "{0},{1},{2},{3},", monitor.FinishedAt.ToLocalTime().ToString( "g" ), monitor.Length, monitor.Mode, monitor.Error ) );
					writer.Write( String.Format( "{0},{1},{2},", monitor.AvgHR, monitor.MinHR, monitor.MaxHR ) );
					writer.Write( string.Format( "{0},{1},", monitor.RR, monitor.RA ) );
					writer.Write( string.Format( "{0},{1},{2},{3},", monitor.LF, monitor.HF, monitor.TP, monitor.LH ) );
					writer.Write( string.Format( "{0},{1},{2},{3},{4},{5}\r\n", monitor.SDLH, monitor.CVRR, monitor.CCVTP, aage, monitor.Debiation, monitor.Rating ) );
				}
			}
		}

		private async void exportInquiries( UserModel user, DateTime? from, DateTime? to )
		{
			// select file
			var picker = new FileSavePicker();
			picker.SuggestedStartLocation = PickerLocationId.Desktop;
			picker.FileTypeChoices.Add( "CSVファイル", new List<string>() { ".csv" } );
			picker.SuggestedFileName = String.Format( "{0}_{1}.csv", user.Name, DateTime.Now.ToString( "yyyyMMdd_HHmmss" ) );

			var hwnd = WinRT.Interop.WindowNative.GetWindowHandle( this );
			WinRT.Interop.InitializeWithWindow.Initialize( picker, hwnd );

			StorageFile file = await picker.PickSaveFileAsync();
			if( file == null )
			{
				return;
			}

			// write list
			using( StreamWriter writer = new StreamWriter( await file.OpenStreamForWriteAsync(), Encoding.GetEncoding( "Shift_JIS" ) ) )
			{
				writer.Write( "ユーザーID,年齢,性別,測定日時" );
				for( int i = 1; i <= 48; ++i )
				{
					writer.Write( ",Q{0:D02}", i );
				}
				for( int i = 1; i <= 3; ++i )
				{
					writer.Write( ",R{0:D02}", i );
				}
				for( int i = 11; i <= 17; ++i )
				{
					writer.Write( ",R{0:D02}", i );
				}
				writer.Write( "\r\n" );

				// inquiries
				Int32 fromS = 0;
				if( from != null )
				{
					fromS = DataModel.date2unixtime( ( DateTime )from );
				}
				Int32 toS;
				if( to != null )
				{
					toS = DataModel.date2unixtime( ( DateTime )to );
				}
				else
				{
					toS = DataModel.date2unixtime( DateTime.Now ) + 86400;
				}
				IQueryable<InquiryModel> inquiries = from t1 in mDataModel.InquiryModel
													 where t1.UserId == user.Id && t1._AnsweredAt >= fromS && t1._AnsweredAt <= toS
													 orderby t1._AnsweredAt descending
													 select t1;
				foreach( InquiryModel inquiry in inquiries )
				{
					string sex = user.Sex == UserModel.SEX_MALE ? "男性" : "女性";
					writer.Write( String.Format( "\"{0}\",{1},\"{2}\",", user.Name, user.Age, sex ) );
					writer.Write( String.Format( "{0}", inquiry.AnsweredAt.ToLocalTime().ToString( "g" ) ) );

					for( int i = 1; i <= 42; ++i )
					{
						PropertyInfo info = ( typeof( InquiryModel ) ).GetProperty( String.Format( "Answer{0:D02}", i ) );
						int value;
						if( int.TryParse( info.GetValue( inquiry ).ToString(), out value ) )
						{
							writer.Write( String.Format( ",{0}", value ) );
						}
					}

					writer.Write( String.Format( ",{0},{1},{2}", inquiry.Answer43, inquiry.Answer44, inquiry.Answer45 ) );

					for( int i = 46; i <= 48; ++i )
					{
						PropertyInfo info = ( typeof( InquiryModel ) ).GetProperty( String.Format( "Answer{0:D02}", i ) );
						writer.Write( String.Format( ",{0}", info.GetValue( inquiry ).ToString().Replace( ",", "、" ) ) );
					}

					for( int i = 1; i <= 3; ++i )
					{
						PropertyInfo info = ( typeof( InquiryModel ) ).GetProperty( String.Format( "Result{0:D02}", i ) );
						double value;
						if( double.TryParse( info.GetValue( inquiry ).ToString(), out value ) )
						{
							writer.Write( String.Format( ",{0}", value ) );
						}
					}

					for( int i = 11; i <= 17; ++i )
					{
						PropertyInfo info = ( typeof( InquiryModel ) ).GetProperty( String.Format( "Result{0:D02}", i ) );
						double value;
						if( double.TryParse( info.GetValue( inquiry ).ToString(), out value ) )
						{
							writer.Write( String.Format( ",{0}", value ) );
						}
					}
					writer.Write( "\r\n" );
				}
			}
		}


	}
}
