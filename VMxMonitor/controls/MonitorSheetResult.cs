using System;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using System.Threading;
using Microsoft.UI;
using Windows.UI;

namespace VMxMonitor.controls
{
	public partial class MonitorSheet : CommonSheet
    {
        /* =====[ private methods ]===== */

        private void drawResult()
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );

			// title.
			Rect rect = new Rect(0, RESULT_TITLE_TOP, 0, SECTION_TITLE_HEIGHT);
          //  drawText(Properties.Resources.labelSectionResult, rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);labelResultBad
            drawText( resourceLoader.GetString( "labelSectionResult" ), rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			
			// table.
			double x = 0;
            double[] cols = new double[5]
            {
                RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_STANDARD_WIDTH, 0
            };
            {
                double yy = RESULT_TABLE_TOP + RESULT_TABLE_HEADER_HEIGHT + RESULT_TABLE_ROW_HEIGHT;
                Rectangle left = new Rectangle();
                left.Fill = mTableBackground;
                left.Margin = new Thickness(mOriginX, mOriginY + yy * mScale, 0, 0);
                left.Width = RESULT_TABLE_TITLE_WIDTH * mScale;
                left.Height = RESULT_TABLE_ROW_HEIGHT * 10 * mScale;
                this.Children.Add(left);
                double xx = RESULT_TABLE_TITLE_WIDTH + RESULT_TABLE_OVERVIEW_WIDTH;
                Rectangle right = new Rectangle();
                right.Fill = mTableBackground;
                right.Margin = new Thickness(mOriginX + xx * mScale, mOriginY + yy * mScale, 0, 0);
                right.Width = RESULT_TABLE_RESULT_WIDTH * mScale;
                right.Height = RESULT_TABLE_ROW_HEIGHT * 10 * mScale;
                this.Children.Add(right);
            }
            for (int i = 0; i < 5; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.X1 = mOriginX + x * mScale;
                if (i == 0 || i == 4)
                {
                    line.Y1 = mOriginY + RESULT_TABLE_TOP * mScale;
                }
                else
                {
                    line.Y1 = mOriginY + (RESULT_TABLE_TOP + RESULT_TABLE_HEADER_HEIGHT) * mScale;
                }
                line.X2 = line.X1;
                if (i == 0 || i == 4)
                {
                    line.Y2 = line.Y1 + (RESULT_TABLE_HEADER_HEIGHT + RESULT_TABLE_ROW_HEIGHT * 11) * mScale;
                }
                else
                {
                    line.Y2 = line.Y1 + (RESULT_TABLE_ROW_HEIGHT * 11) * mScale;
                }
                line.StrokeThickness = RESULT_TABLE_LINE_THICKNESS * mScale;
                this.Children.Add(line);
                x += cols[i];
            }
            double y = RESULT_TABLE_TOP;
            for (int i = 0; i < 13; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.X1 = mOriginX;
                line.Y1 = mOriginY + y * mScale;
                if (i == 3 || i == 4 || i == 9)
                {
                    line.X2 = line.X1 + (RESULT_TABLE_WIDTH - RESULT_TABLE_STANDARD_WIDTH) * mScale;
                }
                else
                {
                    line.X2 = line.X1 + RESULT_TABLE_WIDTH * mScale;
                }
                line.Y2 = line.Y1;
                line.StrokeThickness = RESULT_TABLE_LINE_THICKNESS * mScale;
                this.Children.Add(line);
                if (i == 0)
                {
                    y += RESULT_TABLE_HEADER_HEIGHT;
                }
                else
                {
                    y += RESULT_TABLE_ROW_HEIGHT;
                }
            }

            // header.
            string result = "";
            if (mMonitor.LH <= 2.0 && mMonitor.Debiation >= 43)
            {
               // result = Properties.Resources.labelResultGood;
                result = resourceLoader.GetString( "labelSectionResult" );
				

			}
            else if (mMonitor.LH <= 5.0 && mMonitor.Debiation >= 38)
            {
              //  result = Properties.Resources.labelResultWarning;
				result = resourceLoader.GetString( "labelResultWarning" );
			}
            else
            {
               // result = Properties.Resources.labelResultBad;
				result = resourceLoader.GetString( "labelResultBad" );
			}
          //  string header = String.Format("{0} - {1}", Properties.Resources.labelResultLeft, result);
            string header = String.Format("{0} - {1}", resourceLoader.GetString( "labelResultLeft" ), result);
            rect = new Rect(0, RESULT_TABLE_TOP, RESULT_TABLE_WIDTH, RESULT_TABLE_HEADER_HEIGHT);
            drawText(header, rect, RESULT_LARGE_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            {
                //BitmapImage src = new BitmapImage();
                //src.BeginInit();
                //src.UriSource = mMonitor.Icon;
                //src.EndInit();
				BitmapImage src = new BitmapImage( mMonitor.Icon);
				Image image = new Image();
                image.Source = src;
                image.Margin = new Thickness(mOriginX + RESULT_TABLE_ICON_LEFT * mScale, mOriginY + RESULT_TABLE_ICON_TOP * mScale, 0, 0);
                image.Width = image.Height = RESULT_TABLE_ICON_SIZE * mScale;
                this.Children.Add(image);
            }
            x = RESULT_TABLE_TITLE_WIDTH;
            rect = new Rect(x, RESULT_TABLE_TOP + RESULT_TABLE_HEADER_HEIGHT, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            //drawText(Properties.Resources.labelTableOverview, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            drawText( resourceLoader.GetString( "labelTableOverview" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			

			x += rect.Width;
            rect = new Rect(x, RESULT_TABLE_TOP + RESULT_TABLE_HEADER_HEIGHT, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//drawText(Properties.Resources.labelTableScore, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			drawText( resourceLoader.GetString( "labelTableScore" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, RESULT_TABLE_TOP + RESULT_TABLE_HEADER_HEIGHT, RESULT_TABLE_STANDARD_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//drawText(Properties.Resources.labelTableStandard, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			drawText( resourceLoader.GetString( "labelTableStandard" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);

            // results.
            x = 0.0;
            y = RESULT_TABLE_TOP + RESULT_TABLE_HEADER_HEIGHT + RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            //drawText(Properties.Resources.labelAverageHR, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelAverageHR" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//drawText(Properties.Resources.labelOverviewAverageHR, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewAverageHR" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(mMonitor.AvgHR.ToString(), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_STANDARD_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText("60～100", rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelMaximumHR, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelMaximumHR" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelOveviewMaximumHR, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOveviewMaximumHR" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(mMonitor.MaxHR.ToString(), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelMinimumHR, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelMinimumHR" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelOverviewMinimumHR, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewMinimumHR" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(mMonitor.MinHR.ToString(), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelLF, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelLF" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelOverviewLF, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewLF" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(String.Format("{0:F0}", mMonitor.LF), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_STANDARD_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText("-", rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelHF, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelHF" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelOverviewHF, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewHF" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(String.Format("{0:F0}", mMonitor.HF), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_STANDARD_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText("-", rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelLH, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelLH" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelOverviewLH, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewLH" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(String.Format("{0:F1}", mMonitor.LH), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_STANDARD_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText("0.8～2.0", rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//drawText(Properties.Resources.labelTP, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelTP" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelOverviewTP, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewTP" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(String.Format("{0:F0}", mMonitor.TP), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_STANDARD_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelStandardPower, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			drawText( resourceLoader.GetString( "labelStandardPower" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelCCVTP, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelCCVTP" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelOverviewCCVTP, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewCCVTP" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(String.Format("{0:F2}", mMonitor.CCVTP), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelDebiationCCVTP, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelDebiationCCVTP" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelOverviewDebiation, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewDebiation" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText(mMonitor.Debiation.ToString(), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_STANDARD_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText("43～57", rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            //
            x = 0.0;
            y += RESULT_TABLE_ROW_HEIGHT;
            rect = new Rect(x, y, RESULT_TABLE_TITLE_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			//  drawText(Properties.Resources.labelAgeCCVTP, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelAgeCCVTP" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_OVERVIEW_WIDTH, RESULT_TABLE_ROW_HEIGHT);
			// drawText(Properties.Resources.labelOverviewAge, rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelOverviewAge" ), rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_RESULT_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            if(mUser.Age >= 20 && mUser.Age <= 70)
            {
                drawText(mMonitor.Age.ToString(), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            }
            else
            {
                drawText("-", rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            }
            x += rect.Width;
            rect = new Rect(x, y, RESULT_TABLE_STANDARD_WIDTH, RESULT_TABLE_ROW_HEIGHT);
            drawText("-", rect, RESULT_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);

            // graph.
            Rectangle orange = new Rectangle();
            orange.Fill = new SolidColorBrush(Color.FromArgb(144, 255, 167, 0));
            orange.Margin = new Thickness(mOriginX + (RESULT_GRAPH_LEFT + RESULT_GRAPH_LEFT_WIDTH) * mScale, mOriginY + RESULT_GRAPH_TOP * mScale, 0, 0);
            orange.Width = (RESULT_GRAPH_WIDTH - RESULT_GRAPH_LEFT_WIDTH) * mScale;
            orange.Height = (RESULT_GRAPH_HEIGHT - RESULT_GRAPH_FOOT_HEIGHT) * mScale;
            this.Children.Add(orange);
            //
            Rectangle white1 = new Rectangle();
            white1.Fill = new SolidColorBrush(Colors.White);
            white1.Margin = orange.Margin;
            white1.Width = orange.Width - RESULT_GRAPH_RIGHT_WIDTH * mScale;
            white1.Height = orange.Height - RESULT_GRAPH_LOW_HEIGHT * mScale;
            this.Children.Add(white1);
            Rectangle yellow = new Rectangle();
            yellow.Fill = new SolidColorBrush(Color.FromArgb(144, 250, 243, 106));
            yellow.Margin = white1.Margin;
            yellow.Width = white1.Width;
            yellow.Height = white1.Height;
            this.Children.Add(yellow);
            //
            Rectangle white2 = new Rectangle();
            white2.Fill = new SolidColorBrush(Colors.White);
            white2.Margin = yellow.Margin;
            white2.Width = yellow.Width - RESULT_GRAPH_HIGH_WIDTH * mScale;
            white2.Height = yellow.Height - RESULT_GRAPH_MID_HEIGHT * mScale;
            this.Children.Add(white2);
            Rectangle blue = new Rectangle();
            blue.Fill = new SolidColorBrush(Color.FromArgb(144, 0, 189, 255));
            blue.Margin = white2.Margin;
            blue.Width = white2.Width;
            blue.Height = white2.Height;
            this.Children.Add(blue);
            //
            double[] vert = new double[4] {
                RESULT_GRAPH_LOW_WIDTH, RESULT_GRAPH_MID_WIDTH, RESULT_GRAPH_HIGH_WIDTH, RESULT_GRAPH_RIGHT_WIDTH
            };
            x = orange.Margin.Left;
            for (int i = 0; i < 5; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.StrokeThickness = RESULT_GRAPH_LINE_THICKNESS * mScale;
                line.X1 = x;
                line.Y1 = orange.Margin.Top;
                line.X2 = line.X1;
                line.Y2 = line.Y1 + orange.Height;
                this.Children.Add(line);
                if (i < 4)
                {
                    x += vert[i] * mScale;
                }
            }
            //
            string[] btext = new string[] { "0.8", "2.0", "5.0" };
            x = RESULT_GRAPH_LEFT + RESULT_GRAPH_LEFT_WIDTH + RESULT_GRAPH_LOW_WIDTH;
            y = RESULT_GRAPH_TOP + (RESULT_GRAPH_HEIGHT - RESULT_GRAPH_FOOT_HEIGHT);
            for (int i = 0; i < 3; ++i)
            {
                rect = new Rect(x - RESULT_GRAPH_TEXT_WIDTH / 2.0, y, RESULT_GRAPH_TEXT_WIDTH, 0);
                drawText(btext[i], rect, RESULT_GRAPH_FONT_SIZE, TextAlignment.Center, mBlackBrush);
                x += vert[i + 1];
            }
            rect = new Rect(RESULT_GRAPH_LEFT + RESULT_GRAPH_LEFT_WIDTH, y + RESULT_GRAPH_AXIS_HEIGHT, RESULT_GRAPH_WIDTH - RESULT_GRAPH_LEFT_WIDTH, 0);
          //  drawText(Properties.Resources.labelGraphX, rect, RESULT_GRAPH_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            drawText( resourceLoader.GetString( "labelGraphX" ), rect, RESULT_GRAPH_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            //
            double[] horz = new double[4]
            {
                RESULT_GRAPH_TOP_HEIGHT, RESULT_GRAPH_HIGH_HEIGHT, RESULT_GRAPH_MID_HEIGHT, RESULT_GRAPH_LOW_HEIGHT
            };
            y = orange.Margin.Top;
            for (int i = 0; i < 5; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.StrokeThickness = RESULT_GRAPH_LINE_THICKNESS * mScale;
                line.X1 = orange.Margin.Left;
                line.Y1 = y;
                line.X2 = line.X1 + orange.Width;
                line.Y2 = line.Y1;
                this.Children.Add(line);
                if (i < 4)
                {
                    y += horz[i] * mScale;
                }
            }
            //
            double[] debs = new double[3]
            {
                RESULT_GRAPH_HIGH_DEBIATION, RESULT_GRAPH_MID_DEBIATION, RESULT_GRAPH_LOW_DEBIATION
            };
            x = RESULT_GRAPH_LEFT;
            y = RESULT_GRAPH_TOP + RESULT_GRAPH_TOP_HEIGHT;
            for (int i = 0; i < 3; ++i)
            {
                string deb = ((int)debs[i]).ToString();
                rect = new Rect(x, y - RESULT_GRAPH_FONT_SIZE / 2.0, RESULT_GRAPH_LEFT_WIDTH, 0);
                drawText(deb, rect, RESULT_GRAPH_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += horz[i + 1];
            }
            TextBlock yLabel = new TextBlock();
            double dim = RESULT_GRAPH_HEIGHT - RESULT_GRAPH_FOOT_HEIGHT;
            x = RESULT_GRAPH_LEFT + RESULT_GRAPH_AXIS_WIDTH - dim / 2.0;
            y = RESULT_GRAPH_TOP + dim / 2.0 - RESULT_GRAPH_FONT_SIZE;
            yLabel.HorizontalAlignment = HorizontalAlignment.Left;
            yLabel.VerticalAlignment = VerticalAlignment.Center;
            yLabel.Margin = new Thickness(mOriginX + x * mScale, mOriginY + y * mScale, 0, 0);
            yLabel.Width = dim * mScale;
            yLabel.TextAlignment = TextAlignment.Center;
            yLabel.FontSize = RESULT_GRAPH_FONT_SIZE * mScale;
         //   yLabel.Text = Properties.Resources.labelGraphY;
            yLabel.Text = resourceLoader.GetString( "labelGraphY" );
			yLabel.RenderTransformOrigin = new Point(0.5, 0.5);
            //yLabel.RenderTransform = new RotateTransform(-90.0);
			RotateTransform rotateTransform = new RotateTransform();
			rotateTransform.Angle = -90.0;
			yLabel.RenderTransform = rotateTransform;
			this.Children.Add(yLabel);
            //
            if (mMonitor.LH < 0.8)
            {
                x = (RESULT_GRAPH_LOW_WIDTH / 0.8) * mMonitor.LH;
            }
            else if (mMonitor.LH <= 2.0)
            {
                x = RESULT_GRAPH_LOW_WIDTH + (RESULT_GRAPH_MID_WIDTH / 1.2) * (mMonitor.LH - 0.8);
            }
            else if (mMonitor.LH <= 5.0)
            {
                x = RESULT_GRAPH_LOW_WIDTH + RESULT_GRAPH_MID_WIDTH + (RESULT_GRAPH_HIGH_WIDTH / 3.0) * (mMonitor.LH - 2.0);
            }
            else
            {
                x = RESULT_GRAPH_LOW_WIDTH + RESULT_GRAPH_MID_WIDTH + RESULT_GRAPH_HIGH_WIDTH + (RESULT_GRAPH_RIGHT_WIDTH / 3.0) * (mMonitor.LH - 5.0);
                x = Math.Min(x, RESULT_GRAPH_WIDTH - RESULT_GRAPH_LEFT_WIDTH);
            }
            x += RESULT_GRAPH_LEFT + RESULT_GRAPH_LEFT_WIDTH - RESULT_GRAPH_ICON_SIZE / 2.0;
            double debiation = Math.Min(Math.Max(mMonitor.Debiation, RESULT_GRAPH_MIN_DEBIATION), RESULT_GRAPH_MAX_DEBIATION);
            if (debiation >= RESULT_GRAPH_HIGH_DEBIATION)
            {
                y = ((RESULT_GRAPH_MAX_DEBIATION - debiation) / (RESULT_GRAPH_MAX_DEBIATION - RESULT_GRAPH_HIGH_DEBIATION)) * RESULT_GRAPH_TOP_HEIGHT;
            }
            else if(debiation >= RESULT_GRAPH_MID_DEBIATION)
            {
                y = RESULT_GRAPH_TOP_HEIGHT + ((RESULT_GRAPH_HIGH_DEBIATION - debiation) / (RESULT_GRAPH_HIGH_DEBIATION - RESULT_GRAPH_MID_DEBIATION)) * RESULT_GRAPH_HIGH_HEIGHT;
            }
            else if(debiation >= RESULT_GRAPH_LOW_DEBIATION)
            {
                y = RESULT_GRAPH_TOP_HEIGHT + RESULT_GRAPH_HIGH_HEIGHT +
                    ((RESULT_GRAPH_MID_DEBIATION - debiation) / (RESULT_GRAPH_MID_DEBIATION - RESULT_GRAPH_LOW_DEBIATION)) * RESULT_GRAPH_MID_HEIGHT;
            }
            else
            {
                y = RESULT_GRAPH_TOP_HEIGHT + RESULT_GRAPH_HIGH_HEIGHT + RESULT_GRAPH_MID_HEIGHT+
                    ((RESULT_GRAPH_LOW_DEBIATION - debiation) / (RESULT_GRAPH_LOW_DEBIATION - RESULT_GRAPH_MIN_DEBIATION)) * RESULT_GRAPH_LOW_HEIGHT;
            }
            y = RESULT_GRAPH_TOP + y - RESULT_GRAPH_ICON_SIZE / 2.0;
            {
                Ellipse point = new Ellipse();
                point.Fill = mThemeBrush;
                point.Margin = new Thickness(mOriginX + x * mScale, mOriginY + y * mScale, 0, 0);
                point.Width = point.Height = RESULT_GRAPH_ICON_SIZE * mScale;
                this.Children.Add(point);
            }
        }
    }
}
