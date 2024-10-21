using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Shapes;
using VMxMonitor.models;
using Windows.Foundation;

namespace VMxMonitor.controls
{
	public partial class MonitorSheet : CommonSheet
    {
        /* =====[ constants ]===== */

        public const int HISTORY_COUNT = 5;
        //
        private const double RESULT_TITLE_TOP = 23.0;
        private const double RESULT_LARGE_FONT_SIZE = 5.1;
        private const double RESULT_MEDIUM_FONT_SIZE = 3.3;
        private const double RESULT_SMALL_FONT_SIZE = 3.0;
        private const double RESULT_TABLE_TOP = 31.0;
        private const double RESULT_TABLE_WIDTH = 100.0;
        private const double RESULT_TABLE_TITLE_WIDTH = 19.0;
        private const double RESULT_TABLE_OVERVIEW_WIDTH = 48.0;
        private const double RESULT_TABLE_RESULT_WIDTH = 16.5;
        private const double RESULT_TABLE_STANDARD_WIDTH = 16.5;
        private const double RESULT_TABLE_HEADER_HEIGHT = 8.5;
        private const double RESULT_TABLE_ROW_HEIGHT = 6.2;
        private const double RESULT_TABLE_LINE_THICKNESS = 0.2;
        private const double RESULT_TABLE_ICON_LEFT = 22.0;
        private const double RESULT_TABLE_ICON_TOP = 31.6;
        private const double RESULT_TABLE_ICON_SIZE = 7.2;
        private const double RESULT_GRAPH_LEFT = 105.0;
        private const double RESULT_GRAPH_TOP = 33.0;
        private const double RESULT_GRAPH_WIDTH = 75.0;
        private const double RESULT_GRAPH_HEIGHT = 75.0;
        private const double RESULT_GRAPH_AXIS_WIDTH = 3.5;
        private const double RESULT_GRAPH_AXIS_HEIGHT = 4.5;
        private const double RESULT_GRAPH_LEFT_WIDTH = 10.0;
        private const double RESULT_GRAPH_LOW_WIDTH = 18.0;
        private const double RESULT_GRAPH_MID_WIDTH = 22.0;
        private const double RESULT_GRAPH_HIGH_WIDTH = 15.0;
        private const double RESULT_GRAPH_RIGHT_WIDTH = 10.0;
        private const double RESULT_GRAPH_FOOT_HEIGHT = 10.0;
        private const double RESULT_GRAPH_LOW_HEIGHT = 10.0;
        private const double RESULT_GRAPH_MID_HEIGHT = 12.0;
        private const double RESULT_GRAPH_HIGH_HEIGHT = 30.0;
        private const double RESULT_GRAPH_TOP_HEIGHT = 13.0;
        private const double RESULT_GRAPH_MIN_DEBIATION = 10.0;
        private const double RESULT_GRAPH_LOW_DEBIATION = 37.0;
        private const double RESULT_GRAPH_MID_DEBIATION = 42.0;
        private const double RESULT_GRAPH_HIGH_DEBIATION = 57.0;
        private const double RESULT_GRAPH_MAX_DEBIATION = 90.0;
        private const double RESULT_GRAPH_FONT_SIZE = 3.3;
        private const double RESULT_GRAPH_TEXT_WIDTH = 8.0;
        private const double RESULT_GRAPH_LINE_THICKNESS = 0.15;
        private const double RESULT_GRAPH_ICON_SIZE = 4.2;
        //
        private const double COMMENT_TITLE_TOP = 112.0;
        private const double COMMENT_BODY_TOP = 120.0;
        private const double COMMENT_BODY_HEIGHT = 40.0;
        private const double COMMENT_MARGIN = 1.1;
        private const double COMMENT_FONT_SIZE = 3.3;
        private const double COMMENT_LINE_THICKNESS = 0.2;
        //
        private const double HISTORY_TITLE_TOP = 165.0;
        private const double HISTORY_TABLE_TOP = 173.0;
        private const double HISTORY_TABLE_TITLE_WIDTH = 20.0;
        private const double HISTORY_TABLE_ROW_HEIGHT = 5.8;
        private const double HISTORY_TABLE_FOOT_HEIGHT = 7.2;
        private const double HISTORY_TABLE_FONT_SIZE = 3.3;
        private const double HISTORY_TABLE_LINE_THICKNESS = 0.2;
        private const double HISTORY_ICON_SIZE = 6.4;
        private const double HISTORY_ICON_LEFT = 3.3;
        //
        private const double SIGN_LEFT = 8.0;
        private const double SIGN_TOP = 247.0;
        private const double SIGN_FONT_SIZE = 2.7;
        private const double SIGN_ROW_HEIGHT = 3.2;
        private const double SIGN_WIDTH = 160.0;

        /* =====[ properties ]===== */

        private DataModel mDataModel;
        private MonitorModel mMonitor;
        private List<MonitorModel> mHistory;
        private int mBalance;
        private int mPower;
        private string[,] COMMENTS = new string[4, 4] {
            {
                "交感神経系╱副交感神経系のバランスはリラックス状態にありますが、自律神経機能活動が明らかに低下しています。意欲の低下や抑うつ症状が続くようでしたら、かかりつけの医師と相談しましょう。対処法としては、十分な睡眠時間を確保するとともに、規則的な生活をするように心がけましょう。また、起床時には体操や熱めのシャワーを浴びるなど交感神経系の活動を高めることも有効です。夕方以降は、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高める取り組みを取り入れ、一日の規則正しいリズムを作られることをお勧めします。",
                "交感神経系╱副交感神経系のバランスはリラックス状態にありますが、自律神経機能活動が少し低下しています。仕事や勉強をするときに活動モードに切り替えることができない場合は、起床時には体操や熱めのシャワーを浴びるなど交感神経系の活動を高めるようにしましょう。また、夕方以降はヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高める取り組みを取り入れ、一日の規則正しいリズムを作られることをお勧めします。",
                "交感神経系／副交感神経系のバランスはリラックス状態にあり、自律神経機能活動も正常です。睡眠や休息をとるのには適した状態ですが、時に仕事や勉強をするときに活動モードに切り替えることができない場合がありますので、抑うつ、意欲の低下などがみられるようでしたら、再検査をお勧めします。",
                "交感神経系／副交感神経系のバランスはリラックス状態にあり、自律神経機能活動も活発な状態です。睡眠や休息をとるのには適しており、理想的な状態です。時に、仕事や勉強をするときに活動モードに切り替えることができない場合がありますので、抑うつ、意欲の低下などがみられるようでしたら、再検査をお勧めします。"
            },
            {
                "交感神経系╱副交感神経系のバランスはうまく保たれていますが、自律神経機能活動が明らかに低下しています。意欲の低下や抑うつ症状が続くようでしたら、かかりつけの医師と相談しましょう。対処法としては、十分な睡眠時間を確保するとともに、規則的な生活をするように心がけましょう。また、起床時には体操や熱めのシャワーを浴びるなど交感神経系の活動を高めることも有効です。夕方以降は、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高める取り組みを取り入れ、一日の規則正しいリズムを作られることをお勧めします。",
                "交感神経系／副交感神経系のバランスはうまく保たれていますが、自律神経機能活動が少し低下しています。仕事や勉強をするときに活動モードに切り替えることができない場合は、起床時には体操や熱めのシャワーを浴びるなど交感神経系の活動を高めるようにしましょう。また、夕方以降はヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高める取り組みを取り入れ、一日の規則正しいリズムを作られることをお勧めします。",
                "交感神経系／副交感神経系のバランスはうまく保たれており、自律神経機能活動も正常です。自律神経機能は極めて良好な状態です。この状態を維持するように心がけましょう！",
                "交感神経系／副交感神経系のバランスはうまく保たれており、自律神経機能活動も活発な状態です。自律神経機能は極めて良好な状態ですので、この状態を維持するように心がけましょう！"
            },
            {
                "交感神経系╱副交感神経系のバランスが崩れて交感神経系の軽度過緊張がみられ、自律神経機能活動が明らかに低下しています。慢性的な疲労が続いている方にみられる変化ですので、注意が必要です。体調不良が続くようでしたら、かかりつけの医師と相談しましょう。対処法としては、十分な睡眠時間を確保するとともに、規則的な生活をするように心がけましょう。また、常にリラックスをするのではなく、起床時には体操や熱めのシャワーを浴びるなどにより交感神経系の活動を高め、夕方以降は、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高める取り組みを取り入れるなど、ONとOFFのメリハリをつけることも大切です。",
                "交感神経系╱副交感神経系のバランスが崩れて交感神経系の軽度過緊張がみられ、自律神経機能活動も少し低下しています。慢性的に疲労が続いている方にみられる変化ですので、注意が必要です。安静にしていてもこのような状態が続くようでしたら、不眠や中途覚醒などの睡眠障害に結びつく可能性もありますので、夕方以降はヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高めるリラックス法を取り入れられることをお勧めいたします。",
                "交感神経系╱副交感神経系のバランスが崩れて交感神経系の軽度過緊張がみられますが、自律神経機能活動は正常です。軽度のストレス時にみられる変化ですので、時間をおいて再検査をされることをお勧めします。安静にしていてもこのような状態が続くようでしたら、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高めるリラックス法を取り入れられることをお勧めいたします。",
                "交感神経系╱副交感神経系のバランスが崩れて交感神経系の軽度過緊張がみられ、自律神経機能活動も亢進しています。心身のストレス状況下においてみられる変化ですので、時間をおいて再検査をされることをお勧めします。安静にしていてもこのような状態が続くようでしたら、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高めるリラックス法を取り入れられることをお勧めいたします。"
            },
            {
                "交感神経系╱副交感神経系のバランスが大きく崩れて交感神経系の過緊張がみられ、自律神経活動は明らかに低下しています。慢性的な疲労がみられる方にみられる変化の１つですので、注意が必要です。このような状態が続くようでしたら、不眠や中途覚醒などの睡眠障害に結びつき、体調不良の原因となりますので、かかりつけの医師と相談されることも大切です。また、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高めるリラックス法を取り入れられることをお勧めいたします。",
                "交感神経系╱副交感神経系のバランスが大きく崩れて交感神経系の過緊張がみられ、自律神経活動は少し低下しています。慢性的な疲労がみられる方にみられる変化の１つですので、時間をおいて再検査をお勧めします。安静にしていてもこのような状態が続くようでしたら、不眠や中途覚醒などの睡眠障害に結びつき、体調不良の原因となりますので、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高めるリラックス法を取り入れられるようにしましょう。",
                "交感神経系╱副交感神経系のバランスが大きく崩れて交感神経系の過緊張がみられます。自律神経活動は正常範囲ですが、心身のストレス状況下においてみられる変化の１つですので、時間をおいて再検査をお勧めします。このような状態が続くようでしたら、体調不良の原因となりますので、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高めるリラックス法を取り入れられるようにしましょう。なお、このような状況が続いている場合は、不眠や中途覚醒などの睡眠障害に結びつくこともありますので、注意が必要です。",
                "交感神経系╱副交感神経系のバランスが大きく崩れて交感神経系の過緊張がみられ、自律神経機能活動も亢進しています。急性の心身のストレス状況下においてみられる変化ですが、安静にしていてもこのような状態が続くようでしたら、不眠や中途覚醒などの睡眠障害に結びつき、慢性的な疲労やメンタルヘルス障害に結びつくこともあります。対処法としては、ヨガ、呼吸法、音楽、アロマなどの副交感神経活動を高めるリラックス法を取り入れられることをお勧めいたします。また、時間をおいての再検査を受けられることも大切です。"
            }
            };

        /* =====[ public methods ]===== */

        public void draw(DataModel model, CommunityModel community, UserModel user, MonitorModel monitor, List<MonitorModel> history)
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );

			// initialize properties.
			mDataModel = model;
            mCommunity = community;
            mUser = user;
            mMonitor = monitor;
            mHistory = history;
            //
            calculate();

            // clear.
            this.Children.Clear();

            // measure.
            measure();

            // frame.
            paintFrame();

            // draw.
            //drawHeader(Properties.Resources.labelMonitorSheet); resourceLoader.GetString( "labelSectionFactor" )
			drawHeader( resourceLoader.GetString( "labelSectionFactor" ));

			drawFooter();
            drawProfile(mMonitor.Title);
            drawResult();
            drawComment();
            drawHistory();
            drawSign();
        }

        /* =====[ private methods ]===== */

        private void calculate()
        {
            // get balance level.
            if (mMonitor.LH < 0.8)
            {
                mBalance = 0;
            }
            else if (mMonitor.LH <= 2.0)
            {
                mBalance = 1;
            }
            else if (mMonitor.LH <= 5.0)
            {
                mBalance = 2;
            }
            else
            {
                mBalance = 3;
            }

            // get power level.
            if (mMonitor.Debiation <= 37)
            {
                mPower = 0;
            }
            else if (mMonitor.Debiation <= 42)
            {
                mPower = 1;
            }
            else if (mMonitor.Debiation <= 56)
            {
                mPower = 2;
            }
            else
            {
                mPower = 3;
            }
        }

        private void drawComment()
        {
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse( "Resources" );

			// title.
			Rect rect = new (0, COMMENT_TITLE_TOP, 0, SECTION_TITLE_HEIGHT);
           // drawText(Properties.Resources.labelSectionComment, rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            drawText( resourceLoader.GetString( "labelSectionComment" ), rect, SECTION_TITLE_FONT_SIZE, TextAlignment.Left, mBlackBrush);

            // comment.
            Rectangle frame = new ();
            frame.Stroke = mBlackBrush;
            frame.StrokeThickness = COMMENT_LINE_THICKNESS * mScale;
            frame.Margin = new Thickness(mOriginX, mOriginY + COMMENT_BODY_TOP * mScale, 0, 0);
            frame.Width = (PAPER_WIDTH - PAPER_MARGIN * 2.0) * mScale;
            frame.Height = COMMENT_BODY_HEIGHT * mScale;
            this.Children.Add(frame);
            //
            rect = new Rect(COMMENT_MARGIN, COMMENT_BODY_TOP + COMMENT_MARGIN, (PAPER_WIDTH - PAPER_MARGIN * 2.0 - COMMENT_MARGIN * 2.0), 0);
            drawText(COMMENTS[mBalance, mPower], rect, COMMENT_FONT_SIZE, TextAlignment.Left, mBlackBrush);
        }

        private void drawSign()
        {
            double y = SIGN_TOP;
            Rect rect = new (SIGN_LEFT, y, SIGN_WIDTH, SIGN_ROW_HEIGHT);
            drawText("測定結果は健康状態の目安で、医師による診断を代用するものではありません。", rect, SIGN_FONT_SIZE, TextAlignment.Left, mBlackBrush);
            y += SIGN_ROW_HEIGHT;
            rect = new Rect(SIGN_LEFT, y, SIGN_WIDTH, SIGN_ROW_HEIGHT);
            drawText("結果の如何にかかわらず不安がある場合は、専門の医師に相談されることをお薦めします。", rect, SIGN_FONT_SIZE, TextAlignment.Left, mBlackBrush);
        }
    }
}
