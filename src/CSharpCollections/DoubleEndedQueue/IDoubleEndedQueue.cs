using System.Collections.Generic;

namespace System.Collections.SharpCollect
{
    /// <summary>
    /// A Double Ended Queue allows for Enqueue-ing and Dequeue-ing from the front and back
    /// </summary>
    /// <typeparam name="T">The type of object stored in the queue</typeparam>
    public interface IDoubleEndedQueue<T> : IReadOnlyCollection<T>, IList<T>
    {
        /// <summary>
        /// The number of items in the queue
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Adds obj to the back/tail/right of the queue
        /// </summary>
        /// <param name="obj">The object to add</param>
        void Enqueue(T obj);

        /// <summary>
        /// Adds obj to the front/head/left of the queue
        /// </summary>
        /// <param name="obj">The object to add</param>
        void EnqueueFront(T obj);

        /// <summary>
        /// Removes the object at the front/head/left of the queue and returns it.
        /// If the queue is empty, this method simply returns false.
        /// </summary>
        /// <param name="obj">The returned object</param>
        /// <returns>An object or default</returns>
        bool TryDequeue(out T obj);

        /// <summary>
        /// Removes the object at the front/head/left of the queue and returns it. 
        /// If the queue is empty, this method simply returns null.
        /// </summary>
        /// <returns>An object or default</returns>
        T Dequeue();

        /// <summary>
        /// Removes the object at the back/tail/right of the queue and returns it. 
        /// If the queue is empty, this method simply returns false.
        /// </summary>
        /// <param name="obj">The returned object</param>
        /// <returns>True if an object returned</returns>
        bool TryDequeueBack(out T obj);

        /// <summary>
        /// Removes the object at the tail of the queue and returns it. If the queue
        /// is empty, this method simply returns null.
        /// </summary>
        /// <returns>An object or default</returns>
        T DequeueBack();

        /// <summary>
        /// Gets the front item
        /// </summary>
        /// <returns>The item or default</returns>
        T Peek();

        /// <summary>
        /// Gets the back item
        /// </summary>
        /// <returns>The item or default</returns>
        T PeekBack();

        /// <summary>
        /// ToArray returns a new Object array containing the contents of the List. 
        /// </summary>
        /// <returns>A new object array</returns>
        T[] ToArray();
    }
}
