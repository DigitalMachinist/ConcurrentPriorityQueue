ConcurrentPriorityQueue
=======================

A .NET 3.5 C# implementation of a thread-safe priority queue, suitable for use with MonoGame/Unity3D.

# Description

A thread-safe generic priority queue implemented in C# using a binary heap for sorting elements by priority.

*This queue does not guarantee FIFO ordering of elements with the same priority value.*

Thread safety is achieved by using System.Monitor to lock critical sections within heap operations whenever appropriate, since more sophisticated options such as SpinLock/SpinWait are not available without .NET Framework 4.0 support.

**Note: This project depends upon my [ConcurrentBinaryMinHeap](https://github.com/sivyr/ConcurrentBinaryMinHeap) project. Be sure to clone that in a sibling folder to this project if you intend to build this project.**

This priority queue implementation is inspired by the work of Alexey Kurakin on [codeproject.org](http://www.codeproject.com/Articles/126751/Priority-queue-in-C-with-the-help-of-heap-data-str), so partial credit for this project goes to him.

## Computational Complexity

As should be the case for an data structure based upon a binary heap, this priority queue has logarithmic time-complexity for enqueue and dequeue operations. Peeking at the front/next element requires only constant time. Less common operations are not guaranteed to be so efficient, but no operation should be worse than O(n) in the worst case.

| Operation | Average   | Worst Case   |
|-----------|-----------|--------------|
| Dequeue   | O(log(n)) | O(log(n))    |
| Enqueue   | O(log(n)) | O(log(n))    |
| Peek      | O(1)      | O(1)         |

## Documentation

Doxygen HTML docs are provided and are relatively complete.

## Tests

A NUnit test project is included with full unit test coverage.

As of the time of writing, I have not yet written any tests to validate concurrency to be 100% sure that this code is thread-safe.

If I missed anything, please let me know or submit a PR.

## Developer

 Written by Jeff Rose (jrose0@gmail.com)

## License

Licensed under the MIT License. See the LICENSE file for more information.
