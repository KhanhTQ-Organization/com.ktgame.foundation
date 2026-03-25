using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace com.ktgame.extensions.floats
{
	public static class FloatExtensions
	{
#region ===== Constants =====
		public const float EPSILON = 0.0001f;
#endregion

#region ===== Core Math =====
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float AbsFast(this float v) => v >= 0f ? v : -v;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ClampFast(this float v, float min, float max) => v < min ? min : (v > max ? max : v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp01Fast(this float v) => v < 0f ? 0f : (v > 1f ? 1f : v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Snap(this float v, float step) => step > 0f ? Mathf.Round(v / step) * step : v;
#endregion

#region ===== Compare (IMPORTANT) =====
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Approximately(this float a, float b, float epsilon = EPSILON) => Mathf.Abs(a - b) <= epsilon;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsZero(this float v, float epsilon = EPSILON) => Mathf.Abs(v) <= epsilon;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsOne(this float v, float epsilon = EPSILON) => Mathf.Abs(v - 1f) <= epsilon;
#endregion

#region ===== Lerp / Interpolation =====
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpTo(this float from, float to, float t) => from + (to - from) * t;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpClamped(this float from, float to, float t) => from + (to - from) * t.Clamp01Fast();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float InverseLerpFast(this float a, float b, float value)
		{
			if (Mathf.Abs(b - a) < EPSILON)
				return 0f;

			return (value - a) / (b - a);
		}
#endregion

#region ===== Smooth / Damping =====
		/// <summary> Smooth damp without ref velocity (lightweight version) </summary>
		public static float SmoothTo(this float current, float target, float speed, float deltaTime) =>
			Mathf.Lerp(current, target, 1f - Mathf.Exp(-speed * deltaTime));

		/// <summary> Critically damped smoothing (better feel for UI/gameplay) </summary>
		public static float Damp(this float current, float target, float lambda, float deltaTime) =>
			Mathf.Lerp(current, target, 1f - Mathf.Exp(-lambda * deltaTime));
#endregion

#region ===== Loop / Wrap =====
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Wrap(this float v, float min, float max)
		{
			float range = max - min;
			if (range <= 0f)
				return min;

			return v - Mathf.Floor((v - min) / range) * range;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PingPong(this float v, float length) => Mathf.PingPong(v, length);
#endregion

#region ===== Angle =====
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float NormalizeAngle(this float angle)
		{
			angle %= 360f;
			if (angle > 180f)
				angle -= 360f;
			if (angle < -180f)
				angle += 360f;
			return angle;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DeltaAngleFast(this float current, float target)
		{
			float delta = (target - current) % 360f;
			if (delta > 180f)
			{
				delta -= 360f;
			}

			if (delta < -180f)
			{
				delta += 360f;
			}

			return delta;
		}
#endregion

#region ===== Remap =====
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Remap(this float v, float a1, float a2, float b1, float b2)
		{
			if (Mathf.Abs(a2 - a1) < EPSILON)
			{
				return b1;
			}

			return (v - a1) / (a2 - a1) * (b2 - b1) + b1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RemapClamped(this float v, float a1, float a2, float b1, float b2)
		{
			float t = ((v - a1) / (a2 - a1)).Clamp01Fast();
			return b1 + (b2 - b1) * t;
		}
#endregion

#region ===== Utility =====
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FloorToIntFast(this float v) => (int)Math.Floor(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CeilToIntFast(this float v) => (int)Math.Ceiling(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RoundToIntFast(this float v) => (int)Math.Round(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Frac(this float v) => v - Mathf.Floor(v);
#endregion

#region ===== Formatting =====
		public static string ToFixed(this float v, int digits = 2) => v.ToString($"F{digits}");

		public static string ToPercent(this float v, int digits = 0) => (v * 100f).ToString($"F{digits}") + "%";
#endregion
	}
}
