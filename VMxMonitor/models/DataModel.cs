using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace VMxMonitor.models
{
	public class DataModel : DbContext
	{
		/* =====[ constants ]===== */
		public static DateTime UNIX_EPOCH = new( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );

		/* =====[ properties ]===== */
		public DbSet<CommunityModel> CommunityModel { get; set; }
		public DbSet<UserModel> UserModel { get; set; }
		public DbSet<MonitorModel> MonitorModel { get; set; }
		public DbSet<InquiryModel> InquiryModel { get; set; }
		public DbSet<SequenceModel> SequenceModel { get; set; }

		// ConnectionString プロパティの追加
		public string ConnectionString { get; private set; }

		public static DataModel create()
		{
			// データベースファイルのパスを取得
			var databasePath = getDatabaseFile();

			// DbContextOptionsを構築
			var optionsBuilder = new DbContextOptionsBuilder<DataModel>();
			optionsBuilder.UseSqlite( $"Data Source={databasePath}" );

			// DataModelの新しいインスタンスを作成
			return new DataModel( optionsBuilder.Options, databasePath );
		}

		protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
		{
			string databasePath = getDatabaseFile();
			optionsBuilder.UseSqlite( $"Data Source={databasePath}" );
			ConnectionString = $"Data Source={databasePath}";
		}

		public static string getDatabaseFile()
		{
			string root = Environment.GetFolderPath( Environment.SpecialFolder.CommonApplicationData );
			string dir = Path.Combine( root, "FMCC", "VMxMonitor1" );
			if( !Directory.Exists( dir ) )
			{
				Directory.CreateDirectory( dir );
			}
			string dbPath = Path.Combine( dir, "database.sqlite" );
			if( !File.Exists( dbPath ) )
			{
				string[] args = Environment.GetCommandLineArgs();
				dir = Path.GetDirectoryName( args[0] );
				string file = Path.Combine( dir, "data", "database.sqlite" );
				File.Copy( file, dbPath );
			}
			return dbPath;
		}

		public static int date2unixtime( DateTime date )
		{
			TimeSpan span = date.ToUniversalTime() - UNIX_EPOCH;
			return ( int )span.TotalSeconds;
		}

		public SequenceModel getSequence( string table )
		{
			var sequence = SequenceModel.FirstOrDefault( s => s.Name == table );
			if( sequence == null )
			{
				sequence = new SequenceModel { Name = table, Id = 1 };
				SequenceModel.Add( sequence );
				SaveChanges();
			}
			return sequence;
		}

		// コンストラクタでDbContextOptionsとConnectionStringを受け取るように変更
		public DataModel( DbContextOptions<DataModel> options, string connectionString ) : base( options )
		{
			ConnectionString = connectionString;
		}

		// SubmitChanges メソッドの追加
		public void SubmitChanges()
		{
			SaveChanges();
		}
	}
}
