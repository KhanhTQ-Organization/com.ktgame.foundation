using System;
using UnityEngine;

namespace com.ktgame.foundation.patterns.creational.singleton
{
    [DisallowMultipleComponent]
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance => Lazy.Value;

        public static bool IsCreated => lazy != null && lazy.IsValueCreated == true;

        private static Lazy<T> Lazy
        {
            get => lazy ??= new Lazy<T>(LazyCreate);
            set => lazy = value;
        }

        private static Lazy<T> lazy;

        private static T LazyCreate()
        {
            var instance = SingletonUtility.FindExisting<T>();
            return instance != null ? instance : new GameObject(typeof(T).Name).AddComponent<T>();
        }

        protected virtual void OnDestroy()
        {
            // Chỉ reset khi chính instance đang dùng bị hủy (bản cũ reset cả khi một bản trùng bị hủy).
            if (lazy != null && lazy.IsValueCreated && ReferenceEquals(lazy.Value, this))
            {
                Lazy = null;
            }
        }
    }

    internal static class SingletonUtility
    {
        public static T FindExisting<T>() where T : UnityEngine.Object
        {
#if UNITY_2023_1_OR_NEWER
            // FindObjectOfType đã lỗi thời từ Unity 2023.1.
            return UnityEngine.Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);
#else
            return UnityEngine.Object.FindObjectOfType<T>(true);
#endif
        }
    }
}
