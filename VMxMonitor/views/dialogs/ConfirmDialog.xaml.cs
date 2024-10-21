using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace VMxMonitor.dialogs
{
	public sealed partial class ConfirmDialog : ContentDialog
	{
		private static TaskCompletionSource<ContentDialogResult> _resultSource;
		private ResourceLoader resourceLoader;

		public string Title { get; set; }
		public string PrimaryButtonText { get; set; }
		public string SecondaryButtonText { get; set; }
		public string Message { get; set; }

		public ConfirmDialog( string message )
		{
			this.InitializeComponent();
			resourceLoader = ResourceLoader.GetForCurrentView();
			Title = resourceLoader.GetString( "titleConfirm" );
			PrimaryButtonText = resourceLoader.GetString( "buttonYes" );
			SecondaryButtonText = resourceLoader.GetString( "buttonNo" );
			Message = message;
			this.DataContext = this;
		}

		private void OnPrimaryButtonClick( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			_resultSource.TrySetResult( ContentDialogResult.Primary );
		}

		private void OnSecondaryButtonClick( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			_resultSource.TrySetResult( ContentDialogResult.Secondary );
		}

		public static Task<ContentDialogResult> ShowAsync( string message )
		{
			_resultSource = new TaskCompletionSource<ContentDialogResult>();
			var dialog = new ConfirmDialog( message );
			dialog.ShowAsync();
			return _resultSource.Task;
		}
	}
}
