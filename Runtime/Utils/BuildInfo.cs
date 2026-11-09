#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
#endif
using UnityEngine;

namespace com.ktgame.foundation.utils
{
    [CreateAssetMenu(fileName = "BuildInfoSettings", menuName = "Build info settings")]
    public class BuildInfo : ScriptableObject
    {
        public static BuildInfo Instance
        {
            get
            {
                if (_loaded == false || (_instance == null && Application.isEditor))
                {
                    _instance = LoadInfo();
                    _loaded = true;
                }

                return _instance;
            }
        }

        private static BuildInfo _instance;
        private static bool _loaded;

        public string Version;
        public string BuildId;

        private const string FilePath = "Assets/Resources/BuildInfoSettings.asset";

        private static BuildInfo LoadInfo()
        {
            var result = Resources.Load<BuildInfo>("BuildInfoSettings");
            if (result == null)
            {
#if UNITY_EDITOR
                var directory = Path.GetDirectoryName(FilePath);

                if (directory == null)
                {
                    throw new InvalidOperationException($"Failed to get directory from package settings path: {FilePath}");
                }

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                result = CreateInstance<BuildInfo>();
                AssetDatabase.CreateAsset(result, FilePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#else
                Debug.LogWarning("You need create asset file before build from context menu \"Assets/Create/Build info settings\"");
#endif
            }

            return result;
        }
    }
}