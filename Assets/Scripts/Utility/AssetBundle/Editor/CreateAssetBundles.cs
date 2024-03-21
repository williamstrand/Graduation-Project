#if UNITY_EDITOR
using System.IO;
using UnityEditor;

namespace Utility.AssetBundle.Editor
{
    public static class CreateAssetBundles
    {
        const string AssetBundleDirectory = "Assets/Asset Bundles";

        [MenuItem("Assets/Build AssetBundles")]
        static void BuildAllAssetBundles()
        {
            if (!Directory.Exists(AssetBundleDirectory))
            {
                Directory.CreateDirectory(AssetBundleDirectory);
            }

            BuildPipeline.BuildAssetBundles(AssetBundleDirectory, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows);
        }
    }
}
#endif