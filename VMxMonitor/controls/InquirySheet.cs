using System;
using Microsoft.UI.Xaml;
using System.Reflection;
using Microsoft.UI.Xaml.Media.Imaging;
using VMxMonitor.models;
using Windows.Foundation;

namespace VMxMonitor.controls
{
	public partial class InquirySheet : CommonSheet
    {
        /* =====[ constants ]===== */

        private const double CHECK_TITLE_TOP = 23.0;
        private const double CHECK_FONT_SIZE = 3.3;
        private const double CHECK_ICON_SIZE = 4.5;
        private const double CHECK_ICON_MARGIN = 1.0;
        private const double CHECK_TABLE_TOP = 31.0;
        private const double CHECK_TABLE_TITLE_WIDTH = 30.0;
        private const double CHECK_TABLE_SCORE_WIDTH = 20.0;
        private const double CHECK_TABLE_RESULT_WIDTH = 45.0;
        private const double CHECK_TABLE_COMMENT_WIDTH = 85.0;
        private const double CHECK_TABLE_HEADER_HEIGHT = 6.0;
        private const double CHECK_TABLE_ROW_HEIGHT = 10.0;
        private const double CHECK_TABLE_LINE_THICKNESS = 0.2;
        //
        private const double CHART_TITLE_TOP = 75.0;
        private const double CHART_LARGE_FONT_SIZE = 3.5;
        private const double CHART_SMALL_FONT_SIZE = 3.0;
        private const double CHART_LABEL_TOP = 85.0;
        private const double CHART_LABEL_WIDTH = 50.0;
        private const double CHART_LABEL_HEIGHT = 8.0;
        private const double CHART_LABEL_LINE_THICKNESS = 0.15;
        private const double CHART_GRAPH_TOP = 90.0;
        private const double CHART_GRAPH_CENTER = 90.0;
        private const double CHART_GRAPH_RADIUS = 44.0;
        private const double CHART_GRAPH_RADIUS_SPACE = 0.2;
        private const double CHART_GRAPH_TEXT_WIDTH = 55.0;
        private const double CHART_GRAPH_TEXT_HEIGHT = 3.5;
        private const double CHART_GRAPH_LINE_THICKNESS = 0.15;
        private const double CHART_GRAPH_FONT_SIZE = 2.0;
        private const double CHART_GRAPH_LABEL_WIDTH = 15.0;
        private const double CHART_GRAPH_LABEL_HEIGHT = 2.4;
        private const double CHART_RESULT_LINE_THICKNESS = 0.15;
        private const double CHART_RESULT_POINT_RADIUS = 1.1;
        //
        private const double FACTOR_TITLE_TOP = 190.0;
        private const double FACTOR_FONT_SIZE = 3.3;
        private const double FACTOR_ICON_SIZE = 4.5;
        private const double FACTOR_ICON_MARGIN = 0.7;
        private const double FACTOR_TABLE_TOP = 198.0;
        private const double FACTOR_TABLE_TITLE_WIDTH = 45.0;
        private const double FACTOR_TABLE_RESULT_WIDTH = 45.0;
        private const double FACTOR_TABLE_RANGE_WIDTH = 90.0;
        private const double FACTOR_TABLE_ROW_HEIGHT = 6.5;
        private const double FACTOR_REASON_ICON_LEFT = 10.0;
        private const double FACTOR_REASON_ICON_SIZE = 5.5;
        private const double FACTOR_REASON_TEXT_LEFT = 18.0;
        private const double FACTOR_REASON_TEXT_WIDTH = 15.0;
        private const double FACTOR_REASON_RANGE_LEFT = 40.0;
        private const double FACTOR_REASON_RANGE_WIDTH = 16.0;
        private const double FACTOR_TABLE_LINE_THICKNESS = 0.2;
        //
        private const double SIGN_LEFT = 7.0;
        private const double SIGN_TOP = 252.0;
        private const double SIGN_FONT_SIZE = 2.8;
        private const double SIGN_WIDTH = 160.0;
        private const double SIGN_ROW_HEIGHT = 3.2;

        /* =====[ properties ]===== */

        private DataModel mDataModel;
        private InquiryModel mInquiry;
        private int[] mChecks = new int[3];
        private int[] mFactors = new int[7];
        private string[] CHECK_RESULTS = new string[]
        {
            "良好（～≦4.0）", "注意（4.0＜～≦5.0）", "要注意（5.0＜～）"
        };
        private string[,] CHECK_COMMENTS = new string[,]
        {
            {
                "全般的な疲れはあまりないようです。",
                "少し疲れがみられます。",
                "かなり疲れが溜まっているようです。"
            },
            {
                "精神的な疲れはあまりないようです。",
                "疲れに伴う精神症状がやや認められます。",
                "疲れに伴う精神症状が強く認められます。"
            },
            {
                "身体的な疲れはあまりないようです。",
                "少しからだがお疲れのようです。休息をとって回復に努めましょう。",
                "この状態が１ヶ月以上続いているのなら要注意。半年以上続く場合は何らかの病気である可能性が高いと思われます。"
            }
        };
        private string[] FACTOR_RESULTS = new string[]
        {
            "良好", "注意", "要注意"
        };
        private double[] FACTOR_RANGES = new double[]
        {
            1.00, 2.00
        };

        /* =====[ public methods ]===== */

        public void draw(DataModel model, CommunityModel community, UserModel user, InquiryModel inquiry)
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );

			// initialize properties.
			mDataModel = model;
            mCommunity = community;
            mUser = user;
            mInquiry = inquiry;
            //
            calculate();

            // clear.
            this.Children.Clear();

            // measure.
            measure();

            // frame.
            paintFrame();

            // draw.
            //drawHeader(Properties.Resources.labelInquirySheet);
            drawHeader( resourceLoader.GetString( "labelInquirySheet" ));
			drawFooter();
            drawProfile(mInquiry.Title);
            drawCheck();
            drawChart();
            drawFactor();
            drawSign();
        }

        /* =====[ private methods ]===== */

        private void calculate()
        {
            // calculate results.
            for (int i = 0; i < 3; ++i)
            {
                PropertyInfo info = (typeof(InquiryModel)).GetProperty(String.Format("Result{0:D02}", i + 1));
                double value;
                if (double.TryParse(info.GetValue(mInquiry).ToString(), out value))
                {
                    if (value <= 4.0)
                    {
                        mChecks[i] = 0;
                    }
                    else if (value <= 5.0)
                    {
                        mChecks[i] = 1;
                    }
                    else
                    {
                        mChecks[i] = 2;
                    }
                }
            }
            for (int i = 0; i < 7; ++i)
            {
                PropertyInfo info = (typeof(InquiryModel)).GetProperty(String.Format("Result{0:D02}", i + 11));
                double value;
                if (double.TryParse(info.GetValue(mInquiry).ToString(), out value))
                {
                    if (value <= 1.0)
                    {
                        mFactors[i] = 0;
                    }
                    else if (value <= 2.0)
                    {
                        mFactors[i] = 1;
                    }
                    else
                    {
                        mFactors[i] = 2;
                    }
                }
            }
        }

        private void drawSign()
        {
            Rect rect = new(SIGN_LEFT, SIGN_TOP, SIGN_WIDTH, SIGN_ROW_HEIGHT);
            drawText("測定結果にて何度も要注意が出る場合は、医師にご相談ください。", rect, SIGN_FONT_SIZE, TextAlignment.Left, mBlackBrush);
        }

        private BitmapImage getIcon(int icon)
        {
			//    BitmapImage src = new BitmapImage();
			//    src.BeginInit();
			//    if (icon == 0)
			//    {
			//        src.UriSource = new Uri("pack://application:,,,/Resources/resultBlue.png", UriKind.Absolute);
			//    }
			//    else if(icon == 1)
			//    {
			//        src.UriSource = new Uri("pack://application:,,,/Resources/resultYellow.png", UriKind.Absolute);
			//    }
			//    else
			//    {
			//        src.UriSource = new Uri("pack://application:,,,/Resources/resultOrange.png", UriKind.Absolute);
			//    }
			//    src.EndInit();

			BitmapImage src = new ();
			if( icon == 0 )
			{
				src = new BitmapImage( new Uri( "pack://application:,,,/Resources/resultBlue.png", UriKind.Absolute ) );
			}
			else if( icon == 1 )
			{
				src = new BitmapImage( new Uri( "pack://application:,,,/Resources/resultYellow.png", UriKind.Absolute ) );
			}
			else
			{
				src = new BitmapImage(new  Uri( "pack://application:,,,/Resources/resultOrange.png", UriKind.Absolute ) );
			}
			return src;
        }
    }
}
