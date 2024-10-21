using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System.Linq;

namespace VMxMonitor.controls
{
	public partial class InquiryChoice : UserControl
	{
		public event RoutedEventHandler Check;
		public int Index { get; private set; }
		public int Choice { get; private set; }
		private bool mIsBusy;

		public InquiryChoice( int index, int choice )
		{
			InitializeComponent();
			Index = index;
			Choice = choice;
			mIsBusy = false;
		}

		private void onLoad( object sender, RoutedEventArgs e )
		{
			// ここでリソースからテキストを設定
			wQuestionText.Text = $"Loaded question for index {Index}";
			UpdateToggleButtons( Choice );
		}

		private void onCheck( object sender, RoutedEventArgs e )
		{
			if( mIsBusy )
				return;

			mIsBusy = true;
			ToggleButton button = sender as ToggleButton;
			if( button.IsChecked == true )
			{
				Choice = int.Parse( button.Tag.ToString() );
				UpdateToggleButtons( Choice );
			}
			else
			{
				Choice = -1;
			}

			Check?.Invoke( this, new RoutedEventArgs() );
			mIsBusy = false;
		}

		private void UpdateToggleButtons( int selectedChoice )
		{
			foreach( ToggleButton button in wChoicePanel.Children.OfType<ToggleButton>() )
			{
				button.IsChecked = int.Parse( button.Tag.ToString() ) == selectedChoice;
			}
		}
	}
}
