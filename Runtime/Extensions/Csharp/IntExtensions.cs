using System;
using UnityEngine;

namespace com.ktgame.foundation.extensions.csharp
{
    /// <summary> Int extensions. </summary>
    public static class IntExtensions
    {
        private static readonly string[] SizeSuffixes = { @"bytes", @"KB", @"MB", @"GB", @"TB", @"PB", @"EB", @"ZB", @"YB" };

        /// <summary> Value sign. </summary>
        /// <param name="self">Value</param>
        /// <returns>1 if greater than or equal to 0, -1 if less than 0.</returns>
        public static int Sign(this int self) => self >= 0 ? 1 : -1;

        /// <summary> Returns the maximum value. </summary>
        /// <param name="a">Value</param>
        /// <param name="b">Value</param>
        /// <returns>Int</returns>
        public static int Max(this int a, int b) => a < b ? b : a;

        /// <summary> Returns the minimum value. </summary>
        /// <param name="a">Value</param>
        /// <param name="b">Value</param>
        /// <returns>Int</returns>
        public static int Min(this int a, int b) => a < b ? a : b;

        /// <summary> Returns the absolute value. </summary>
        /// <param name="self">Value</param>
        /// <returns>Int</returns>
        public static int Abs(this int self) => Math.Abs(self);

        /// <summary> Returns the rounded value. </summary>
        /// <param name="snap"> Rounding distance </param>
        /// <returns>Int</returns>
        public static int Snap(this int self, int snap) => snap > 0 ? Mathf.RoundToInt((float)self / snap) * snap : self;

        /// <summary> Constrain the value to a range. </summary>
        /// <param name="self">Value</param>
        /// <param name="min">Lower range</param>
        /// <param name="max">Upper range</param>
        /// <returns>Int</returns>
        public static int Clamp(this int self, int min, int max)
        {
            if (self < min)
                return min;

            return self > max ? max : self;
        }

        /// <summary> Pow. </summary>
        /// <param name="self">Value</param>
        /// <param name="exp">Exponent</param>
        /// <returns>Int</returns>
        public static int Pow(this int self, int exp)
        {
            int result = 1;

            for (int i = 0; i < exp; ++i)
                result *= self;

            return result;
        }

        /// <summary> Value is even. </summary>
        /// <param name="self">Value</param>
        /// <returns>True/false</returns>
        public static bool IsEven(this int self) => self % 2 == 0;

        /// <summary> Value is odd. </summary>
        /// <param name="self">Value</param>
        /// <returns>True/false</returns>
        public static bool IsOdd(this int self) => self % 2 != 0;

        /// <summary> Next power of two. </summary>
        /// <param name="self">Value</param>
        /// <returns>Int</returns>
        public static int NextPowerOfTwo(this int self)
        {
            self = Math.Abs(self);
            self--;
            self |= self >> 1;
            self |= self >> 2;
            self |= self >> 4;
            self |= self >> 8;
            self |= self >> 16;
            self++;

            return self;
        }

        /// <summary> Calculate the mask of a layer. </summary>
        /// <param name="self">Value</param>
        /// <returns>Int</returns>
        public static int GetMask(this int self) => 1 << self;

        /// <summary>Calculates the mask of a set of layers. </summary>
        /// <param name="self">Value</param>
        /// <returns>Int</returns>
        public static int GetMask(this int[] self)
        {
            int layerMask = 0;
            for (int i = 0; i < self.Length; ++i)
                layerMask |= self[i].GetMask();

            return layerMask;
        }

        /// <summary> Layer included? </summary>
        /// <param name="self">Value</param>
        /// <param name="layermask">Layer mask</param>
        /// <returns>Layer included</returns>
        public static bool IsInLayerMask(this int self, LayerMask layermask) => layermask == (layermask | (1 << self));

        /// <summary> Layer included? </summary>
        /// <param name="self">Value</param>
        /// <param name="layerName">Layer name</param>
        /// <returns>Layer included</returns>
        public static bool IsInLayerMask(this int self, string layerName)
        {
            // NameToLayer trả về chỉ số layer, không phải mask. Bản cũ dùng nó như mask nên kết quả sai
            // (ví dụ layer 0 bị coi là thuộc "UI").
            int layer = LayerMask.NameToLayer(layerName);

            return layer >= 0 && layer == self;
        }

        /// <summary> Bytes to a text string. </summary>
        /// <param name="self">Value</param>
        /// <returns>String</returns>
        public static string BytesToHumanReadable(this int self)
        {
            if (self == 0)
                return "0 bytes";

            // Dùng long: -int.MinValue tràn số và trước đây gây đệ quy vô hạn (StackOverflow).
            long value = self;
            if (value < 0)
                return $"-{BytesToHumanReadable(-value)}";

            return BytesToHumanReadable(value);
        }

        private static string BytesToHumanReadable(long self)
        {
            int mag = (int)Math.Log(self, 1024);
            float adjustedSize = (float)(self / Math.Pow(1024, mag));

            return $"{adjustedSize.ToInvariantCulture()} {SizeSuffixes[mag]}";
        }

        /// <summary> Seconds to a text string. </summary>
        /// <param name="self">Value</param>
        /// <returns>String</returns>
        public static string SecondsToHumanReadable(this int self) => $"{self / 3600:00}:{(self / 60) % 60:00}:{self % 60:00}";

        /// <summary> Returns the number of digits in the number. </summary>
        /// <param name="self">Value.</param>
        /// <returns>The number of digits in the number.</returns>
        public static int NumDigits(this int self)
        {
            // Đếm bằng số nguyên: bản cũ dùng Log10 trên float nên NumDigits(0) = int.MinValue và
            // NumDigits(999999999) = 10 (float làm tròn lên 1e9). Dấu âm không được tính.
            long value = Math.Abs((long)self);
            int digits = 1;
            while (value >= 10)
            {
                value /= 10;
                digits++;
            }

            return digits;
        }
    }
}