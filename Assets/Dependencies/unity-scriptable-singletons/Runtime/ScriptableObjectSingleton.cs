using UnityEngine;

namespace Abrusle
{
    public abstract class ScriptableObjectSingleton<TObject> : ScriptableObject where TObject : ScriptableObject
    {
        private static TObject _Instance;

        public static TObject Instance
        {
            get
            {
                if (_Instance == null) CreateOrLoadInstance();
                return _Instance;
            }
        }

        private static void CreateOrLoadInstance()
        {
            string filePath = GetResourcePath();
            if (!string.IsNullOrEmpty(filePath))
                _Instance = Resources.Load<TObject>(filePath);

#if UNITY_EDITOR
            if (_Instance != null) return;
            _Instance = CreateInstance<TObject>();
            UnityEditor.AssetDatabase.CreateAsset(_Instance, $"Assets/Resources/{filePath}.asset");
#endif
        }

        private static string GetResourcePath()
        {
            var attributes = typeof(TObject).GetCustomAttributes(true);

            foreach (object attribute in attributes)
            {
                if (attribute is AssetPathAttribute pathAttribute)
                    return pathAttribute.Path;
            }
            Debug.LogError($"{typeof(TObject)} does not have {nameof(AssetPathAttribute)}.");
            return string.Empty;
        }

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (_Instance != null && _Instance != this)
                {
                    Debug.LogError($"An instance of {typeof(TObject)} already exist.");
                    DestroyImmediate(this, true);
                }
            }
#endif
        }
    }
}