using System;
using System.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Shapes;
using System.Reflection;
using VMxMonitor.models;
using Windows.Foundation;

namespace VMxMonitor.controls
{
	public partial class InquirySheet : CommonSheet
    {
        /* =====[ private methods ]===== */

        private void drawCheck()
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );

			// title.
			Rect rect = new Rect(0, CHECK_TITLE_TOP, 0, SECTION_TITLE_HEIGHT);
          //  drawText(Properties.Resources.labelSectionCheck, rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelSectionCheck" ), rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);

            // table.
            double x = 0;
            double[] cols = new double[5]
            {
                CHECK_TABLE_TITLE_WIDTH, CHECK_TABLE_SCORE_WIDTH, CHECK_TABLE_RESULT_WIDTH, CHECK_TABLE_COMMENT_WIDTH, 0
            };
            for (int i = 0; i < 5; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.X1 = mOriginX + x * mScale;
                line.Y1 = mOriginY + CHECK_TABLE_TOP * mScale;
                line.X2 = line.X1;
                line.Y2 = line.Y1 + (CHECK_TABLE_HEADER_HEIGHT + CHECK_TABLE_ROW_HEIGHT * 3) * mScale;
                line.StrokeThickness = CHECK_TABLE_LINE_THICKNESS * mScale;
                this.Children.Add(line);
                x += cols[i];
            }
            double y = CHECK_TABLE_TOP;
            for (int i = 0; i < 5; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.X1 = mOriginX;
                line.Y1 = mOriginY + y * mScale;
                line.X2 = line.X1 + (PAPER_WIDTH - PAPER_MARGIN * 2) * mScale;
                line.Y2 = line.Y1;
                line.StrokeThickness = CHECK_TABLE_LINE_THICKNESS * mScale;
                this.Children.Add(line);
                if (i == 0)
                {
                    y += CHECK_TABLE_HEADER_HEIGHT;
                }
                else
                {
                    y += CHECK_TABLE_ROW_HEIGHT;
                }
            }

            // header.
            x = CHECK_TABLE_TITLE_WIDTH;
            rect = new Rect(x, CHECK_TABLE_TOP, CHECK_TABLE_SCORE_WIDTH, CHECK_TABLE_HEADER_HEIGHT);
            //drawText(Properties.Resources.labelTableScore, rect, CHECK_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			drawText( resourceLoader.GetString( "labelTableScore" ), rect, CHECK_FONT_SIZE, TextAlignment.Center, mBlackBrush );

			x += rect.Width;
            rect = new Rect(x, CHECK_TABLE_TOP, CHECK_TABLE_RESULT_WIDTH, CHECK_TABLE_HEADER_HEIGHT);
            //drawText(Properties.Resources.labelTableResult, rect, CHECK_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			drawText( resourceLoader.GetString( "labelTableResult"), rect, CHECK_FONT_SIZE, TextAlignment.Center, mBlackBrush );
			x += rect.Width;
            rect = new Rect(x, CHECK_TABLE_TOP, CHECK_TABLE_COMMENT_WIDTH, CHECK_TABLE_HEADER_HEIGHT);
           // drawText(Properties.Resources.labelTableComment, rect, CHECK_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			drawText( resourceLoader.GetString( "labelTableComment" ), rect, CHECK_FONT_SIZE, TextAlignment.Center, mBlackBrush);

            // results.
            y = CHECK_TABLE_TOP + CHECK_TABLE_HEADER_HEIGHT;
            for (int i = 0; i < 3; ++i)
            {
                x = 0.0;
                string prop = String.Format("labelInquiryResult{0:D02}", i + 1);
                rect = new Rect(x, y, CHECK_TABLE_TITLE_WIDTH, CHECK_TABLE_ROW_HEIGHT);
				//  drawText(Properties.Resources.ResourceManager.GetString(prop), rect, CHECK_FONT_SIZE, TextAlignment.Left, mBlackBrush);
				string resourceString = resourceLoader.GetString( "ResourceName" );
				drawText( resourceString, rect, CHECK_FONT_SIZE, TextAlignment.Left, mBlackBrush);
                x += rect.Width;
                PropertyInfo info = (typeof(InquiryModel)).GetProperty(String.Format("Result{0:D02}", i + 1));
                double value;
                rect = new Rect(x, y, CHECK_TABLE_SCORE_WIDTH, CHECK_TABLE_ROW_HEIGHT);
                if (double.TryParse(info.GetValue(mInquiry).ToString(), out value))
                {
                    value = Math.Max(value, 0.0);
                    drawText(value.ToString("F2"), rect, CHECK_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                }
                x += rect.Width;
                double iconW = CHECK_ICON_SIZE + CHECK_ICON_MARGIN * 2;
                {
                    double top = y + (CHECK_TABLE_ROW_HEIGHT - CHECK_ICON_SIZE) / 2;
                    Image image = new Image();
                    image.Source = getIcon(mChecks[i]);
                    image.Margin = new Thickness(mOriginX + (x + CHECK_ICON_MARGIN) * mScale, mOriginY + top * mScale, 0, 0);
                    image.Width = image.Height = CHECK_ICON_SIZE * mScale;
                    this.Children.Add(image);
                }
                x += iconW;
                rect = new Rect(x, y, CHECK_TABLE_RESULT_WIDTH - iconW, CHECK_TABLE_ROW_HEIGHT);
                drawText(CHECK_RESULTS[mChecks[i]], rect, CHECK_FONT_SIZE, TextAlignment.Left, mBlackBrush);
                x += rect.Width;
                if(i == 2 && mChecks[i] == 2)
                {
                    rect = new Rect(x, y - CHECK_FONT_SIZE * 0.8, CHECK_TABLE_COMMENT_WIDTH, CHECK_TABLE_ROW_HEIGHT);
                }
                else
                {
                    rect = new Rect(x, y, CHECK_TABLE_COMMENT_WIDTH, CHECK_TABLE_ROW_HEIGHT);
                }
                drawText(CHECK_COMMENTS[i, mChecks[i]], rect, CHECK_FONT_SIZE, TextAlignment.Left, mBlackBrush);
                y += CHECK_TABLE_ROW_HEIGHT;
            }
        }

        private void drawFactor()
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );
			// title.
			Rect rect = new Rect(0, FACTOR_TITLE_TOP, 0, SECTION_TITLE_HEIGHT);
            //drawText(Properties.Resources.labelSectionFactor, rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			//drawText( Properties.Resources.labelSectionFactor, rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelSectionFactor" ), rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);

            // table.
            double x = 0;
            double[] cols = new double[4]
            {
                FACTOR_TABLE_TITLE_WIDTH, FACTOR_TABLE_RESULT_WIDTH, FACTOR_TABLE_RANGE_WIDTH, 0
            };
            for (int i = 0; i < 4; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.X1 = mOriginX + x * mScale;
                line.Y1 = mOriginY + FACTOR_TABLE_TOP * mScale;
                line.X2 = line.X1;
                line.Y2 = line.Y1 + FACTOR_TABLE_ROW_HEIGHT * 8 * mScale;
                line.StrokeThickness = FACTOR_TABLE_LINE_THICKNESS * mScale;
                this.Children.Add(line);
                x += cols[i];
            }
            double y = FACTOR_TABLE_TOP;
            for (int i = 0; i < 9; ++i)
            {
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.X1 = mOriginX;
                line.Y1 = mOriginY + y * mScale;
                if (i == 0 || i == 1 || i == 8)
                {
                    line.X2 = line.X1 + (PAPER_WIDTH - PAPER_MARGIN * 2) * mScale;
                }
                else
                {
                    line.X2 = line.X1 + (FACTOR_TABLE_TITLE_WIDTH + FACTOR_TABLE_RESULT_WIDTH) * mScale;
                }
                line.Y2 = line.Y1;
                line.StrokeThickness = FACTOR_TABLE_LINE_THICKNESS * mScale;
                this.Children.Add(line);
                y += FACTOR_TABLE_ROW_HEIGHT;
            }

            // header.
            x = FACTOR_TABLE_TITLE_WIDTH;
            rect = new Rect(x, FACTOR_TABLE_TOP, FACTOR_TABLE_RESULT_WIDTH, FACTOR_TABLE_ROW_HEIGHT);
          //  drawText( AppResources.Resources.labelTableResult, rect, FACTOR_FONT_SIZE, TextAlignment.Center, mBlackBrush);

			var label = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" ).GetString( "labelTableResult" );
			drawText( label, rect, FACTOR_FONT_SIZE, TextAlignment.Center, mBlackBrush );

			x += rect.Width;
            rect = new Rect(x, FACTOR_TABLE_TOP, FACTOR_TABLE_RANGE_WIDTH, FACTOR_TABLE_ROW_HEIGHT);
           // drawText(Properties.Resources.labelTableReason, rect, FACTOR_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			label = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" ).GetString( "labelTableReason" );
			drawText( label, rect, FACTOR_FONT_SIZE, TextAlignment.Center, mBlackBrush );

			// results.
			x = 0.0;
            y = FACTOR_TABLE_TOP + FACTOR_TABLE_ROW_HEIGHT;
            for (int i = 0; i < 7; ++i)
            {
               // String title = Properties.Resources.ResourceManager.GetString(String.Format("labelInquiryResult{0:D02}", i + 11));

				String title = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" ).GetString( String.Format("labelInquiryResult{0:D02}", i + 11) );


				rect = new Rect(x, y, FACTOR_TABLE_TITLE_WIDTH, FACTOR_TABLE_ROW_HEIGHT);
                drawText(title, rect, FACTOR_FONT_SIZE, TextAlignment.Left, mBlackBrush);
                //
                PropertyInfo info = (typeof(InquiryModel)).GetProperty(String.Format("Result{0:D02}", i + 11));
                double value;
                if (double.TryParse(info.GetValue(mInquiry).ToString(), out value))
                {
                    double iconX = x + FACTOR_TABLE_TITLE_WIDTH + FACTOR_ICON_MARGIN;
                    double iconY = y + (FACTOR_TABLE_ROW_HEIGHT - FACTOR_ICON_SIZE) / 2.0;
                    Image image = new Image();
                    image.Source = getIcon(mFactors[i]);
                    image.Margin = new Thickness(mOriginX + iconX * mScale, mOriginY + iconY * mScale, 0, 0);
                    image.Width = image.Height = FACTOR_ICON_SIZE * mScale;
                    this.Children.Add(image);
                    //
                    value = Math.Max(value, 0.0);
                    rect = new Rect(x + FACTOR_TABLE_TITLE_WIDTH, y, FACTOR_TABLE_RESULT_WIDTH, FACTOR_TABLE_ROW_HEIGHT);
                    drawText(value.ToString("F2"), rect, FACTOR_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                }
                //
                y += FACTOR_TABLE_ROW_HEIGHT;
            }
            //
            x = FACTOR_TABLE_TITLE_WIDTH + FACTOR_TABLE_RESULT_WIDTH;
            y = FACTOR_TABLE_TOP + FACTOR_TABLE_ROW_HEIGHT * 2;
            for(int i = 0; i < 3; ++i)
            {
                double iconX = x + FACTOR_REASON_ICON_LEFT;
                double iconY = y + (FACTOR_TABLE_ROW_HEIGHT - FACTOR_REASON_ICON_SIZE) / 2.0;
                Image image = new Image();
                image.Source = getIcon(i);
                image.Margin = new Thickness(mOriginX + iconX * mScale, mOriginY + iconY * mScale, 0, 0);
                image.Width = image.Height = FACTOR_REASON_ICON_SIZE * mScale;
                this.Children.Add(image);
                //
                rect = new Rect(x + FACTOR_REASON_TEXT_LEFT, y, FACTOR_REASON_TEXT_WIDTH, FACTOR_TABLE_ROW_HEIGHT);
                drawText(FACTOR_RESULTS[i], rect, FACTOR_FONT_SIZE, TextAlignment.Left, mBlackBrush);
                //
                double xx = x + FACTOR_REASON_RANGE_LEFT;
                if(i != 0)
                {
                    rect = new Rect(xx, y, FACTOR_REASON_RANGE_WIDTH, FACTOR_TABLE_ROW_HEIGHT);
                    drawText(String.Format("{0:F2}　＜", FACTOR_RANGES[i- 1]), rect, FACTOR_FONT_SIZE, TextAlignment.Left, mBlackBrush);
                }
                xx += FACTOR_REASON_RANGE_WIDTH;
                if(i == 1)
                {
                    rect = new Rect(xx, y, FACTOR_REASON_RANGE_WIDTH, FACTOR_TABLE_ROW_HEIGHT);
                    drawText("～", rect, FACTOR_FONT_SIZE, TextAlignment.Center, mBlackBrush);
                }
                xx += FACTOR_REASON_RANGE_WIDTH;
                if(i != 2)
                {
                    rect = new Rect(xx, y, FACTOR_REASON_RANGE_WIDTH, FACTOR_TABLE_ROW_HEIGHT);
                    drawText(String.Format("≦　{0:F2}", FACTOR_RANGES[i]), rect, FACTOR_FONT_SIZE, TextAlignment.Right, mBlackBrush);
                }
                //
                y += FACTOR_TABLE_ROW_HEIGHT * 2;
            }
        }
    }
}
