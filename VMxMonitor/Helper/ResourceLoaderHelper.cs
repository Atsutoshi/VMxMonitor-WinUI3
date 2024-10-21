using Windows.ApplicationModel.Resources;

namespace VMxMonitor.Helper
{
	public static class ResourceLoaderHelper
	{
		private static ResourceLoader resourceLoader = new ResourceLoader( "Properties/Resources" );

		public static string GetString( string resourceName )
		{
			return resourceLoader.GetString( resourceName );
		}
	}
}
