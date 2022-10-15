using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.SharpCollect
{
    /// <summary>
    /// a radix tree is a data structure that represents a space-optimized prefix tree
    /// in which each node that is the only child is merged with its parent
    /// </summary>
    public class RadixTree<T> : IDictionary<byte[], T>
    {
        /// <summary>
        /// Storage class for Radix Tee
        /// </summary>
        private sealed class RadixTreeNode
        {
            /// <summary>
            /// The 
            /// </summary>
            public byte[] Key { get; set; } = Array.Empty<byte>();

            /// <summary>
            /// Set to true when the value is set
            /// </summary>
            public bool HasValue { get; set; } = false;

            /// <summary>
            /// The value the node contains
            /// </summary>
            public T Value { get; set; } = default(T);

            /// <summary>
            /// The radix node's children where the key is the next byte in the tree
            /// </summary>
            public SortedDictionary<byte, RadixTreeNode> Children { get; set; } = new SortedDictionary<byte, RadixTreeNode>();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="bytes">The remaining key to add</param>
            /// <param name="value">The value to add</param>
            /// <returns>True if something updated</returns>
            public bool Set(byte[] bytes, T value)
            {
                if (bytes == null || bytes.Length < 1)
                {
                    // Nothing can be done
                    return false;
                }
                if (!this.HasValue && !this.Children.Any())
                {
                    // This node is updated
                    this.Key = bytes;
                    this.Value = value;
                    this.HasValue = true;
                    return true;
                }
                if (Enumerable.SequenceEqual(this.Key, bytes))
                {
                    // The keys match, update the value
                    this.Value = value;
                    return true;
                }
                if (Children.ContainsKey(bytes[0]))
                {
                    
                }
                return false;
            }
        }

        private RadixTreeNode _root = new RadixTreeNode();

        #region IDictionary
        public ICollection<byte[]> Keys => throw new NotImplementedException();

        public ICollection<T> Values => throw new NotImplementedException();

        /// <summary>
        /// Returns the number of items in the tree
        /// </summary>
        public int Count { get; private set; } = 0;

        /// <summary>
        /// True if the collection is read only
        /// </summary>
        public bool IsReadOnly => false;

        public T this[byte[] key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Add(byte[] key, T value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(byte[] key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(byte[] key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(byte[] key, out T value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<byte[], T> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<byte[], T> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<byte[], T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<byte[], T> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<byte[], T>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
