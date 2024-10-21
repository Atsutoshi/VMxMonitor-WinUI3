using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMxMonitor.models;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Reflection;
using System.IO;
using Microsoft.UI.Xaml.Controls;

namespace VMxMonitor.pages
{
	public partial class CommunityPage : UserControl
	{
		private async void exportMonitors( DateTime? from, DateTime? to )
		{
			// select folder.
			var picker = new FolderPicker();
			picker.SuggestedStartLocation = PickerLocationId.Desktop;
			picker.FileTypeFilter.Add( "*" );

			StorageFolder folder = await picker.PickSingleFolderAsync();
			if( folder == null )
			{
				return;
			}

			// models.
			Int32 fromS = 0;
			if( from != null )
			{
				fromS = DataModel.date2unixtime( ( DateTime )from );
			}
			Int32 toS;
			if( to != null )
			{
				toS = DataModel.date2unixtime( ( DateTime )to ) + 86400;
			}
			else
			{
				toS = DataModel.date2unixtime( DateTime.Now );
			}
			List<MonitorModel> monitors = new List<MonitorModel>();
			IQueryable<MonitorModel> records = from t1 in mDataModel.MonitorModel
											   where t1.CommunityId == mCommunity.Id && t1._FinishedAt >= fromS && t1._FinishedAt <= toS
											   orderby t1._FinishedAt descending, t1.Id ascending
											   select t1;
			foreach( MonitorModel record in records )
			{
				monitors.Add( record );
			}

			// copy file.
			foreach( MonitorModel monitor in monitors )
			{
				if( monitor.Mode.Equals( "VSM" ) )
				{
					IQueryable<UserModel> users = from t2 in mDataModel.UserModel
												  where t2.Id == monitor.UserId
												  select t2;
					string name = null;
					foreach( UserModel user in users )
					{
						name = user.Name;
					}
					if( name != null )
					{
						string src;
						string csv = String.Format( "{0}_{1}", name, monitor.FinishedAt.ToLocalTime().ToString( "yyyyMMdd_HHmmss" ) );
						foreach( string ext in new string[] { "P", "W", "ECG", "PPG", "VSM" } )
						{
							src = String.Format( "{0}_{1}.csv", monitor.File, ext );
							if( File.Exists( src ) )
							{
								StorageFile srcFile = await StorageFile.GetFileFromPathAsync( src );
								await srcFile.CopyAsync( folder, String.Format( "{0}_{1}.csv", csv, ext ), NameCollisionOption.ReplaceExisting );
							}
						}
					}
				}
			}

			// write list.
			string allCSV = String.Format( "{0}_{1}_all.csv", mCommunity.Name, DateTime.Now.ToString( "yyyyMMdd_HHmmss" ) );
			StorageFile allFile = await folder.CreateFileAsync( allCSV, CreationCollisionOption.ReplaceExisting );
			using( IRandomAccessStream stream = await allFile.OpenAsync( FileAccessMode.ReadWrite ) )
			{
				using( StreamWriter writer = new StreamWriter( stream.AsStreamForWrite(), Encoding.GetEncoding( "Shift_JIS" ) ) )
				{
					writer.Write( "グループID,ユーザーID,年齢,性別,測定日時,測定時間,種別,警告/エラー," );
					writer.Write( "平均HR,最小HR,最大HR,平均RR,平均RA,LF,HF,TP,LF/HF,SDLH,CVRR,CCVTP,自律神経機能年齢,自律神経機能偏差値,総合評価\r\n" );
					foreach( MonitorModel monitor in monitors )
					{
						IQueryable<UserModel> users = from t2 in mDataModel.UserModel
													  where t2.Id == monitor.UserId
													  select t2;
						UserModel user = null;
						foreach( UserModel record in users )
						{
							user = record;
						}
						if( user != null )
						{
							writer.Write( String.Format( "\"{0}\",", mCommunity.Name ) );
							string sex = "";
							if( user.Sex == UserModel.SEX_MALE )
							{
								sex = "男性";
							}
							else if( user.Sex == UserModel.SEX_FEMALE )
							{
								sex = "女性";
							}
							string aage = "-";
							if( user.Age >= 20 && user.Age <= 70 )
							{
								aage = monitor.Age.ToString();
							}
							writer.Write( String.Format( "\"{0}\",{1},\"{2}\",", user.Name, user.Age, sex ) );
							writer.Write( String.Format( "{0},{1},{2},{3},", monitor.FinishedAt.ToLocalTime().ToString( "g" ), monitor.Length, monitor.Mode, monitor.Error ) );
							writer.Write( String.Format( "{0},{1},{2},", monitor.AvgHR, monitor.MinHR, monitor.MaxHR ) );
							writer.Write( string.Format( "{0},{1},", monitor.RR, monitor.RA ) );
							writer.Write( string.Format( "{0},{1},{2},{3},", monitor.LF, monitor.HF, monitor.TP, monitor.LH ) );
							writer.Write( string.Format( "{0},{1},{2},{3},{4},{5}\r\n", monitor.SDLH, monitor.CVRR, monitor.CCVTP, aage, monitor.Debiation, monitor.Rating ) );
						}
					}
				}
			}
		}

		private async void exportInquiries( DateTime? from, DateTime? to )
		{
			// select file
			var savePicker = new FileSavePicker();
			savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
			savePicker.FileTypeChoices.Add( "CSVファイル", new List<string>() { ".csv" } );
			savePicker.SuggestedFileName = String.Format( "{0}_{1}.csv", mCommunity.Name, DateTime.Now.ToString( "yyyyMMdd_HHmmss" ) );

			StorageFile file = await savePicker.PickSaveFileAsync();
			if( file == null )
			{
				return;
			}

			// write list
			using( IRandomAccessStream stream = await file.OpenAsync( FileAccessMode.ReadWrite ) )
			{
				using( StreamWriter writer = new StreamWriter( stream.AsStreamForWrite(), Encoding.GetEncoding( "Shift_JIS" ) ) )
				{
					writer.Write( "グループID,ユーザーID,年齢,性別,測定日時" );
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
						toS = DataModel.date2unixtime( ( DateTime )to ) + 86400;
					}
					else
					{
						toS = DataModel.date2unixtime( DateTime.Now );
					}
					var records = from t1 in mDataModel.InquiryModel
								  where t1.CommunityId == mCommunity.Id && t1._AnsweredAt >= fromS && t1._AnsweredAt <= toS
								  orderby t1._AnsweredAt descending
								  select t1;
					foreach( var inquiry in records )
					{
						var users = from t2 in mDataModel.UserModel
									where t2.Id == inquiry.UserId
									select t2;
						var user = users.FirstOrDefault();
						writer.Write( String.Format( "\"{0}\",", mCommunity.Name ) );
						if( user != null )
						{
							string sex = "";
							if( user.Sex == UserModel.SEX_MALE )
							{
								sex = "男性";
							}
							else if( user.Sex == UserModel.SEX_FEMALE )
							{
								sex = "女性";
							}
							writer.Write( String.Format( "\"{0}\",{1},\"{2}\",", user.Name, user.Age, sex ) );
						}
						else
						{
							writer.Write( ",,," );
						}
						writer.Write( String.Format( "{0}", inquiry.AnsweredAt.ToLocalTime().ToString( "g" ) ) );
						for( int i = 1; i <= 42; ++i )
						{
							PropertyInfo info = typeof( InquiryModel ).GetProperty( String.Format( "Answer{0:D02}", i ) );
							if( info != null )
							{
								writer.Write( String.Format( ",{0}", info.GetValue( inquiry ) ) );
							}
						}
						writer.Write( String.Format( ",{0},{1},{2}", inquiry.Answer43, inquiry.Answer44, inquiry.Answer45 ) );
						for( int i = 46; i <= 48; ++i )
						{
							PropertyInfo info = typeof( InquiryModel ).GetProperty( String.Format( "Answer{0:D02}", i ) );
							if( info != null )
							{
								writer.Write( String.Format( ",{0}", info.GetValue( inquiry ).ToString().Replace( ",", "、" ) ) );
							}
						}
						for( int i = 1; i <= 3; ++i )
						{
							PropertyInfo info = typeof( InquiryModel ).GetProperty( String.Format( "Result{0:D02}", i ) );
							if( info != null )
							{
								writer.Write( String.Format( ",{0}", info.GetValue( inquiry ) ) );
							}
						}
						for( int i = 11; i <= 17; ++i )
						{
							PropertyInfo info = typeof( InquiryModel ).GetProperty( String.Format( "Result{0:D02}", i ) );
							if( info != null )
							{
								writer.Write( String.Format( ",{0}", info.GetValue( inquiry ) ) );
							}
						}
						writer.Write( "\r\n" );
					}
				}
			}
		}
	}
}
