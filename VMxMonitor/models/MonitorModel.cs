

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace VMxMonitor.models
{
	[Table( "t_monitor" )]
	public class MonitorModel
	{
		[Key]
		[DatabaseGenerated( DatabaseGeneratedOption.Identity )]
		[Column( "id", TypeName = "INT" )]
		public long Id { get; set; }

		public const string TABLE_NAME = "t_monitor";

		[Required]
		[Column( "community_id", TypeName = "INT" )]
		public long CommunityId { get; set; }

		[Required]
		[Column( "user_id", TypeName = "INT" )]
		public long UserId { get; set; }

		[Required]
		[Column( "file", TypeName = "NVARCHAR(256)" )]
		public string File { get; set; }

		[Required]
		[Column( "error", TypeName = "INT" )]
		public long Error { get; set; }

		[Required]
		[Column( "length", TypeName = "INT" )]
		public long Length { get; set; }

		[Required]
		[Column( "mode", TypeName = "NVARCHAR(100)" )]
		public string Mode { get; set; }

		[Required]
		[Column( "avg_hr", TypeName = "INT" )]
		public long AvgHR { get; set; }

		[Required]
		[Column( "min_hr", TypeName = "INT" )]
		public long MinHR { get; set; }

		[Required]
		[Column( "max_hr", TypeName = "INT" )]
		public long MaxHR { get; set; }

		[Required]
		[Column( "rr", TypeName = "REAL" )]
		public double RR { get; set; }

		[Required]
		[Column( "ra", TypeName = "REAL" )]
		public double RA { get; set; }

		[Required]
		[Column( "hf", TypeName = "REAL" )]
		public double HF { get; set; }

		[Required]
		[Column( "lf", TypeName = "REAL" )]
		public double LF { get; set; }

		[Required]
		[Column( "tp", TypeName = "REAL" )]
		public double TP { get; set; }

		[Required]
		[Column( "lh", TypeName = "REAL" )]
		public double LH { get; set; }

		[Required]
		[Column( "sdlh", TypeName = "REAL" )]
		public double SDLH { get; set; }

		[Required]
		[Column( "cvrr", TypeName = "REAL" )]
		public double CVRR { get; set; }

		[Required]
		[Column( "ccvtp", TypeName = "REAL" )]
		public double CCVTP { get; set; }

		[Required]
		[Column( "age", TypeName = "INT" )]
		public long Age { get; set; }

		[Required]
		[Column( "debiation", TypeName = "INT" )]
		public long Debiation { get; set; }

		[Required]
		[Column( "finished_at", TypeName = "INT" )]
		public long _FinishedAt { get; set; }

		[NotMapped]
		public DateTime FinishedAt
		{
			get { return DataModel.UNIX_EPOCH.AddSeconds( _FinishedAt ); }
			set { _FinishedAt = ( long )( value.ToUniversalTime() - DataModel.UNIX_EPOCH ).TotalSeconds; }
		}

		[NotMapped]
		public Uri Icon
		{
			get
			{
				if( LH <= 2.0 && Debiation >= 43 )
				{
					return new Uri( "pack://application:,,,/Resources/resultBlue.png", UriKind.Absolute );
				}
				else if( LH <= 5.0 && Debiation >= 38 )
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
			get { return FinishedAt.ToLocalTime().ToString( "g" ); }
		}

		[NotMapped]
		public string Rating
		{
			get
			{
				if( LH <= 2.0 && Debiation >= 43 )
				{
					return "Good";
				}
				else if( LH <= 5.0 && Debiation >= 38 )
				{
					return "Warning";
				}
				else
				{
					return "Bad";
				}
			}
		}
	}
	//[Table(Name = "t_monitor")]
	//public class MonitorModel
	//{
	//    /* =====[ constants ]===== */

	//    public const string TABLE_NAME = "t_monitor";

	//    /* =====[ properties ]===== */

	//    [Column(Name = "id", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
	//    public Int64 Id { get; set; }
	//    [Column(Name = "community_id", DbType = "INT", CanBeNull = false)]
	//    public Int64 CommunityId { get; set; }
	//    [Column(Name = "user_id", DbType = "INT", CanBeNull = false)]
	//    public Int64 UserId { get; set; }
	//    [Column(Name = "file", DbType = "NVARCHAR", CanBeNull = false)]
	//    public String File { get; set; }
	//    [Column(Name = "error", DbType = "INT", CanBeNull = false)]
	//    public Int64 Error { get; set; }
	//    [Column(Name = "length", DbType = "INT", CanBeNull = false)]
	//    public Int64 Length { get; set; }
	//    [Column(Name = "mode", DbType = "NVARCHAR", CanBeNull = false)]
	//    public String Mode { get; set; }
	//    [Column(Name = "avg_hr", DbType = "INT", CanBeNull = false)]
	//    public Int64 AvgHR { get; set; }
	//    [Column(Name = "min_hr", DbType = "INT", CanBeNull = false)]
	//    public Int64 MinHR { get; set; }
	//    [Column(Name = "max_hr", DbType = "INT", CanBeNull = false)]
	//    public Int64 MaxHR { get; set; }
	//    [Column(Name = "rr", DbType = "REAL", CanBeNull = false)]
	//    public Double RR { get; set; }
	//    [Column(Name = "ra", DbType = "REAL", CanBeNull = false)]
	//    public Double RA { get; set; }
	//    [Column(Name = "hf", DbType = "REAL", CanBeNull = false)]
	//    public Double HF { get; set; }
	//    [Column(Name = "lf", DbType = "REAL", CanBeNull = false)]
	//    public Double LF { get; set; }
	//    [Column(Name = "tp", DbType = "REAL", CanBeNull = false)]
	//    public Double TP { get; set; }
	//    [Column(Name = "lh", DbType = "REAL", CanBeNull = false)]
	//    public Double LH { get; set; }
	//    [Column(Name = "sdlh", DbType = "REAL", CanBeNull = false)]
	//    public Double SDLH { get; set; }
	//    [Column(Name = "cvrr", DbType = "REAL", CanBeNull = false)]
	//    public Double CVRR { get; set; }
	//    [Column(Name = "ccvtp", DbType = "REAL", CanBeNull = false)]
	//    public Double CCVTP { get; set; }
	//    [Column(Name = "age", DbType = "INT", CanBeNull = false)]
	//    public Int64 Age { get; set; }
	//    [Column(Name = "debiation", DbType = "INT", CanBeNull = false)]
	//    public Int64 Debiation { get; set; }
	//    [Column(Name = "finished_at", DbType = "INT", CanBeNull = false)]
	//    public Int64 _FinishedAt { get; set; }
	//    //
	//    public DateTime FinishedAt
	//    {
	//        get
	//        {
	//            return DataModel.UNIX_EPOCH.AddSeconds(_FinishedAt);
	//        }
	//        set
	//        {
	//            _FinishedAt = DataModel.date2unixtime(value);
	//        }
	//    }
	//    public Uri Icon
	//    {
	//        get
	//        {
	//            if (LH <= 2.0 && Debiation >= 43)
	//            {
	//                return new Uri("pack://application:,,,/Resources/resultBlue.png", UriKind.Absolute);
	//            }
	//            else if(LH <= 5.0 && Debiation >= 38)
	//            {
	//                return new Uri("pack://application:,,,/Resources/resultYellow.png", UriKind.Absolute);
	//            }
	//            else
	//            {
	//                return new Uri("pack://application:,,,/Resources/resultOrange.png", UriKind.Absolute);
	//            }
	//        }
	//    }
	//    public string Title
	//    {
	//        get
	//        {
	//            return FinishedAt.ToLocalTime().ToString("g");
	//        }
	//    }
	//    public string Rating
	//    {
	//        get
	//        {
	//            if (LH <= 2.0 && Debiation >= 43)
	//            {
	//                return Properties.Resources.labelResultGood;
	//            }
	//            else if (LH <= 5.0 && Debiation >= 38)
	//            {
	//                return Properties.Resources.labelResultWarning;
	//            }
	//            else
	//            {
	//                return Properties.Resources.labelResultBad;
	//            }
	//        }
	//    }
	//}
}
