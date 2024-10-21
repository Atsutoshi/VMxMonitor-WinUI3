using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace VMxMonitor.controls
{
	public partial class InquiryNumber : UserControl
	{
		public event RoutedEventHandler ValueChanged;
		public int Index { get; private set; }
		public double Value { get; private set; }
		private bool mIsBusy;

		public InquiryNumber( int index, double value )
		{
			this.InitializeComponent();
			Index = index;
			Value = value;
			mIsBusy = false;

			// イベントハンドラーの追加
			this.Loaded += onLoad;
		}

		private void onLoad( object sender, RoutedEventArgs e )
		{
			// Simulate resource fetching and setup.
			wQuestionText.Text = "Simulated question?";
			wPrefixText.Text = "Prefix";
			wPostfixText.Text = "Postfix";
			wValueText.Text = Value >= 0 ? Value.ToString( "F2" ) : "";
		}

		private void onChange( object sender, TextChangedEventArgs e )
		{
			if( double.TryParse( wValueText.Text, out double value ) )
			{
				Value = value;
				ValueChanged?.Invoke( this, new RoutedEventArgs() );
			}
		}
	}
}
