using UnityEngine;
using UnityEditor;

public class BuildAssetBundles : MonoBehaviour
{
	[MenuItem("Assets/Build Asset Bundles")]
	static void BuildAllAssetBundles()
	{
		BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None,
			BuildTarget.StandaloneWindows);
	}
}
