using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System.Text;


namespace UserControlExample
{
	public partial class NameReporter : UserControl
	{
		public NameReporter()
		{
			InitializeComponent();
		}

		private void Button_Click( object sender, RoutedEventArgs e )
		{
			StringBuilder displayText = new StringBuilder( "Hello, " );
			displayText.AppendFormat( "{0} {1}.", firstName.Text, lastName.Text );
			result.Text = displayText.ToString();
		}
	}
}