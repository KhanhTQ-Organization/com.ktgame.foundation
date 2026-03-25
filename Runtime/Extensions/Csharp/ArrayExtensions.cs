using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using com.ktgame.foundation.math;

namespace com.ktgame.foundation.extensions.csharp
{
	public static class ArrayExtensions
	{
#region Append
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] Append<T>(this T[] self, T item)
		{
			if (self == null)
				return new[] { item };

			int len = self.Length;
			var result = new T[len + 1];

			Array.Copy(self, result, len);
			result[len] = item;

			return result;
		}

		public static T[] Append<T>(this T[] self, T[] items)
		{
			if (self == null)
				return items?.Copy();
			if (items == null || items.Length == 0)
				return self.Copy();

			int len = self.Length;
			int len2 = items.Length;

			var result = new T[len + len2];

			Array.Copy(self, 0, result, 0, len);
			Array.Copy(items, 0, result, len, len2);

			return result;
		}
#endregion
		
#region Clear
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clear<T>(this T[] self)
		{
			if (self == null)
				return;

			Array.Clear(self, 0, self.Length);
		}

		/// <summary>
		/// Sets a range of elements in the array to default.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clear<T>(this T[] self, int index, int count)
		{
			if (self == null || count <= 0)
				return;

			int len = self.Length;

			if (index < 0)
				index = 0;

			if (index >= len)
				return;

			if (index + count > len)
				count = len - index;

			Array.Clear(self, index, count);
		}
#endregion

#region Contains / Index
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Contains<T>(this T[] self, T value)
		{
			if (self == null)
				return false;

			var comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < self.Length; i++)
			{
				if (comparer.Equals(self[i], value))
					return true;
			}

			return false;
		}

		public static int IndexOf<T>(this T[] self, T value)
		{
			if (self == null)
				return -1;

			var comparer = EqualityComparer<T>.Default;

			for (int i = 0; i < self.Length; i++)
			{
				if (comparer.Equals(self[i], value))
					return i;
			}

			return -1;
		}

		public static int IndexOf<T>(this T[] self, Func<T, bool> predicate)
		{
			if (self == null || predicate == null)
				return -1;

			for (int i = 0; i < self.Length; i++)
			{
				if (predicate(self[i]))
					return i;
			}

			return -1;
		}
#endregion

#region Remove
		public static T[] RemoveAt<T>(this T[] self, int index)
		{
			if (self == null || self.Length == 0)
				return Array.Empty<T>();

			if (index < 0 || index >= self.Length)
				return self.Copy();

			int len = self.Length;
			var result = new T[len - 1];

			if (index > 0)
				Array.Copy(self, 0, result, 0, index);

			if (index < len - 1)
				Array.Copy(self, index + 1, result, index, len - index - 1);

			return result;
		}

		public static T[] Remove<T>(this T[] self, T value)
		{
			int index = self.IndexOf(value);
			return index >= 0 ? self.RemoveAt(index) : self;
		}
#endregion

#region Fill
		public static void Fill<T>(this T[] self, T value)
		{
			if (self == null)
				return;

			for (int i = 0; i < self.Length; i++)
				self[i] = value;
		}

		public static void Fill<T>(this T[] self, Func<int, T> filler)
		{
			if (self == null || filler == null)
				return;

			for (int i = 0; i < self.Length; i++)
				self[i] = filler(i);
		}
#endregion

#region Random
		public static T Random<T>(this T[] self)
		{
			if (self == null || self.Length == 0)
				return default;

			return self[MathUtilsRandom.Range(0, self.Length)];
		}
#endregion

#region Copy / Sub
		public static T[] Copy<T>(this T[] self)
		{
			if (self == null)
				return null;

			var result = new T[self.Length];
			Array.Copy(self, result, self.Length);
			return result;
		}

		public static T[] Sub<T>(this T[] self, int offset, int length)
		{
			if (self == null || length <= 0)
				return Array.Empty<T>();

			int max = self.Length;

			if (offset < 0)
				offset = 0;
			if (offset >= max)
				return Array.Empty<T>();

			if (offset + length > max)
				length = max - offset;

			var result = new T[length];
			Array.Copy(self, offset, result, 0, length);

			return result;
		}
#endregion

#region Swap / Reverse / Shuffle
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Swap<T>(this T[] self, int i, int j)
		{
			if (self == null)
				return;

			(self[i], self[j]) = (self[j], self[i]);
		}

		public static void Reverse<T>(this T[] self)
		{
			if (self == null)
				return;

			Reverse(self, 0, self.Length);
		}

		public static void Reverse<T>(this T[] self, int from, int to)
		{
			if (self == null)
				return;

			while (--to > from)
				self.Swap(from++, to);
		}

		public static void Shuffle<T>(this T[] self)
		{
			if (self == null)
				return;

			for (int i = self.Length - 1; i > 0; i--)
			{
				int j = MathUtilsRandom.Range(0, i + 1);
				self.Swap(i, j);
			}
		}
#endregion

#region Math
		public static int Sum(this int[] self)
		{
			if (self == null)
				return 0;

			int sum = 0;
			for (int i = 0; i < self.Length; i++)
				sum += self[i];

			return sum;
		}

		public static float Sum(this float[] self)
		{
			if (self == null)
				return 0;

			float sum = 0;
			for (int i = 0; i < self.Length; i++)
				sum += self[i];

			return sum;
		}

		public static T Max<T>(this T[] self) where T : IComparable<T>
		{
			if (self == null || self.Length == 0)
				return default;

			T max = self[0];

			for (int i = 1; i < self.Length; i++)
			{
				if (self[i].CompareTo(max) > 0)
					max = self[i];
			}

			return max;
		}

		public static T Min<T>(this T[] self) where T : IComparable<T>
		{
			if (self == null || self.Length == 0)
				return default;

			T min = self[0];

			for (int i = 1; i < self.Length; i++)
			{
				if (self[i].CompareTo(min) < 0)
					min = self[i];
			}

			return min;
		}
#endregion

#region Advanced (PRO)
		public static T MaxBy<T>(this T[] self, Func<T, float> selector)
		{
			if (self == null || self.Length == 0)
				return default;

			T best = self[0];
			float bestVal = selector(best);

			for (int i = 1; i < self.Length; i++)
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

		public static bool TryGet<T>(this T[] self, int index, out T value)
		{
			if (self != null && index >= 0 && index < self.Length)
			{
				value = self[index];
				return true;
			}

			value = default;
			return false;
		}
#endregion
	}
}
