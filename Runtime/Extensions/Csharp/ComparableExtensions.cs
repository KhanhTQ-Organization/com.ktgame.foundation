using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace com.ktgame.foundation.extensions.csharp
{
    public static class ComparableExtensions
    {
        #region Core

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBetween<T>(
            this T value,
            T min,
            T max,
            bool includeMin = true,
            bool includeMax = true,
            IComparer<T> comparer = null)
        {
            comparer ??= Comparer<T>.Default;

            // swap nếu min > max
            if (comparer.Compare(min, max) > 0)
                (min, max) = (max, min);

            int cMin = comparer.Compare(value, min);
            int cMax = comparer.Compare(value, max);

            if (includeMin)
            {
                if (cMin < 0) return false;
            }
            else
            {
                if (cMin <= 0) return false;
            }

            if (includeMax)
            {
                if (cMax > 0) return false;
            }
            else
            {
                if (cMax >= 0) return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBetweenExclusive<T>(this T value, T min, T max, IComparer<T> comparer = null)
            => value.IsBetween(min, max, false, false, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBetweenInclusive<T>(this T value, T min, T max, IComparer<T> comparer = null)
            => value.IsBetween(min, max, true, true, comparer);

        #endregion

        #region Fast Range (no flag, fastest path)

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool InRange<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            if (min.CompareTo(max) > 0)
                (min, max) = (max, min);

            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }

        #endregion

        #region Clamp

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            if (min.CompareTo(max) > 0)
                (min, max) = (max, min);

            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;

            return value;
        }

        public static T Clamp<T>(this T value, T min, T max, IComparer<T> comparer)
        {
            comparer ??= Comparer<T>.Default;

            if (comparer.Compare(min, max) > 0)
                (min, max) = (max, min);

            if (comparer.Compare(value, min) < 0) return min;
            if (comparer.Compare(value, max) > 0) return max;

            return value;
        }

        #endregion

        #region Compare Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGreaterThan<T>(this T value, T other) where T : IComparable<T>
            => value.CompareTo(other) > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLessThan<T>(this T value, T other) where T : IComparable<T>
            => value.CompareTo(other) < 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGreaterOrEqual<T>(this T value, T other) where T : IComparable<T>
            => value.CompareTo(other) >= 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsLessOrEqual<T>(this T value, T other) where T : IComparable<T>
            => value.CompareTo(other) <= 0;

        #endregion
    }
}