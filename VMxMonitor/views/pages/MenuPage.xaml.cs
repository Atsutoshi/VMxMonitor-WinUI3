using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace VMxMonitor.pages
{
	public sealed partial class MenuPage : UserControl
	{
		public RoutedEventHandler StartMonitor;
		public RoutedEventHandler ListMonitor;
		public RoutedEventHandler StartInquiry;
		public RoutedEventHandler ListInquiry;
		public RoutedEventHandler Finish;

		public MenuPage()
		{
			this.InitializeComponent();
		}

		private void onStartMonitorButton( object sender, RoutedEventArgs e )
		{
			StartMonitor?.Invoke( this, e );
		}

		private void onListMonitorButton( object sender, RoutedEventArgs e )
		{
			ListMonitor?.Invoke( this, e );
		}

		private void onStartInquiryButton( object sender, RoutedEventArgs e )
		{
			StartInquiry?.Invoke( this, e );
		}

		private void onListInquiryButton( object sender, RoutedEventArgs e )
		{
			ListInquiry?.Invoke( this, e );
		}

		private void onCancelButton( object sender, RoutedEventArgs e )
		{
			Finish?.Invoke( this, e );
		}
	}
}
