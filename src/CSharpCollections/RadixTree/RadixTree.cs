using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.SharpCollect
{
    /// <summary>
    /// a radix tree is a data structure that represents a space-optimized prefix tree
    /// in which each node that is the only child is merged with its parent
    /// </summary>
    public class RadixTree<T> : IDictionary<byte[], T>
    {
        #region IDictionary
        public ICollection<byte[]> Keys => throw new NotImplementedException();

        public ICollection<T> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

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
