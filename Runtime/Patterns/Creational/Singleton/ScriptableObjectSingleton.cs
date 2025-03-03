using System;
using UnityEngine;

namespace com.ktgame.foundation.patterns.creational.singleton
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        /// <summary>Instance.</summary>
        public static T Instance => lazy.Value;

        [NonSerialized] private static readonly Lazy<T> lazy = new Lazy<T>(() =>
        {
            var name = typeof(T).Name;
            var instance = Resources.Load<T>(name);
            if (instance is null)
                Debug.LogError($"No instance of '{name}' found in the Resources folder. Create one inside the Resources folder, and name the file '{name}'");

            return instance;
        });
    }   
}