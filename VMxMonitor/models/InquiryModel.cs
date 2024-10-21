using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VMxMonitor.models
{
	[Table( "t_inquiry" )]
	public class InquiryModel
	{
		/* =====[ Disease ]===== */
		public class Disease
		{
			public List<int> Checks = new List<int>();
			public string Text = "";
		}

		/* =====[ constants ]===== */
		public const string TABLE_NAME = "t_inquiry";

		/* =====[ properties ]===== */
		[Key]
		[DatabaseGenerated( DatabaseGeneratedOption.Identity )]
		[Column( "id", TypeName = "INT" )]
		public long Id { get; set; }

		[Required]
		[Column( "community_id", TypeName = "INT" )]
		public long CommunityId { get; set; }

		[Required]
		[Column( "user_id", TypeName = "INT" )]
		public long UserId { get; set; }

		[Required]
		[Column( "answer_01", TypeName = "INT" )]
		public long Answer01 { get; set; }

		[Required]
		[Column( "answer_02", TypeName = "INT" )]
		public long Answer02 { get; set; }

		[Required]
		[Column( "answer_03", TypeName = "INT" )]
		public long Answer03 { get; set; }

		[Required]
		[Column( "answer_04", TypeName = "INT" )]
		public long Answer04 { get; set; }

		[Required]
		[Column( "answer_05", TypeName = "INT" )]
		public long Answer05 { get; set; }

		[Required]
		[Column( "answer_06", TypeName = "INT" )]
		public long Answer06 { get; set; }

		[Required]
		[Column( "answer_07", TypeName = "INT" )]
		public long Answer07 { get; set; }

		[Required]
		[Column( "answer_08", TypeName = "INT" )]
		public long Answer08 { get; set; }

		[Required]
		[Column( "answer_09", TypeName = "INT" )]
		public long Answer09 { get; set; }

		[Required]
		[Column( "answer_10", TypeName = "INT" )]
		public long Answer10 { get; set; }

		[Required]
		[Column( "answer_11", TypeName = "INT" )]
		public long Answer11 { get; set; }

		[Required]
		[Column( "answer_12", TypeName = "INT" )]
		public long Answer12 { get; set; }

		[Required]
		[Column( "answer_13", TypeName = "INT" )]
		public long Answer13 { get; set; }

		[Required]
		[Column( "answer_14", TypeName = "INT" )]
		public long Answer14 { get; set; }

		[Required]
		[Column( "answer_15", TypeName = "INT" )]
		public long Answer15 { get; set; }

		[Required]
		[Column( "answer_16", TypeName = "INT" )]
		public long Answer16 { get; set; }

		[Required]
		[Column( "answer_17", TypeName = "INT" )]
		public long Answer17 { get; set; }

		[Required]
		[Column( "answer_18", TypeName = "INT" )]
		public long Answer18 { get; set; }

		[Required]
		[Column( "answer_19", TypeName = "INT" )]
		public long Answer19 { get; set; }

		[Required]
		[Column( "answer_20", TypeName = "INT" )]
		public long Answer20 { get; set; }

		[Required]
		[Column( "answer_21", TypeName = "INT" )]
		public long Answer21 { get; set; }

		[Required]
		[Column( "answer_22", TypeName = "INT" )]
		public long Answer22 { get; set; }

		[Required]
		[Column( "answer_23", TypeName = "INT" )]
		public long Answer23 { get; set; }

		[Required]
		[Column( "answer_24", TypeName = "INT" )]
		public long Answer24 { get; set; }

		[Required]
		[Column( "answer_25", TypeName = "INT" )]
		public long Answer25 { get; set; }

		[Required]
		[Column( "answer_26", TypeName = "INT" )]
		public long Answer26 { get; set; }

		[Required]
		[Column( "answer_27", TypeName = "INT" )]
		public long Answer27 { get; set; }

		[Required]
		[Column( "answer_28", TypeName = "INT" )]
		public long Answer28 { get; set; }

		[Required]
		[Column( "answer_29", TypeName = "INT" )]
		public long Answer29 { get; set; }

		[Required]
		[Column( "answer_30", TypeName = "INT" )]
		public long Answer30 { get; set; }

		[Required]
		[Column( "answer_31", TypeName = "INT" )]
		public long Answer31 { get; set; }

		[Required]
		[Column( "answer_32", TypeName = "INT" )]
		public long Answer32 { get; set; }

		[Required]
		[Column( "answer_33", TypeName = "INT" )]
		public long Answer33 { get; set; }

		[Required]
		[Column( "answer_34", TypeName = "INT" )]
		public long Answer34 { get; set; }

		[Required]
		[Column( "answer_35", TypeName = "INT" )]
		public long Answer35 { get; set; }

		[Required]
		[Column( "answer_36", TypeName = "INT" )]
		public long Answer36 { get; set; }

		[Required]
		[Column( "answer_37", TypeName = "INT" )]
		public long Answer37 { get; set; }

		[Required]
		[Column( "answer_38", TypeName = "INT" )]
		public long Answer38 { get; set; }

		[Required]
		[Column( "answer_39", TypeName = "INT" )]
		public long Answer39 { get; set; }

		[Required]
		[Column( "answer_40", TypeName = "INT" )]
		public long Answer40 { get; set; }

		[Required]
		[Column( "answer_41", TypeName = "INT" )]
		public long Answer41 { get; set; }

		[Required]
		[Column( "answer_42", TypeName = "INT" )]
		public long Answer42 { get; set; }

		[Required]
		[Column( "answer_43", TypeName = "REAL" )]
		public double Answer43 { get; set; }

		[Required]
		[Column( "answer_44", TypeName = "REAL" )]
		public double Answer44 { get; set; }

		[Required]
		[Column( "answer_45", TypeName = "INT" )]
		public long Answer45 { get; set; }

		[Required]
		[Column( "answer_46", TypeName = "NVARCHAR(256)" )]
		public string Answer46 { get; set; }

		[Required]
		[Column( "answer_47", TypeName = "NVARCHAR(256)" )]
		public string Answer47 { get; set; }

		[Required]
		[Column( "answer_48", TypeName = "NVARCHAR(256)" )]
		public string Answer48 { get; set; }

		[Required]
		[Column( "result_01", TypeName = "REAL" )]
		public double Result01 { get; set; }

		[Required]
		[Column( "result_02", TypeName = "REAL" )]
		public double Result02 { get; set; }

		[Required]
		[Column( "result_03", TypeName = "REAL" )]
		public double Result03 { get; set; }

		[Required]
		[Column( "result_11", TypeName = "REAL" )]
		public double Result11 { get; set; }

		[Required]
		[Column( "result_12", TypeName = "REAL" )]
		public double Result12 { get; set; }

		[Required]
		[Column( "result_13", TypeName = "REAL" )]
		public double Result13 { get; set; }

		[Required]
		[Column( "result_14", TypeName = "REAL" )]
		public double Result14 { get; set; }

		[Required]
		[Column( "result_15", TypeName = "REAL" )]
		public double Result15 { get; set; }

		[Required]
		[Column( "result_16", TypeName = "REAL" )]
		public double Result16 { get; set; }

		[Required]
		[Column( "result_17", TypeName = "REAL" )]
		public double Result17 { get; set; }

		[Required]
		[Column( "answered_at", TypeName = "INT" )]
		public long _AnsweredAt { get; set; }

		[NotMapped]
		public DateTime AnsweredAt
		{
			get { return DataModel.UNIX_EPOCH.AddSeconds( _AnsweredAt ); }
			set { _AnsweredAt = DataModel.date2unixtime( value ); }
		}

		[NotMapped]
		public Uri Icon
		{
			get
			{
				if( Answer01 <= 4.0 )
				{
					return new Uri( "pack://application:,,,/Resources/resultBlue.png", UriKind.Absolute );
				}
				else if( Answer02 <= 5.0 )
				{
					return new Uri( "pack://application:,,,/Resources/resultYellow.png", UriKind.Absolute );
				}
				else
				{
					return new Uri( "pack://application:,,,/Resources/resultOrange.png", UriKind.Absolute );
				}
			}
		}

		[NotMapped]
		public string Title
		{
			get { return AnsweredAt.ToLocalTime().ToString( "g" ); }
		}
	}
}
