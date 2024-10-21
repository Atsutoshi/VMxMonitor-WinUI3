using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System.Reflection;
using VMxMonitor.models;
using Windows.Foundation;
using Windows.UI;
using Microsoft.UI;

namespace VMxMonitor.controls
{
	public partial class InquirySheet : CommonSheet
    {
        /* =====[ private methods ]===== */

        private void drawChart()
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );

			// title.
			Rect rect = new Rect(0, CHART_TITLE_TOP, 0, SECTION_TITLE_HEIGHT);
           // drawText(Properties.Resources.labelSectionChart, rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush); 
            drawText( resourceLoader.GetString( "labelSectionChart" ), rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush); 

			// chart.
			double centerX = CHART_GRAPH_CENTER;
            double centerY = CHART_GRAPH_TOP + CHART_GRAPH_RADIUS;
            double radius = CHART_GRAPH_RADIUS;
            //
            double x = centerX;
            double y = centerY - radius;
            rect = new Rect(x - CHART_GRAPH_TEXT_WIDTH / 2.0, y - CHART_GRAPH_TEXT_HEIGHT, CHART_GRAPH_TEXT_WIDTH, CHART_GRAPH_TEXT_HEIGHT);
           // drawText(Properties.Resources.labelInquiryResult11, rect, CHART_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
			drawText( resourceLoader.GetString( "labelInquiryResult11" ), rect, CHART_SMALL_FONT_SIZE, TextAlignment.Center, mBlackBrush);
            x = centerX + Math.Sin((Math.PI * 2) / 7.0) * radius;
            y = centerY - Math.Cos((Math.PI * 2) / 7.0) * radius;
            rect = new Rect(x, y - CHART_GRAPH_TEXT_HEIGHT, CHART_GRAPH_TEXT_WIDTH, CHART_GRAPH_TEXT_HEIGHT);
			// drawText(Properties.Resources.labelInquiryResult12, rect, CHART_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelInquiryResult12" ), rect, CHART_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x = centerX + Math.Sin((Math.PI * 4) / 7.0) * radius;
            y = centerY - Math.Cos((Math.PI * 4) / 7.0) * radius;
            rect = new Rect(x, y, CHART_GRAPH_TEXT_WIDTH, CHART_GRAPH_TEXT_HEIGHT);
			//drawText(Properties.Resources.labelInquiryResult13, rect, CHART_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelInquiryResult13" ), rect, CHART_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x = centerX + Math.Sin((Math.PI * 6) / 7.0) * radius;
            y = centerY - Math.Cos((Math.PI * 6) / 7.0) * radius;
            rect = new Rect(x, y, CHART_GRAPH_TEXT_WIDTH, CHART_GRAPH_TEXT_HEIGHT);
			//drawText(Properties.Resources.labelInquiryResult14, rect, CHART_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
			drawText( resourceLoader.GetString( "labelInquiryResult14" ), rect, CHART_SMALL_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            x = centerX + Math.Sin((Math.PI * 8) / 7.0) * radius;
            y = centerY - Math.Cos((Math.PI * 8) / 7.0) * radius;
            rect = new Rect(x - CHART_GRAPH_TEXT_WIDTH, y, CHART_GRAPH_TEXT_WIDTH, CHART_GRAPH_TEXT_HEIGHT);
			//drawText(Properties.Resources.labelInquiryResult15, rect, CHART_SMALL_FONT_SIZE, TextAlignment.Right, mBlackBrush);
			drawText( resourceLoader.GetString( "labelInquiryResult15" ), rect, CHART_SMALL_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            x = centerX + Math.Sin((Math.PI * 10) / 7.0) * radius;
            y = centerY - Math.Cos((Math.PI * 10) / 7.0) * radius;
            rect = new Rect(x - CHART_GRAPH_TEXT_WIDTH, y, CHART_GRAPH_TEXT_WIDTH, CHART_GRAPH_TEXT_HEIGHT);
			//drawText(Properties.Resources.labelInquiryResult16, rect, CHART_SMALL_FONT_SIZE, TextAlignment.Right, mBlackBrush);
			drawText( resourceLoader.GetString( "labelInquiryResult16" ), rect, CHART_SMALL_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            x = centerX + Math.Sin((Math.PI * 12) / 7.0) * radius;
            y = centerY - Math.Cos((Math.PI * 12) / 7.0) * radius;
            rect = new Rect(x - CHART_GRAPH_TEXT_WIDTH, y - CHART_GRAPH_TEXT_HEIGHT, CHART_GRAPH_TEXT_WIDTH, CHART_GRAPH_TEXT_HEIGHT);
			//drawText(Properties.Resources.labelInquiryResult17, rect, CHART_SMALL_FONT_SIZE, TextAlignment.Right, mBlackBrush);
			drawText( resourceLoader.GetString( "labelInquiryResult17" ), rect, CHART_SMALL_FONT_SIZE, TextAlignment.Right, mBlackBrush);
            //
            for(int i = 0; i < 9; ++i)
            {
                Polygon polygon = new Polygon();
                for (int j = 0; j < 8; ++j)
                {
                    double angle = (Math.PI * 2 * j) / 7.0;
                    Point point = new Point();
                    point.X = mOriginX + (centerX + Math.Sin(angle) * radius) * mScale;
                    point.Y = mOriginY + (centerY - Math.Cos(angle) * radius) * mScale;
                    polygon.Points.Add(point);
                }
                if(i % 2 == 0)
                {
                    if(i == 4)
                    {
                        polygon.Fill = new SolidColorBrush(Color.FromArgb(144, 250, 243, 106));
                    }
                    else if(i >= 6)
                    {
                        polygon.Fill = new SolidColorBrush(Color.FromArgb(144, 0, 189, 255));
                    }
                    else
                    {
                        polygon.Fill = new SolidColorBrush(Color.FromArgb(144, 255, 167, 0));
                    }
                }
                else
                {
					polygon.Fill = new SolidColorBrush( Microsoft.UI.Colors.White );
				}
                this.Children.Add(polygon);
                //
                if(i % 2== 0)
                {
                    radius -= (CHART_GRAPH_RADIUS / 5) - CHART_GRAPH_RADIUS_SPACE;
                }
                else
                {
                    radius -= CHART_GRAPH_RADIUS_SPACE;
                }
            }
            for (int i = 0; i < 8; ++i)
            {
                double angle = (Math.PI * 2 * i) / 7.0;
                Line line = new Line();
                line.Stroke = mBlackBrush;
                line.StrokeThickness = CHART_GRAPH_LINE_THICKNESS * mScale;
                line.X1 = mOriginX + centerX * mScale;
                line.Y1 = mOriginY + centerY * mScale;
                line.X2 = mOriginX + (centerX + Math.Sin(angle) * CHART_GRAPH_RADIUS) * mScale;
                line.Y2 = mOriginY + (centerY - Math.Cos(angle) * CHART_GRAPH_RADIUS) * mScale;
                this.Children.Add(line);
            }
            //
            Polygon chart = new Polygon();
            double step = CHART_GRAPH_RADIUS / 5.0;
            for (int i = 0; i < 7; ++i)
            {
                PropertyInfo info = (typeof(InquiryModel)).GetProperty(String.Format("Result{0:D02}", i + 11));
                double value;
                if(double.TryParse(info.GetValue(mInquiry).ToString(), out value)){
                    value = Math.Min(Math.Max(value, 0.0), 4.0);
                    double angle = (Math.PI * 2 * i) / 7.0;
                    Point point = new Point();
                    point.X = mOriginX + (centerX + Math.Sin(angle) * step * (1 + value)) * mScale;
                    point.Y = mOriginY + (centerY - Math.Cos(angle) * step * (1 + value)) * mScale;
                    chart.Points.Add(point);
                }
            }
            chart.Points.Add(chart.Points.First());
            chart.Stroke = new SolidColorBrush(Colors.Black);
            chart.StrokeThickness = CHART_RESULT_LINE_THICKNESS * mScale;
            chart.Fill = new SolidColorBrush(Color.FromArgb(77, 255, 255, 255));
            this.Children.Add(chart);
            foreach(Point point in chart.Points)
            {
                Ellipse p = new Ellipse();
                p.Fill = new SolidColorBrush(Colors.DarkGreen);
                p.Margin = new Thickness(point.X - CHART_RESULT_POINT_RADIUS * mScale, point.Y - CHART_RESULT_POINT_RADIUS * mScale, 0, 0);
                p.Width = p.Height = CHART_RESULT_POINT_RADIUS * 2 * mScale;
                this.Children.Add(p);
            }

            // labels.
            y = centerY - CHART_GRAPH_RADIUS;
            rect = new Rect(centerX, y, CHART_GRAPH_LABEL_WIDTH, CHART_GRAPH_LABEL_HEIGHT);
            drawText("4.0", rect, CHART_GRAPH_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += CHART_GRAPH_RADIUS / 5;
            rect = new Rect(centerX, y, CHART_GRAPH_LABEL_WIDTH, CHART_GRAPH_LABEL_HEIGHT);
            drawText("3.0", rect, CHART_GRAPH_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += CHART_GRAPH_RADIUS / 5;
            rect = new Rect(centerX, y, CHART_GRAPH_LABEL_WIDTH, CHART_GRAPH_LABEL_HEIGHT);
            drawText("2.0", rect, CHART_GRAPH_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += CHART_GRAPH_RADIUS / 5;
            rect = new Rect(centerX, y, CHART_GRAPH_LABEL_WIDTH, CHART_GRAPH_LABEL_HEIGHT);
            drawText("1.0", rect, CHART_GRAPH_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += CHART_GRAPH_RADIUS / 5;
            rect = new Rect(centerX, y, CHART_GRAPH_LABEL_WIDTH, CHART_GRAPH_LABEL_HEIGHT);
            drawText("0.0", rect, CHART_GRAPH_FONT_SIZE, TextAlignment.Left, mBlackBrush);
        }
    }
}
