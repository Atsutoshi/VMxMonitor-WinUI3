using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Linq;

namespace VMxMonitor.controls
{
	public partial class InquiryYesNo : UserControl
	{
		public event RoutedEventHandler Check;
		public int YesNo { get; private set; }
		private bool mIsBusy;

		public InquiryYesNo( int yesNo )
		{
			InitializeComponent();
			YesNo = yesNo;
			mIsBusy = false;
		}

		private void onLoad( object sender, RoutedEventArgs e )
		{
			foreach( ToggleButton button in wChoicePanel.Children.OfType<ToggleButton>() )
			{
				button.IsChecked = button.Tag.ToString() == YesNo.ToString();
			}
		}

		private void onCheck( object sender, RoutedEventArgs e )
		{
			if( mIsBusy ) return;
			mIsBusy = true;

			var button = sender as ToggleButton;
			if( button.IsChecked == true )
			{
				if( int.TryParse( button.Tag.ToString(), out var tag ) )
				{
					YesNo = tag;
					foreach( ToggleButton other in wChoicePanel.Children.OfType<ToggleButton>() )
					{
						if( other != button ) other.IsChecked = false;
					}
				}
			}
			else
			{
				YesNo = -1;
			}

			Check?.Invoke( this, e );
			mIsBusy = false;
		}
	}
}
