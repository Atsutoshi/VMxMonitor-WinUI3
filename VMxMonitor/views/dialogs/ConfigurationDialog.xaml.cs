using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.IO.Ports;

namespace VMxMonitor.views.dialogs
{
	public sealed partial class ConfigurationDialog : ContentDialog
	{
		public string Port { get; private set; }
		public int Length { get; private set; }

		private string[] mPorts;
		private int[] mLengths;

		public ConfigurationDialog()
		{
			this.InitializeComponent();
			this.Opened += ConfigurationDialog_Opened;
		}

		private void ConfigurationDialog_Opened( ContentDialog sender, ContentDialogOpenedEventArgs args )
		{
			onLoad();
		}

		private void onLoad()
		{
			string portName = readPortName();
			int selected = 0;

			mPorts = SerialPort.GetPortNames();
			for( int i = 0; i < mPorts.Length; ++i )
			{
				wPortCombo.Items.Add( mPorts[i] );
				if( portName.Equals( mPorts[i] ) )
				{
					selected = i;
				}
			}
			if( wPortCombo.Items.Count > 0 )
			{
				wPortCombo.SelectedIndex = selected;
			}

			mLengths = new int[] { 60, 90, 120, 150, 180, 240, 300 };
			foreach( int length in mLengths )
			{
				wLengthCombo.Items.Add( length.ToString() );
			}
			wLengthCombo.SelectedIndex = 2;

			validate();
		}

		private void onPortCombo( object sender, SelectionChangedEventArgs e )
		{
			validate();
		}

		private void onLengthCombo( object sender, SelectionChangedEventArgs e )
		{
			validate();
		}

		private void onStartButton( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			if( wPortCombo.SelectedIndex >= 0 && wLengthCombo.SelectedIndex >= 0 )
			{
				Port = mPorts[wPortCombo.SelectedIndex];
				Length = mLengths[wLengthCombo.SelectedIndex];
				writePortName( Port );
			}
		}

		private void onCancelButton( ContentDialog sender, ContentDialogButtonClickEventArgs args )
		{
			// Optional: Handle cancel button click if needed
		}

		private void validate()
		{
			this.IsPrimaryButtonEnabled = wPortCombo.SelectedIndex >= 0 && wLengthCombo.SelectedIndex >= 0;
		}

		private string readPortName()
		{
			string port = "";
			string file = getPortFile();
			if( System.IO.File.Exists( file ) )
			{
				using( System.IO.StreamReader reader = new System.IO.StreamReader( getPortFile() ) )
				{
					port = reader.ReadLine()?.Trim();
				}
			}
			return port;
		}

		private void writePortName( string port )
		{
			using( System.IO.StreamWriter output = new System.IO.StreamWriter( getPortFile() ) )
			{
				output.WriteLine( port );
			}
		}

		private string getPortFile()
		{
			string root = Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData );
			return System.IO.Path.Combine( root, "FMCC", "VMxMonitor1", "port.txt" );
		}
	}
}
