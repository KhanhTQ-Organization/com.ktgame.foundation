using System;
using UnityEngine;

namespace com.ktgame.foundation.patterns.creational.singleton
{
    [DisallowMultipleComponent]
    public abstract class PersistentMonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary> Instance. </summary>
        public static T Instance => Lazy.Value;

        /// <summary> Instance created? </summary>
        public static bool IsCreated => lazy != null && lazy.IsValueCreated == true;

        private static Lazy<T> Lazy
        {
            get => lazy ??= new Lazy<T>(LazyCreate);
            set => lazy = value;
        }

        private static Lazy<T> lazy;

        private static T LazyCreate()
        {
            T instance = FindObjectOfType<T>(true);
            if (instance is null)
            {
                GameObject ownerObject = new GameObject(typeof(T).Name);
                instance = ownerObject.AddComponent<T>();
            }

            // Ensure the object is at the root level before calling DontDestroyOnLoad
            instance.transform.SetParent(null);
            DontDestroyOnLoad(instance.gameObject);

            return instance;
        }

        /// <remarks> Don't forget to call 'base.OnDestroy()' in the overloaded method. </remarks>>
        protected virtual void OnDestroy() => Lazy = null;
    }
}