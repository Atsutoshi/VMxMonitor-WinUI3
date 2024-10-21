using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Linq;
using VMxMonitor.models;

namespace VMxMonitor.dialogs
{
	public sealed partial class CommunityDialog : ContentDialog
	{
		private DataModel mDataModel;
		private CommunityModel mCommunity;
		private bool mIsIME;
		private ResourceLoader resourceLoader;
		private bool isDialogOpen = false;

		public CommunityDialog( DataModel model, CommunityModel community )
		{
			this.InitializeComponent();

			mDataModel = model;
			mCommunity = community;
			mIsIME = false;

			resourceLoader = new ResourceLoader();
			this.Loaded += OnLoaded;
		}

		private void OnLoaded( object sender, RoutedEventArgs e )
		{
			if( mCommunity != null )
			{
				wPromptText.Text = resourceLoader.GetString( "promptCommunityModification" );
				wNameText.Text = mCommunity.Name;
				wPasswordText.Text = mCommunity.Password;
				wSaveButton.Content = resourceLoader.GetString( "buttonUpdate" );
			}
			else
			{
				wPromptText.Text = resourceLoader.GetString( "promptCommunityRegistration" );
				wSaveButton.Content = resourceLoader.GetString( "buttonRegister" );
				wSaveButton.IsEnabled = true;
				wDeleteButton.Visibility = Visibility.Collapsed;
			}

			wPasswordText.InputScope = new InputScope
			{
				Names = { new InputScopeName( InputScopeNameValue.Default ) }
			};

			wNameText.KeyDown += OnKeyDown;
			wNameText.KeyUp += OnKeyUp;

			this.Title = resourceLoader.GetString( "titleCommunity" );
			this.PrimaryButtonText = resourceLoader.GetString( "buttonRegister" );
			this.SecondaryButtonText = resourceLoader.GetString( "buttonCancel" );
			wDeleteButton.Content = resourceLoader.GetString( "buttonDelete" );
		}

		private void OnTextBox( object sender, TextChangedEventArgs e )
		{
			if( !mIsIME )
			{
				Validate();
			}
		}

		private async void OnSaveButton( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			string error = null;
			if( wNameText.Text.Length == 0 )
			{
				error = resourceLoader.GetString( "errorCommunityNameEmpty" );
			}
			else
			{
				IQueryable<CommunityModel> communities;
				if( mCommunity != null )
				{
					communities = from t1 in mDataModel.CommunityModel
								  where t1.Id != mCommunity.Id && t1.Name.Equals( wNameText.Text )
								  select t1;
				}
				else
				{
					communities = from t1 in mDataModel.CommunityModel
								  where t1.Name.Equals( wNameText.Text )
								  select t1;
				}
				foreach( CommunityModel entry in communities )
				{
					error = String.Format( resourceLoader.GetString( "errorCommunityNameDuplicated" ), wNameText.Text );
				}
			}
			if( wPasswordText.Text.Length == 0 )
			{
				error = resourceLoader.GetString( "errorPasswordEmpty" );
			}
			if( error != null )
			{
				Hide(); // 現在のダイアログを閉じる
				var errorDialog = new ContentDialog
				{
					Title = "エラー",
					Content = error,
					CloseButtonText = "OK",
					XamlRoot = this.XamlRoot // XamlRootを設定
				};
				await errorDialog.ShowAsync();
				ShowAsync(); // ダイアログを再表示する
				args.Cancel = true;
				return;
			}

			SequenceModel sequence = null;
			if( mCommunity == null )
			{
				sequence = mDataModel.getSequence( CommunityModel.TABLE_NAME );
				mCommunity = new CommunityModel();
				mCommunity.Id = sequence.Id;
			}
			mCommunity.Name = wNameText.Text;
			mCommunity.Password = wPasswordText.Text;
			if( sequence != null )
			{
				mCommunity.RegisteredAt = DateTime.Now;
				mDataModel.CommunityModel.Add( mCommunity );
				++sequence.Id;
			}
			mDataModel.SaveChanges();

			Hide();
		}

		private void OnCancelButton( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			Hide();
		}

		private async void OnDeleteButton( object sender, RoutedEventArgs e )
		{
			if( mCommunity != null )
			{
				String message = String.Format( resourceLoader.GetString( "promptUserDeletion" ), mCommunity.Name );
				Hide(); // 現在のダイアログを閉じる
				var deleteDialog = new ContentDialog
				{
					Title = "確認",
					Content = message,
					PrimaryButtonText = "はい",
					SecondaryButtonText = "いいえ",
					XamlRoot = this.XamlRoot // XamlRootを設定
				};
				var result = await deleteDialog.ShowAsync();

				if( result == ContentDialogResult.Primary )
				{
					mDataModel.CommunityModel.Remove( mCommunity );
					mDataModel.SaveChanges();
					Hide();
				}
				else
				{
					ShowAsync(); // ダイアログを再表示する
				}
			}
		}

		private void OnKeyDown( object sender, KeyRoutedEventArgs e )
		{
			mIsIME = true;
		}

		private void OnKeyUp( object sender, KeyRoutedEventArgs e )
		{
			mIsIME = false;
		}

		private void Validate()
		{
			wSaveButton.IsEnabled = wNameText.Text.Length > 0;
		}
	}
}
