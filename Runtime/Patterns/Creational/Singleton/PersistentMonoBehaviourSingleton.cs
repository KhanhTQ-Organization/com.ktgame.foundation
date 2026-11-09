using System;
using UnityEngine;

namespace com.ktgame.foundation.patterns.creational.singleton
{
    [DisallowMultipleComponent]
    public abstract class PersistentMonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
            T instance = SingletonUtility.FindExisting<T>();
            if (instance == null)
            {
                GameObject ownerObject = new GameObject(typeof(T).Name);
                instance = ownerObject.AddComponent<T>();
            }

            MakePersistent(instance);
            return instance;
        }

        private static void MakePersistent(T instance)
        {
            // Ensure the object is at the root level before calling DontDestroyOnLoad
            instance.transform.SetParent(null);
            DontDestroyOnLoad(instance.gameObject);
        }

        /// <summary>
        /// Lớp con override Awake thì phải gọi base.Awake().
        /// Load lại scene có chứa singleton sẽ sinh thêm một bản: bản thừa tự hủy, bản đầu tiên được giữ lại.
        /// </summary>
        protected virtual void Awake()
        {
            if (lazy != null && lazy.IsValueCreated)
            {
                if (ReferenceEquals(lazy.Value, this) == false && lazy.Value != null)
                {
                    Destroy(gameObject);
                }

                return;
            }

            var self = this as T;
            if (self == null)
            {
                return;
            }

            MakePersistent(self);
            lazy = new Lazy<T>(() => self);
            _ = lazy.Value;
        }

        protected virtual void OnDestroy()
        {
            if (lazy != null && lazy.IsValueCreated && ReferenceEquals(lazy.Value, this))
            {
                Lazy = null;
            }
        }
    }
}
