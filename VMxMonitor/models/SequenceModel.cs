

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VMxMonitor.models
{
	[Table( "t_sequence" )]
	public class SequenceModel
	{
		[Key]
		[Required]
		[Column( "name", TypeName = "NVARCHAR(256)" )]
		public string Name { get; set; }

		[Required]
		[Column( "id", TypeName = "INT" )]
		public long Id { get; set; }
	}
	//[Table(Name = "t_sequence")]
	//public class SequenceModel
	//{
	//    /* =====[ properties ]===== */

	//    [Column(Name = "name", DbType = "NVARCHAR", CanBeNull = false, IsPrimaryKey = true)]
	//    public String Name { get; set; }
	//    [Column(Name = "id", DbType = "INT", CanBeNull = false)]
	//    public Int64 Id { get; set; }
	//}
}
