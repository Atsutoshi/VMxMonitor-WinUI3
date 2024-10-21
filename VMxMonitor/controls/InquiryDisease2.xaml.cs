using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace VMxMonitor.controls
{
	public partial class InquiryDisease2 : UserControl
	{
		public event RoutedEventHandler Change;
		public int Index { get; private set; }
		public string Value { get; private set; }
		private bool mIsIME;

		public InquiryDisease2( int index, string value )
		{
			InitializeComponent();
			Index = index;
			Value = value;
			mIsIME = false;
		}

		private void onLoad( object sender, RoutedEventArgs e )
		{
			// Update text based on the value or settings.
			wQuestionText.Text = "Loaded question based on index?";
			UpdateRadioButtonsBasedOnValue( Value );
		}

		private void onCheck( object sender, RoutedEventArgs e )
		{
			RadioButton radioButton = sender as RadioButton;
			if( radioButton != null && radioButton.IsChecked == true )
			{
				// Update the text box's enabled state based on the tag of the radio button.
				wAnswerText.IsEnabled = radioButton.Tag.ToString() == "1";
			}
			readValue();
		}

		private void onChange( object sender, TextChangedEventArgs e )
		{
			if( !mIsIME )
			{
				readValue();
			}
		}

		private void readValue()
		{
			if( wAnswerText.IsEnabled && !string.IsNullOrEmpty( wAnswerText.Text ) )
			{
				Value = wAnswerText.Text;
			}
			else
			{
				Value = null;
			}
			Change?.Invoke( this, new RoutedEventArgs() );
		}

		private void UpdateRadioButtonsBasedOnValue( string value )
		{
			bool isYesSelected = !string.IsNullOrEmpty( value );
			foreach( UIElement element in wRadioPanel.Children )
			{
				if( element is RadioButton radioButton )
				{
					if( radioButton.Tag.ToString() == "1" )
					{
						radioButton.IsChecked = isYesSelected;
					}
					else if( radioButton.Tag.ToString() == "0" )
					{
						radioButton.IsChecked = !isYesSelected;
					}
				}
			}
			wAnswerText.IsEnabled = isYesSelected;
		}
	}
}
