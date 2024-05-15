using System.Linq;
using UnityEngine;
using Utility;

namespace WSP.Units
{
    public class HitEffect : MonoBehaviour
    {
        const string ShaderName = "Hit Material";
        const string AssetBundleName = "materials";

        static AssetLoader<Material> assetLoader = new(AssetBundleName);
        static Material HitMaterial => assetLoader.LoadAsset(ShaderName);
        static readonly int White = Shader.PropertyToID("_White");

        [SerializeField] Renderer[] renderers;
        Material material;

        [SerializeField] float speed = 5;
        float lerp = 1;

        void Awake()
        {
            material = Instantiate(HitMaterial);
            material.name = ShaderName;
        }

        void OnEnable()
        {
            for (var i = 0; i < renderers.Length; i++)
            {
                var materials = renderers[i].materials.ToList();

                materials.Add(material);
                renderers[i].materials = materials.ToArray();
            }

            material.SetFloat(White, 0);
        }

        void Update()
        {
            if (lerp >= 1) return;

            lerp += Time.deltaTime * speed;
            material.SetFloat(White, Mathf.Lerp(1, 0, lerp));
        }

        public void Play()
        {
            lerp = 0;
        }
    }
}