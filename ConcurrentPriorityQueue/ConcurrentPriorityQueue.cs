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
    : ICollection< PriorityValuePair<T> >
    {
        #region Instance members

        /// <summary>
        /// The heap that implements the priority queue in memory.
        /// </summary>
        private ConcurrentBinaryMinHeap<T> __heap;


        /// <summary>
        /// Returns the number of elements the queue can hold without auto-resizing.
        /// </summary>
        public int Capacity { get { return __heap.Capacity; } }


        /// <summary>
        /// Return the number of elements in the queue.
        /// </summary>
        public int Count { get { return __heap.Count; } }


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
            __heap = new ConcurrentBinaryMinHeap<T>();
        }


        /// <summary>
        /// Create a new priority queue with the given initial size of the heap implementing it
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
                __heap = new ConcurrentBinaryMinHeap<T>( initialCapacity );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Enqueues a PriorityValuePair as a new element into the queue.
        /// </summary>
        /// <param name="item">PriorityValuePair to add as a new element.</param>
        public
        void
        Add( PriorityValuePair<T> element )
        {
            __heap.Add( element );
        }


        /// <summary>
        /// Clears the queue of all elements.
        /// </summary>
        public
        void
        Clear()
        {
            __heap.Clear();
        }


        /// <summary>
        /// Returns whether or not the queue contains the given PriorityValuePair element.
        /// </summary>
        /// <param name="item">The PriorityValuePair to locate in the queue.</param>
        /// <returns><c>true</c> if item is found in the queue; otherwise, <c>false</c>.</returns>
        public
        bool
        Contains( PriorityValuePair<T> element )
        {
            return __heap.Contains( element );
        }


        /// <summary>
        /// Copies the elements of the priority queue to an Array, starting at a particular index.
        /// This method does not guarantee that elements will be copied in the sorted order.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements
        /// copied from the priority queue. The Array must have zero-based indexing. </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public
        void
        CopyTo( PriorityValuePair<T>[] array, int arrayIndex )
        {
            __heap.CopyTo( array, arrayIndex );
        }


        /// <summary>
        /// Enqueues an existing PriorityValuePair element into the priority queue.
        /// </summary>
        /// <param name="element">A PriorityValuePair element.</param>
        public
        void
        Enqueue( PriorityValuePair<T> element )
        {
            __heap.Push( element );
        }


        /// <summary>
        /// Enqueues an element into the priority queue.
        /// </summary>
        /// <param name="priority">A float priority.</param>
        /// <param name="value">A generically-typed object.</param>
        public
        void
        Enqueue( float priority, T value )
        {
            __heap.Push( priority, value );
        }


        /// <summary>
        /// Dequeues the element with the highest priority and returns it.
        /// </summary>
        /// <returns>A PriorityValuePair where the key is set to the priority of the dequeued element
        /// and the value is set to the value of the dequeued element.</returns>
        public
        PriorityValuePair<T>
        Dequeue()
        {
            return __heap.Pop();
        }


        /// <summary>
        /// Dequeues the element with the highest priority and returns its priority.
        /// </summary>
        /// <returns>The priority of the dequeued element.</returns>
        public
        float
        DequeuePriority()
        {
            return __heap.PopPriority();
        }


        /// <summary>
        /// Dequeues the element with the highest priority and returns its value.
        /// </summary>
        /// <returns>The value of the dequeued element.</returns>
        public
        T
        DequeueValue()
        {
            return __heap.PopValue();
        }


        /// <summary>
        /// Returns an enumerator that iterates through the queue's elements. This enumerator is
        /// not guaranteed to iterate through elements in sorted order.
        /// </summary>
        /// <returns>An generic enumerator of the queue's contents.</returns>
        public
        IEnumerator< PriorityValuePair<T> >
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
        /// Returns the element with the highest priority, but doesn't remove it from the queue.
        /// </summary>
        /// <returns>A PriorityValuePair where the key is set to the priority of the dequeued element
        /// and the value is set to the data/value of the dequeued element.</returns>
        public
        PriorityValuePair<T>
        Peek()
        {
            return __heap.Peek();
        }


        /// <summary>
        /// Returns the priority of the element with the highest priority, but doesn't remove the 
		/// element from the queue.
        /// </summary>
        /// <returns>The priority of the PriorityValuePair at the front of the queue.</returns>
        public
        float
        PeekPriority()
        {
            return __heap.PeekPriority();
        }


        /// <summary>
        /// Returns the value of the element with the highest priority, but doesn't remove the 
		/// element from the queue.
        /// </summary>
        /// <returns>The value of the PriorityValuePair at the front of the queue.</returns>
        public
        T
        PeekValue()
        {
            return __heap.PeekValue();
        }


        /// <summary>
        /// Removes the first occurrence of the given PriorityValuePair element within the queue.
        /// </summary>
        /// <param name="item">The PriorityValuePair element to remove from the queue.</param>
        /// <returns><c>true</c> if item was successfully removed from the priority queue.
        /// This method returns false if item is not found in the collection.</returns>
        public
        bool
        Remove( PriorityValuePair<T> element )
        {
            return __heap.Remove( element );
        }


        #endregion
    }
}
