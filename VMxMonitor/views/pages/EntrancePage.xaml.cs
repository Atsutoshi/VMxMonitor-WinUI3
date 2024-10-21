using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using VM_app_221_WAS.services;
using VMxMonitor.dialogs;
using VMxMonitor.models;

namespace VMxMonitor.pages
{
	public sealed partial class EntrancePage : UserControl
	{
		public const int USER_TAB = 1;
		public const int COMMUNITY_TAB = 0;

		public event EventHandler<UserEventArgs> SelectUser;
		public event EventHandler<CommunityEventArgs> SelectCommunity;
		public event RoutedEventHandler Quit;

		private DataModel mDataModel;
		private List<UserModel> mUsers;
		private List<CommunityModel> mCommunities;
		private int mInitialTab;
		private bool mIsIME;

		public EntrancePage( DataModel model, int tab )
		{
			this.InitializeComponent();

			mDataModel = model;
			mUsers = new List<UserModel>();
			mCommunities = new List<CommunityModel>();
			mInitialTab = tab;
			mIsIME = false;

			LoadResources();
		}

		private void LoadResources()
		{
			var resourceLoader = new ResourceLoader();
			wSearchButton.Content = resourceLoader.GetString( "buttonSearch" );
			wAllButton.Content = resourceLoader.GetString( "buttonListAll" );
			wCommunityTab.Header = resourceLoader.GetString( "labelCommunity" );
			wUserTab.Header = resourceLoader.GetString( "labelUser" );
			wNewButton.Content = resourceLoader.GetString( "buttonNewUser" );
			wModifyButton.Content = "ï“èW";
			wExportButton.Content = resourceLoader.GetString( "buttonExport" );
			wEnterButton.Content = resourceLoader.GetString( "buttonEnter" );
			wQuitButton.Content = resourceLoader.GetString( "buttonQuit" );
		}

		private void OnLoad( object sender, RoutedEventArgs e )
		{
			wListTab.SelectedIndex = mInitialTab;
			OnAllButton( null, null );
		}

		private void OnSearchText( object sender, TextChangedEventArgs e )
		{
			if( !mIsIME )
			{
				wSearchButton.IsEnabled = wSearchText.Text.Length > 0;
			}
		}

		private void OnSearchButton( object sender, RoutedEventArgs e )
		{
			if( wListTab.SelectedIndex == USER_TAB )
			{
				SearchUsers();
			}
			else if( wListTab.SelectedIndex == COMMUNITY_TAB )
			{
				SearchCommunities();
			}
		}

		private void SearchUsers()
		{
			mUsers.Clear();
			if( wSearchText.Text.Length > 0 )
			{
				var records = from user in mDataModel.UserModel
							  where user.CommunityId == 0 && user.Name.Contains( wSearchText.Text )
							  select user;
				mUsers.AddRange( records );
			}
			else
			{
				var records = from user in mDataModel.UserModel
							  where user.CommunityId == 0
							  select user;
				mUsers.AddRange( records );
			}
			wUserList.ItemsSource = mUsers;
			OnSelectUser( null, null );
		}

		private void SearchCommunities()
		{
			mCommunities.Clear();
			if( wSearchText.Text.Length > 0 )
			{
				var records = from community in mDataModel.CommunityModel
							  where community.Name.Contains( wSearchText.Text )
							  select community;
				mCommunities.AddRange( records );
			}
			else
			{
				var records = from community in mDataModel.CommunityModel
							  select community;
				mCommunities.AddRange( records );
			}
			wCommunityList.ItemsSource = mCommunities;
			OnSelectGroup( null, null );
		}

		private void OnAllButton( object sender, RoutedEventArgs e )
		{
			wSearchText.Text = "";
			wSearchButton.IsEnabled = false;
			OnSearchButton( null, null );
		}

		private void OnTab( object sender, SelectionChangedEventArgs e )
		{
			wEnterButton.IsEnabled = false;
			OnAllButton( null, null );
			if( wListTab.SelectedIndex == USER_TAB )
			{
				//wNewButton.Content = "New User";
				wExportButton.Visibility = Visibility.Visible;
			}
			else if( wListTab.SelectedIndex == COMMUNITY_TAB )
			{
				//wNewButton.Content = "New Community";
				wExportButton.Visibility = Visibility.Collapsed;
			}
		}

		private void OnSelectUser( object sender, SelectionChangedEventArgs e )
		{
			wModifyButton.IsEnabled = wUserList.SelectedIndex >= 0;
			wExportButton.IsEnabled = wUserList.SelectedIndex >= 0;
			wEnterButton.IsEnabled = wUserList.SelectedIndex >= 0;
		}

		private void OnSelectGroup( object sender, SelectionChangedEventArgs e )
		{
			wModifyButton.IsEnabled = wCommunityList.SelectedIndex >= 0;
			wExportButton.IsEnabled = wCommunityList.SelectedIndex >= 0;
			wEnterButton.IsEnabled = wCommunityList.SelectedIndex >= 0;
		}

		private async void OnNewButton( object sender, RoutedEventArgs e )
		{
			if( wListTab.SelectedIndex == USER_TAB )
			{
				var dialog = new UserDialog( mDataModel, null, null )
				{
					XamlRoot = this.XamlRoot // XamlRootÇê›íË
				};
				await dialog.ShowAsync();
			}
			else if( wListTab.SelectedIndex == COMMUNITY_TAB )
			{
				var dialog = new CommunityDialog( mDataModel, null )
				{
					XamlRoot = this.XamlRoot // XamlRootÇê›íË
				};
				await dialog.ShowAsync();
			}
		}

		private async void OnModifyButton( object sender, RoutedEventArgs e )
		{
			if( wListTab.SelectedIndex == USER_TAB )
			{
				if( wUserList.SelectedIndex >= 0 )
				{
					var dialog = new PasswordDialog( mUsers[wUserList.SelectedIndex], PasswordDialog.MODE_MODIFY )
					{
						XamlRoot = this.XamlRoot // XamlRootÇê›íË
					};
					var result = await dialog.ShowAsync();
					if( result == ContentDialogResult.Primary )
					{
						var userDialog = new UserDialog( mDataModel, null, mUsers[wUserList.SelectedIndex] )
						{
							XamlRoot = this.XamlRoot // XamlRootÇê›íË
						};
						await userDialog.ShowAsync();
					}
				}
			}
			else if( wListTab.SelectedIndex == COMMUNITY_TAB )
			{
				if( wCommunityList.SelectedIndex >= 0 )
				{
					var dialog = new PasswordDialog( mCommunities[wCommunityList.SelectedIndex], PasswordDialog.MODE_MODIFY )
					{
						XamlRoot = this.XamlRoot // XamlRootÇê›íË
					};
					var result = await dialog.ShowAsync();
					if( result == ContentDialogResult.Primary )
					{
						var communityDialog = new CommunityDialog( mDataModel, mCommunities[wCommunityList.SelectedIndex] )
						{
							XamlRoot = this.XamlRoot // XamlRootÇê›íË
						};
						await communityDialog.ShowAsync();
					}
				}
			}
		}

		private async void OnExportButton( object sender, RoutedEventArgs e )
		{
			if( wUserList.SelectedIndex >= 0 )
			{
				var dialog = new PasswordDialog( mUsers[wUserList.SelectedIndex], PasswordDialog.MODE_EXPORT )
				{
					XamlRoot = this.XamlRoot // XamlRootÇê›íË
				};
				var result = await dialog.ShowAsync();
				if( result == ContentDialogResult.Primary )
				{
					var exportDialog = new DataExportDialog( mDataModel, mUsers[wUserList.SelectedIndex] )
					{
						XamlRoot = this.XamlRoot // XamlRootÇê›íË
					};
					await exportDialog.ShowAsync();
				}
			}
		}

		private async void OnEnterButton( object sender, RoutedEventArgs e )
		{
			if( wListTab.SelectedIndex == USER_TAB )
			{
				if( wUserList.SelectedIndex >= 0 )
				{
					var dialog = new PasswordDialog( mUsers[wUserList.SelectedIndex], PasswordDialog.MODE_ENTER )
					{
						XamlRoot = this.XamlRoot // XamlRootÇê›íË
					};
					var result = await dialog.ShowAsync();
					if( result == ContentDialogResult.Primary )
					{
						SelectUser?.Invoke( this, new UserEventArgs( mUsers[wUserList.SelectedIndex] ) );
					}
				}
			}
			else if( wListTab.SelectedIndex == COMMUNITY_TAB )
			{
				if( wCommunityList.SelectedIndex >= 0 )
				{
					var dialog = new PasswordDialog( mCommunities[wCommunityList.SelectedIndex], PasswordDialog.MODE_ENTER )
					{
						XamlRoot = this.XamlRoot // XamlRootÇê›íË
					};
					var result = await dialog.ShowAsync();
					if( result == ContentDialogResult.Primary )
					{
						SelectCommunity?.Invoke( this, new CommunityEventArgs( mCommunities[wCommunityList.SelectedIndex] ) );
					}
				}
			}
		}

		private void OnQuitButton( object sender, RoutedEventArgs e )
		{
			Quit?.Invoke( this, e );
		}
	}
}
