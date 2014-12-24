using System;
using System.Collections;
using System.Collections.Generic;

namespace Axon.Collections
{
    /// <summary>
    /// The ConcurrentPriorityQueue class is a thread-safe generic priority queue using a binary
    /// heap for sorting elements by priority. The underlying binary heap uses a min-heap property,
    /// therefore the element with the highest priority will always be dequeued first.
    /// </summary>
    /// <typeparam name="T">The type of data to be queued.</typeparam>
    public
    class ConcurrentPriorityQueue<T>
    : ICollection< KeyValuePair<float, T> >
    {


        #region Instance members


        /// <summary>
        /// The heap that implements the priority queue in memory.
        /// </summary>
        private ConcurrentBinaryMinHeap< KeyValuePair<float, T> > __heap;


        /// <summary>
        /// Returns the number of elements the queue can hold without auto-resizing.
        /// </summary>
        public int Capacity { get { return __heap.Capacity; } }


        /// <summary>
        /// Return the number of elements in the queue.
        /// </summary>
        public int Count { get {  __heap.Count; } }


        /// <summary>
        /// Returns whether or not the queue is empty.
        /// </summary>
        public bool IsEmpty { get { return __heap.Count == 0; } }


        /// <summary>
        /// Returns whether or not the collection is read-only. For a priority queue this property
        /// returns <c>false</c>.
        /// </summary>
        public bool IsReadOnly { get { return false; } }


        #endregion





        #region Constructors


        /// <summary>
        /// Create a new default priority queue.
        /// </summary>
        public
        ConcurrentPriorityQueue()
        {
            try
            {
                __heap = new ConcurrentBinaryMinHeap< KeyValuePair<float, T> >();
            }
            catch ( Exception e )
            {
                throw e;
            }
        }


        /// <summary>
        /// Create a new priority queue with the given initial size of the array implementing it
        /// internally.
        /// </summary>
        /// <param name="initialCapacity">The initial size of the heap underlying priority queue.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by the List constructor when the given initialCapacity is less than 0.
        /// </exception>
        public
        ConcurrentPriorityQueue( int initialCapacity )
        {
            try
            {
                __heap = new ConcurrentBinaryMinHeap< KeyValuePair<float, T> >( initialCapacity );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }


        #endregion





        #region Public methods


        /// <summary>
        /// Enqueues a KeyValuePair as a new element into the queue.
        /// </summary>
        /// <param name="item">KeyValuePair to add as a new element.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the given element is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by HeapifyBottomUp() when the given index is out of range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by SwapElements() when the inputs to SwapElements() are invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by SwapElements() when there are less than 2 elements in the heap.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by HeapifyBottomUp() when the heap is empty.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown by List.Add() when the heap is in read-only mode.
        /// </exception>
        public
        void
        Add( KeyValuePair<float, T> element )
        {
            try
            {
                __heap.Add( element );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }


        /// <summary>
        /// Clears the queue of all elements.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Thrown when the heap is in read-only mode.
        /// </exception>
        public
        void
        Clear()
        {
            // No exceptions can be thrown by HeapClear().
            try
            {
                __heap.Clear();
            }
            catch ( Exception e )
            {
                throw e;
            }
        }


        /// <summary>
        /// Returns whether or not the queue contains the given KeyValuePair element.
        /// </summary>
        /// <param name="item">The KeyValuePair to locate in the queue.</param>
        /// <returns><c>true</c> if item is found in the queue; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the given element is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown when the heap is in read-only mode.
        /// </exception>
        public
        bool
        Contains( KeyValuePair<float, T> element )
        {
            bool result = false;
            try
            {
                result = __heap.Contains( element );
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                return result;
            }
        }


        /// <summary>
        /// Copies the elements of the priority queue to an Array, starting at a particular index.
        /// This method does not guarantee that elements will be copied in the sorted order.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements
        /// copied from the priority queue. The Array must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the given array is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the given index is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the given array is multi-dimensional.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if there is not enough space in the array from arrayIndex to the end to hold the
        /// heap data to be copied.
        /// </exception>
        public
        void
        CopyTo( KeyValuePair<float, T>[] array, int arrayIndex )
        {
            try
            {
                __heap.CopyTo( array, arrayIndex );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }


        /// <summary>
        /// Returns an enumerator that iterates through the queue's elements. This enumerator is
        /// not guaranteed to iterate through elements in sorted order.
        /// </summary>
        /// <returns>An generic enumerator of the queue's contents.</returns>
        public
        IEnumerator< KeyValuePair<float, T> >
        GetEnumerator()
        {
            return __heap.GetEnumerator();
        }


        /// <summary>
        /// Returns an enumerator that iterates through the queue's elements. This enumerator is
        /// not guaranteed to iterate through elements in sorted order.
        /// </summary>
        /// <returns>An enumerator of the queue's contents.</returns>
        IEnumerator
        IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        /// <summary>
        /// Enqueues an existing KeyValuePair element into the priority queue.
        /// </summary>
        /// <param name="element">A KeyValuePair element.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the given element is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by HeapifyBottomUp() when the given index is out of range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by SwapElements() when the inputs to SwapElements() are invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by SwapElements() when there are less than 2 elements in the heap.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by HeapifyBottomUp() when the heap is empty.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown by Add() when the heap is in read-only mode.
        /// </exception>
        public
        void
        Enqueue( KeyValuePair<float, T> element )
        {
            try
            {
                __heap.Push( element );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }


        /// <summary>
        /// Enqueues an element into the priority queue.
        /// </summary>
        /// <param name="priority">A float priority.</param>
        /// <param name="value">A generically-typed object.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by HeapifyBottomUp() when the given index is out of range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by SwapElements() when the inputs to SwapElements() are invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by SwapElements() when there are less than 2 elements in the heap.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by HeapifyBottomUp() when the heap is empty.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown by Add() when the heap is in read-only mode.
        /// </exception>
        public
        void
        Enqueue( float priority, T value )
        {
            try
            {
                __heap.Push( priority, value );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }


        /// <summary>
        /// Dequeues the element with the highest priority and returns it as a
        /// <see cref="KeyValuePair{ float, T }"/>. Throws a
        /// <see cref="InvalidOperationException"/> if the queue is empty.
        /// </summary>
        /// <returns>A KeyValuePair where the key is set to the priority of the dequeued element
        /// and the value is set to the value of the dequeued element.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the given element is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by HeapifyTopDown() when the given index is out of range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by SwapElements() when the inputs to SwapElements() are invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by List.RemoveAt() when the inputs to List.RemoveAt() are invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by SwapElements() when there are less than 2 elements in the heap.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap is empty.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown by List.Clear() when the heap is in read-only mode.
        /// </exception>
        public
        KeyValuePair<float, T>
        Dequeue()
        {
            KeyValuePair<float, T> result;
            try
            {
                result = __heap.Pop();
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                return result;
            }
        }


        /// <summary>
        /// Dequeues the element with the highest priority and returns its data/value. Throws
        /// an <see cref="InvalidOperationException"/> if the queue is empty.
        /// </summary>
        /// <returns>The value of the dequeued element.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the given element is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by HeapifyTopDown() when the given index is out of range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by SwapElements() when the inputs to SwapElements() are invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by List.RemoveAt() when the inputs to List.RemoveAt() are invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by SwapElements() when there are less than 2 elements in the heap.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap is empty.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Thrown by List.Clear() when the heap is in read-only mode.
        /// </exception>
        public
        T
        DequeueValue()
        {
            T result;
            try
            {
                result = __heap.Pop().Value;
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                return result;
            }
        }


        /// <summary>
        /// Returns the element with the highest priority as a
        /// <see cref="KeyValuePair{ float, T }"/>, but doesn't remove it from the queue.
        /// Throws a <see cref="InvalidOperationException"/> if the queue is empty.
        /// </summary>
        /// <returns>A KeyValuePair where the key is set to the priority of the dequeued element
        /// and the value is set to the data/value of the dequeued element.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap is empty.
        /// </exception>
        public
        KeyValuePair<float, T>
        Peek()
        {
            KeyValuePair<float, T> result;
            try
            {
                result = __heap.Peek();
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                return result;
            }
        }


        /// <summary>
        /// Returns the value of the element with the highest priority as a
        /// <see cref="KeyValuePair{ float, T }"/>, but doesn't remove the element from
        /// the queue. Throws a <see cref="InvalidOperationException"/> if the queue is empty.
        /// </summary>
        /// <returns>A KeyValuePair where the key is set to the priority of the dequeued element
        /// and the value is set to the data/value of the dequeued element.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap is empty.
        /// </exception>
        public
        T
        PeekValue()
        {
            T result;
            try
            {
                result = __heap.Peek().Value;
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                return result;
            }
        }


        /// <summary>
        /// Removes the first occurrence of the given KeyValuePair element within the queue.
        /// </summary>
        /// <param name="item">The KeyValuePair element to remove from the queue.</param>
        /// <returns><c>true</c> if item was successfully removed from the priority queue.
        /// This method returns false if item is not found in the collection.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the given element is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by HeapifyTopDown() when the inputs to HeapifyTopDown() are out of range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by SwapElements() when the inputs to SwapElements() are invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by List.RemoveAt() when the inputs to List.RemoveAt() are invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by SwapElements() when there are less than 2 elements in the heap.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap is empty.
        /// </exception>
        /// <remarks>
        /// See Inspiration.cs Remove() for the original algorithm.
        /// </remarks>
        public
        bool
        Remove( KeyValuePair<float, T> element )
        {
            bool result = false;
            try
            {
                result = __heap.Remove( element );
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                return result;
            }
        }


        #endregion


    }
}
