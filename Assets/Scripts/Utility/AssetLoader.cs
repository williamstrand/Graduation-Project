using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public abstract class AssetLoader
    {
        protected static Dictionary<string, UnityEngine.AssetBundle> AssetBundles { get; } = new();
    }

    public class AssetLoader<T> : AssetLoader where T : Object
    {
        string AssetBundleName { get; }
        Dictionary<string, T> Assets { get; }
        T DefaultAsset { get; }

        public AssetLoader(string assetBundleName, string defaultName = "")
        {
            AssetBundleName = assetBundleName;
            Assets = new Dictionary<string, T>();
            DefaultAsset = defaultName == "" ? null : LoadAsset(defaultName);
        }

        public T LoadAsset(string name)
        {
            if (name == "") return DefaultAsset;

            // Check if asset is already loaded
            if (Assets.TryGetValue(name, out var loadedAsset)) return loadedAsset;

            // Check if AssetBundle is already loaded
            if (!AssetBundles.TryGetValue(AssetBundleName, out var assetBundle))
            {
                assetBundle = UnityEngine.AssetBundle.LoadFromFile($"Assets/Asset Bundles/{AssetBundleName}");
                AssetBundles.Add(AssetBundleName, assetBundle);
            }

            // Load asset from AssetBundle
            var asset = assetBundle.LoadAsset<T>(name);

            // If no asset was found, return default asset
            if (asset == null)
            {
                var gameObject = assetBundle.LoadAsset<GameObject>(name);
                if (gameObject)
                {
                    if (!gameObject.TryGetComponent(out asset))
                    {
                        Debug.LogError($"{typeof(T).Name} not found in AssetBundle.");
                        return DefaultAsset;
                    }
                }
                else
                {
                    Debug.LogError($"{typeof(T).Name} not found in AssetBundle.");
                    return DefaultAsset;
                }
            }

            // Cache asset and return it
            Assets.Add(name, asset);
            return asset;
        }
    }
}