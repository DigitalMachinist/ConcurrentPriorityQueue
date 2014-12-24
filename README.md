ConcurrentPriorityQueue
=======================

# Description

A thread-safe generic priority queue implemented in C# using a binary heap for sorting elements by priority. However, it does not quarantee FIFO ordering of elements with the same priority value.

A Visual Studio test project is included with full test coverage. If I missed anything, please let me know or submit a PR.

This code is inspired by the work of Alexey Kurakin on [codeproject.org](http://www.codeproject.com/Articles/126751/Priority-queue-in-C-with-the-help-of-heap-data-str).

# Developer

 Written by Jeff Rose of Axon Interactive Inc.

 Website: axoninteractive.ca

 Email: theteam@axoninteractive.ca

# Computational Complexity

This priority queue was written for use in game development and features a binary heap data structure for efficiently sorting elements by priority. I don't claim to provide the best performance metrics on the Internet, but this queue has optimal big O performance characteristics for common operations.

| Operation | Average | Worst Case |
|-----------|---------|------------|
| Enqueue   | O(logn) | O(logn)    |
| Dequeue   | O(logn) | O(logn)    |

# License

Licensed under the MIT License.

See [LICENSE.md](LICENCE.md) for more information.
