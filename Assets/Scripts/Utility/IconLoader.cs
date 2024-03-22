using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class IconLoader
    {
        const string AssetBundleName = "icons";
        const string DefaultIconName = "Empty Icon";

        static Dictionary<string, Sprite> icons = new();
        static UnityEngine.AssetBundle assetBundle;

        public static Sprite LoadIcon(string name)
        {
            if (icons.TryGetValue(name, out var loadIcon)) return loadIcon;

            assetBundle ??= UnityEngine.AssetBundle.LoadFromFile($"Assets/Asset Bundles/{AssetBundleName}");

            var icon = assetBundle.LoadAsset<Sprite>(name);
            if (icon == null)
            {
                Debug.LogError("Icon not found in asset bundle, loading default icon.)");
                icon = assetBundle.LoadAsset<Sprite>(DefaultIconName);
            }

            icons.Add(name, icon);
            return icon;
        }
    }
}