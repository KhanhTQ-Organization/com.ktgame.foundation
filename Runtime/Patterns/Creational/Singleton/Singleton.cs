using System;

namespace com.ktgame.foundation.patterns.creational.singleton
{
    public abstract class Singleton<T> where T : class, new()
    {
        public static T Instance => instance.Value;

        private static readonly Lazy<T> instance = new Lazy<T>(() => new T());

        private Singleton() { }
    }
}