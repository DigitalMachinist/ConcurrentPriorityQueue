using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Axon.Collections
{
    /// <summary>
    /// The ConcurrentPriorityQueue class is a thread-safe generic priority queue using a binary
    /// heap for sorting elements by priority. The underlying binary heap uses a min-heap property,
    /// therefore the element with the highest priority will always be dequeued first. If several
	/// items share the same priority, they are dequeued in the same order that they were enqueued
	/// (FIFO).
    /// </summary>
	/// <remarks>
	/// Using priorities of larger than 9999 can jeopardize the FIFO behaviour of queued items that
	/// have the same priority. Conversely, using priorities of 1 or smaller can as well. It is 
	/// advised that you use priorities in the range [1000, 9000] to play it safe, and separate 
	/// your priority levels by at least 100 priority units from eachother if the queue will run
	/// for long durations without emptying or being cleared.
	/// </remarks>
    /// <typeparam name="T">The type of data to be queued.</typeparam>
    public
    class ConcurrentPriorityQueue<T>
    : ICollection< PriorityValuePair<T> >
    {
		/// <summary>
		/// This value defines the step used to increment the PriorityAdjustment for each item 
		/// enqueued. It should be set to approximately one ten-billionth, such that priority 
		/// values of up to 9999.99999999999 are valid. The suggested range for priority values 
		/// is between 1000 and 9999 and that priority values should be separated by at least 100 
		/// from eachother if the priority queue will run for long periods without being cleared or 
		/// becoming empty.
		/// </summary>
		public const double EPSILON = 0.00000000001;


        #region Instance members

        /// <summary>
        /// The heap that implements the priority queue in memory.
		/// Thread-safety of the heap is managed by the heap internally. Use this however you like.
        /// </summary>
        private ConcurrentBinaryMinHeap<T> __heap;


		/// <summary>
		/// The number of items that have been queued since the priority queue was created or it 
		/// was last cleared using the Clear() method. 
		/// </summary>
		/// <remarks>
		/// Don't use this value directly. Access it through the NumQueuedItems property to 
		/// preserve thread-safety.
		/// </remarks>
		private long __numQueuedItems;


		/// <summary>
		/// This object exists only as a necessity to lock __numQueuedItems for thread-safety. 
		/// A reference-type object is required to pass into Monitor.Enter() and Monitor.Exit().
		/// </summary>
		private object __numQueuedItemsLock;


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


		/// <summary>
		/// The number of items that have been queued since the priority queue was created or it 
		/// was last cleared using the Clear() method.
		/// </summary>
		/// <remarks>
		/// This property implements Monitor-based thread synchronization to protect 
		/// __numQueuedItems from being accessed by more than 1 thread at a time. It is the only 
		/// property that does this, since most thread-safety is implemented by the heap.
		/// </remarks>
		public long NumQueuedItems { 
			get { 
				// Lock the heap -- CRITICAL SECTION BEGIN
                Monitor.Enter( __numQueuedItemsLock );
                long result = 0L;
                try
                {
                    // Compute the property value
                    result = __numQueuedItems;
                }
                finally
                {
                    Monitor.Exit( __numQueuedItemsLock );
                    // Unlock the heap -- CRITICAL SECTION END
                }
                return result;
			} 
			private set {
				// Lock the heap -- CRITICAL SECTION BEGIN
                Monitor.Enter( __numQueuedItemsLock );
                try
                {
                    // Compute the property value
                    __numQueuedItems = value;
                }
                finally
                {
                    Monitor.Exit( __numQueuedItemsLock );
                    // Unlock the heap -- CRITICAL SECTION END
                }
			}
		}


		/// <summary>
		/// The adjustment applied to the priority of the most recently queued item. This 
		/// adjustment is applied by subtracting this value from the priorty of the item requested
		/// by the caller of Enqueue() to ensure that items of the same priority are dequeued in 
		/// the same order which they were queued (ensures the queue has the FIFO property).
		/// </summary>
		/// <remarks>
		/// If the queue becomes empty, this adjustment will be reset to 0, since it is 
		/// based upon the number of items queued since start or the last clear operation.
		/// It is ideal to empty the queue regularly to keep this value in a safe range.
		/// </remarks>
		public double PriorityAdjustment { get { return EPSILON * NumQueuedItems; } }

        #endregion


        #region Constructors

        /// <summary>
        /// Create a new default priority queue.
        /// </summary>
        public
        ConcurrentPriorityQueue()
        {
            __heap = new ConcurrentBinaryMinHeap<T>();
			__numQueuedItemsLock = new object();
			NumQueuedItems = 0L;
        }


        /// <summary>
        /// Create a new priority queue with the given initial size of the heap implementing it
        /// internally.
        /// </summary>
        /// <param name="initialCapacity">The initial size of the heap underlying priority queue.
        /// </param>
        public
        ConcurrentPriorityQueue( int initialCapacity )
        {
            __heap = new ConcurrentBinaryMinHeap<T>( initialCapacity );
			__numQueuedItemsLock = new object();
			NumQueuedItems = 0L;
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
			// Apply the priorty adjustment before adding the new element so it orders correctly.
			element.Priority -= PriorityAdjustment;

			// Increment the number of queued items to the priority adjustment updates.
			NumQueuedItems++;

			// Add the new element to the queue.
            __heap.Add( element );
        }


        /// <summary>
        /// Clears the queue of all elements.
        /// </summary>
        public
        void
        Clear()
        {
			// Reset the queued items count to reset the priority adjustment.
			NumQueuedItems = 0L;

			// Clear the queue of all existing elements.
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
        /// </summary>
		/// <remarks>
        /// This method does not copy the queued elements in the order they would be dequeued. This
		/// method returns the items in the queue as they are stored in memory. The binary min-heap 
		/// underlying this queue does not store elements in a sorted order, the result of this 
		/// method is not sorted either.
		/// </remarks>
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
        /// Dequeues the element with the highest priority and returns it.
        /// </summary>
		/// <remarks>
		/// If the queue becomes empty because of the requested dequeue operation, the 
		/// PriorityAdjustment will be reset to 0 since when the queue is empty, we know that 
		/// resetting this value will not affect the ordering of any pre-existing elements.
		/// </remarks>
        /// <returns>A PriorityValuePair where the key is set to the priority of the dequeued 
		/// element and the value is set to the value of the dequeued element.</returns>
        public
        PriorityValuePair<T>
        Dequeue()
        {
			PriorityValuePair<T> result = __heap.Pop();
			if ( Count <= 0 )
			{
				// If the queue is empty now, clear the queue to reset the PriorityAdjustment.
				Clear();
			}
            return result;
        }


        /// <summary>
        /// Enqueues an existing PriorityValuePair element into the priority queue.
        /// </summary>
		/// <remarks>
		/// Increments the PriorityAdjustment by one EPSILON value for each queued item to ensure
		/// FIFO ordering of elements with the same priority.
		/// </remarks>
        /// <param name="element">A PriorityValuePair element.</param>
        public
        void
        Enqueue( PriorityValuePair<T> element )
        {
			// Apply the priorty adjustment before adding the new element so it orders correctly.
			element.Priority -= PriorityAdjustment;

			// Increment the number of queued items to the priority adjustment updates.
			NumQueuedItems++;
			
			// Add the new element to the queue.
            __heap.Push( element );
        }


        /// <summary>
        /// Enqueues an element into the priority queue.
        /// </summary>
		/// <remarks>
		/// Increments the PriorityAdjustment by one EPSILON value for each queued item to ensure
		/// FIFO ordering of elements with the same priority.
		/// </remarks>
        /// <param name="priority">A double-precision floating-point priority.</param>
        /// <param name="value">A generically-typed object.</param>
        public
        void
        Enqueue( double priority, T value )
        {
			// Apply the priorty adjustment before adding the new element so it orders correctly.
            __heap.Push( priority - PriorityAdjustment, value );
			// Increment the number of queued items to the priority adjustment updates.
			NumQueuedItems++;
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
		/// <remarks>
		/// Because this does not change the contents of the queue, it has no impact upon the
		/// PriorityAdjustment.
		/// </remarks>
        /// <returns>A PriorityValuePair where the key is set to the priority of the dequeued element
        /// and the value is set to the data/value of the dequeued element.</returns>
        public
        PriorityValuePair<T>
        Peek()
        {
            return __heap.Peek();
        }


        /// <summary>
        /// Removes the first occurrence of the given PriorityValuePair element within the queue.
        /// </summary>
		/// <remarks>
		/// If the queue becomes empty because of the requested dequeue operation, the 
		/// PriorityAdjustment will be reset to 0 since when the queue is empty, we know that 
		/// resetting this value will not affect the ordering of any pre-existing elements.
		/// </remarks>
        /// <param name="item">The PriorityValuePair element to remove from the queue.</param>
        /// <returns><c>true</c> if item was successfully removed from the priority queue.
        /// This method returns <c>false</c> if item is not found in the collection.</returns>
        public
        bool
        Remove( PriorityValuePair<T> element )
        {
			bool result = __heap.Remove( element );
			if ( Count <= 0 )
			{
				// If the queue is empty now, clear the queue to reset the PriorityAdjustment.
				Clear();
			}
            return result;
        }


        #endregion
    }
}
