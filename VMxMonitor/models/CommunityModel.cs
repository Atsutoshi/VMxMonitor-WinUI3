using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VMxMonitor.models
{
	[Table( "t_community" )]
	public class CommunityModel
	{
		public const string TABLE_NAME = "t_community";

		[Key]
		[DatabaseGenerated( DatabaseGeneratedOption.Identity )]
		[Column( "id", TypeName = "INT" )]
		public Int64 Id { get; set; }

		[Required]
		[Column( "name", TypeName = "NVARCHAR" )]
		public String Name { get; set; }

		[Required]
		[Column( "password", TypeName = "NVARCHAR" )]
		public String Password { get; set; }

		[Column( "registered_at", TypeName = "INT" )]
		public Int64 _RegisteredAt { get; set; }

		[NotMapped]
		public DateTime RegisteredAt
		{
			get { return DataModel.UNIX_EPOCH.AddSeconds( _RegisteredAt ); }
			set { _RegisteredAt = DataModel.date2unixtime( value ); }
		}

		[NotMapped]
		public Uri Icon
		{
			get
			{
				// Assuming the icon is placed in the Assets folder of the project
				var baseUri = "ms-appx:///Assets/";
				var iconUri = new Uri( $"{baseUri}groupIcon.png", UriKind.Absolute );
				return iconUri;
			}
		}
	}
}
