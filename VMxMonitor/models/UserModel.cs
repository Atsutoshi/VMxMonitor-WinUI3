
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace VMxMonitor.models
{
	[Table( "t_user" )]
	public class UserModel
	{
		public const string TABLE_NAME = "t_user";
		public const Int32 SEX_MALE = 1;
		public const Int32 SEX_FEMALE = 2;

		[Key]
		[DatabaseGenerated( DatabaseGeneratedOption.Identity )]
		[Column( "id", TypeName = "INT" )]
		public long Id { get; set; }

		[Required]
		[Column( "community_id", TypeName = "INT" )]
		public long CommunityId { get; set; }

		[Required]
		[Column( "name", TypeName = "NVARCHAR(100)" )]
		public string Name { get; set; }

		[Required]
		[Column( "password", TypeName = "NVARCHAR(100)" )]
		public string Password { get; set; }

		[Required]
		[Column( "birthday", TypeName = "INT" )]
		public long _Birthday { get; set; }

		[Required]
		[Column( "sex", TypeName = "INT" )]
		public long Sex { get; set; }

		[Required]
		[Column( "registered_at", TypeName = "INT" )]
		public long _RegisteredAt { get; set; }

		[NotMapped]
		public DateTime Birthday
		{
			get { return DataModel.UNIX_EPOCH.AddSeconds( _Birthday ); }
			set { _Birthday = ( long )( value.ToUniversalTime() - DataModel.UNIX_EPOCH ).TotalSeconds; }
		}

		[NotMapped]
		public DateTime RegisteredAt
		{
			get { return DataModel.UNIX_EPOCH.AddSeconds( _RegisteredAt ); }
			set { _RegisteredAt = ( long )( value.ToUniversalTime() - DataModel.UNIX_EPOCH ).TotalSeconds; }
		}

		[NotMapped]
		public Uri Icon
		{
			get
			{
				if( Sex == SEX_FEMALE )
				{
					return new Uri( "pack://application:,,,/Resources/femaleIcon.png", UriKind.Absolute );
				}
				else
				{
					return new Uri( "pack://application:,,,/Resources/maleIcon.png", UriKind.Absolute );
				}
			}
		}

		[NotMapped]
		public int Age
		{
			get
			{
				var today = DateTime.Today;
				var age = today.Year - Birthday.Year;
				if( Birthday.Date > today.AddYears( -age ) ) age--;
				return age;
			}
		}
	}
}
