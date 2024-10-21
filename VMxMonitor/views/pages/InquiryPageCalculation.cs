using System;
using System.Linq;
using System.Reflection;
using Microsoft.UI.Xaml.Controls;
using VMxMonitor.models;
using VM_app_221_WAS.services;

namespace VMxMonitor.views.pages
{
	public partial class InquiryPage : UserControl
	{
		/* =====[ properties ]===== */

		private int[] RESULT_2 = new int[] { 4, 7, 24, 30, 6, 34, 13, 28, 32, 19 };
		private int[] RESULT_3 = new int[] { 3, 16, 17, 5, 8, 22, 9, 26, 18, 21 };

		/* =====[ private methods ]===== */

		private void finish()
		{
			// store inquiry.
			SequenceModel sequence = mDataModel.getSequence( InquiryModel.TABLE_NAME );
			InquiryModel inquiry = new InquiryModel
			{
				Id = sequence.Id,
				CommunityId = mUser.CommunityId,
				UserId = mUser.Id
			};
			for( int i = 0; i < 42; ++i )
			{
				PropertyInfo info = typeof( InquiryModel ).GetProperty( $"Answer{i + 1:D2}" );
				info.SetValue( inquiry, mAnswers[i] );
			}
			inquiry.Answer43 = mSleeps[0];
			inquiry.Answer44 = mSleeps[1];
			inquiry.Answer45 = mCondition;
			inquiry.Answer46 = mDiseases[0];
			inquiry.Answer47 = mDiseases[1];
			inquiry.Answer48 = mDiseases[2];
			int r2 = RESULT_2.Sum( i => mAnswers[i - 1] );
			int r3 = RESULT_3.Sum( i => mAnswers[i - 1] );
			inquiry.Result01 = 3.0 + ( ( double )( r2 + r3 ) - 10.1 ) / 6.4;
			inquiry.Result02 = 3.0 + ( ( double )r2 - 6.0 ) / 3.5;
			inquiry.Result03 = 3.0 + ( ( double )r3 - 4.3 ) / 4.0;
			inquiry.Result11 = ( ( double )( mAnswers[6 - 1] + mAnswers[28 - 1] + mAnswers[32 - 1] ) - 3.597015 ) / 2.007952;
			inquiry.Result12 = ( ( double )( mAnswers[1 - 1] + mAnswers[7 - 1] + mAnswers[10 - 1] ) - 2.880597 ) / 2.073988;
			inquiry.Result13 = ( ( double )( mAnswers[25 - 1] + mAnswers[26 - 1] + mAnswers[34 - 1] ) - 1.731343 ) / 2.026754;
			inquiry.Result14 = ( ( double )( mAnswers[21 - 1] + mAnswers[22 - 1] + mAnswers[38 - 1] ) - 1.126866 ) / 1.362338;
			inquiry.Result15 = ( ( double )( mAnswers[9 - 1] + mAnswers[18 - 1] ) - 0.701493 ) / 1.315208;
			inquiry.Result16 = ( ( double )( mAnswers[4 - 1] + mAnswers[23 - 1] ) - 1.261194 ) / 1.712094;
			inquiry.Result17 = ( ( double )( mAnswers[2 - 1] + mAnswers[19 - 1] + mAnswers[36 - 1] ) - 2.343284 ) / 2.066942;
			inquiry.AnsweredAt = DateTime.Now;

			mDataModel.InquiryModel.Add( inquiry );  // 修正点
			++sequence.Id;
			mDataModel.SaveChanges();  // 修正点

			// finish.
			Finish?.Invoke( this, new InquiryEventArgs( inquiry ) );
		}
	}
}