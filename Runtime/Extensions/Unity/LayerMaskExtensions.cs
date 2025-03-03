using UnityEngine;

namespace com.ktgame.foundation.extensions.unity
{
    /// <summary> LayerMask extensions. </summary>
    public static class LayerMaskExtensions
    {
        /// <summary> GameObject in the layer? </summary>
        /// <param name="other">GameObject</param>
        /// <returns>True/false</returns>
        public static bool CheckLayerMask(this LayerMask self, GameObject other) => CheckLayerMask(self, other.layer);

        /// <summary> In the layer? </summary>
        /// <param name="layer">Value</param>
        /// <returns>True/false</returns>
        public static bool CheckLayerMask(this LayerMask self, int layer) => ((1 << layer) & self) != 0;
    }
}