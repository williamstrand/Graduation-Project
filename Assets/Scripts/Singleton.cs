using UnityEngine;

namespace WSP
{
    public static class Singleton<T> where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        public static void Initialize(T newInstance)
        {
            if (Instance != null) return;

            Instance = newInstance;
        }
    }
}