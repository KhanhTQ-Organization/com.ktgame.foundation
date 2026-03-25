using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using com.ktgame.foundation.math;

namespace com.ktgame.foundation.extensions.csharp
{
    public static class ListExtensions
    {
        #region Random

        public static T Random<T>(this IList<T> self)
        {
            if (self == null || self.Count == 0)
                return default;

            return self[MathUtilsRandom.Range(0, self.Count)];
        }

        public static bool TryRandom<T>(this IList<T> self, out T value)
        {
            if (self != null && self.Count > 0)
            {
                value = self[MathUtilsRandom.Range(0, self.Count)];
                return true;
            }

            value = default;
            return false;
        }

        #endregion

        #region Swap

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(this IList<T> self, int i, int j)
        {
            if (self == null) return;

            (self[i], self[j]) = (self[j], self[i]);
        }

        #endregion

        #region Remove

        /// <summary>
        /// Remove nhanh O(1) nhưng không giữ thứ tự
        /// </summary>
        public static void RemoveAtSwapBack<T>(this IList<T> self, int index)
        {
            if (self == null || self.Count == 0) return;

            int last = self.Count - 1;

            self[index] = self[last];
            self.RemoveAt(last);
        }

        /// <summary>
        /// Remove nhiều phần tử (optimized)
        /// </summary>
        public static void RemoveRange<T>(this IList<T> self, ICollection<T> entries)
        {
            if (self == null || entries == null || entries.Count == 0)
            {
                return;
            }

            var set = new HashSet<T>(entries);

            for (int i = self.Count - 1; i >= 0; i--)
            {
                if (set.Contains(self[i]))
                {
                    self.RemoveAt(i);
                }
            }
        }
        
        public static void RemoveRange<T>(this IList<T> self, IEnumerable<T> entries)
        {
            if (self == null || entries == null)
            {
                return;
            }

            var set = new HashSet<T>(entries);
            if (set.Count == 0)
            {
                return;
            }

            for (int i = self.Count - 1; i >= 0; i--)
            {
                if (set.Contains(self[i]))
                {
                    self.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Reverse / Shuffle

        public static void Reverse<T>(this IList<T> self)
        {
            if (self == null) return;
            Reverse(self, 0, self.Count);
        }

        public static void Reverse<T>(this IList<T> self, int from, int to)
        {
            if (self == null) return;

            while (--to > from)
                self.Swap(from++, to);
        }

        public static void Shuffle<T>(this IList<T> self)
        {
            if (self == null) return;

            for (int i = self.Count - 1; i > 0; i--)
            {
                int j = MathUtilsRandom.Range(0, i + 1);
                self.Swap(i, j);
            }
        }

        #endregion

        #region Query

        public static bool TryGet<T>(this IList<T> self, int index, out T value)
        {
            if (self != null && index >= 0 && index < self.Count)
            {
                value = self[index];
                return true;
            }

            value = default;
            return false;
        }

        #endregion

        #region Math

        public static T Max<T>(this IList<T> self) where T : IComparable<T>
        {
            if (self == null || self.Count == 0)
                return default;

            T max = self[0];

            for (int i = 1; i < self.Count; i++)
            {
                if (self[i].CompareTo(max) > 0)
                    max = self[i];
            }

            return max;
        }

        public static T Min<T>(this IList<T> self) where T : IComparable<T>
        {
            if (self == null || self.Count == 0)
                return default;

            T min = self[0];

            for (int i = 1; i < self.Count; i++)
            {
                if (self[i].CompareTo(min) < 0)
                    min = self[i];
            }

            return min;
        }

        public static T MaxBy<T>(this IList<T> self, Func<T, float> selector)
        {
            if (self == null || self.Count == 0)
                return default;

            T best = self[0];
            float bestVal = selector(best);

            for (int i = 1; i < self.Count; i++)
            {
                float v = selector(self[i]);
                if (v > bestVal)
                {
                    bestVal = v;
                    best = self[i];
                }
            }

            return best;
        }

        #endregion

        #region Add

        /// <summary>
        /// AddRange không alloc Enumerator (for List)
        /// </summary>
        public static void AddRangeFast<T>(this List<T> self, List<T> other)
        {
            if (self == null || other == null || other.Count == 0)
                return;

            int count = other.Count;
            self.Capacity = Math.Max(self.Capacity, self.Count + count);

            for (int i = 0; i < count; i++)
                self.Add(other[i]);
        }

        #endregion
    }
}