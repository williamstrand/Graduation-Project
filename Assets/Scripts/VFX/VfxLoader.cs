using System.Collections.Generic;
using UnityEngine;

namespace WSP.VFX
{
    public static class VfxLoader
    {
        const string AssetBundleName = "vfx";

        static Dictionary<string, VfxObject> vfxs = new();
        static AssetBundle assetBundle;

        public static VfxObject LoadVfx(string name)
        {
            if (vfxs.TryGetValue(name, out var loadIcon)) return loadIcon;

            assetBundle ??= AssetBundle.LoadFromFile($"Assets/Asset Bundles/{AssetBundleName}");

            var vfx = assetBundle.LoadAsset<GameObject>(name).GetComponent<VfxObject>();
            if (vfx == null)
            {
                Debug.LogError("Vfx not found in AssetBundle.)");
                return null;
            }

            vfxs.Add(name, vfx);
            return vfx;
        }
    }
}