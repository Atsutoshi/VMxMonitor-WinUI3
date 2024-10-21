using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Linq;

namespace VMxMonitor.controls
{
	public partial class InquiryPeriod : UserControl
	{
		public event RoutedEventHandler Check;
		public int Choice { get; private set; }
		private bool mIsBusy;

		public InquiryPeriod( int choice )
		{
			InitializeComponent();
			Choice = choice;
			mIsBusy = false;
		}

		private void onLoad( object sender, RoutedEventArgs e )
		{
			// Setup initial toggle state based on 'Choice'
			foreach( ToggleButton button in wChoicePanel.Children.OfType<ToggleButton>() )
			{
				button.IsChecked = int.TryParse( button.Tag as string, out int tag ) && tag == Choice;
			}
		}

		private void onCheck( object sender, RoutedEventArgs e )
		{
			if( mIsBusy ) return;
			mIsBusy = true;

			var button = sender as ToggleButton;
			if( button.IsChecked == true )
			{
				if( int.TryParse( button.Tag as string, out int tag ) )
				{
					Choice = tag;
					// Uncheck other buttons
					foreach( ToggleButton other in wChoicePanel.Children.OfType<ToggleButton>() )
					{
						if( other != button ) other.IsChecked = false;
					}
				}
			}
			else
			{
				Choice = -1;
			}

			Check?.Invoke( this, e );
			mIsBusy = false;
		}
	}
}

