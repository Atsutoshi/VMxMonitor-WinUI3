using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Shapes;
using VMxMonitor.models;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;

namespace VMxMonitor.controls
{
	public partial class MonitorSheet : CommonSheet
    {
        /* =====[ private methods ]===== */

        private void drawHistory()
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );

			// title.
			Rect rect = new Rect(0, HISTORY_TITLE_TOP, 0, SECTION_TITLE_HEIGHT);
            //drawText(Properties.Resources.labelSectionHistory, rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelSectionHistory" ), rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);

            // table.
            double x = 0;
            double col = ((PAPER_WIDTH - PAPER_MARGIN * 2.0) - HISTORY_TABLE_TITLE_WIDTH) / (double)HISTORY_COUNT;
            {
                Rectangle left = new Rectangle();
                left.Fill = mTableBackground;
                left.Margin = new Thickness(mOriginX, mOriginY + HISTORY_TABLE_TOP * mScale, 0, 0);
                left.Width = HISTORY_TABLE_TITLE_WIDTH * mScale;
                left.Height = (HISTORY_TABLE_ROW_HEIGHT * 11 + HISTORY_TABLE_FOOT_HEIGHT) * mScale;
                this.Children.Add(left);
                Rectangle top = new Rectangle();
                top.Fill = mTableBackground;
                top.Margin = new Thickness(mOriginX, mOriginY + HISTORY_TABLE_TOP * mScale, 0, 0);
                top.Width = (HISTORY_TABLE_TITLE_WIDTH + col * HISTORY_COUNT) * mScale;
                top.Height = HISTORY_TABLE_ROW_HEIGHT * mScale;
                this.Children.Add(top);
            }
            for (int i = 0; i < HISTORY_COUNT + 2; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.X1 = mOriginX + x * mScale;
                line.Y1 = mOriginY + HISTORY_TABLE_TOP * mScale;
                line.X2 = line.X1;
                line.Y2 = line.Y1 + (HISTORY_TABLE_ROW_HEIGHT * 11 + HISTORY_TABLE_FOOT_HEIGHT) * mScale;
                line.StrokeThickness = HISTORY_TABLE_LINE_THICKNESS * mScale;
                this.Children.Add(line);
                if (i == 0)
                {
                    x += HISTORY_TABLE_TITLE_WIDTH;
                }
                else
                {
                    x += col;
                }
            }
            double y = mOriginY + HISTORY_TABLE_TOP * mScale;
            for (int i = 0; i < 13; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.X1 = mOriginX;
                line.Y1 = y;
                line.X2 = line.X1 + (PAPER_WIDTH - PAPER_MARGIN * 2.0) * mScale;
                line.Y2 = y;
                line.StrokeThickness = HISTORY_TABLE_LINE_THICKNESS * mScale;
                this.Children.Add(line);
                if(i == 11)
                {
                    y += HISTORY_TABLE_FOOT_HEIGHT * mScale;
                }
                else
                {
                    y += HISTORY_TABLE_ROW_HEIGHT * mScale;
                }
            }

            // labels.
            y = HISTORY_TABLE_TOP;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
            //drawText(Properties.Resources.labelMeasuredAt, rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelMeasuredAt" ), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
          //  drawText(Properties.Resources.labelAverageHR, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelAverageHR" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
           // drawText(Properties.Resources.labelMaximumHR, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelMaximumHR" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
            //drawText(Properties.Resources.labelMinimumHR, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelMinimumHR" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
            //drawText(Properties.Resources.labelLF, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelLF" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
            //drawText(Properties.Resources.labelHF, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelHF" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
           // drawText(Properties.Resources.labelLH, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelLH" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
         //   drawText(Properties.Resources.labelTP, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelTP" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
          //  drawText(Properties.Resources.labelCCVTP, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelCCVTP" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
            //drawText(Properties.Resources.labelDebiationCCVTP, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelDebiationCCVTP" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_ROW_HEIGHT);
          //  drawText(Properties.Resources.labelAgeCCVTP, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelAgeCCVTP" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += HISTORY_TABLE_ROW_HEIGHT;
            rect = new Rect(0, y, HISTORY_TABLE_TITLE_WIDTH, HISTORY_TABLE_FOOT_HEIGHT);
          //  drawText(Properties.Resources.labelResultLeft, rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelResultLeft" ), rect, RESULT_MEDIUM_FONT_SIZE, TextAlignment.Left, mBlackBrush);

            // history.
            x = HISTORY_TABLE_TITLE_WIDTH;
            foreach (MonitorModel monitor in mHistory)
            {
                y = HISTORY_TABLE_TOP;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(monitor.Title, rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(monitor.AvgHR.ToString(), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(monitor.MaxHR.ToString(), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(monitor.MinHR.ToString(), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(String.Format("{0:F0}", monitor.LF), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(String.Format("{0:F0}", monitor.HF), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(String.Format("{0:F1}", monitor.LH), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(String.Format("{0:F0}", monitor.TP), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(String.Format("{0:F2}", monitor.CCVTP), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                drawText(monitor.Debiation.ToString(), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                y += HISTORY_TABLE_ROW_HEIGHT;
                rect = new Rect(x, y, col, HISTORY_TABLE_ROW_HEIGHT);
                if(mUser.Age >= 20 && mUser.Age <= 70)
                {
                    drawText(monitor.Age.ToString(), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                }
                else
                {
                    drawText("-", rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                }
                y += HISTORY_TABLE_ROW_HEIGHT;
				//BitmapImage src = new BitmapImage();
				//src.BeginInit();
				//src.UriSource = monitor.Icon;
				//src.EndInit();

				BitmapImage src = new BitmapImage( monitor.Icon );

				Image image = new Image();
                image.Source = src;
                image.Margin = new Thickness(mOriginX + (x + HISTORY_ICON_LEFT) * mScale, mOriginY + (y + (HISTORY_TABLE_FOOT_HEIGHT - HISTORY_ICON_SIZE) / 2.0) * mScale, 0, 0);
                image.Width = image.Height = HISTORY_ICON_SIZE * mScale;
                this.Children.Add(image);
                rect = new Rect(x, y, col, HISTORY_TABLE_FOOT_HEIGHT);
                if(monitor.LH <= 2.0 && monitor.Debiation >= 43)
                {
                   // drawText(Properties.Resources.labelResultGood, rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Center, mBlackBrush);
					drawText( resourceLoader.GetString( "labelResultGood" ), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Center, mBlackBrush);
                }
                else if(monitor.LH <= 5.0 && monitor.Debiation >= 38)
                {
                   // drawText(Properties.Resources.labelResultWarning, rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Center, mBlackBrush);
					drawText( resourceLoader.GetString( "labelResultWarning" ), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Center, mBlackBrush);
                }
                else
                {
                  //  drawText(Properties.Resources.labelResultBad, rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Center, mBlackBrush);
					drawText( resourceLoader.GetString( "labelResultBad" ), rect, HISTORY_TABLE_FONT_SIZE, TextAlignment.Center, mBlackBrush);
                }
                x += col;
            }
        }
    }
}
