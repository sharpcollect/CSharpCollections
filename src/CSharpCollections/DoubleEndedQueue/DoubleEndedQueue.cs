using System.Collections.Generic;

namespace System.Collections.SharpCollect
{
    /// <summary>
    /// A Double Ended Queue allows for pushing and popping from the front 
    /// </summary>
    /// <typeparam name="T">The type of object stored in the queue</typeparam>
    public class DoubleEndedQueue<T> : IReadOnlyCollection<T>, IList<T>, ICollection
    {
        // The items begin stored
        private T[] _items;
        // The position of the next item to be popped
        private int _indexFront;
        // The position of the next item to be pushed
        private int _indexBack;
        // The original size of the items
        private readonly int _initialCapacity;
        // Locking for multi-threaded operations
        private object _syncRoot;

        /// <summary>
        /// Initializes a Double Ended Queue with a capacity
        /// </summary>
        /// <param name="capacity">The number of items before the queue attempts to grow</param>
        /// <exception cref="ArgumentException">Thrown if the specified capacity is invalid</exception>
        public DoubleEndedQueue(int capacity) 
        {
            if (capacity < 3) { throw new ArgumentOutOfRangeException(nameof(capacity),"Initial capacity must be greater than 2", nameof(capacity)); }
            _initialCapacity = capacity;
            Clear();
        }

        /// <summary>
        /// Parameter-less constructor
        /// </summary>
        /// <remarks>
        /// The default size is 32
        /// </remarks>
        public DoubleEndedQueue() : this(32) { }

        /// <summary>
        /// The number of items in the queue
        /// </summary>
        public int Capacity => _items.Length;

        /// <summary>
        /// The number of items in the queue
        /// </summary>
        public int Count => _indexBack - _indexFront;

        /// <summary>
        /// Is this Collection synchronized (thread-safe)?
        /// </summary>
        public bool IsSynchronized => false;

        /// <summary>
        /// Synchronization root for this object.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                }
                return _syncRoot;
            }
        }

        /// <summary>
        /// Returns true if the list can be written to
        /// </summary>
        public bool IsReadOnly => false;


        /// <summary>
        /// Grows or shifts the underlying array to accommodate more items
        /// </summary>
        private void AdjustCapacity()
        {
            var prevCount = Count;
            if (prevCount < _items.Length / 2)
            {
                // Shift the data to make better use of the space
                Array.Copy(_items, _indexFront, _items, _items.Length / 4, prevCount);
                _indexFront = _items.Length / 4;
                _indexBack = _indexFront + prevCount;
                return;
            }
            // Double the capacity
            var newItems = new T[_items.Length * 2];
            Array.Copy(_items, _indexFront, newItems, _items.Length / 2, prevCount);
            _indexFront = _items.Length / 2;
            _indexBack = _indexFront + prevCount;
            _items = newItems;
        }

        /// <summary>
        /// Adds obj to the back/tail/right of the queue
        /// </summary>
        /// <param name="obj">The object to add</param>
        public void Enqueue(T obj)
        {
            _items[_indexBack++] = obj;
            if (_indexBack >= _items.Length)
            {
                AdjustCapacity();
            }
        }

        /// <summary>
        /// Adds obj to the front/head/left of the queue
        /// </summary>
        /// <param name="obj">The object to add</param>
        public void EnqueueFront(T obj)
        {
            _items[--_indexFront] = obj;
            if (_indexFront <= 0)
            {
                AdjustCapacity();
            }
        }

        /// <summary>
        /// Removes the object at the front/head/left of the queue and returns it.
        /// If the queue is empty, this method simply returns false.
        /// </summary>
        /// <param name="obj">The returned object</param>
        /// <returns>An object or default</returns>
        public bool TryDequeue(out T obj)
        {
            if (_indexFront < _indexBack)
            {
                obj = _items[_indexFront];
                _items[_indexFront++] = default;
                return true;
            }
            obj = default;
            return false;
        }


        /// <summary>
        /// Removes the object at the front/head/left of the queue and returns it. 
        /// If the queue is empty, this method simply returns null.
        /// </summary>
        /// <returns>An object or default</returns>
        public T Dequeue()
        {
            TryDequeue(out T obj);
            return obj;
        }

        /// <summary>
        /// Removes the object at the back/tail/right of the queue and returns it. 
        /// If the queue is empty, this method simply returns false.
        /// </summary>
        /// <param name="obj">The returned object</param>
        /// <returns>True if an object returned</returns>
        public bool TryDequeueBack(out T obj)
        {
            if (_indexBack > _indexFront)
            {
                obj = _items[_indexBack-1];
                _items[--_indexBack] = default;
                return true;
            }
            obj = default;
            return false;

        }

        /// <summary>
        /// Removes the object at the tail of the queue and returns it. If the queue
        /// is empty, this method simply returns null.
        /// </summary>
        /// <returns>An object or default</returns>
        public T DequeueBack()
        {
            TryDequeueBack(out T obj);
            return obj;
        }

        /// <summary>
        /// Gets the front item
        /// </summary>
        /// <returns>The item or default</returns>
        public T Peek()
        {
            if (_indexBack == _indexFront)
            {
                return default;
            }
            return _items[_indexFront];
        }

        /// <summary>
        /// Gets the back item
        /// </summary>
        /// <returns>The item or default</returns>
        public T PeekBack()
        {
            if (_indexBack == _indexFront)
            {
                return default;
            }
            return _items[_indexBack - 1];
        }

        /// <summary>
        /// Gets an object at a given index
        /// </summary>
        /// <param name="index">The index in the queue, from the front</param>
        /// <returns>An object or exception</returns>
        /// <exception cref="IndexOutOfRangeException">IndexOutOfRangeException if the index will not return a result</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Smell", "S112:General exceptions should never be thrown", Justification = "Desired behavior is to extend system code")]
        public T this[int index]
        {
            set
            {
                if (index > Count || index < -1)
                {
                    throw new IndexOutOfRangeException();
                }
                if (index == Count)
                {
                    if (value == null) { return; }
                    Enqueue(value);
                    return;
                }
                if (index == -1)
                {
                    if (value == null) { return; }
                    EnqueueFront(value);
                    return;
                }
                if (value == null)
                {
                    RemoveAt(index);
                    return;
                }
                _items[_indexFront + index] = value;
            }
            get
            {
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException();
                }

                return _items[_indexFront + index];
            }
        }

        /// <summary>
        /// Resets the queue to the original
        /// </summary>
        public void Clear()
        {
            _items = new T[_initialCapacity];
            _indexFront = _initialCapacity / 2;
            _indexBack = _indexFront;
        }

        /// <summary>
        /// Checks if there is an item in the array
        /// </summary>
        /// <param name="item">The item</param>
        /// <remarks>
        /// This executes in O(N) time
        /// </remarks>
        /// <returns>True if found</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The item enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _indexFront; i < _indexBack; i++) yield return _items[i];
        }

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>The object enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// Copies a collection into an Array, starting at a particular
        /// index into the array.
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The index in the array to copy to</param>
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            var remaining = array.Length - index;
            if (remaining <= 0)
            {
                throw new ArgumentOutOfRangeException($"Index {index} exceeds the array's size of {array.Length}");
            }
            var count = Count;

            if (count > 0)
            {
                if (remaining < count)
                {
                    count = remaining;
                }
                if (count > 0)
                {
                    Array.Copy(_items, _indexFront, array, index, count);
                }
            }
        }

        /// <summary>
        /// Copies a collection into an Array, starting at a particular
        /// index into the array.
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="arrayIndex">The index in the array to copy to</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection)this).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// ToArray returns a new Object array containing the contents of the List. 
        /// </summary>
        /// <returns>A new object array</returns>
        public T[] ToArray()
        {
            var count = Count;

            T[] array = new T[count];
            if (count > 0)
            {
                Array.Copy(_items, _indexFront, array, 0, count);
            }
            return array;
        }

        /// <summary>
        /// Shrinks the underlying structure such that there is only on space available
        /// in the front or back of the array.
        /// </summary>
        public void TrimExcess()
        {
            var count = Count;

            if (count > 0)
            {
                T[] array = new T[count + 2];
                Array.Copy(_items, _indexFront, array, 1, count);
                _items = array;
                _indexFront = 1;
                _indexBack = _indexFront + count;
            }
            else
            {
                _items = new T[3];
                _indexFront = 1;
                _indexBack = _indexFront;
            }
        }

        /// <summary>
        /// Gets the index of an item (-1 for not found)
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <remarks>This complete in O(N) time</remarks>
        /// <returns>The first index of the item or -1 if not found</returns>
        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            for (int i = _indexFront; i < _indexBack; i++)
            {
                if (comparer.Equals(_items[i], item))
                {
                    return i - _indexFront;
                }
            }
            return -1;
        }

        /// <summary>
        /// Inserts an element at a location in the queue
        /// </summary>
        /// <param name="index">The index where to insert the element</param>
        /// <param name="item">The item to insert</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is invalid</exception>
        public void Insert(int index, T item)
        {
            if (item == null) { return; }
            if (index < 0 || (index > Count && Count > 0)) 
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (index == 0)
            {
                EnqueueFront(item);
                return;
            }
            if (index == Count)
            {
                Enqueue(item);
                return;
            }
            Array.Copy(_items, _indexFront + index, _items, _indexFront + index + 1, Count - index);
            _indexBack++;
            _items[_indexFront + index] = item;
            if (_indexBack >= _items.Length)
            {
                AdjustCapacity();
            }
        }

        /// <summary>
        /// Removes an item at a given index
        /// </summary>
        /// <param name="index">The index of the item to remove</param>
        public void RemoveAt(int index)
        {
            if (_indexFront == _indexBack || index < 0 || index >= Count)
            {
                // Nothing to do
                return;
            }
            if (index == 0)
            {
                _items[_indexFront++] = default;
                return;
            }
            if (index == Count - 1)
            {
                _items[--_indexBack] = default;
                return;
            }
            Array.Copy(_items, _indexFront+index + 1, _items, _indexFront+index, _indexBack - index);
            _items[--_indexBack] = default;
        }

        /// <summary>
        /// Adds an item to the queue using Enqueue
        /// </summary>
        /// <param name="item">The item to add</param>
        public void Add(T item)
        {
            Enqueue(item);
        }

        /// <summary>
        /// Removes the first instance equaling the item
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if removed, false if not found</returns>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }
    }
}
