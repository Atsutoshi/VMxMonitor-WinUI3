// MainWindow.xaml.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using System;
using VMxMonitor.models;
using VMxMonitor.pages;
using VMxMonitor.views.pages;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System.Collections.Generic;
using VM_app_221_WAS.services;
using System.Runtime.InteropServices;

namespace VMxMonitor
{
	public sealed partial class MainWindow : Window
	{
		private const int TRANSITION_DURATION = 350;
		private DataModel mDataModel;
		private CommunityModel mCommunity;
		private UserModel mUser;
		private int mHistory;
		private AppWindow appWindow;

		// MainPage のインスタンスを保持
		private MainPage mainPage;

		public MainWindow()
		{
			this.InitializeComponent();
			InitializeAppWindow();
			mDataModel = DataModel.create();
			mCommunity = null;
			mUser = null;
			mHistory = 0;

			// MainPage を作成し、ウィンドウのコンテンツとして設定
			mainPage = new MainPage();
			this.Content = mainPage;

			// イベントハンドラーを登録
			this.Activated += OnWindowActivated;
			this.SizeChanged += OnResizeWindow;
		}

		private void InitializeAppWindow()
		{
			var hwnd = WinRT.Interop.WindowNative.GetWindowHandle( this );
			var windowId = Win32Interop.GetWindowIdFromWindow( hwnd );
			appWindow = AppWindow.GetFromWindowId( windowId );

			if( appWindow != null )
			{
				var presenter = appWindow.Presenter as OverlappedPresenter;
				if( presenter != null )
				{
					presenter.IsMaximizable = true;
					presenter.IsMinimizable = true;
					presenter.IsResizable = true;
				}
			}

			appWindow.Resize( new Windows.Graphics.SizeInt32( 800, 600 ) );
		}

		private void OnWindowActivated( object sender, WindowActivatedEventArgs e )
		{
			// ウィンドウが非アクティブ化されたときは何もしない
			if( e.WindowActivationState == WindowActivationState.Deactivated )
			{
				return;
			}

			mainPage.wVersionText.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			mainPage.wProfileBoard.Visibility = Visibility.Collapsed;

			if( !IsAuthorized() )
			{
				OnQuitApplication( null, null );
				return;
			}

			int tab = EntrancePage.COMMUNITY_TAB;
			foreach( string arg in Environment.GetCommandLineArgs() )
			{
				if( arg == "--user" )
				{
					tab = EntrancePage.USER_TAB;
				}
			}

			var page = new EntrancePage( mDataModel, tab )
			{
				Width = mainPage.wContainer.ActualWidth,
				Height = mainPage.wContainer.ActualHeight,
				Margin = new Thickness( 0 )
			};
			page.SelectUser += OnSelectUser;
			page.SelectCommunity += OnSelectCommunity;
			page.Quit += OnQuitApplication;
			mainPage.wContainer.Children.Add( page );

			InvalidateLayout();
		}

		public void InvalidateLayout()
		{
			try
			{
				if( this.Content is FrameworkElement rootElement )
				{
					rootElement.InvalidateArrange();
					rootElement.InvalidateMeasure();
				}
			}
			catch( COMException )
			{
				// ウィンドウが既に閉じられている場合は例外を無視
			}

		}

		private void OnResizeWindow( object sender, WindowSizeChangedEventArgs e )
		{
			foreach( UIElement element in mainPage.wContainer.Children )
			{
				if( element is UserControl page )
				{
					page.Width = mainPage.wContainer.ActualWidth;
					page.Height = mainPage.wContainer.ActualHeight;
				}
			}
		}

		private void OnSelectCommunity( object sender, CommunityEventArgs e )
		{
			mCommunity = e.community;

			var page = new CommunityPage( mDataModel, e.community );
			page.SelectUser += OnSelectUser;
			page.Cancel += OnCancelCommunity;
			Forward( page );
		}

		private void OnSelectUser( object sender, UserEventArgs e )
		{
			// ユーザーを取得
			mUser = e.User;
			if( mUser.CommunityId == 0 )
			{
				mCommunity = null;
			}

			// ユーザーアイコンを読み込む
			mainPage.wUserIcon.Source = new Microsoft.UI.Xaml.Media.Imaging.BitmapImage( new Uri( mUser.Icon.ToString(), UriKind.Absolute ) );

			mainPage.wUserName.Text = mUser.Name;
			mainPage.wUserAge.Text = $"Age: {mUser.Age}";
			if( mUser.Sex == UserModel.SEX_MALE )
			{
				mainPage.wUserSex.Text = "Male";
			}
			else if( mUser.Sex == UserModel.SEX_FEMALE )
			{
				mainPage.wUserSex.Text = "Female";
			}
			else
			{
				mainPage.wUserSex.Text = "";
			}
			if( mCommunity != null )
			{
				mainPage.wCommunityName.Text = mCommunity.Name;
			}
			else
			{
				mainPage.wCommunityName.Text = "";
			}

			// プロファイルを表示
			mainPage.wProfileBoard.Visibility = Visibility.Visible;

			// メニューを開く
			MenuPage menu = new MenuPage();
			menu.StartMonitor += OnStartMonitor;
			menu.ListMonitor += OnListMonitor;
			menu.StartInquiry += OnOpenInquiry;
			menu.ListInquiry += OnListInquiry;
			menu.Finish += OnLogout;
			Forward( menu );
		}

		private void OnStartMonitor( object sender, RoutedEventArgs e )
		{
			if( !IsAuthorized() ) return;

			var monitor = new MonitorPage( mDataModel, mUser );
			monitor.Finish += OnOpenMonitorResult;
			monitor.Cancel += OnReturn;
			Forward( monitor );
			mHistory = 0;
		}

		private void OnListMonitor( object sender, RoutedEventArgs e )
		{
			var history = new HistoryPage( mDataModel, mUser, HistoryPage.HISTORY_MONITOR );
			history.SelectMonitor += OnOpenMonitorResult;
			history.Cancel += OnReturn;
			Forward( history );
			mHistory = HistoryPage.HISTORY_MONITOR;
		}

		private void OnOpenMonitorResult( object sender, MonitorEventArgs e )
		{
			var page = new ResultPage( mDataModel, mCommunity, mUser, e.Monitor );
			page.Finish += OnFinishResult;
			Forward( page );
			mainPage.wProfileBoard.Visibility = Visibility.Collapsed;
		}

		private void OnOpenInquiry( object sender, RoutedEventArgs e )
		{
			if( !IsAuthorized() ) return;

			var inquiry = new InquiryPage( mDataModel, mUser );
			inquiry.Finish += OnOpenInquiryResult;
			inquiry.Cancel += OnReturn;
			Forward( inquiry );
			mHistory = 0;
		}

		private void OnListInquiry( object sender, RoutedEventArgs e )
		{
			var history = new HistoryPage( mDataModel, mUser, HistoryPage.HISTORY_INQUIRY );
			history.SelectInquiry += OnOpenInquiryResult;
			history.Cancel += OnReturn;
			Forward( history );
			mHistory = HistoryPage.HISTORY_INQUIRY;
		}

		private void OnOpenInquiryResult( object sender, InquiryEventArgs e )
		{
			var page = new ResultPage( mDataModel, mCommunity, mUser, e.Inquiry );
			page.Finish += OnFinishResult;
			Forward( page );
			mainPage.wProfileBoard.Visibility = Visibility.Collapsed;
		}

		private void OnFinishResult( object sender, RoutedEventArgs e )
		{
			if( mHistory == 0 )
			{
				OnReturn( sender, e );
			}
			else
			{
				var history = new HistoryPage( mDataModel, mUser, mHistory );
				history.SelectMonitor += OnOpenMonitorResult;
				history.SelectInquiry += OnOpenInquiryResult;
				history.Cancel += OnReturn;
				Backward( history );
			}

			mainPage.wProfileBoard.Visibility = Visibility.Visible;
		}

		private void OnReturn( object sender, RoutedEventArgs e )
		{
			var menu = new MenuPage();
			menu.StartMonitor += OnStartMonitor;
			menu.ListMonitor += OnListMonitor;
			menu.StartInquiry += OnOpenInquiry;
			menu.ListInquiry += OnListInquiry;
			menu.Finish += OnLogout;
			Backward( menu );
		}

		private void OnLogout( object sender, RoutedEventArgs e )
		{
			if( mCommunity != null )
			{
				var page = new CommunityPage( mDataModel, mCommunity );
				page.SelectUser += OnSelectUser;
				page.Cancel += OnCancelCommunity;
				Backward( page );
			}
			else
			{
				var page = new EntrancePage( mDataModel, EntrancePage.USER_TAB );
				page.SelectUser += OnSelectUser;
				page.SelectCommunity += OnSelectCommunity;
				page.Quit += OnQuitApplication;
				Backward( page );
				mCommunity = null;
				mUser = null;
			}
			mainPage.wProfileBoard.Visibility = Visibility.Collapsed;
		}

		private void OnCancelCommunity( object sender, RoutedEventArgs e )
		{
			var page = new EntrancePage( mDataModel, EntrancePage.COMMUNITY_TAB );
			page.SelectUser += OnSelectUser;
			page.SelectCommunity += OnSelectCommunity;
			page.Quit += OnQuitApplication;
			Backward( page );
			mUser = null;
		}

		private void OnQuitApplication( object sender, RoutedEventArgs e )
		{
			this.Close();
		}

		private bool IsAuthorized()
		{
			// 認証処理をここに実装
			return true;
		}

		private void Forward( UserControl to )
		{
			mainPage.wTransitionScreen.Visibility = Visibility.Visible;

			var controls = new List<UserControl>();
			foreach( UIElement element in mainPage.wContainer.Children )
			{
				if( element is UserControl control )
				{
					controls.Add( control );
				}
			}

			var destination = new Thickness( 0 );
			to.Width = mainPage.wContainer.ActualWidth;
			to.Height = mainPage.wContainer.ActualHeight;
			to.Margin = new Thickness( mainPage.wContainer.ActualWidth, 0, 0, 0 );
			mainPage.wContainer.Children.Add( to );

			// アニメーション対象にRenderTransformを追加
			to.RenderTransform = new TranslateTransform();
			foreach( var control in controls )
			{
				control.RenderTransform = new TranslateTransform();
			}

			var storyboard = new Storyboard();
			var duration = new TimeSpan( 0, 0, 0, 0, TRANSITION_DURATION );

			foreach( var control in controls )
			{
				var animation = new DoubleAnimation
				{
					From = 0,
					To = -mainPage.wContainer.ActualWidth,
					Duration = duration
				};
				Storyboard.SetTarget( animation, control );
				Storyboard.SetTargetProperty( animation, "(UIElement.RenderTransform).(TranslateTransform.X)" );
				storyboard.Children.Add( animation );
			}

			var forwardAnimation = new DoubleAnimation
			{
				From = mainPage.wContainer.ActualWidth,
				To = 0,
				Duration = duration
			};
			Storyboard.SetTarget( forwardAnimation, to );
			Storyboard.SetTargetProperty( forwardAnimation, "(UIElement.RenderTransform).(TranslateTransform.X)" );
			storyboard.Children.Add( forwardAnimation );

			storyboard.FillBehavior = FillBehavior.Stop;
			storyboard.Completed += ( s, e ) =>
			{
				to.Margin = destination;

				foreach( var control in controls )
				{
					control.Visibility = Visibility.Collapsed;
					mainPage.wContainer.Children.Remove( control );
				}

				mainPage.wTransitionScreen.Visibility = Visibility.Collapsed;
			};
			storyboard.Begin();
		}

		private void Backward( UserControl to )
		{
			mainPage.wTransitionScreen.Visibility = Visibility.Visible;

			var controls = new List<UserControl>();
			foreach( UIElement element in mainPage.wContainer.Children )
			{
				if( element is UserControl control )
				{
					controls.Add( control );
				}
			}

			var destination = new Thickness( 0 );
			to.Width = mainPage.wContainer.ActualWidth;
			to.Height = mainPage.wContainer.ActualHeight;
			to.Margin = new Thickness( -mainPage.wContainer.ActualWidth, 0, 0, 0 );
			mainPage.wContainer.Children.Insert( 0, to );

			// アニメーション対象にRenderTransformを追加
			to.RenderTransform = new TranslateTransform();
			foreach( var control in controls )
			{
				control.RenderTransform = new TranslateTransform();
			}

			var storyboard = new Storyboard();
			var duration = new TimeSpan( 0, 0, 0, 0, TRANSITION_DURATION );

			foreach( var control in controls )
			{
				var animation = new DoubleAnimation
				{
					From = 0,
					To = mainPage.wContainer.ActualWidth,
					Duration = duration
				};
				Storyboard.SetTarget( animation, control );
				Storyboard.SetTargetProperty( animation, "(UIElement.RenderTransform).(TranslateTransform.X)" );
				storyboard.Children.Add( animation );
			}

			var backwardAnimation = new DoubleAnimation
			{
				From = -mainPage.wContainer.ActualWidth,
				To = 0,
				Duration = duration
			};
			Storyboard.SetTarget( backwardAnimation, to );
			Storyboard.SetTargetProperty( backwardAnimation, "(UIElement.RenderTransform).(TranslateTransform.X)" );
			storyboard.Children.Add( backwardAnimation );

			storyboard.FillBehavior = FillBehavior.Stop;
			storyboard.Completed += ( s, e ) =>
			{
				to.Margin = destination;

				foreach( var control in controls )
				{
					control.Visibility = Visibility.Collapsed;
					mainPage.wContainer.Children.Remove( control );
				}

				mainPage.wTransitionScreen.Visibility = Visibility.Collapsed;
			};
			storyboard.Begin();
		}
	}
}
