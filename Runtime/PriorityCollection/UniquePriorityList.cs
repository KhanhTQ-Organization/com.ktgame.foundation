using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace com.ktgame.foundation.priorityCollection
{
	/// <summary>
	/// The list that will sort the items in order of priority.
	/// </summary>
	/// <remarks>
	/// This list ensures that each added item is unique.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class UniquePriorityList<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
	{
		private readonly HashSet<T> _unique = new HashSet<T>();
		private readonly LinkedList<Node> _nodes = new LinkedList<Node>();

		/// <summary>
		/// Duyệt theo thứ tự priority, không cấp phát (struct enumerator).
		/// Được phép Add/Remove trong lúc duyệt: phần tử bị gỡ mà chưa duyệt tới sẽ bị bỏ qua,
		/// phần tử thêm vào phía sau vị trí hiện tại sẽ được duyệt. Bản cũ ném InvalidOperationException.
		/// </summary>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(_nodes);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count => _nodes.Count;

		public bool Contains(T item)
		{
			return _unique.Contains(item);
		}

		/// <summary>
		/// Add objects based on priority.
		/// If the priority is the same, the object added earlier will be placed in front.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="priority"></param>
		public void Add(T item, int priority)
		{
			if (_unique.Contains(item))
			{
				return;
			}

			var node = _nodes.First;

			LinkedListNode<Node> beforeNode = null;

			if (node == null)
			{
				_unique.Add(item);
				_nodes.AddFirst(new Node(item, priority));
				return;
			}

			while (true)
			{
				if (node.Value.Priority > priority)
				{
					if (beforeNode == null)
					{
						_unique.Add(item);
						_nodes.AddFirst(new Node(item, priority));
					}
					else
					{
						_unique.Add(item);
						_nodes.AddAfter(beforeNode, new Node(item, priority));
					}

					return;
				}

				beforeNode = node;
				node = node.Next;

				if (node == null)
				{
					_unique.Add(item);
					_nodes.AddLast(new Node(item, priority));
					return;
				}
			}
		}

		/// <summary>
		/// Remove the first appeared one of the specified values.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public bool Remove(T item)
		{
			if (_unique.Contains(item) == false)
			{
				return false;
			}

			var linkedListNode = _nodes.First;

			if (linkedListNode == null)
			{
				return false;
			}

			var equalityComparer = EqualityComparer<T>.Default;

			while (true)
			{
				if (equalityComparer.Equals(linkedListNode.Value.Item, item))
				{
					_unique.Remove(item);
					_nodes.Remove(linkedListNode);
					return true;
				}

				linkedListNode = linkedListNode.Next;

				if (linkedListNode == null)
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Clear all objects.
		/// </summary>
		public void Clear()
		{
			_nodes.Clear();
			_unique.Clear();
		}

		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException(nameof(array));
			}

			if (array.Rank != 1)
			{
				throw new ArgumentException($"{nameof(array)} has invalid rank.");
			}

			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (array.Length - index < Count)
			{
				throw new ArgumentException($"{nameof(array)} does not have enough space.");
			}

			if (array is T[] array1)
			{
				CopyTo(array1, index);
				return;
			}

			var elementType = array.GetType().GetElementType();
			var c = typeof(T);

			if (!elementType.IsAssignableFrom(c) && !c.IsAssignableFrom(elementType))
			{
				throw new ArgumentException($"Type of the {nameof(array)} element is invalid.");
			}

			if (!(array is object[] objArray))
			{
				throw new ArgumentException($"Type of the {nameof(array)} is invalid.");
			}

			var linkedListNode = _nodes.First;
			try
			{
				if (linkedListNode == null)
				{
					return;
				}

				while (true)
				{
					objArray[index++] = linkedListNode.Value.Item;
					linkedListNode = linkedListNode.Next;
					if (linkedListNode == null)
					{
						return;
					}
				}
			}
			catch (ArrayTypeMismatchException)
			{
				throw new ArgumentException($"Type of the {nameof(array)} is invalid.");
			}
		}

		private void CopyTo(T[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException(nameof(array));
			}

			if (index < 0 || index > array.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}

			if (array.Length - index < Count)
			{
				throw new ArgumentException($"{nameof(array)} does not have enough space.");
			}

			var linkedListNode = _nodes.First;
			if (linkedListNode == null)
			{
				return;
			}

			while (true)
			{
				array[index++] = linkedListNode.Value.Item;
				linkedListNode = linkedListNode.Next;
				if (linkedListNode == null)
				{
					return;
				}
			}
		}

		bool ICollection.IsSynchronized => false;

		private object _syncRoot;

		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange<object>(ref _syncRoot, new object(), null);
				}

				return _syncRoot;
			}
		}

		public struct Enumerator : IEnumerator<T>
		{
			private readonly LinkedList<Node> _list;
			private LinkedListNode<Node> _current;
			private LinkedListNode<Node> _next;
			private bool _started;

			internal Enumerator(LinkedList<Node> list)
			{
				_list = list;
				_current = null;
				_next = null;
				_started = false;
			}

			public T Current => _current != null ? _current.Value.Item : default;

			object IEnumerator.Current => Current;

			public bool MoveNext()
			{
				LinkedListNode<Node> candidate;

				if (_started == false)
				{
					_started = true;
					candidate = _list.First;
				}
				else if (_current != null && _current.List == _list)
				{
					// Phần tử hiện tại vẫn còn trong danh sách: đi tiếp theo liên kết của nó.
					candidate = _current.Next;
				}
				else if (_next != null && _next.List == _list)
				{
					// Phần tử hiện tại vừa bị gỡ: dùng phần tử kế tiếp đã ghi nhớ trước đó.
					candidate = _next;
				}
				else
				{
					candidate = null;
				}

				_current = candidate;
				_next = candidate?.Next;
				return candidate != null;
			}

			public void Reset()
			{
				_started = false;
				_current = null;
				_next = null;
			}

			public void Dispose() { }
		}

		internal readonly struct Node
		{
			public readonly T Item;

			public readonly int Priority;

			public Node(T item, int priority)
			{
				Item = item;
				Priority = priority;
			}
		}
	}
}
