using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM_app_221_WAS.services;
using VMxMonitor.dialogs;
using VMxMonitor.models;
using Microsoft.Windows.ApplicationModel.Resources;
using VMxMonitor.views.dialogs;

namespace VMxMonitor.views.pages
{
	public sealed partial class MonitorPage : UserControl
	{
		/* =====[ constants ]===== */

		private const byte MODE_PREPARING = 1;
		private const byte MODE_COUNTING = 2;
		private const byte MODE_MONITORING = 3;
		private const int COUNTING_LENGTH = 5;
		private const int MONITOR_MAX_X = 400;
		private const int MONITOR_MIN_Y = 0;
		private const int MONITOR_MAX_Y = 4095;
		private const byte ECG_FLAG = 0x02;
		private const byte PPG_FLAG = 0x01;
		private const int MONITOR_INTERVAL = 150;
		private const byte TIME_TYPE_R = 1;
		private const byte TIME_TYPE_A = 2;

		/* =====[ properties ]===== */

		public EventHandler<MonitorEventArgs> Finish;
		public RoutedEventHandler Cancel;
		//
		private DataModel mDataModel;
		private UserModel mUser;
		private SerialPort mPort;
		private byte[] mData;
		private int mDataSize;
		private int mDataLength;
		private List<PeakModel.Unit> mUnits;
		private List<WaveModel> mWaves;
		private int mLength;
		private byte mMode;
		private int mState;
		private long mStartTime;
		private int mCount;
		private bool mIsBusy;
		private byte mCommand;
		//
		private bool mReceived;

		private ResourceLoader resourceLoader = new ResourceLoader();

		/* =====[ constructors ]===== */

		public MonitorPage( DataModel model, UserModel user )
		{
			InitializeComponent();

			// initialize properties.
			mDataModel = model;
			mUser = user;
			mPort = null;
			mData = null;
			mDataSize = 0;
			mDataLength = 0;
			mUnits = new List<PeakModel.Unit>();
			mWaves = new List<WaveModel>();
			mIsBusy = false;
		}

		/* =====[ event listeners ]===== */

		private void onLoad( object sender, RoutedEventArgs e )
		{
			// set up widgets.
			wIndicatorECG.Label = "ECG";
			wIndicatorPPG.Label = "PPG";
			wMonitorECG.Label = "ECG";
			wMonitorPPG.Label = "PPG";

			// configure.
			configure();
		}

		private void onStopButton( object sender, RoutedEventArgs e )
		{
			// send stop command.
			stop();

			// alert.
			AlertDialog.ShowAsync( resourceLoader.GetString( "titleCancel" ), resourceLoader.GetString( "promptMonitorCancellation" ) );

			// re-configure.
			configure();
		}

		//private void onReceiveData( object sender, SerialDataReceivedEventArgs e )
		//{
		//	try
		//	{
		//		int bytes = mPort.BytesToRead;
		//		byte[] buffer = new byte[bytes];
		//		mPort.Read( buffer, 0, bytes );

		//		// Process the received data.
		//		processData( buffer );
		//	}
		//	catch( Exception ex )
		//	{
		//		AlertDialog.ShowAsync( resourceLoader.GetString( "errorPortReceiveFailed" ) + ex.Message );
		//	}
		//}

		private void processData( byte[] buffer )
		{
			// Implement the logic to process the received data
			// For example, append the data to mData and process the complete messages
		}

		/* =====[ private methods ]===== */

		private async void configure()
		{
			// set up widgets.
			wProgressBar.Visibility = Visibility.Collapsed;
			wLengthText.Text = "";
			wRemainingText.Text = "";
			wHeartRateText.Text = "";
			wVersionText.Text = "";
			wIndicatorECG.On = false;
			wIndicatorPPG.On = false;
			wMonitorECG.Configure( ( int )Math.Min( wMonitorECG.ActualWidth, MONITOR_MAX_X ), MONITOR_MIN_Y, MONITOR_MAX_Y );
			wMonitorPPG.Configure( ( int )Math.Min( wMonitorPPG.ActualWidth, MONITOR_MAX_X ), MONITOR_MIN_Y, MONITOR_MAX_Y );
			wMessageBoard.Visibility = Visibility.Collapsed;

			// configure.
			ConfigurationDialog dialog = new ConfigurationDialog();
			var result = await dialog.ShowAsync();
			if( result == ContentDialogResult.Primary )
			{
				// start.
				start( dialog.Port, dialog.Length );
			}
			else
			{
				// cancel.
				Cancel?.Invoke( this, null );
			}
		}

		private void start( string port, int length )
		{
			// open serial port.
			mPort = new SerialPort();
			mPort.PortName = port;
			mPort.BaudRate = 115200;
			mPort.Parity = Parity.None;
			mPort.DataBits = 8;
			mPort.StopBits = StopBits.One;
			mPort.Handshake = Handshake.RequestToSend;
			mPort.ReadTimeout = 800;
			mPort.WriteTimeout = 800;
			mPort.DataReceived += new SerialDataReceivedEventHandler( onReceiveData );
			//
			try
			{
				if( !mPort.IsOpen )
				{
					mPort.Open();
					mPort.DiscardInBuffer();
					mPort.DiscardOutBuffer();
				}
			}
			catch( Exception ex )
			{
				AlertDialog.ShowAsync( resourceLoader.GetString( "errorPortOpenFailed" ) + ex.Message );
				if( Cancel != null )
				{
					Cancel( this, null );
				}
				return;
			}

			// initialize.
			try
			{
				byte[] command = new byte[1];
				command[0] = ( byte )0xfb;
				mPort.Write( command, 0, 1 );
				mCommand = command[0];
			}
			catch( Exception ex )
			{
				AlertDialog.ShowAsync( resourceLoader.GetString( "errorPortSendFailed" ) + ex.Message );
				return;
			}

			// loop.
			mUnits.Clear();
			mWaves.Clear();
			mLength = length;
			mMode = MODE_PREPARING;
			mState = PeakModel.STATE_SUCCESS;
			mIsBusy = true;
			monitorLoop();
		}

		private void stop()
		{
			try
			{
				byte[] command = new byte[1];
				command[0] = ( byte )0xfb;
				mPort.Write( command, 0, 1 );
				mCommand = command[0];
			}
			catch( Exception ex )
			{
				AlertDialog.ShowAsync( resourceLoader.GetString( "errorPortSendFailed" ) + ex.Message );
				return;
			}
			//
			mPort.Close();
			mPort = null;
			mIsBusy = false;
		}

		private async void testLoop()
		{
			while( true )
			{
				await Task.Delay( MONITOR_INTERVAL );
			}
		}

		private async void monitorLoop()
		{
			// show progress bar.
			wProgressBar.Value = 0;
			wProgressBar.IsIndeterminate = true;
			wProgressBar.Visibility = Visibility.Visible;

			// loop.
			while( mIsBusy )
			{
				// sleep.
				await Task.Delay( MONITOR_INTERVAL );

				// refresh monitors.
				wMonitorECG.Refresh();
				wMonitorPPG.Refresh();

				if( mMode == MODE_COUNTING )
				{
					int c = ( int )( ( DateTime.Now.Ticks - mStartTime ) / 10000000 );
					if( c != mCount )
					{
						if( c >= COUNTING_LENGTH )
						{
							// start.
							mUnits.Clear();
							mMode = MODE_MONITORING;
							mStartTime = DateTime.Now.Ticks;
							mCount = 0;
							wProgressBar.IsIndeterminate = false;
							wProgressBar.Minimum = 0;
							wProgressBar.Maximum = mLength * 10;
							wProgressBar.Value = 0;
							wLengthText.Text = String.Format( resourceLoader.GetString( "labelMonitorLength" ), mLength );
							wMessageBoard.Visibility = Visibility.Collapsed;
						}
						mCount = c;
					}
				}
				else if( mMode == MODE_MONITORING )
				{
					int c = ( int )( ( DateTime.Now.Ticks - mStartTime ) / 10000000 );
					if( c != mCount )
					{
						if( c > mLength )
						{
							// finish.
							stop();

							// alert.
							AlertDialog.ShowAsync( resourceLoader.GetString( "titleFinish" ), resourceLoader.GetString( "promptMonitorCompletion" ) );

							// analyze.
							wProgressBar.Value = 0;
							wProgressBar.IsIndeterminate = true;
							wMessageText.Text = resourceLoader.GetString( "promptMonitorAnalyzing" );
							wMessageBoard.Visibility = Visibility.Visible;
							analyze();
						}
						else
						{
							// count.
							wRemainingText.Text = String.Format( resourceLoader.GetString( "labelMonitorRemaining" ), mLength - c );
						}
						mCount = c;
					}
					wProgressBar.Value = ( DateTime.Now.Ticks - mStartTime ) / 1000000;
				}
			}
		}

		// other methods like analyze(), onReceiveData(), etc.
	}
}
