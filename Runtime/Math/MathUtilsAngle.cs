using UnityEngine;

namespace com.ktgame.foundation.math
{
    /// <summary> Math utilities. </summary>
    public static partial class MathUtilsAngle
    {
        /// <summary> Angle to direction in the XY plane. </summary>
        /// <param name="radian">Radian angle</param>
        /// <returns>Direction</returns>
        public static Vector2 AngToDir(float radian) => new Vector2(MathUtilsTrigonometry.Cos(radian), MathUtilsTrigonometry.Sin(radian));
    }
}
