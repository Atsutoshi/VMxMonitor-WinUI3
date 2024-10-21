using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging ;
using VMxMonitor.models;
using Windows.Foundation;
using Microsoft.UI;
using Windows.UI;

namespace VMxMonitor.controls
{
    public class CommonSheet : Canvas
    {
        /* =====[ constants ]===== */

        protected const double PAPER_WIDTH = 210.0;
        protected const double PAPER_HEIGHT = 297.0;
        protected const double PAPER_MARGIN = 15.0;
        protected const double TEXT_HORIZONTAL_MARGIN = 0.5;
        //
        protected const double HEADER_HEIGHT = 10.0;
        protected const double HEADER_WIDTH = 110.0;
        protected const double HEADER_FONT_SIZE = 5.0;
        protected const double HEADER_MARGIN = 5.0;
        protected const double LOGO_WIDTH = 53.8;
        protected const double LOGO_LEFT = 126.2;
        //
        protected const double FOOTER_HEIGHT = 3.0;
        //
        protected const double SECTION_TITLE_HEIGHT = 6.5;
        protected const double SECTION_TITLE_FONT_SIZE = 5.0;
        //
        protected const double PROFILE_TOP = 12.0;
        protected const double PROFILE_HEIGHT = 7.0;
        protected const double PROFILE_FONT_SIZE = 4.2;
        protected const double PROFILE_AGE_SEX_LEFT = 80.0;
        protected const double PROFILE_DATE_WIDTH = 80.0;
        protected const double PROFILE_LINE_THICKNESS = 0.2;
        //
        protected const double INTRODUCTION_LEFT = 111.0;
        protected const double INTRODUCTION_ICON_SIZE = 4.4;
        protected const double INTRODUCTION_ICON_MARGIN = 0.5;
        protected const double INTRODUCTION_FONT_SIZE = 3.8;
        protected const double INTRODUCTION_TEXT_WIDTH = 17.0;
        protected const double INTRODUCTION_PART_SPACE = 1.1;
        //
        private const double COPYRIGHT_LEFT = 80.0;
        private const double COPYRIGHT_TOP = 260.0;
        private const double COPYRIGHT_WIDTH = 100.0;
        private const double COPYRIGHT_HEIGHT = 3.2;
        private const double COPYRIGHT_FONT_SIZE = 2.7;

        /* =====[ properties ]===== */

        protected CommunityModel mCommunity;
        protected UserModel mUser;
        protected double mScale;
        protected double mOriginX;
        protected double mOriginY;
        protected Brush mBlackBrush;
        protected Brush mWhiteBrush;
        protected Brush mThemeBrush;
        protected Brush mTableBackground;

        /* =====[ constructors ]===== */

        public CommonSheet()
        {
            // initialize properties.
            mBlackBrush = new SolidColorBrush(Colors.Black);
            mWhiteBrush = new SolidColorBrush(Colors.White);
            mThemeBrush = new SolidColorBrush(Colors.DarkGreen);
			//   mTableBackground = new SolidColorBrush(Color.FromRgb(232, 255, 186));
			Color color = ColorHelper.FromArgb(255,232, 255,186 );
			mTableBackground = new SolidColorBrush( color );

			// set background color.
			this.Background = new SolidColorBrush(Colors.White);
        }

        /* =====[ protected methods ]===== */

        protected void measure()
        {
            // get width.
            double width = this.Width;
            if (Double.IsNaN(width))
            {
                width = this.ActualWidth;
            }

            // calculate scale.
            double scaleH = width / PAPER_WIDTH;
            double scaleV = this.Height / PAPER_HEIGHT;
            mScale = Math.Min(scaleH, scaleV);

            // calculate origin.
            mOriginX = (width - (PAPER_WIDTH - PAPER_MARGIN * 2.0) * mScale) / 2.0;
            mOriginY = (this.Height - (PAPER_HEIGHT - PAPER_MARGIN * 2.0) * mScale) / 2.0;
        }

        protected void paintFrame()
        {
            // frame.
            Rectangle frame = new();
            double x = mOriginX - PAPER_MARGIN * mScale;
            double y = mOriginY - PAPER_MARGIN * mScale;
            frame.Margin = new Thickness(x, y, 0, 0);
            frame.Width = PAPER_WIDTH * mScale;
            frame.Height = PAPER_HEIGHT * mScale;
            frame.Fill = mWhiteBrush;
            this.Children.Add(frame);
        }

        protected void drawHeader(string title)
        {
            // rectangle.
            Rectangle header = new();
            header.Fill = mThemeBrush;
            header.HorizontalAlignment = HorizontalAlignment.Left;
            header.VerticalAlignment = VerticalAlignment.Top;
            header.Margin = new Thickness(mOriginX, mOriginY, 0, 0);
            header.Width = HEADER_WIDTH * mScale;
            //header.Width = (PAPER_WIDTH - PAPER_MARGIN * 2.0) * mScale;
            header.Height = HEADER_HEIGHT * mScale;
            this.Children.Add(header);

            // header label.
            Rect rect = new (HEADER_MARGIN, 0, 0, HEADER_HEIGHT);
            drawText(title, rect, HEADER_FONT_SIZE, TextAlignment.Left, mWhiteBrush);

            // logo.
            //BitmapImage src = new BitmapImage();
            //src.BeginInit();
            //src.UriSource = new Uri("pack://application:,,,/Resources/logo.png", UriKind.Absolute);
            //src.EndInit();

			BitmapImage src = new( new Uri( "\"pack://application:,,,/Resources/logo.png",UriKind.Absolute ) );
			Image image = new ();
            image.Source = src;
            image.Margin = new Thickness(mOriginX + LOGO_LEFT * mScale, mOriginY, 0, 0);
            image.Width = LOGO_WIDTH * mScale;
            image.Height = HEADER_HEIGHT * mScale;
            this.Children.Add(image);
        }

        protected void drawFooter()
        {
            // rectangle.
            Rectangle footer = new();
            footer.Fill = mThemeBrush;
            footer.HorizontalAlignment = HorizontalAlignment.Left;
            footer.VerticalAlignment = VerticalAlignment.Top;
            footer.Margin = new Thickness(mOriginX, (PAPER_HEIGHT - PAPER_MARGIN - FOOTER_HEIGHT) * mScale, 0, 0);
            footer.Width = (PAPER_WIDTH - PAPER_MARGIN * 2.0) * mScale;
            footer.Height = FOOTER_HEIGHT * mScale;
            this.Children.Add(footer);

            // copyright.
            Rect rect = new(COPYRIGHT_LEFT, COPYRIGHT_TOP, COPYRIGHT_WIDTH, COPYRIGHT_HEIGHT);
            drawText("Copyright © Fatigue Science Laboratory Inc. 2017, All Rights reserved.", rect, COPYRIGHT_FONT_SIZE, TextAlignment.Right, mBlackBrush);
        }

        protected void drawProfile(string datetime)
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );
			// profile.
			Rect rect = new(0, PROFILE_TOP, 0, PROFILE_HEIGHT);
          //  string age = String.Format(Properties.Resources.labelAge, mUser.Age);
            string age = String.Format( resourceLoader.GetString( "labelAge" ), mUser.Age);
		

			string sex = "";
            if (mUser.Sex == UserModel.SEX_MALE)
            {
               // sex = Properties.Resources.labelMale;
                sex = resourceLoader.GetString( "labelMale" );
			}
            else if (mUser.Sex == UserModel.SEX_FEMALE)
            {
              //  sex = Properties.Resources.labelFemale;
                sex = resourceLoader.GetString( "labelFemale" );
			}
            if (mCommunity != null)
            {
                drawText(String.Format("ID: {0} <{1}> / {2} {3}", mUser.Name, mCommunity.Name, age, sex), rect, PROFILE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            }
            else
            {
                drawText(String.Format("ID: {0} / {1} {2}", mUser.Name, age, sex), rect, PROFILE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            }
            //
            rect = new Rect((PAPER_WIDTH - PAPER_MARGIN * 2) - PROFILE_DATE_WIDTH, PROFILE_TOP, PROFILE_DATE_WIDTH, PROFILE_HEIGHT);
           // drawText(String.Format("{0}: {1}", Properties.Resources.labelMeasuredAt, datetime), rect, PROFILE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            drawText(String.Format("{0}: {1}", resourceLoader.GetString( "labelMeasuredAt" ), datetime), rect, PROFILE_FONT_SIZE, TextAlignment.Right, mBlackBrush);

            // line.
            Line line = new ();
            line.Stroke = mBlackBrush;
            line.X1 = mOriginX;
            line.Y1 = mOriginY + (PROFILE_TOP + PROFILE_HEIGHT) * mScale;
            line.X2 = mOriginX + (PAPER_WIDTH - PAPER_MARGIN * 2.0) * mScale;
            line.Y2 = line.Y1;
            line.StrokeThickness = PROFILE_LINE_THICKNESS * mScale;
            this.Children.Add(line);

            // introduction.
            double x = INTRODUCTION_LEFT;
            double y = PROFILE_TOP + PROFILE_HEIGHT + INTRODUCTION_ICON_MARGIN;
            {
                //BitmapImage src = new BitmapImage();
                //src.BeginInit();
                //src.UriSource = new Uri("pack://application:,,,/Resources/resultBlue.png", UriKind.Absolute);
                //src.EndInit();
				BitmapImage src = new ( new Uri( "\"pack://application:,,,/Resources/resultBlue.png", UriKind.Absolute ) );


				Image image = new ();
                image.Source = src;
                image.Margin = new Thickness(mOriginX + x * mScale, mOriginY + y * mScale, 0, 0);
                image.Width = image.Height = INTRODUCTION_ICON_SIZE * mScale;
                this.Children.Add(image);
            }
            x += INTRODUCTION_ICON_SIZE + INTRODUCTION_ICON_MARGIN;
            rect = new Rect(x, y, INTRODUCTION_TEXT_WIDTH, INTRODUCTION_ICON_SIZE);
           // drawText(Properties.Resources.labelResultGood, rect, INTRODUCTION_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelResultGood" ), rect, INTRODUCTION_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			x += INTRODUCTION_TEXT_WIDTH + INTRODUCTION_PART_SPACE;
            {
                //BitmapImage src = new BitmapImage();
                //src.BeginInit();
                //src.UriSource = new Uri("pack://application:,,,/Resources/resultYellow.png", UriKind.Absolute);
                //src.EndInit();
				BitmapImage src = new ( new Uri( "\"pack://application:,,,/Resources/resultYellow.png", UriKind.Absolute ) );
				Image image = new ();
                image.Source = src;
                image.Margin = new Thickness(mOriginX + x * mScale, mOriginY + y * mScale, 0, 0);
                image.Width = image.Height = INTRODUCTION_ICON_SIZE * mScale;
                this.Children.Add(image);
            }
            x += INTRODUCTION_ICON_SIZE + INTRODUCTION_ICON_MARGIN;
            rect = new Rect(x, y, INTRODUCTION_TEXT_WIDTH, INTRODUCTION_ICON_SIZE);
           // drawText(Properties.Resources.labelResultWarning, rect, INTRODUCTION_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelResultWarning" ), rect, INTRODUCTION_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += INTRODUCTION_TEXT_WIDTH + INTRODUCTION_PART_SPACE;
            {
                //BitmapImage src = new BitmapImage();
                //src.BeginInit();
                //src.UriSource = new Uri("pack://application:,,,/Resources/resultOrange.png", UriKind.Absolute);
                //src.EndInit();
				BitmapImage src = new ( new Uri( "\"pack://application:,,,/Resources/resultOrange.png", UriKind.Absolute ) );
				Image image = new ();
                image.Source = src;
                image.Margin = new Thickness(mOriginX + x * mScale, mOriginY + y * mScale, 0, 0);
                image.Width = image.Height = INTRODUCTION_ICON_SIZE * mScale;
                this.Children.Add(image);
            }
            x += INTRODUCTION_ICON_SIZE + INTRODUCTION_ICON_MARGIN;
            rect = new Rect(x, y, INTRODUCTION_TEXT_WIDTH, INTRODUCTION_ICON_SIZE);
            //drawText(Properties.Resources.labelResultBad, rect, INTRODUCTION_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelResultBad" ), rect, INTRODUCTION_FONT_SIZE, TextAlignment.Left, mBlackBrush);
		}

        protected TextBlock drawText(string text, Rect bounds, double fontSize, TextAlignment alignment, Brush brush)
        {
            // create text block.
            TextBlock textBlock = new ();
            double top = bounds.Top;
            if (bounds.Height > 0)
            {
                //top += (bounds.Height - textBlock.FontFamily.LineSpacing * fontSize) / 2.0;
				// 計算でFontSizeを使用し、適切に垂直方向の位置を調整
				top += ( bounds.Height - fontSize ) / 2.0;
			}
            textBlock.Text = text;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Margin = new Thickness(mOriginX + (bounds.Left + TEXT_HORIZONTAL_MARGIN) * mScale, mOriginY + top * mScale, 0, 0);
            if (bounds.Width > 0)
            {
                textBlock.Width = (bounds.Width - TEXT_HORIZONTAL_MARGIN * 2.0) * mScale;
            }
            textBlock.FontSize = fontSize * mScale;
            textBlock.TextAlignment = alignment;
            textBlock.TextWrapping = TextWrapping.Wrap;
            if (brush != null)
            {
                textBlock.Foreground = brush;
            }

            // locate.
            this.Children.Add(textBlock);

            //
            return textBlock;
        }
    }
}
