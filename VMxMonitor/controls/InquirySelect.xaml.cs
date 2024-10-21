using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Linq;

namespace VMxMonitor.controls
{
	public partial class InquirySelect : UserControl
	{
		public event RoutedEventHandler Check;
		public int Select { get; private set; }
		private bool mIsBusy;

		public InquirySelect( int select )
		{
			InitializeComponent();
			Select = select;
			mIsBusy = false;
		}

		private void onLoad( object sender, RoutedEventArgs e )
		{
			foreach( ToggleButton button in wSelectPanel.Children.OfType<ToggleButton>() )
			{
				button.IsChecked = button.Tag.ToString() == Select.ToString();
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
					Select = tag;
					foreach( ToggleButton other in wSelectPanel.Children.OfType<ToggleButton>() )
					{
						if( other != button ) other.IsChecked = false;
					}
				}
			}
			else
			{
				Select = -1;
			}

			Check?.Invoke( this, e );
			mIsBusy = false;
		}
	}
}
