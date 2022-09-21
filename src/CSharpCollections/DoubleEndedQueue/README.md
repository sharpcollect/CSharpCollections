# Double Ended Queue

The [`IDoubleEndedQueue`](IDoubleEndedQueue.cs) interface defines the methods associated with a DoubleEndedQueue. It has the following methods:
- __void Enqueue(item)__ adds an item to the tail/right/back of the queue
- __item Dequeue()__ removes and returns an item from the head/left/front of the queue
- __bool TryDequeue(out item)__ attempts to remove an item from the head/left/front of the queue and returns true if successful
- __void EnqueueFront(item)__ adds an item to the head/left/front of the queue
- __item DequeueBack()__ removes and returns an item from the tail/right/back of the queue
- __bool TryDequeueBack(out item)__ attempts to remove an item from the tail/right/back of the queue and returns true if successful

It has the following properties:
- __int Capacity__ the size of the underlying storage/memory allocation (for certain collections this can dynamically grow and shrink)
- __int Count__ the number of items in the queue (from `IReadOnlyCollection<T>`)

It also extends `IReadOnlyCollection<T>, IList<T>` such that it can be used with `System.Linq` as if it were a part of `System.Collections.Generic`.

The [`DoubleEndedQueue`](DoubleEndedQueue.cs) is a queue in which items can be added to either end. The [Test Cases](../../../tests/CSharpCollections.Tests/DoubleEndedQueue_Tests.cs) contain several examples working with [`DoubleEndedQueue`](DoubleEndedQueue.cs). This queue will dynamically expand as more items are added. It will also attempt to shuffle its data to make slightly better use of the underlying allocation.

A possible use case for this class may be to traverse a tree without having to use recursion ([breadth first search, shortest path](../../../tests/CSharpCollections.Tests/DoubleEndedQueue_Examples.cs)). 
```
var queue = new DoubleEndedQueue<(DN, int)>() { (head, 0) };
var seen = new HashSet<DN>();
while (queue.TryDequeue(out (DN node, int depth) current))
{
    if (exitCondition(current.node)) { return current.depth; }
    foreach (DN next in current.node.GetNext()
        .Where(obj => !seen.Contains(obj)).OfType<DN>())
    {
        seen.Add(next);
        queue.Enqueue((next, current.depth + 1));
    }
}
return -1;
```
For big data problems, recursion can lead to stack overflow where using a queue is limited only by the allocatable memory.

The [`CircularDoubleEndedQueue`](CircularDoubleEndedQueue.cs) is just like a [`DoubleEndedQueue`](DoubleEndedQueue.cs), but its capacity will never grow or change in size. The [Test Cases](../../../tests/CSharpCollections.Tests/CircularDoubleEndedQueue_Tests.cs) contain several examples of working with [`CircularDoubleEndedQueue`](CircularDoubleEndedQueue.cs).

A possible use case for this might be an undo-redo stack or a video player that allows for quick rewind of N number of frames. This structure is write optimized - so it can be used like a circular buffer.

### Note: 

Neither class is thread-safe.
