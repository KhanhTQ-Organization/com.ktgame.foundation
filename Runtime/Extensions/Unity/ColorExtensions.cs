using com.ktgame.foundation.math;
using System.Globalization;
using UnityEngine;

namespace com.ktgame.foundation.extensions.unity
{
/// <summary> Color extensions. </summary>
    public static class ColorExtensions
    {
        /// <summary> Color with the R component changed. </summary>
        /// <param name="r">component</param>
        /// <returns>Color</returns>
        public static Color SetR(this Color self, float r) => new Color(r, self.g, self.b, self.a);

        /// <summary> Color with the G component changed. </summary>
        /// <param name="r">component</param>
        /// <returns>Color</returns>
        public static Color SetG(this Color self, float g) => new Color(self.r, g, self.b, self.a);

        /// <summary> Color with the B component changed. </summary>
        /// <param name="r">component</param>
        /// <returns>Color</returns>
        public static Color SetB(this Color self, float b) => new Color(self.r, self.g, b, self.a);

        /// <summary> Color with the A component changed. </summary>
        /// <param name="r">component</param>
        /// <returns>Color</returns>
        public static Color SetA(this Color self, float a) => new Color(self.r, self.g, self.b, a);

        /// <summary> Color with the hue component displaced. </summary>
        /// <param name="hue">Hue</param>
        /// <param name="hdr">HDR color?</param>
        /// <returns>Color</returns>
        public static Color SetHue(this Color self, float hue, bool hdr = false)
        {
            Color.RGBToHSV(self, out _, out var saturation, out var value);
            return Color.HSVToRGB(hue, saturation, value, hdr);
        }

        /// <summary> Color with the saturation changed. </summary>
        /// <param name="saturation">Saturation</param>
        /// <param name="hdr">HDR color?</param>
        /// <returns>Color</returns>
        public static Color SetSaturation(this Color self, float saturation, bool hdr = false)
        {
            Color.RGBToHSV(self, out var hue, out _, out var value);
            return Color.HSVToRGB(hue, saturation, value, hdr);
        }

        /// <summary> Color with the value (HSV space) changed. </summary>
        /// <param name="value">Value</param>
        /// <param name="hdr">HDR color?</param>
        /// <returns>Color</returns>
        public static Color SetValue(this Color self, float value, bool hdr = false)
        {
            Color.RGBToHSV(self, out var hue, out var saturation, out _);
            return Color.HSVToRGB(hue, saturation, value, hdr);
        }

        /// <summary> From hex string. </summary>
        /// <param name="text">HTML color, ie '#FF00FF'</param>
        /// <returns>String</returns>
        public static Color FromHex(this string text)
        {
            ColorUtility.TryParseHtmlString(text, out Color color);
            return color;
        }

        /// <summary> To hex string. </summary>
        /// <returns>String</returns>
        public static string ToHex(this Color self)
        {
            Color32 color32 = self;
            return $"#{color32.r:X2}{color32.g:X2}{color32.b:X2}";
        }

        /// <summary> Color to string. </summary>
        /// <param name="self">Value</param>
        /// <returns></returns>
        public static string ToString(this Color self) =>
            string.Join(",", self.r.ToString(CultureInfo.InvariantCulture), self.g.ToString(CultureInfo.InvariantCulture),
                self.b.ToString(CultureInfo.InvariantCulture), self.a.ToString(CultureInfo.InvariantCulture));

        /// <summary> Brightness correction. </summary>
        /// <param name="correctionFactor">Brightness correction factor [-1 - 1]</param>
        /// <returns>Color with corrected brightness.</returns>
        public static Color ChangeBrightness(this Color self, float correctionFactor)
        {
            correctionFactor = Mathf.Clamp(correctionFactor, -1.0f, 1.0f);

            float red = self.r;
            float green = self.g;
            float blue = self.b;

            if (correctionFactor < 0.0f)
            {
                float factor = 1.0f + correctionFactor;
                red *= factor;
                green *= factor;
                blue *= factor;
            }
            else
            {
                red += (1.0f - red) * correctionFactor;
                green += (1.0f - green) * correctionFactor;
                blue += (1.0f - blue) * correctionFactor;
            }

            return new Color(red, green, blue, self.a);
        }

        /// <summary> Is it almost black? </summary>
        /// <returns>True if it is almost black.</returns>
        public static bool IsApproximatelyBlack(this Color self) => self.r <= MathConstants.ComparisonEpsilon &&
                                                                    self.g <= MathConstants.ComparisonEpsilon &&
                                                                    self.b <= MathConstants.ComparisonEpsilon;

        /// <summary> Is it almost white? </summary>
        /// <returns>True if it is almost white.</returns>
        public static bool IsApproximatelyWhite(this Color self) => self.r >= 1.0f - MathConstants.ComparisonEpsilon &&
                                                                    self.g >= 1.0f - MathConstants.ComparisonEpsilon &&
                                                                    self.b >= 1.0f - MathConstants.ComparisonEpsilon;

        /// <summary> Opaque version of the color. </summary>
        /// <returns>New opaque color.</returns>
        public static Color Opaque(this Color self) => new Color(self.r, self.g, self.b);

        /// <summary> Inverted color. </summary>
        /// <returns>New inverted color.</returns>
        public static Color Invert(this Color self) => new Color(1.0f - self.r, 1.0f - self.g, 1.0f - self.b, self.a);

        /// <summary> Same color, different alpha. </summary>
        /// <param name="alpha">Alpha</param>
        /// <returns>New color.</returns>
        public static Color WithAlpha(this Color self, float alpha) => new Color(self.r, self.g, self.b, alpha);

        /// <summary> Random color. </summary>
        /// <returns>New color.</returns>
        public static Color Random()
        {
            return new Color
            {
                r = UnityEngine.Random.Range(0.0f, 1.0f),
                g = UnityEngine.Random.Range(0.0f, 1.0f),
                b = UnityEngine.Random.Range(0.0f, 1.0f),
                a = 1.0f
            };
        }

        /// <summary> Random color. </summary>
        /// <returns>New random color.</returns>
        public static Color Random(this Color self)
        {
            self.r = UnityEngine.Random.Range(0.0f, 1.0f);
            self.g = UnityEngine.Random.Range(0.0f, 1.0f);
            self.b = UnityEngine.Random.Range(0.0f, 1.0f);
            self.a = 1.0f;
            return self;
        }
    }
}
