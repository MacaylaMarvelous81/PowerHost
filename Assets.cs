using Reactor.Utilities.Extensions;
using UnityEngine;

namespace PowerHost
{
	public static class Assets
	{
		public static Sprite UIButtonExecute { get; private set; }
		
		internal static void LoadAssets()
		{
			AssetBundle assetBundle = AssetBundle.LoadFromStream(typeof(PowerHostPlugin).Assembly
				.GetManifestResourceStream("PowerHost.Resources.powerhost").AsIl2Cpp());
			
			UIButtonExecute = assetBundle.LoadAsset<Sprite>("UI_Button_Execute").DontDestroy();
			
			assetBundle.Unload(false);
		}
	}
}