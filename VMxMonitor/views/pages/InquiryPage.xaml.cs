using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using VM_app_221_WAS.services;
using VMxMonitor.controls;
using VMxMonitor.models;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VMxMonitor.views.pages
{
	public sealed partial class InquiryPage : UserControl
	{
		private const double ITEM_HEIGHT = 45.0;
		private const double INPUT_HEIGHT = 55.0;
		private const int ANIMATION_DURATION = 300;

		public event EventHandler<InquiryEventArgs> Finish;
		public event RoutedEventHandler Cancel;

		private DataModel mDataModel;
		private UserModel mUser;
		private int[] mAnswers;
		private double[] mSleeps;
		private int mCondition;
		private string[] mDiseases;
		private int mPage;
		private int mChoiceCount;
		private int mPageCount;
		private int[,] PAGES;

		public InquiryPage( DataModel model, UserModel user )
		{
			this.InitializeComponent();
			mDataModel = model;
			mUser = user;
			mAnswers = new int[42];
			for( int i = 0; i < 42; ++i )
			{
				mAnswers[i] = -1;
			}
			mSleeps = new double[] { -1, -1 };
			mCondition = -1;
			mDiseases = new string[] { "", null, null };
			mPage = 1;
		}

		private void OnLoad( object sender, RoutedEventArgs e )
		{
			mChoiceCount = ( int )Math.Floor( ( wScrollViewer.ActualHeight - 3.0 ) / ITEM_HEIGHT );
			int choicePages = ( 38 + ( mChoiceCount - 1 ) ) / mChoiceCount;
			int itemCount = Math.Max( mChoiceCount, 6 );
			mPageCount = choicePages + 3;
			PAGES = new int[mPageCount, itemCount + 1];
			for( int i = 0; i < mPageCount; ++i )
			{
				if( i < choicePages )
				{
					for( int j = 0; j < mChoiceCount; ++j )
					{
						if( ( PAGES[i, j] = ( mChoiceCount * i ) + j + 1 ) > 38 )
						{
							PAGES[i, j] = 0;
						}
					}
				}
				else if( i == choicePages )
				{
					for( int j = 0; j < 6; ++j )
					{
						PAGES[i, j] = 39 + j;
					}
				}
				else if( i == choicePages + 1 )
				{
					PAGES[i, 0] = 45;
				}
				else
				{
					PAGES[i, 0] = 46;
					PAGES[i, 1] = 47;
					PAGES[i, 2] = 48;
				}
			}
			setUpPage();
		}

		private void OnAnswer( object sender, RoutedEventArgs e )
		{
			if( sender is InquiryChoice choice )
			{
				mAnswers[choice.Index - 1] = choice.Choice;
			}
			else if( sender is InquiryYesNo yesNo )
			{
				mAnswers[38] = yesNo.YesNo;
			}
			else if( sender is InquiryPeriod period )
			{
				mAnswers[39] = period.Choice;
			}
			else if( sender is InquiryNumber input )
			{
				if( input.Index <= 42 )
				{
					mAnswers[input.Index - 1] = ( int )input.Value;
				}
				else
				{
					mSleeps[input.Index - 43] = input.Value;
				}
			}
			else if( sender is InquirySelect select )
			{
				mCondition = select.Select;
			}
			else if( sender is InquiryDisease9 disease9 )
			{
				mDiseases[0] = disease9.Value;
			}
			else if( sender is InquiryDisease2 disease2 )
			{
				mDiseases[disease2.Index - 46] = disease2.Value;
			}
			validate();
		}

		private void OnNextButton( object sender, RoutedEventArgs e )
		{
			if( mPage < mPageCount )
			{
				slideOut( true );
			}
			else
			{
				//finish();
			}
		}

		private void OnPreviousButton( object sender, RoutedEventArgs e )
		{
			slideOut( false );
		}

		private void OnCancelButton( object sender, RoutedEventArgs e )
		{
			Cancel?.Invoke( this, e );
		}

		private void slideOut( bool isForward )
		{
			Storyboard storyboard = new Storyboard();
			TimeSpan duration = new TimeSpan( 0, 0, 0, 0, ANIMATION_DURATION );
			foreach( UIElement element in wPanel.Children )
			{
				if( element is UserControl control )
				{
					TranslateTransform translateTransform = new TranslateTransform();
					control.RenderTransform = translateTransform;

					DoubleAnimation animation = isForward
						? new DoubleAnimation { To = -wPanel.ActualWidth, Duration = duration }
						: new DoubleAnimation { To = wPanel.ActualWidth, Duration = duration };
					Storyboard.SetTarget( animation, control );
					Storyboard.SetTargetProperty( animation, "(UIElement.RenderTransform).(TranslateTransform.X)" );
					storyboard.Children.Add( animation );
				}
			}
			storyboard.FillBehavior = FillBehavior.Stop;
			storyboard.Completed += ( o, e ) =>
			{
				foreach( UIElement element in wPanel.Children )
				{
					element.Visibility = Visibility.Collapsed;
				}

				if( isForward )
				{
					++mPage;
				}
				else
				{
					--mPage;
				}
				setUpPage();
				slideIn( isForward );
			};
			storyboard.Begin();
		}

		private void slideIn( bool isForward )
		{
			Storyboard storyboard = new Storyboard();
			TimeSpan duration = new TimeSpan( 0, 0, 0, 0, ANIMATION_DURATION );

			foreach( UIElement element in wPanel.Children )
			{
				if( element is UserControl control )
				{
					TranslateTransform translateTransform = new TranslateTransform();
					control.RenderTransform = translateTransform;

					control.Visibility = Visibility.Visible;

					DoubleAnimation animation = isForward
						? new DoubleAnimation { From = wPanel.ActualWidth, To = 0, Duration = new Duration( TimeSpan.FromMilliseconds( ANIMATION_DURATION ) ) }
						: new DoubleAnimation { From = -wPanel.ActualWidth, To = 0, Duration = new Duration( TimeSpan.FromMilliseconds( ANIMATION_DURATION ) ) };
					Storyboard.SetTarget( animation, control );
					Storyboard.SetTargetProperty( animation, "(UIElement.RenderTransform).(TranslateTransform.X)" );
					storyboard.Children.Add( animation );
				}
			}
			storyboard.Begin();
		}

		private void setUpPage()
		{
			wPanel.Children.Clear();
			int i = 0;
			int index;
			while( ( index = PAGES[mPage - 1, i] ) != 0 )
			{
				UserControl control = null;
				if( index <= 38 )
				{
					control = new InquiryChoice( index, mAnswers[index - 1] );
					control.Height = ITEM_HEIGHT;
					( ( InquiryChoice )control ).Check += new RoutedEventHandler( OnAnswer );
				}
				else if( index == 39 )
				{
					control = new InquiryYesNo( mAnswers[index - 1] );
					control.Height = ITEM_HEIGHT;
					( ( InquiryYesNo )control ).Check += new RoutedEventHandler( OnAnswer );
				}
				else if( index == 40 && mUser.Sex == UserModel.SEX_FEMALE )
				{
					control = new InquiryPeriod( mAnswers[index - 1] );
					control.Height = ITEM_HEIGHT;
					( ( InquiryPeriod )control ).Check += new RoutedEventHandler( OnAnswer );
				}
				else if( index >= 41 && index <= 42 )
				{
					control = new InquiryNumber( index, mAnswers[index - 1] );
					control.Height = INPUT_HEIGHT;
					( ( InquiryNumber )control ).ValueChanged += new RoutedEventHandler( OnAnswer );
				}
				else if( index >= 43 && index <= 44 )
				{
					control = new InquiryNumber( index, mSleeps[index - 43] );
					control.Height = INPUT_HEIGHT;
					( ( InquiryNumber )control ).ValueChanged += new RoutedEventHandler( OnAnswer );
				}
				else if( index == 45 )
				{
					control = new InquirySelect( mCondition );
					( ( InquirySelect )control ).Check += new RoutedEventHandler( OnAnswer );
				}
				else if( index == 46 )
				{
					control = new InquiryDisease9( mDiseases[0] );
					( ( InquiryDisease9 )control ).Change += new RoutedEventHandler( OnAnswer );
				}
				else if( index >= 47 && index <= 48 )
				{
					control = new InquiryDisease2( index, mDiseases[index - 46] );
					( ( InquiryDisease2 )control ).Change += new RoutedEventHandler( OnAnswer );
				}

				if( control != null )
				{
					configureWidget( control );
					wPanel.Children.Add( control );
				}
				++i;
			}

			wPreviousButton.Visibility = mPage == 1 ? Visibility.Collapsed : Visibility.Visible;
			wNextButton.Content = mPage < mPageCount ? "Next" : "Finish";
			validate();

			wPromptText.Text = PAGES[mPage - 1, 0] <= 30 ? "Prompt 1" : "Prompt 2";
			wPageText.Text = $"{mPage} / {mPageCount}";
		}

		private void configureWidget( UserControl control )
		{
			control.HorizontalAlignment = HorizontalAlignment.Left;
			control.VerticalAlignment = VerticalAlignment.Top;
			control.Width = wPanel.ActualWidth;
		}

		private void validate()
		{
			bool isValid = true;
			if( mPage == mPageCount )
			{
				for( int i = 0; i < 3; ++i )
				{
					if( mDiseases[i] == null )
					{
						isValid = false;
						break;
					}
				}
			}
			else if( mPage == mPageCount - 1 )
			{
				if( mCondition < 0 )
				{
					isValid = false;
				}
			}
			else if( mPage == mPageCount - 2 )
			{
				for( int i = 0; i < 2; ++i )
				{
					if( mAnswers[PAGES[mPage - 1, i] - 1] < 0 )
					{
						isValid = false;
						break;
					}
				}
				for( int i = 0; i < 2; ++i )
				{
					if( mSleeps[i] < 0 )
					{
						isValid = false;
						break;
					}
				}
			}
			else
			{
				int i = 0;
				while( PAGES[mPage - 1, i] != 0 )
				{
					if( mAnswers[PAGES[mPage - 1, i] - 1] < 0 )
					{
						isValid = false;
						break;
					}
					++i;
				}
			}
			wNextButton.IsEnabled = isValid;
		}
	}
}