using System;
using System.Collections;
using System.Collections.Generic;

namespace Axon.Collections
{
    /// <summary>
    /// The ConcurrentBinaryMinHeap class is a thread-safe generic binary heap for sorting
    /// such that the largest element is always the root element. This heap uses the min-heap
    /// property, therefore the element with the highest priority will always be removed first.
    /// </summary>
    /// <typeparam name="T">The type of data to be queued.</typeparam>
    public
    class ConcurrentBinaryMinHeap<T>
    : ICollection< KeyValuePair<float, T> >
    {


        #region Instance members


        /// <summary>
        /// The actual List array structure that backs the implementation of the heap.
        /// </summary>
        private List< KeyValuePair<float, T> > __data;


        /// <summary>
        /// Returns the number of elements the heap can hold without auto-resizing.
        /// </summary>
        public int Capacity
        {
            get
            {
                // Lock the heap -- CRITICAL SECTION BEGIN
                Monitor.Enter( __data );
                int result = 0;
                try
                {
                    // Compute the property value
                    result = __data.Capacity;
                }
                finally
                {
                    Monitor.Exit( __data );
                    // Unlock the heap -- CRITICAL SECTION END
                    return result;
                }
            }
        }


        /// <summary>
        /// Return the number of elements in the heap.
        /// </summary>
        public int Count
        {
            get
            {
                // Lock the heap -- CRITICAL SECTION BEGIN
                Monitor.Enter( __data );
                int result = 0;
                try
                {
                    // Compute the property value
                    result = __data.Count;
                }
                finally
                {
                    Monitor.Exit( __data );
                    // Unlock the heap -- CRITICAL SECTION END
                    return result;
                }
            }
        }


        /// <summary>
        /// Returns whether or not the heap is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                // Lock the heap -- CRITICAL SECTION BEGIN
                Monitor.Enter( __data );
                bool result = false;
                try
                {
                    // Compute the property value
                    result = __data.Count == 0;
                }
                finally
                {
                    Monitor.Exit( __data );
                    // Unlock the heap -- CRITICAL SECTION END
                    return result;
                }
            }
        }


        /// <summary>
        /// Returns whether or not the collection is read-only. For a heap this property returns
        /// <c>false</c>.
        /// </summary>
        public bool IsReadOnly { get { return false; } }


        #endregion





        #region Constructors


        /// <summary>
        /// Create a new default heap.
        /// </summary>
        public
        ConcurrentBinaryMinHeap()
        {
            // Lock the heap -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                __data = new List< KeyValuePair<float, T> >();
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the heap -- CRITICAL SECTION END
            }
        }


        /// <summary>
        /// Create a new heap with the given initial size of the array implementing it internally.
        /// </summary>
        /// <param name="initialCapacity">The initial size of the data List internal to the heap.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by the List constructor when the given initialCapacity is less than 0.
        /// </exception>
        public
        ConcurrentBinaryMinHeap( int initialCapacity )
        {
            // Lock the heap -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                __data = new List< KeyValuePair<float, T> >( initialCapacity );
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the heap -- CRITICAL SECTION END
            }
        }


        #endregion





        #region Public methods


        /// <summary>
        /// Inserts a KeyValuePair as a new element into the heap.
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
                Push( element );
            }
            catch ( Exception e )
            {
                throw e;
            }
        }


        /// <summary>
        /// Clears all elements from the heap.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Thrown when the heap is in read-only mode.
        /// </exception>
        public
        void
        Clear()
        {
            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                __data.Clear();
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
            }
        }


        /// <summary>
        /// Returns whether or not the heap contains the given KeyValuePair element.
        /// </summary>
        /// <param name="item">The KeyValuePair to locate in the heap.</param>
        /// <returns><c>true</c> if item is found in the heap; otherwise, <c>false</c>.</returns>
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
            if ( !element )
            {
                throw new ArgumentNullException( "element to find must be non-null." );
            }

            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            bool result = false;
            try
            {
                result = __data.Contains( element );
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
                return result;
            }
        }


        /// <summary>
        /// Copies the elements of the heap to an Array, starting at a particular index. This
        /// method does not guarantee that elements will be copied in the sorted order.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements
        /// copied from theheap. The Array must have zero-based indexing. </param>
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
            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                __data.CopyTo( array, arrayIndex );
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
            }
        }


        /// <summary>
        /// Returns an enumerator that iterates through the heap's elements. This enumerator is
        /// not guaranteed to iterate through elements in sorted order.
        /// </summary>
        /// <returns>An generic enumerator of the heap's contents.</returns>
        public
        IEnumerator< KeyValuePair<float, T> >
        GetEnumerator()
        {
            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            IEnumerator< KeyValuePair<float, T> > result = null;
            try
            {
                result = __data.GetEnumerator();
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
                return result;
            }
        }


        /// <summary>
        /// Returns an enumerator that iterates through the heap's elements. This enumerator is
        /// not guaranteed to iterate through elements in sorted order.
        /// </summary>
        /// <returns>An enumerator of the heap's contents.</returns>
        IEnumerator
        IEnumerable.GetEnumerator()
        {
            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            IEnumerator result = null;
            try
            {
                // Call the above function (pretty sure? -- Thread lock this anyway to be safe!)
                result = this.GetEnumerator();
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
                return result;
            }
        }


        /// <summary>
        /// Return the current root element of the heap, but don't remove it.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap is empty.
        /// </exception>
        public
        KeyValuePair<float, T>
        Peek()
        {
            if ( IsEmpty )
            {
                throw new InvalidOperationException( "The heap is empty." );
            }

            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            KeyValuePair<float, T> result = null;
            try
            {
                // Return the root element of the heap.
                result = __data[ 0 ];
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
                return result;
            }
        }


        /// <summary>
        /// Return the current root element of the heap, and then remove it. This operation will
        /// heapify the heap after removal to ensure that it remains sorted.
        /// </summary>
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
        Pop()
        {
            if ( IsEmpty )
            {
                throw new InvalidOperationException( "The heap is empty." );
            }

            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            KeyValuePair<float, T> result
            try
            {
                // Keep a reference to the element at the root of the heap.
                result = __data[ 0 ];

                if ( __data.Count <= 1 )
                {
                    // Clear the last element. No need to heapify since there's nothing left.
                    __data.Clear();
                }
                else
                {
                    // Move the last element up to be the root of the heap.
                    SwapElements( 0, __data.Count - 1 );
                    __data.RemoveAt( __data.Count - 1 );

                    // Heapify to move the new root into its correct position within the heap.
                    HeapifyTopDown( 0 );
                }
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
                return result;
            }
        }


        /// <summary>
        /// Insert a new element into the heap and heapify it into its correct position, given
        /// an existing KeyValuePair containing a float priority as its key and a value.
        /// </summary>
        /// <param name="element">A KeyValuePair containing a float priority as its key and a
        /// generically-typed value.</param>
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
        Push( KeyValuePair<float, T> element )
        {
            if ( !element )
            {
                throw new ArgumentNullException( "KeyValuePair element to insert cannot be null." );
            }

            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                // Add a new element to the heap at the end of the data list.
                __data.Add( element );

                // Heapify bottom up to sort the element into the correct position in the heap.
                HeapifyBottomUp( __data.Count - 1 );
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
            }
        }


        /// <summary>
        /// Insert a new element into the heap and heapify it into its correct position, given
        /// a float priority and some value to create an element from.
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
        Push( float priority, T value )
        {
            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                // Add a new element to the heap at the end of the data list.
                __data.Add( new KeyValuePair<float, T>( priority, value ) );

                // Heapify bottom up to sort the element into the correct position in the heap.
                HeapifyBottomUp( __data.Count - 1 );
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
            }
        }


        /// <summary>
        /// Removes the first occurrence of the given KeyValuePair element within the heap.
        /// </summary>
        /// <param name="item">The KeyValuePair element to remove from the heap.</param>
        /// <returns><c>true</c> if item was successfully removed from the priority heap.
        /// This method returns <c>false</c> if item is not found in the collection.</returns>
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
            if ( __data.IsEmpty )
            {
                throw new InvalidOperationException( "The heap is empty." );
            }
            if ( !element )
            {
                throw new ArgumentNullException( "element to remove must be non-null." );
            }

            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            bool result = false;
            try
            {
                // Find the element within the heap.
                int index = __data.IndexOf( element );
                if ( index < 0 )
                {
                    // Return false to indicate that the element was not found in the heap.
                    result = false;
                }
                else
                {
                    // Move the last element up to index of the found element.
                    SwapElements( index, __data.Count - 1 );
                    __data.RemoveAt( __data.Count - 1 );

                    // Heapify to move the element at index into its correct position within the heap.
                    int newIndex = HeapifyBottomUp( index );
                    if ( newIndex == index )
                    {
                        HeapifyTopDown( index );
                    }

                    // Return true to indicate that the element was found.
                    result = true;
                }
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
                return result;
            }
        }


        #endregion





        #region Private methods


        /// <summary>
        /// Swap the heap element at index1 with the heap element at index2.
        /// </summary>
        /// <param name="index1">The first heap element to swap.</param>
        /// <param name="index2">The second heap element to swap.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the given index1 is out of range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the given index2 is out of range.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap contains less than 2 elements.
        /// </exception>
        private
        void
        SwapElements( int index1, int index2 )
        {
            if ( __data.Count < 2 )
            {
                throw new InvalidOperationException( "The heap must contain at least 2 elements." );
            }
            if ( index1 < 0 || index1 >= __data.Count )
            {
                throw new ArgumentOutOfRangeException( "index1 must be within the range [0,Count-1]." );
            }
            if ( index2 < 0 || index2 >= __data.Count )
            {
                throw new ArgumentOutOfRangeException( "index2 must be within the range [0,Count-1]." );
            }

            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                // Do a plain old swap of the elements at index1 and index2 in the heap.
                KeyValuePair<float, T> temp = __data[ index1 ];
                __data[ index1 ] = __data[ index2 ];
                __data[ index2 ] = temp;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
            }
        }


        /// <summary>
        /// Use the upward-sorting heapify algorithm to sort the element at the given index
        /// upward to the correct position it should have within the heap. To be a bit clearer,
        /// an element sorted using this heapify tends to move up through the tree, closer to
        /// the root node.
        /// </summary>
        /// <param name="index">The index of the element to sort within the heap.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the given index is out of range.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown by SwapElements() when the inputs to SwapElements() are invalid.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown by SwapElements() when there are less than 2 elements in the heap.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap is empty.
        /// </exception>
        /// <remarks>
        /// See Inspiration.cs HeapifyFromEndToBeginning() for the original algorithm.
        /// </remarks>
        private
        int
        HeapifyBottomUp( int index )
        {
            if ( IsEmpty )
            {
                throw new InvalidOperationException( "The heap is empty." );
            }
            if ( index < 0 || index >= __data.Count )
            {
                throw new ArgumentOutOfRangeException( "index must be within the range [0,Count-1]." );
            }

            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                // Given an index i of some heap node:
                // Index of the LEFT CHILD of i  = 2i + 1
                // Index of the RIGHT CHILD of i = 2i + 2
                // Index of the PARENT of i      = (i - 1) / 2

                float priority = 0f;
                float priorityParent = 0f;
                while ( index > 0 && priority < priorityParent )
                {
                    int parentIndex = ( index - 1 ) / 2;
                    priority        = __data[ index ].Key;
                    priorityParent  = __data[ parentIndex ].Key;

                    comparison = priority < priorityParent;
                    if ( cpriority < priorityParent )
                    {
                        SwapElements( index, parentIndex );
                        index = parentIndex;
                    }
                }
            }
            catch ( Exception e )
            {
                throw e;
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
                return index;
            }
        }


        /// <summary>
        /// Use the downward-sorting heapify algorithm to sort the element at the given index
        /// downward to the correct position it should have within the heap. To be a bit clearer,
        /// an element sorted using this heapify tends to move down through the tree, further from
        /// the root node (and toward the leaves at the bottom).
        /// </summary>
        /// <param name="index">The index of the element to sort within the heap.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the given index is out of range.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the heap is empty.
        /// </exception>
        /// <remarks>
        /// See Inspiration.cs HeapifyFromBeginningToEnd() for the original algorithm.
        /// </remarks>
        private
        void
        HeapifyTopDown( int index )
        {
            if ( IsEmpty )
            {
                throw new InvalidOperationException( "The heap is empty." );
            }
            if ( index < 0 || index >= __data.Count )
            {
                throw new ArgumentOutOfRangeException( "Index must be a valid index within the heap." );
            }

            // Lock the thread -- CRITICAL SECTION BEGIN
            Monitor.Enter( __data );
            try
            {
                // Given an index i of some heap node:
                // Index of the LEFT CHILD of i  = 2i + 1
                // Index of the RIGHT CHILD of i = 2i + 2
                // Index of the PARENT of i      = (i - 1) / 2

                int smallest = -1;
                while ( index < __data.Count && index != smallest )
                {
                    // smallest is initialzed here since it can't be set before the while condition
                    // (index != smallest would result in the while never running if so).
                    smallest = index;
                    float priority = __data[ index ].Key;

                    // Check if the priority of the left child is smaller than that of the element at
                    // the current index.
                    int leftIndex = 2 * index + 1;
                    if ( leftIndex < __data.Count )
                    {
                        float priorityLeftChild = __data[ leftIndex ].Key;
                        if ( priority < priorityLeftChild )
                        {
                            // Update the smallest index with the index of the left child.
                            smallest = leftIndex;
                        }
                    }

                    // Check if the priority of the left child is smaller than that of the element at
                    // the current index.
                    int rightIndex = 2 * index + 2;
                    if ( rightIndex < __data.Count )
                    {
                        float priorityRightChild = __data[ rightIndex ].Key;
                        if ( priority < priorityRightChild )
                        {
                            // Update the smallet index with the index of the right child.
                            smallest = rightIndex;
                        }
                    }
                }
            }
            finally
            {
                Monitor.Exit( __data );
                // Unlock the thread -- CRITICAL SECTION END
            }
        }


        #endregion


    }
}