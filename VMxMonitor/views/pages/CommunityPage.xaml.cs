using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using VM_app_221_WAS.services;
using VMxMonitor.dialogs;
using VMxMonitor.models;

namespace VMxMonitor.pages
{
	public partial class CommunityPage : UserControl
	{
		/* =====[ properties ]===== */
		public EventHandler<UserEventArgs> SelectUser;
		public RoutedEventHandler Cancel;
		//
		private DataModel mDataModel;
		private CommunityModel mCommunity;
		private List<UserModel> mUsers;
		private bool mIsIME;

		/* =====[ constructors ]===== */
		public CommunityPage()
		{
			this.InitializeComponent();
		}

		public CommunityPage( DataModel model, CommunityModel community ) : this()
		{
			// initialize properties.
			mDataModel = model;
			mCommunity = community;
			mUsers = new List<UserModel>();
			mIsIME = false;
		}

		/* =====[ event listeners ]===== */
		private void onLoad( object sender, RoutedEventArgs e )
		{
			// set up events.
			TextBox wSearchText = FindName( "wSearchText" ) as TextBox;
			if( wSearchText != null )
			{
				wSearchText.TextChanged += onSearchText;
				wSearchText.PreviewKeyDown += onPreviewKeyDown;
			}

			// initialize.
			onAllButton( null, null );
		}

		private void onSearchText( object sender, TextChangedEventArgs e )
		{
			if( !mIsIME )
			{
				// reset search button state.
				Button wSearchButton = FindName( "wSearchButton" ) as Button;
				if( wSearchButton != null )
				{
					TextBox wSearchText = sender as TextBox;
					wSearchButton.IsEnabled = wSearchText.Text.Length > 0;
				}
			}
		}

		private void onSearchButton( object sender, RoutedEventArgs e )
		{
			// search users.
			mUsers.Clear();
			IQueryable<UserModel> records;
			TextBox wSearchText = FindName( "wSearchText" ) as TextBox;
			if( wSearchText != null && wSearchText.Text.Length > 0 )
			{
				records = from user in mDataModel.UserModel
						  where user.CommunityId == mCommunity.Id && user.Name.Contains( wSearchText.Text )
						  select user;
			}
			else
			{
				records = from user in mDataModel.UserModel
						  where user.CommunityId == mCommunity.Id
						  select user;
			}

			foreach( UserModel record in records )
			{
				mUsers.Add( record );
			}

			ListView wUserList = FindName( "wUserList" ) as ListView;
			if( wUserList != null )
			{
				wUserList.Items.Clear();
				foreach( UserModel user in mUsers )
				{
					wUserList.Items.Add( user );
				}
			}

			// select.
			onSelectUser( null, null );
		}

		private void onAllButton( object sender, RoutedEventArgs e )
		{
			// clear search text.
			TextBox wSearchText = FindName( "wSearchText" ) as TextBox;
			if( wSearchText != null )
			{
				wSearchText.Text = "";
			}

			Button wSearchButton = FindName( "wSearchButton" ) as Button;
			if( wSearchButton != null )
			{
				wSearchButton.IsEnabled = false;
			}

			// search.
			onSearchButton( null, null );
		}

		private void onSelectUser( object sender, SelectionChangedEventArgs e )
		{
			// change enter button state.
			ListView wUserList = FindName( "wUserList" ) as ListView;
			Button wModifyButton = FindName( "wModifyButton" ) as Button;
			Button wEnterButton = FindName( "wEnterButton" ) as Button;

			if( wUserList != null && wModifyButton != null && wEnterButton != null )
			{
				bool isEnabled = wUserList.SelectedIndex >= 0;
				wModifyButton.IsEnabled = isEnabled;
				wEnterButton.IsEnabled = isEnabled;
			}
		}

		private void onDoubleClickUser( object sender, DoubleTappedRoutedEventArgs e )
		{
			// user.
			ListView wUserList = FindName( "wUserList" ) as ListView;
			if( wUserList != null && wUserList.SelectedIndex >= 0 && wUserList.SelectedIndex < mUsers.Count() )
			{
				SelectUser?.Invoke( this, new UserEventArgs( mUsers[wUserList.SelectedIndex] ) );
			}
		}

		private async void onNewButton( object sender, RoutedEventArgs e )
		{
			// try to register new user.
			UserDialog dialog = new UserDialog( mDataModel, mCommunity, null );
			var result = await dialog.ShowAsync();
			if( result == ContentDialogResult.Primary )
			{
				// reload user list.
				onAllButton( null, null );
			}
		}

		private async void onModifyButton( object sender, RoutedEventArgs e )
		{
			// try to modify user. au
			ListView wUserList = FindName( "wUserList" ) as ListView;
			if( wUserList != null && wUserList.SelectedIndex >= 0 && wUserList.SelectedIndex < mUsers.Count() )
			{
				UserDialog dialog = new UserDialog( mDataModel, mCommunity, mUsers[wUserList.SelectedIndex] );
				var result = await dialog.ShowAsync();
				if( result == ContentDialogResult.Primary )
				{
					// reload user list.
					onAllButton( null, null );
				}
			}
		}

		private async void onExportButton( object sender, RoutedEventArgs e )
		{
			// try to export data.
			DataExportDialog dialog = new DataExportDialog( mDataModel, mCommunity );
			var result = await dialog.ShowAsync();
			if( result == ContentDialogResult.Primary )
			{
				DateTime? fromDate = dialog.FromDate?.DateTime;
				DateTime? toDate = dialog.ToDate?.DateTime;

				if( dialog.Type == DataExportDialog.EXPORT_MONITOR )
				{
					exportMonitors( fromDate, toDate );
				}
				else if( dialog.Type == DataExportDialog.EXPORT_INQUIRY )
				{
					exportInquiries( fromDate, toDate );
				}
			}
		}

		private void onEnterButton( object sender, RoutedEventArgs e )
		{
			// user.
			ListView wUserList = FindName( "wUserList" ) as ListView;
			if( wUserList != null && wUserList.SelectedIndex >= 0 && wUserList.SelectedIndex < mUsers.Count() )
			{
				SelectUser?.Invoke( this, new UserEventArgs( mUsers[wUserList.SelectedIndex] ) );
			}
		}

		private void onCancelButton( object sender, RoutedEventArgs e )
		{
			// cancel.
			Cancel?.Invoke( this, e );
		}

		private void onPreviewKeyDown( object sender, KeyRoutedEventArgs e )
		{
			mIsIME = e.Key == Windows.System.VirtualKey.Convert || e.Key == Windows.System.VirtualKey.NonConvert;
		}
	}
}
