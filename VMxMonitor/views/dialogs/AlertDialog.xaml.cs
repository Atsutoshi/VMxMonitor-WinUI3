using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using System;

namespace VMxMonitor.dialogs
{
	public sealed partial class AlertDialog : ContentDialog
	{
		public AlertDialog()
		{
			this.InitializeComponent();
		}

		public static async Task ShowAsync( string title, string message )
		{
			var dialog = new AlertDialog
			{
				Title = title,
				Content = message,
				PrimaryButtonText = "OK"
			};
			await dialog.ShowAsync();
		}

		public static async Task ShowAsync( string message )
		{
			await ShowAsync( "Error", message );
		}
	}
}