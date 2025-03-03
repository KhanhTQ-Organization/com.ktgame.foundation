using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace com.ktgame.foundation.math
{
    /// <summary> Random functions. </summary>
    public static class MathUtilsRandom
    {
        // Int

        /// <summary> A random int within [min..max] (range is inclusive). </summary>
        /// <param name="min">Minimo</param>
        /// <param name="max">Maximo</param>
        /// <returns>Number</returns>
        public static int Range(int min, int max) => UnityRandom.Range(min, max);

        // 1D

        /// <summary>Random float within [0 .. 1]</summary>
        public static float Value => UnityRandom.value;

        /// <summary> Positive or negative at 50% </summary>
        public static float Sign => Value > 0.5f ? 1.0f : -1.0f;

        /// <summary> One dimension direction at 50% </summary>
        public static float Direction1D => Sign;

        /// <summary> A random float within [min..max] (range is inclusive). </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Range(float min, float max) => UnityRandom.Range(min, max);

        // 2D

        /// <summary> Random unit Vector2 </summary>
        public static Vector2 OnUnitCircle => MathUtilsAngle.AngToDir(Value * MathConstants.Tau);

        /// <summary> Random unit 2D direction </summary>
        public static Vector2 Direction2D => OnUnitCircle;

        /// <summary> A random point inside or on a circle with radius 1 </summary>
        public static Vector2 InUnitCircle => UnityRandom.insideUnitCircle;

        /// <summary> A random point inside or on a square with sides 1 </summary>
        public static Vector2 InUnitSquare => new Vector2(Value, Value);

        // 3D

        /// <summary> A random point on the surface of a sphere with radius 1 </summary>
        public static Vector3 OnUnitSphere => UnityRandom.onUnitSphere;

        /// <summary> Random unit 3D direction </summary>
        public static Vector3 Direction3D => OnUnitSphere;

        /// <summary> A random point inside or on a sphere with radius 1 </summary>
        public static Vector3 InUnitSphere => UnityRandom.insideUnitSphere;

        /// <summary> A random point inside or on a cube with sides 1 </summary>
        public static Vector3 InUnitCube => new Vector3(Value, Value, Value);

        // 2D orientation

        /// <summary>Returns a random angle in radians from 0 to TAU</summary>
        public static float Angle => Value * MathConstants.Tau;

        // 3D Orientation

        /// <summary>Returns a random uniformly distributed rotation</summary>
        public static Quaternion Rotation => UnityRandom.rotationUniform;
        
        public static int RandomWeighted<T>(IList<T> weighList) where T : IWeighRandom
        {
            var sumChance = 0f;
            foreach (var random in weighList)
            {
                sumChance += random.Ratio;
            }

            var rand = UnityRandom.Range(0, sumChance);
            var minRange = 0f;
            var maxRange = 0f;
            var result = 0;

            for (var index = 0; index < weighList.Count; ++index)
            {
                if (weighList[index].Ratio <= 0f)
                {
                    continue;
                }

                minRange = maxRange;
                maxRange = minRange + weighList[index].Ratio;

                if (rand >= minRange && rand <= maxRange)
                {
                    result = index;
                    break;
                }
            }

            return result;
        }
    }
}
