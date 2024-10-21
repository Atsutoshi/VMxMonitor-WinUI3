using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace VMxMonitor.dialogs
{
	public sealed partial class AuthenticationDialog : ContentDialog
	{
		public AuthenticationDialog()
		{
			this.InitializeComponent();
		}

		public async Task<ContentDialogResult> ShowAsync()
		{
			return await this.ShowAsync();
		}


		public static async Task<bool> ShowDialogAsync( string title, string message )
		{
			var dialog = new AuthenticationDialog
			{
				Title = title,
				PrimaryButtonText = "Reconfirm",
				SecondaryButtonText = "Quit"
			};

			var result = await dialog.ShowAsync();
			return result == ContentDialogResult.Primary;
		}

		private void OnPrimaryButtonClick( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			// Handle reconfirm button click
		}

		private void OnSecondaryButtonClick( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			// Handle quit button click
		}
	}
}
