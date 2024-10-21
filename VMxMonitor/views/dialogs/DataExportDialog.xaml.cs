using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using VMxMonitor.models;
using Windows.ApplicationModel.Resources;

namespace VMxMonitor.dialogs
{
	public sealed partial class DataExportDialog : ContentDialog
	{
		public const int EXPORT_MONITOR = 1;
		public const int EXPORT_INQUIRY = 2;

		public int Type { get; private set; }
		public DateTimeOffset? FromDate { get; private set; }
		public DateTimeOffset? ToDate { get; private set; }

		private DataModel mDataModel;
		private CommunityModel mCommunity;
		private UserModel mUser;
		private ResourceLoader resourceLoader;

		public DataExportDialog( DataModel model, CommunityModel community )
		{
			this.InitializeComponent();
			mDataModel = model;
			mCommunity = community;
			mUser = null;
			resourceLoader = ResourceLoader.GetForCurrentView();
		}

		public DataExportDialog( DataModel model, UserModel user )
		{
			this.InitializeComponent();
			mDataModel = model;
			mCommunity = null;
			mUser = user;
			resourceLoader = ResourceLoader.GetForCurrentView();
		}

		private void OnLoad( object sender, RoutedEventArgs e )
		{
			Title = resourceLoader.GetString( "titleExport" );
			PrimaryButtonText = resourceLoader.GetString( "buttonOk" );
			SecondaryButtonText = resourceLoader.GetString( "buttonCancel" );

			if( mCommunity != null )
			{
				wPromptText.Text = string.Format( resourceLoader.GetString( "promptCommunityData" ), mCommunity.Name );
			}
			else if( mUser != null )
			{
				wPromptText.Text = string.Format( resourceLoader.GetString( "promptUserData" ), mUser.Name );
			}

			wLabelMeasuredOn.Text = resourceLoader.GetString( "labelMeasuredOn" );
			wLabelDataType.Text = resourceLoader.GetString( "labelDataType" );
			wMonitorRadio.Content = resourceLoader.GetString( "labelDataMonitor" );
			wInquiryRadio.Content = resourceLoader.GetString( "labelDataInquiry" );

			wMonitorRadio.IsChecked = true;
		}

		private void OnPrimaryButtonClick( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			// get values.
			if( ( bool )wMonitorRadio.IsChecked )
			{
				Type = EXPORT_MONITOR;
			}
			else if( ( bool )wInquiryRadio.IsChecked )
			{
				Type = EXPORT_INQUIRY;
			}
			FromDate = wFromDate.Date;
			ToDate = wToDate.Date;

			// set result.
			Hide();
		}

		private void OnSecondaryButtonClick( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			// set result.
			Hide();
		}

		public static async Task<ContentDialogResult> ShowAsync( DataModel model, CommunityModel community )
		{
			var dialog = new DataExportDialog( model, community );
			return await dialog.ShowAsync();
		}

		public static async Task<ContentDialogResult> ShowAsync( DataModel model, UserModel user )
		{
			var dialog = new DataExportDialog( model, user );
			return await dialog.ShowAsync();
		}
	}
}
