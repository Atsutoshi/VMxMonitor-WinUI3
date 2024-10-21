using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace VMxMonitor.controls
{
	public partial class InquiryDisease9 : UserControl
	{
		public event RoutedEventHandler Change;
		public string Value { get; private set; }
		private bool mIsIME;

		// Assume DiseaseOption contains properties `Content` and `Tag` that correspond to checkbox properties
		public ObservableCollection<DiseaseOption> DiseaseOptions { get; set; } = new ObservableCollection<DiseaseOption>();

		public InquiryDisease9( string value )
		{
			InitializeComponent();
			this.Loaded += onLoad;
			Value = value;
			mIsIME = false;
			InitializeDiseaseOptions();
		}

		private void onLoad( object sender, RoutedEventArgs e )
		{
			wCheckPanel.ItemsSource = DiseaseOptions;
			ParseValueToCheckbox( Value );
		}

		private void InitializeDiseaseOptions()
		{
			// Populate your disease options here, based on your actual data model
			DiseaseOptions.Add( new DiseaseOption { Content = "Disease 1", Tag = "1" } );
			// Add more diseases as required
		}

		private void onCheck( object sender, RoutedEventArgs e )
		{
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
			// Read and compile the checkbox values into a single string
			var checkedItems = DiseaseOptions.Where( x => ( ( CheckBox )wCheckPanel.FindName( x.Tag ) ).IsChecked == true );
			Value = string.Join( ",", checkedItems.Select( x => x.Tag ) );
			Change?.Invoke( this, null );
		}

		private void ParseValueToCheckbox( string value )
		{
			// Implementation to parse Value back to checkbox states if needed
			// Example parsing not implemented
		}
	}

	public class DiseaseOption
	{
		public string Content { get; set; }
		public string Tag { get; set; }
	}
}
