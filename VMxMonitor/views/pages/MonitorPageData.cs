using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using VMxMonitor.models;
using VMxMonitor.dialogs;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace VMxMonitor.views.pages
{
	public partial class MonitorPage : UserControl
	{
		/* =====[ properties ]===== */

		private byte mLastCounter = 0;
		private int mLastHR = 0;
		private double mLastRT = 0;
		private double mLastAT = 0;
		private long mLastTickRT = 0;
		private long mLastTickAT = 0;
		private MediaPlayer mediaPlayer;

		/* =====[ constructors ]===== */

		public MonitorPage()
		{
			this.InitializeComponent();
			mediaPlayer = new MediaPlayer();
		}

		/* =====[ event listeners ]===== */

		private void onReceiveData1( object sender, SerialDataReceivedEventArgs e )
		{
			int size = mPort.BytesToRead;
			byte[] buf = new byte[size];
			mPort.Read( buf, 0, size );
			/*
            Console.Write("({0})", size);
            for (int i = 0; i < size; ++i)
            {
                Console.Write("{0},", buf[i]);
            }
            Console.WriteLine();
            */
		}

		private void onReceiveData( object sender, SerialDataReceivedEventArgs e )
		{
			// store received data.
			int offset = mDataLength;
			int size = mPort.BytesToRead;
			if( size == 0 )
			{
				return;
			}
			if( mDataSize < offset + size )
			{
				if( mData == null )
				{
					mData = new byte[size];
					mDataSize = size;
				}
				else
				{
					byte[] buf = new byte[offset + size];
					for( int i = 0; i < offset; ++i )
					{
						buf[i] = mData[i];
					}
					mData = buf;
					mDataSize = offset + size;
				}
			}
			mDataLength += mPort.Read( mData, offset, size );
			//
			while( mDataLength > 0 )
			{
				/*
                Console.Write("DATA:({0})", mDataLength);
                for (int i = 0; i < mDataLength; ++i)
                {
                    Console.Write("{0},", mData[i]);
                }
                Console.WriteLine();
                */
				//
				byte[] response = readBuffer();
				if( response != null )
				{
					/*
                    Console.Write("RESPONSE:({0})", response.Length);
                    for (int i = 0; i < response.Length; ++i)
                    {
                        Console.Write("{0},", response[i]);
                    }
                    Console.WriteLine();
                    */
					//
					if( mCommand == ( byte )0xfb )
					{
						// read firmware version.
						try
						{
							byte[] command = new byte[1];
							command[0] = ( byte )0xfd;
							mPort.Write( command, 0, 1 );
							mCommand = command[0];
						}
						catch( Exception ex )
						{
							DispatcherQueue.TryEnqueue(
								() =>
								{
									// show error.
									AlertDialog.ShowAsync( "Error: Port Send Failed - " + ex.Message );

									// close port.
									mPort.Close();
									mPort = null;
									mIsBusy = false;

									// re-configure.
									configure();
								} );
							return;
						}
					}
					else if( mCommand == ( byte )0xfd )
					{
						// read version.
						DispatcherQueue.TryEnqueue(
							() =>
							{
								wVersionText.Text = "version ";
							} );

						// start.
						try
						{
							byte[] command = new byte[1];
							command[0] = ( byte )0xfa;
							mPort.Write( command, 0, 1 );
							mCommand = command[0];
						}
						catch( Exception ex )
						{
							DispatcherQueue.TryEnqueue(
								() =>
								{
									// show error.
									AlertDialog.ShowAsync( "Error: Port Send Failed - " + ex.Message );

									// close port.
									mPort.Close();
									mPort = null;
									mIsBusy = false;

									// re-configure.
									configure();
								} );
							return;
						}
					}
					else if( mCommand == ( byte )0xfa )
					{
						mCommand = 0;
					}
					else if( mCommand == 0 )
					{
						if( response[0] == ( byte )0xf0 )
						{
							// peak #1.
							byte counter = response[1];
							byte status = response[2];
							byte HR = response[3];
							double RR = ( double )( ( response[4] << 4 ) | ( response[5] & 0x0f ) ) / 0.6;
							double AA = ( double )( ( response[6] << 4 ) | ( response[7] & 0x0f ) ) / 0.6;
							double RA = ( double )( response[8] & 0xff ) / 0.6;
							double RT = ( double )( ( ( response[9] & 0x3f ) << 18 ) | ( ( response[10] & 0x3f ) << 12 ) | ( ( response[11] & 0x3f ) << 6 ) | ( response[12] & 0x3f ) ) / 0.6;
							double AT = ( double )( ( ( response[13] & 0x3f ) << 18 ) | ( ( response[14] & 0x3f ) << 12 ) | ( ( response[15] & 0x3f ) << 6 ) | ( response[16] & 0x3f ) ) / 0.6;
							Console.WriteLine( "P1: {0},{1},{2},{3},{4},{5},{6},{7}", counter, status, HR, RR, AA, RA, RT, AT );
							//
							mLastCounter = counter;
							if( RR != 0 )
							{
								mLastHR = ( int )( 60000.0 / RR );
							}
							else if( AA != 0 )
							{
								mLastHR = ( int )( 60000.0 / AA );
							}
							mLastRT = RT;
							mLastAT = AT;
						}
						else if( response[0] == ( byte )0xf2 )
						{
							// peak #2.
							byte counter = response[1];
							Console.WriteLine( "P2: {0}", counter );
							//
							if( counter == mLastCounter )
							{
								// validate data.
								if( mLastRT != 0 )
								{
									mUnits.Add( new PeakModel.Unit( TIME_TYPE_R, mLastRT ) );
								}
								if( mLastAT != 0 )
								{
									mUnits.Add( new PeakModel.Unit( TIME_TYPE_A, mLastAT ) );
								}
								validateData();

								// update indicators.
								if( mLastRT != 0 || mLastAT != 0 )
								{
									DispatcherQueue.TryEnqueue(
										() =>
										{
											long now = DateTime.Now.Ticks;
											if( mLastHR > 0 )
											{
												wHeartRateText.Text = String.Format( "Heart Rate: {0}", mLastHR );
											}
											if( mLastRT != 0 )
											{
												wIndicatorECG.On = true;
												mLastTickRT = now;
											}
											if( mLastAT != 0 )
											{
												wIndicatorPPG.On = true;
												mLastTickAT = now;
											}
										} );
								}
							}

						}
						else if( response[0] == ( byte )0xf9 )
						{
							// wave.
							byte counter = response[1];
							short ECG = ( short )( ( ( response[2] & 0x3f ) << 6 ) | ( response[3] & 0x3f ) );
							short PPG = ( short )( ( ( response[4] & 0x3f ) << 6 ) | ( response[5] & 0x3f ) );
							short APG = ( short )( ( ( response[6] & 0x3f ) << 6 ) | ( response[7] & 0x3f ) );
							//Console.WriteLine("W: {0},{1},{2},{3}", counter, ECG, PPG, APG);

							// add buffer.
							mWaves.Add( new WaveModel( ( short )ECG, ( short )PPG, ( short )APG ) );

							// update wave monitors and indicators.
							DispatcherQueue.TryEnqueue(
								() =>
								{
									wMonitorECG.AddWave( ECG );
									wMonitorPPG.AddWave( APG );
									//
									long now = DateTime.Now.Ticks;
									TimeSpan spanRT = new TimeSpan( now - mLastTickRT );
									if( spanRT.TotalSeconds > 1.6 )
									{
										wIndicatorECG.On = false;
									}
									TimeSpan spanAT = new TimeSpan( now - mLastTickAT );
									if( spanAT.TotalSeconds > 1.6 )
									{
										wIndicatorPPG.On = false;
									}
								} );
						}
					}
				}
				else
				{
					break;
				}

			}
		}

		/* =====[ private methods ]===== */

		private byte[] readBuffer()
		{
			// find response header.
			if( mCommand != 0 )
			{
				for( int i = 0; i < mDataLength; ++i )
				{
					if( mData[i] == mCommand )
					{
						if( i > 0 )
						{
							// shift buffer.
							for( int j = i; j < mDataLength; ++j )
							{
								mData[j - i] = mData[j];
							}
							mDataLength -= i;
						}
						break;
					}
				}
				if( mData[0] == mCommand )
				{
					return handleData();
				}
				else
				{
					mDataLength = 0;
				}
			}
			else
			{
				return handleData();
			}

			//
			return null;
		}

		private byte[] handleData()
		{
			int length = 1;
			if( mData[0] == ( byte )0xfa )
			{
				length = 1;
			}
			else if( mData[0] == ( byte )0xfb )
			{
				length = 1;
			}
			else if( mData[0] == ( byte )0xfc )
			{
				length = 2;
			}
			else if( mData[0] == ( byte )0xfd )
			{
				length = 7;
			}
			else if( mData[0] == ( byte )0xfe )
			{
				length = 2;
			}
			else if( mData[0] == ( byte )0xff )
			{
				length = 1;
			}
			else if( mData[0] == ( byte )0xf0 )
			{
				length = 17;
			}
			else if( mData[0] == ( byte )0xf2 )
			{
				length = 8;
			}
			else if( mData[0] == ( byte )0xf9 )
			{
				length = 8;
			}
			else if( mData[0] == ( byte )0xf3 )
			{
				length = 11;
			}
			else if( mData[0] == ( byte )0xf4 )
			{
				length = 15;
			}
			else if( mData[0] == ( byte )0xf5 )
			{
				length = 15;
			}
			else if( mData[0] == ( byte )0xef )
			{
				length = 2;
			}
			else if( mData[0] == ( byte )0xee )
			{
				length = 1;
			}
			else if( mData[0] == ( byte )0xe0 )
			{
				length = 2;
			}
			if( mDataLength >= length )
			{
				byte[] response = new byte[length];
				for( int i = 0; i < length; ++i )
				{
					response[i] = mData[i];
				}
				for( int i = length; i < mDataLength; ++i )
				{
					mData[i - length] = mData[i];
				}
				mDataLength -= length;

				//
				return response;
			}

			//
			return null;
		}

		private bool validateChecksum( int length )
		{
			if( length >= 0 && length < mData.Length )
			{
				byte sum = 0;
				for( int i = 0; i < length; ++i )
				{
					sum += mData[i];
				}
				return ( mData[length] == sum );
			}
			return false;
		}

		private void validateData()
		{
			// validate data.
			if( mMode == MODE_PREPARING || mMode == MODE_COUNTING )
			{
				List<PeakModel> peaks = PeakModel.buildList( mUnits );
				if( PeakModel.prepare( peaks ) )
				{
					if( mMode == MODE_PREPARING )
					{
						// start count down.
						mMode = MODE_COUNTING;
						mStartTime = DateTime.Now.Ticks;
						mCount = 0;
						DispatcherQueue.TryEnqueue(
							() =>
							{
								wMessageText.Text = "Countdown: Monitor Starting";
							} );
					}
				}
				else
				{
					if( mMode == MODE_COUNTING )
					{
						// retry preparing.
						mMode = MODE_PREPARING;
						DispatcherQueue.TryEnqueue(
							() =>
							{
								wMessageText.Text = "Preparing: Monitor Starting";
							} );
					}
				}
			}
			else if( mMode == MODE_MONITORING )
			{
				List<PeakModel> peaks = PeakModel.buildList( mUnits );
				int validity = PeakModel.validate( peaks );
				if( validity > mState )
				{
					// show message.
					mState = validity;
					DispatcherQueue.TryEnqueue(
						() =>
						{
							if( mState == PeakModel.STATE_WARNING )
							{
								wMessageText.Text = "Warning: Monitor Issue Detected";
							}
							else if( mState == PeakModel.STATE_ERROR )
							{
								mediaPlayer.Source = MediaSource.CreateFromUri( new Uri( "ms-appx:///Assets/Beep.wav" ) );
								mediaPlayer.Play();
								wMessageText.Text = "Error: Monitor Issue Detected";
							}
							wMessageBoard.Visibility = Visibility.Visible;
						} );
				}
			}
		}
	}
}
