using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Threading.Tasks;
using VMxMonitor.models;
using Windows.ApplicationModel.Resources;

namespace VMxMonitor.dialogs
{
	public sealed partial class PasswordDialog : ContentDialog
	{
		public const int MODE_ENTER = 1;
		public const int MODE_MODIFY = 2;
		public const int MODE_EXPORT = 3;

		private CommunityModel mCommunity;
		private UserModel mUser;
		private int mMode;
		private ResourceLoader resourceLoader;

		public PasswordDialog( CommunityModel community, int mode )
		{
			this.InitializeComponent();
			mCommunity = community;
			mUser = null;
			mMode = mode;
			resourceLoader = ResourceLoader.GetForCurrentView();
		}

		public PasswordDialog( UserModel user, int mode )
		{
			this.InitializeComponent();
			mCommunity = null;
			mUser = user;
			mMode = mode;
			resourceLoader = ResourceLoader.GetForCurrentView();
		}

		private void OnLoad( object sender, RoutedEventArgs e )
		{
			if( mCommunity != null )
			{
				wPromptText.Text = string.Format( resourceLoader.GetString( "promptCommunityPassword" ), mCommunity.Name );
			}
			else if( mUser != null )
			{
				wPromptText.Text = string.Format( resourceLoader.GetString( "promptUserPassword" ), mUser.Name );
			}

			switch( mMode )
			{
				case MODE_ENTER:
					PrimaryButtonText = resourceLoader.GetString( "buttonEnter" );
					break;
				case MODE_MODIFY:
					PrimaryButtonText = resourceLoader.GetString( "buttonModify" );
					break;
				case MODE_EXPORT:
					PrimaryButtonText = resourceLoader.GetString( "buttonExport" );
					break;
			}

			// Set focus to PasswordBox
			wPasswordText.Focus( FocusState.Programmatic );
		}

		private void OnPrimaryButtonClick( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			EnterPassword();
		}

		private void OnSecondaryButtonClick( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			// Close dialog without any action
		}

		private void OnPasswordKeyDown( object sender, KeyRoutedEventArgs e )
		{
			if( e.Key == Windows.System.VirtualKey.Enter )
			{
				EnterPassword();
			}
		}

		private async void EnterPassword()
		{
			string error = null;

			if( mCommunity != null )
			{
				if( wPasswordText.Password != mCommunity.Password )
				{
					error = resourceLoader.GetString( "errorPasswordInvalid" );
				}
			}
			else if( mUser != null )
			{
				if( wPasswordText.Password != mUser.Password )
				{
					error = resourceLoader.GetString( "errorPasswordInvalid" );
				}
			}

			if( error != null )
			{
				await AlertDialog.ShowAsync( error );
				return;
			}

			// Password correct
			Hide();
		}

		public static async Task<ContentDialogResult> ShowAsync( CommunityModel community, int mode )
		{
			var dialog = new PasswordDialog( community, mode );
			return await dialog.ShowAsync();
		}

		public static async Task<ContentDialogResult> ShowAsync( UserModel user, int mode )
		{
			var dialog = new PasswordDialog( user, mode );
			return await dialog.ShowAsync();
		}
	}
}
