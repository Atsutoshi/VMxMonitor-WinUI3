using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VMxMonitor.controls;
using VMxMonitor.models;
using Windows.ApplicationModel.Resources;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace VMxMonitor.pages
{
	public sealed partial class ResultPage : UserControl
	{
		private const double ENLARGE_SCALE = 1.2;
		private const double REDUCE_SCALE = 0.8;

		private CommonSheet wResultSheet;

		public RoutedEventHandler Finish;
		private DataModel mDataModel;
		private CommunityModel mCommunity;
		private UserModel mUser;
		private MonitorModel mMonitor;
		private List<MonitorModel> mHistory;
		private InquiryModel mInquiry;
		private double mMaxHeight;

		public ResultPage( DataModel model, CommunityModel community, UserModel user, MonitorModel monitor )
		{
			InitializeComponent();
			LoadResources();

			mDataModel = model;
			mCommunity = community;
			mUser = user;
			mMonitor = monitor;
			mHistory = new List<MonitorModel>();
			IQueryable<MonitorModel> records = from table in mDataModel.MonitorModel
											   where table.UserId == mUser.Id && table.Mode == "VSM" && table._FinishedAt <= mMonitor._FinishedAt
											   orderby table._FinishedAt descending
											   select table;
			foreach( MonitorModel record in records )
			{
				mHistory.Add( record );
				if( mHistory.Count >= MonitorSheet.HISTORY_COUNT )
				{
					break;
				}
			}
			mInquiry = null;
			wResultSheet = new MonitorSheet();
			wScrollViewer.Content = wResultSheet;
		}

		public ResultPage( DataModel model, CommunityModel community, UserModel user, InquiryModel inquiry )
		{
			InitializeComponent();
			LoadResources();

			mDataModel = model;
			mCommunity = community;
			mUser = user;
			mMonitor = null;
			mHistory = null;
			mInquiry = inquiry;
			wResultSheet = new InquirySheet();
			wScrollViewer.Content = wResultSheet;
			wExportButton.Visibility = Visibility.Collapsed;
		}

		private void LoadResources()
		{
			var resourceLoader = ResourceLoader.GetForCurrentView();
			wReduceButton.Content = resourceLoader.GetString( "buttonReduce" );
			wEnlargeButton.Content = resourceLoader.GetString( "buttonEnlarge" );
			wPrintButton.Content = resourceLoader.GetString( "buttonPrint" );
			wExportButton.Content = resourceLoader.GetString( "buttonExport" );
			wCloseButton.Content = resourceLoader.GetString( "buttonClose" );
		}

		private void onLoadPage( object sender, RoutedEventArgs e )
		{
			if( wResultSheet != null )
			{
				wResultSheet.VerticalAlignment = VerticalAlignment.Top;
				wResultSheet.Margin = new Thickness( 25, 0, 25, 0 );
				wResultSheet.SizeChanged += onResizeSheet;
				wResultSheet.Height = wScrollViewer.ActualHeight;
			}
			wReduceButton.IsEnabled = false;
			mMaxHeight = ( wResultSheet.ActualWidth * 297.0 ) / 210.0;
		}

		private void onResizeSheet( object sender, SizeChangedEventArgs e )
		{
			if( wResultSheet.ActualWidth > 0 && wResultSheet.ActualHeight > 0 )
			{
				if( wResultSheet is MonitorSheet monitorSheet )
				{
					monitorSheet.draw( mDataModel, mCommunity, mUser, mMonitor, mHistory );
				}
				else if( wResultSheet is InquirySheet inquirySheet )
				{
					inquirySheet.draw( mDataModel, mCommunity, mUser, mInquiry );
				}
			}
		}

		private void onEnlargeButton( object sender, RoutedEventArgs e )
		{
			double height = wResultSheet.ActualHeight * ENLARGE_SCALE;
			wResultSheet.Height = Math.Min( height, mMaxHeight );
			if( height >= mMaxHeight )
			{
				wEnlargeButton.IsEnabled = false;
			}
			wReduceButton.IsEnabled = true;
		}

		private void onReduceButton( object sender, RoutedEventArgs e )
		{
			double height = wResultSheet.ActualHeight * REDUCE_SCALE;
			wResultSheet.Height = Math.Max( height, wScrollViewer.ActualHeight );
			if( height <= wScrollViewer.ActualHeight )
			{
				wReduceButton.IsEnabled = false;
			}
			wEnlargeButton.IsEnabled = true;
		}

		private async void onPrintButton( object sender, RoutedEventArgs e )
		{
			// Print functionality is currently not directly supported in WinUI 3.
			// Consider implementing custom print functionality or using a third-party library.
			var dialog = new MessageDialog( "Printing is not supported in this version of the app." );
			await dialog.ShowAsync();
		}

		private async void onExportButton( object sender, RoutedEventArgs e )
		{
			var folderPicker = new FolderPicker();
			folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
			folderPicker.FileTypeFilter.Add( "*" );

			var folder = await folderPicker.PickSingleFolderAsync();
			if( folder == null )
			{
				return;
			}

			string dir = folder.Path;
			string dstName = $"{mUser.Name}_{mMonitor.FinishedAt:yyyyMMdd_HHmmss}";
			foreach( string ext in new[] { "P", "W", "A" } )
			{
				string src = $"{mMonitor.File}_{ext}.csv";
				if( File.Exists( src ) )
				{
					File.Copy( src, Path.Combine( dir, $"{dstName}_{ext}.csv" ), true );
				}
			}
		}

		private void onCloseButton( object sender, RoutedEventArgs e )
		{
			Finish?.Invoke( this, e );
		}
	}
}
