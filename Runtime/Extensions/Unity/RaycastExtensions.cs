using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.ktgame.foundation.extensions.unity
{
	public class RaycastExtensions
	{
        private static readonly List<RaycastResult> Results = new(16);
        private static PointerEventData _pointerEventData;

        #region Public API

        /// <summary>
        /// Check pointer (mouse hoặc touch) có bị UI block không
        /// </summary>
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current == null)
                return false;

#if UNITY_EDITOR || UNITY_STANDALONE
            return IsPointerOverUI(Input.mousePosition);
#else
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (IsPointerOverUI(Input.GetTouch(i).position))
                        return true;
                }
            }
            return false;
#endif
        }

        /// <summary>
        /// Check tại vị trí screen cụ thể
        /// </summary>
        public static bool IsPointerOverUI(Vector2 screenPos)
        {
            if (EventSystem.current == null)
                return false;

            EnsurePointerData();

            _pointerEventData.position = screenPos;

            Results.Clear();
            EventSystem.current.RaycastAll(_pointerEventData, Results);

            return Results.Count > 0;
        }

        /// <summary>
        /// Lấy UI GameObject top (nếu có)
        /// </summary>
        public static GameObject GetTopUI(Vector2 screenPos)
        {
            if (EventSystem.current == null)
                return null;

            EnsurePointerData();

            _pointerEventData.position = screenPos;

            Results.Clear();
            EventSystem.current.RaycastAll(_pointerEventData, Results);

            return Results.Count > 0 ? Results[0].gameObject : null;
        }

        /// <summary>
        /// Check có hit UI cụ thể theo layer (optional)
        /// </summary>
        public static bool IsPointerOverUILayer(Vector2 screenPos, LayerMask mask)
        {
            if (EventSystem.current == null)
                return false;

            EnsurePointerData();

            _pointerEventData.position = screenPos;

            Results.Clear();
            EventSystem.current.RaycastAll(_pointerEventData, Results);

            for (int i = 0; i < Results.Count; i++)
            {
                var go = Results[i].gameObject;
                if (((1 << go.layer) & mask) != 0)
                    return true;
            }

            return false;
        }

        #endregion

        #region Legacy Wrapper (giữ API cũ)

        public static bool IsTouchBlockedByUI()
        {
            return IsPointerOverUI();
        }

        public static bool BlockedByUI()
        {
            return IsPointerOverUI();
        }

        #endregion

        #region Internal

        private static void EnsurePointerData()
        {
            if (_pointerEventData == null)
            {
                _pointerEventData = new PointerEventData(EventSystem.current);
            }
        }

        #endregion
    }
}
