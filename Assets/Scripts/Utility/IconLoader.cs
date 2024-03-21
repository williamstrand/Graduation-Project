using UnityEngine;

namespace Utility
{
    public static class IconLoader
    {
        const string AssetBundleName = "icons";
        const string DefaultIconName = "Empty Icon";

        static UnityEngine.AssetBundle assetBundle;

        public static Sprite LoadIcon(string name)
        {
            assetBundle ??= UnityEngine.AssetBundle.LoadFromFile($"Assets/Asset Bundles/{AssetBundleName}");

            var asset = assetBundle.LoadAsset<Sprite>(name);
            if (asset != null) return asset;

            Debug.LogError("Icon not found in asset bundle, loading default icon.)");

            return assetBundle.LoadAsset<Sprite>(DefaultIconName);
        }
    }
}