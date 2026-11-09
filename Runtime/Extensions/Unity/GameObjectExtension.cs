using System.Collections.Generic;
using UnityEngine;

namespace com.ktgame.foundation.extensions.unity
{
	/// <summary> GameObject extensions. </summary>
	public static class GameObjectExtension
	{
		public static T GetOrAddComponent<T>(this GameObject self)
			where T : Component
		{
			if (self.TryGetComponent<T>(out var component) == false)
			{
				component = self.AddComponent<T>();
			}

			return component;
		}

		public static T GetOrAddComponent<T>(this Component self)
			where T : Component
		{
			if (self.TryGetComponent<T>(out var component) == false)
			{
				component = self.gameObject.AddComponent<T>();
			}

			return component;
		}

		public static bool TryGetOrAddComponent<T>(this GameObject self, out T component)
			where T : Component
		{
			if (self.TryGetComponent<T>(out component) == false)
			{
				component = self.gameObject.AddComponent<T>();
			}

			return component;
		}

		public static bool TryGetOrAddComponent<T>(this Component self, out T component)
			where T : Component
		{
			if (self.TryGetComponent<T>(out component) == false)
			{
				component = self.gameObject.AddComponent<T>();
			}

			return component;
		}

		/// <summary> Returns a component of the parent of a GameObject, ignoring itself. </summary>
		/// <returns>Component</returns>
		public static T GetComponentInParentIgnoreSelf<T>(this GameObject self) =>
			self.transform.parent != null ? self.transform.parent.GetComponentInParent<T>() : default;

		/// <summary> Return a list of all child objects. </summary>
		/// <returns></returns>
		public static List<GameObject> GetAllChildren(this GameObject self)
		{
			Transform[] childTransforms = self.GetComponentsInChildren<Transform>();
			List<GameObject> allChildren = new List<GameObject>(childTransforms.Length);

			foreach (Transform child in childTransforms)
			{
				if (child.gameObject != self)
					allChildren.Add(child.gameObject);
			}

			return allChildren;
		}

		/// <summary> Return a list of all child objects including itself. </summary>
		/// <returns>List.</returns>
		public static List<GameObject> GetAllChildrenAndSelf(this GameObject self)
		{
			Transform[] childTransforms = self.GetComponentsInChildren<Transform>();
			List<GameObject> allChildren = new List<GameObject>(childTransforms.Length);

			for (int transformIndex = 0; transformIndex < childTransforms.Length; ++transformIndex)
				allChildren.Add(childTransforms[transformIndex].gameObject);

			return allChildren;
		}

		/// <summary> Changes all the materials of a GameObject. </summary>
		/// <param name="go">GameObject</param>
		/// <param name="newMaterial">Material</param>
		public static void ChangeMaterial(this GameObject go, Material newMaterial)
		{
			Renderer[] children = go.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < children.Length; ++i)
			{
				Renderer rend = children[i];

				// Dùng sharedMaterials: đọc renderer.materials sẽ nhân bản toàn bộ material rồi bỏ đi (rò rỉ).
				int count = rend.sharedMaterials.Length;
				Material[] mats = new Material[count];
				for (int j = 0; j < count; j++)
					mats[j] = newMaterial;

				rend.sharedMaterials = mats;
			}
		}

		/// <summary> Destroy all children. </summary>
		public static void DestroyAllChildren(this Transform self)
		{
			for (var i = self.childCount - 1; i > -1; i--)
				SafeDestroy(self.GetChild(i).gameObject);
		}

		/// <summary> Destroy Object. </summary>
		public static void SafeDestroy<T>(this T self) where T : Object
		{
			// Chỉ DestroyImmediate khi không chạy game (Edit Mode). Trước đây dùng Application.isEditor,
			// nên trong Play Mode của editor hủy ngay còn trên máy thật hủy cuối frame: hành vi lệch nhau.
			if (Application.isPlaying)
				Object.Destroy(self);
			else
				Object.DestroyImmediate(self);
		}

		/// <summary> Destroy GameObject. </summary>
		public static void SafeDestroy(this GameObject self)
		{
			if (Application.isPlaying)
				Object.Destroy(self);
			else
				Object.DestroyImmediate(self);
		}

		/// <summary> Destroy component. </summary>
		public static void SafeDestroyComponent<T>(this T self) where T : Component
		{
			if (self != null)
				SafeDestroy(self.gameObject);
		}

		/// <summary> Returns the first active GameObject with a name, or null. </summary>
		/// <param name="name">Name</param>
		/// <returns>GameObject or null</returns>
		public static GameObject FindChildrenByName(string name)
		{
			// Replaced catastrophically slow Resources.FindObjectsOfTypeAll with standard Find.
			// Note: This only finds active game objects. If you need to find an inactive child, 
			// use the Transform.FindChildRecursive extension on the parent instead.
			return GameObject.Find(name);
		}
		
		/// <summary>
        /// Set Parent for GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="parent">parent</param>
        public static void SetParent(this GameObject gameObject, Transform parent)
        {
            gameObject.SetParent(parent, Vector3.zero, Vector3.one);
        }

        /// <summary>
        /// Set Parent for GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="parent">parent</param>
        /// <param name="position">local position</param>
        public static void SetParent(this GameObject gameObject, Transform parent, Vector3 position)
        {
            gameObject.SetParent(parent, position, Vector3.one);
        }

        /// <summary>
        /// Set Parent for GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="parent">parent</param>
        /// <param name="position">local position</param>
        /// <param name="scale">local scale</param>
        public static void SetParent(this GameObject gameObject, Transform parent, Vector3 position, Vector3 scale)
        {
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = scale;
        }

        /// <summary>
        /// Try to Get Component
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>Component</returns>
        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }
	}
}
