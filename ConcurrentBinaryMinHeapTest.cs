using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axon.Collections
{
    [TestClass]
    public class ConcurrentBinaryMinHeapTest
    {


        #region Instance members


        [TestMethod]
        public void PropertyCapacity()
        {
            // Create a new priority queue.
            ConcurrentBinaryMinHeap<int> instance = new ConcurrentBinaryMinHeap<int>( 15 );

            // Ensure that Capacity reports 15.
            Assert.That( instance.Capacity, Is.EqualTo( 15 ) );

            // Intentionally over-fill the queue to force it to resize.
            for ( int i = 0; i < 16; i++ )
            {
                instance.Enqueue( 1f, 1 );
            }

            // Ensure that Capacity is now greater than 15.
            Assert.That( instance.Capacity, Is.GreaterThan( 15 ) );
        }


        [TestMethod]
        public void PropertyCount()
        {
            // Create a new priority queue.
            ConcurrentBinaryMinHeap<int> instance = new ConcurrentBinaryMinHeap<int>();

            // Ensure that Count reports 0.
            Assert.That( instance.Count, Is.EqualTo( 0 ) );

            // Enqueue 3 elements in the queue.
            instance.Enqueue( 1f, 1 );
            instance.Enqueue( 3f, 3 );
            instance.Enqueue( 2f, 2 );

            // Ensure that Count now reports 3.
            Assert.That( instance.Count, Is.EqualTo( 3 ) );
        }


        [TestMethod]
        public void PropertyIsEmpty()
        {
            // Create a new priority queue.
            ConcurrentBinaryMinHeap<int> instance = new ConcurrentBinaryMinHeap<int>();

            // Ensure that Count reports TRUE.
            Assert.That( instance.IsEmpty, Is.True );

            // Enqueue an element in the queue.
            instance.Enqueue( 1f, 1 );

            // Ensure that Count now reports FALSE..
            Assert.That( instance.IsEmpty, Is.False );
        }


        [TestMethod]
        public void PropertyIsReadOnly()
        {
            // Create a new priority queue.
            ConcurrentBinaryMinHeap<int> instance = new ConcurrentBinaryMinHeap<int>();

            // Ensure that Count always reports FALSE.
            Assert.That( instance.IsReadOnly, Is.False );
        }


        #endregion





        #region Constructors


        [TestMethod]
        public
        void
        ConstructorParameterless()
        {
            // Create a new priority queue.
            ConcurrentBinaryMinHeap<int> instance = new ConcurrentBinaryMinHeap<int>();

            // Nothing to test here. The following explicitly passes this test:
            Assert.That( true, Is.True );
        }


        [TestMethod]
        public
        void
        ConstructorInitialSize()
        {
            // Declare a priority queue var, but don't assign it yet.
            ConcurrentBinaryMinHeap<int> instance;

            // Try to create a priority queue with a negative initial size and expect
            // an InvalidOperationException to be thrown.
            try {
                instance = new ConcurrentBinaryMinHeap<int>( -10 );
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e ) {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Create a new priority queue.
            ConcurrentBinaryMinHeap<int> instance = new ConcurrentBinaryMinHeap<int>( 15 );

            // Ensure that Capacity reports 15.
            System.Assert.AreEqual( instance.Capacity, 15 );
        }


        #endregion





        #region Heap operations (uncomment these if you make heap operations public to debug them)


        [TestMethod]
        public void Add() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Try to Dequeue() and expect an InvalidOperationException to be thrown.
            try {
                instance.Dequeue();
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e ) {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Call Add() to insert a new element to the queue as a KeyValuePair.
            instance.Add( new KeyValuePair<float, int>( 1f, 2 ) );

            // Expect a value of 1 on the first item to be removed after adding it.
            Assert.That( instance.DequeueValue(), Is.EqualTo( 2 ) );
        }


        [TestMethod]
        public void Clear() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Enqueue a few elements in the queue.
            instance.Enqueue( 1f, 2 );
            instance.Enqueue( 3f, 6 );
            instance.Enqueue( 2f, 4 );

            // Expect there for be at least 1 element so we have proof that something was actually
            // cleared from the queue later.
            Assert.That(
                instance.Dequeue(),
                Is.NotEqualTo( default( KeyValuePair<float, int> ) )
            );

            // Clear the queue.
            instance.Clear();

            // Try to Dequeue() again and expect an InvalidOperationException to be thrown.
            try {
                instance.Dequeue();
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e ) {
                Assert.Fail( "Incorrect exception type thrown!" );
            }
        }


        [TestMethod]
        public void Contains() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Create and store a new element.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );

            // Expect the queue to not contain the element.
            Assert.That( instance.Contains( elem ), Is.False );

            // Enqueue it in the queue.
            instance.Add( elem );

            // Expect the queue to now contain the element.
            Assert.That( instance.Contains( elem ), Is.True );

            // Enqueue it in the queue.
            instance.Dequeue();

            // Expect the queue to no longer contain the element.
            Assert.That( instance.Contains( elem ), Is.False );
        }


        [TestMethod]
        public void CopyTo() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Create a new array of size 4.
            KeyValuePair<float, int>[] arrayCopy = new KeyValuePair<float, int>[ 5 ];

            // Enqueue 3 elements in the queue.
            instance.Enqueue( 1f, 2 );
            instance.Enqueue( 3f, 6 );
            instance.Enqueue( 2f, 4 );

            // Copy the queue data to the array, starting from index 1 (not 0).
            instance.CopyTo( arrayCopy, 1 );

            // Expect the first array index to be unset, but all the rest to be set.
            Assert.That( arrayCopy[ 0 ],       Is.EqualTo( default( KeyValuePair<float, int> ) ) );
            Assert.That( arrayCopy[ 1 ].Value, Is.EqualTo( 2 ) );
            Assert.That( arrayCopy[ 2 ].Value, Is.EqualTo( 4 ) );
            Assert.That( arrayCopy[ 3 ].Value, Is.EqualTo( 6 ) );
            Assert.That( arrayCopy[ 4 ],       Is.EqualTo( default( KeyValuePair<float, int> ) ) );
        }


        [TestMethod]
        public void GetEnumerator() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Enqueue a few elements in the queue.
            instance.Enqueue( 1f, 1 );
            instance.Enqueue( 3f, 3 );
            instance.Enqueue( 2f, 2 );

            // Use the enumerator of instance (using disposes it when we're finished).
            using ( var enumerator = instance.GetEnumerator() )
            {
                // Expect elements to be ordered by descending priority value
                Assert.That( enumerator.Current.Value, Is.EqualTo( 3 ) );
                enumerator.MoveNext();
                Assert.That( enumerator.Current.Value, Is.EqualTo( 2 ) );
                enuSmerator.MoveNext();
                Assert.That( enumerator.Current.Value, Is.EqualTo( 1 ) );
            }
        }


        [TestMethod]
        public
        void
        Peek()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Try to HeapPeek() and expect an InvalidOperationException to be thrown.
            try
            {
                instance.HeapPeek();
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e )
            {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Ensure that heap is empty.
            Assert.That( instance.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, T> elem1 = new KeyValuePair<float, T>( 1f, 2 );
            instance.HeapInsert( elem1 );

            // Ensure that the element was inserted into the heap as the root element.
            Assert.That( instance.Count, Is.EqualTo( 1 ) );
            Assert.That( instance.HeapPeek(), Is EqualTo( elem1 ) );

            // Insert another element with higher priority than the last.
            KeyValuePair<float, T> elem2 = new KeyValuePair<float, T>( 2f, 4 );
            instance.HeapInsert( elem2 );

            // Ensure that HeapPeak() returns the new root element.
            Assert.That( instance.HeapPeek(), Is.EqualTo( elem2 ) );
        }


        [TestMethod]
        public
        void
        Pop()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Try to HeapDequeue() and expect an InvalidOperationException to be thrown.
            try
            {
                instance.HeapDequeue();
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e )
            {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Ensure that heap is empty.
            Assert.That( instance.Count, Is EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, T> elem = new KeyValuePair<float, T>( 1f, 2 );
            instance.HeapInsert( elem )

            // Ensure that the element was inserted into the heap.
            Assert.That( instance.Count, Is.EqualTo( 1 ) );
            Assert.That( instance.HeapPeek(), Is EqualTo( elem ) );

            // Ensure that the returned element points to the same object as the reference we
            // stored in elem earlier.
            Assert.That( instance.HeapDequeue(), Is.EqualTo( elem ) );

            // Ensure that the element was removed from the heap.
            Assert.That( instance.Count, Is.EqualTo( 0 ) );
        }


        [TestMethod]
        public
        void
        Push()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Ensure that heap is empty.
            Assert.That( instance.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, T> elem = new KeyValuePair<float, T>( 1f, 2 );
            instance.HeapInsert( elem );

            // Ensure that the element was inserted into the heap.
            Assert.That( instance.Count, Is.EqualTo( 1 ) );
            Assert.That( instance.HeapPeek(), Is.EqualTo( elem ) );
        }


        [TestMethod]
        public
        void
        Push()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Ensure that heap is empty.
            Assert.That( instance.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, T> elem = new KeyValuePair<float, T>( 1f, 2 );
            instance.HeapInsert( elem );

            // Ensure that the element was inserted into the heap.
            Assert.That( instance.Count, Is.EqualTo( 1 ) );
            Assert.That( instance.HeapPeek(), Is.EqualTo( elem ) );
        }


        [TestMethod]
        public void Remove() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Create and store a new element.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 1 );

            // Expect Remove() to return false, indicating no element was removed.
            Assert.That( instance.Remove( elem ), Is.False );

            // Call Add() to insert a new element to the queue as a KeyValuePair.
            instance.Add( new KeyValuePair<float, int>( 1f, 1 ) );

            // Expect Remove() to return true, indicating the element was removed.
            Assert.That( instance.Remove( elem ), Is.True );

            // Expect Remove() to return false, indicating no element was removed.
            Assert.That( instance.Remove( elem ), Is.False );
        }


        #endregion





        #region Private methods (uncomment these if they have been made public for debugging)


        [TestMethod]
        public
        void
        SwapElements()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Enqueue an element into the queue.
            var elem1 = new KeyValuePair<float, T>( 2f, 4 );
            instance.HeapInsert( elem1 );

            // Ensure that the element was inserted.
            Assert.That( instance.Count, Is.EqualTo( 1 ) );
            Assert.That( instance.HeapPeek(), Is.EqualTo( elem1 ) );

            // Try to HeapSwapElements() while the queue only contains 1 element and expect an
            // InvalidOperationException to be thrown.
            try
            {
                instance.HeapSwapElements( 0, 1 );
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e )
            {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Enqueue another element with higher priority than the last.
            var elem2 = new KeyValuePair<float, T>( 1f, 2 );
            instance.HeapInsert( elem2 );

            // Ensure that the element was inserted.
            Assert.That( instance.Count, Is.EqualTo( 2 ) );
            Assert.That( instance.HeapPeek(), Is EqualTo( elem2 ) );

            // Try to HeapSwapElements() with an invalid index1 and expect an
            // ArgumentOutOfRangeException to be thrown.
            try
            {
                instance.HeapSwapElements( -1, 1 );
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( ArgumentOutOfRangeException e ) {}
            catch ( Exception e )
            {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Try to HeapSwapElements() with an invalid index2 and expect an
            // ArgumentOutOfRangeException to be thrown.
            try
            {
                instance.HeapSwapElements( 0, -1 );
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( ArgumentOutOfRangeException e ) {}
            catch ( Exception e )
            {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Actually swap elements now.
            instance.HeapSwapElements( 0, 1 );

            // Ensure that the elements were swapped.
            Assert.That( instance.Count, Is.EqualTo( 2 ) );
            Assert.That( instance.HeapPeek(), Is.EqualTo( elem1 ) );
            Assert.That( instance.Contains( elem2 ), Is.True ) );
        }


        // TODO HeapifyBottomUp() test
        [TestMethod]
        public
        void
        HeapifyBottomUp()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Execute several HeapifyBottomUp()s to test different tree operations on the heap.
            var index = 0;
            instance.HeapifyBottomUp( index );
        }


        // TODO HeapifyTopDown() test
        [TestMethod]
        public
        void
        HeapifyTopDown()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Execute several HeapifyBottomUp()s to test different tree operations on the heap.
            var index = 0;
            instance.HeapifyTopDown( index );
        }


        #endregion