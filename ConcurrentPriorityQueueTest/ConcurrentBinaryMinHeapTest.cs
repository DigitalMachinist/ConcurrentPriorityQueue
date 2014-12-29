using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Axon.Collections
{
    [TestFixture]
    public class ConcurrentBinaryMinHeapTest
    {
        #region Instance members

        [Test]
        public void PropertyCapacity()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>( 15 );

            // Ensure that Capacity reports 15.
            Assert.That( heap.Capacity, Is.EqualTo( 15 ) );

            // Intentionally over-fill the queue to force it to resize.
            for ( int i = 0; i < 16; i++ )
            {
                heap.Push( 1f, 1 );
            }

            // Ensure that Capacity is now greater than 15.
            Assert.That( heap.Capacity, Is.GreaterThan( 15 ) );
        }


        [Test]
        public void PropertyCount()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Ensure that Count reports 0.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );

            // Enqueue 3 elements in the queue.
            heap.Push( 1f, 1 );
            heap.Push( 3f, 3 );
            heap.Push( 2f, 2 );

            // Ensure that Count now reports 3.
            Assert.That( heap.Count, Is.EqualTo( 3 ) );
        }


        [Test]
        public void PropertyIsEmpty()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Ensure that IsEmpty reports TRUE.
            Assert.That( heap.IsEmpty, Is.True );

            // Enqueue an element in the queue.
            heap.Push( 1f, 1 );

            // Ensure that IsEmpty now reports FALSE.
            Assert.That( heap.IsEmpty, Is.False );
        }


        [Test]
        public void PropertyIsReadOnly()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Ensure that IsReadOnly always reports FALSE.
            Assert.That( heap.IsReadOnly, Is.False );
        }

        #endregion


        #region Constructors

        [Test]
        public void ConstructorParameterless()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Nothing to test here. The following explicitly passes this test:
            Assert.That( true, Is.True );
        }


        [Test]
        public void ConstructorInitialSize()
        {
            // Try to create a heap with a negative initial size and expect an 
			// ArgumentOutOfRangeException to be thrown.
			Assert.Throws<ArgumentOutOfRangeException>( () => {
				new ConcurrentBinaryMinHeap<int>( -10 );
			} );

            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>( 15 );

            // Ensure that Capacity reports 15.
            Assert.That( heap.Capacity, Is.EqualTo( 15 ) );
        }

        #endregion


        #region Public API

        [Test]
        public void Add() {

            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Try to Dequeue() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				heap.Pop();
			} );

            // Call Add() to insert a new element to the queue as a KeyValuePair.
            heap.Add( new KeyValuePair<float, int>( 1f, 2 ) );

            // Expect a value of 2 on the first item to be removed after adding it.
            Assert.That( heap.PopValue(), Is.EqualTo( 2 ) );
        }


        [Test]
        public void Clear() {

            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Push 3 elements onto the heap.
            heap.Push( 1f, 2 );
            heap.Push( 3f, 6 );
            heap.Push( 2f, 4 );

            // Ensure that 3 elements have been added to the heap.
            Assert.That( heap.Count, Is.EqualTo( 3 ) );

            // Clear the heap.
            heap.Clear();

            // Ensure that all of the elements have been removed.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );
        }


        [Test]
        public void Contains() {

            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Create and store a new element.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );

            // Ensure the queue contains the element.
            Assert.That( heap.Contains( elem ), Is.False );

            // Push it onto the heap.
            heap.Push( elem );

            // Ensure the queue now contains the element.
            Assert.That( heap.Contains( elem ), Is.True );
        }


        [Test]
        public void CopyTo() 
		{
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Create a new array of size 5.
            KeyValuePair<float, int>[] arrayCopy = new KeyValuePair<float, int>[ 5 ];

            // Push 3 elements onto the queue.
            heap.Push( 1f, 2 );
            heap.Push( 3f, 6 );
            heap.Push( 2f, 4 );

            // Copy the heap data to the array, starting from index 1 (not 0).
            heap.CopyTo( arrayCopy, 1 );

            // Expect the first array index to be unset, but all the rest to be set.
			// Note: The order of elements after the first can't be guaranteed, because the heap 
			// doesn't store things in an exact linear order, but we can be sure that the elements 
			// aren't going to be equal to a default KeyValuePair because we set them.
            Assert.That( arrayCopy[ 0 ], Is.EqualTo( default( KeyValuePair<float, int> ) ) );
            Assert.That( arrayCopy[ 1 ], Is.EqualTo( new KeyValuePair<float, int>( 3f, 6 ) ) );
            Assert.That( arrayCopy[ 2 ], Is.Not.EqualTo( default( KeyValuePair<float, int> ) ) );
            Assert.That( arrayCopy[ 3 ], Is.Not.EqualTo( default( KeyValuePair<float, int> ) ) );
            Assert.That( arrayCopy[ 4 ], Is.EqualTo( default( KeyValuePair<float, int> ) ) );
        }


        [Test]
        public void GetEnumerator() 
		{
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Enqueue a few elements in the queue.
            heap.Push( 1f, 2 );
            heap.Push( 3f, 6 );
            heap.Push( 2f, 4 );

            // Use the enumerator of heap (using disposes it when we're finished).
            using ( IEnumerator< KeyValuePair<float, int> > enumerator = heap.GetEnumerator() )
            {
                // Expect the first element to have the highest priority, and expect MoveNext() to 
				// return true until the last element. After the end of the heap is reached, it 
				// then returns false.
				// Note: Since the heap doesn't guarantee the order of elements after the first, we 
				// can only be certain of the root element and after that we really can't be sure 
				// of the order -- just the length.
                Assert.That( enumerator.MoveNext(), Is.True );
                Assert.That( enumerator.Current.Value, Is.EqualTo( 6 ) );
                Assert.That( enumerator.MoveNext(), Is.True );
                Assert.That( enumerator.MoveNext(), Is.True );
                Assert.That( enumerator.MoveNext(), Is.False );
            }
        }


        [Test]
        public void Peek()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Try to Peek() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				heap.Peek();
			} );

            // Ensure that the heap is empty.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, int> elem1 = new KeyValuePair<float, int>( 1f, 2 );
            heap.Push( elem1 );

            // Ensure that the element was inserted into the heap as the root element.
            Assert.That( heap.Count, Is.EqualTo( 1 ) );
            Assert.That( heap.Peek(), Is.EqualTo( elem1 ) );

            // Insert another element with higher priority than the last.
            KeyValuePair<float, int> elem2 = new KeyValuePair<float, int>( 2f, 4 );
            heap.Push( elem2 );

            // Ensure that Peak() returns the new root element.
            Assert.That( heap.Peek(), Is.EqualTo( elem2 ) );
        }


        [Test]
        public void Pop()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Try to Pop() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				heap.Pop();
			} );

            // Ensure that the heap is empty.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );
            heap.Push( elem );

            // Ensure that the element was inserted into the heap.
            Assert.That( heap.Count, Is.EqualTo( 1 ) );
            Assert.That( heap.Peek(), Is.EqualTo( elem ) );

            // Ensure that the returned element points to the same object we stored earlier.
            Assert.That( heap.Pop(), Is.EqualTo( elem ) );

            // Ensure that the element was removed from the heap.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );
        }


		[Test]
        public void PopValue()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Try to PopValue() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				heap.PopValue();
			} );

            // Ensure that the heap is empty.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );
            heap.Push( elem );

            // Ensure that the element was inserted into the heap.
            Assert.That( heap.Peek(), Is.EqualTo( elem ) );

            // Ensure that the value of the pushed element is returned.
            Assert.That( heap.PopValue(), Is.EqualTo( 2 ) );

            // Ensure that the element was removed from the heap.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );
        }


        [Test]
        public void PushElement()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Ensure that the heap is empty.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );
            heap.Push( elem );

            // Ensure that the element was inserted into the heap.
            Assert.That( heap.Peek(), Is.EqualTo( elem ) );

			// Store another element with higher priority and insert it as well.
			elem = new KeyValuePair<float, int>( 2f, 4 );
            heap.Push( elem );
			
            // Ensure that the element was inserted into the queue and is at the root.
            Assert.That( heap.Peek(), Is.EqualTo( elem ) );
        }


        [Test]
        public void PushPriorityValue()
        {
            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Ensure that heap is empty.
            Assert.That( heap.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            heap.Push( 1f, 2 );

            // Ensure that the element was inserted into the heap.
            Assert.That( heap.PeekValue(), Is.EqualTo( 2 ) );

			// Store another element with higher priority and insert it as well.
            heap.Push( 2f, 4 );

            // Ensure that the element was inserted into the heap.
            Assert.That( heap.PeekValue(), Is.EqualTo( 4 ) );
        }


        [Test]
        public void Remove() {

            // Create a new heap.
            ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

            // Create and store a few elements.
            KeyValuePair<float, int> elem1 = new KeyValuePair<float, int>( 1f, 2 );
            KeyValuePair<float, int> elem2 = new KeyValuePair<float, int>( 2f, 4 );
            KeyValuePair<float, int> elem3 = new KeyValuePair<float, int>( 3f, 6 );

            // Expect Remove() to return false, indicating no element was removed (since the 
			// heap is empty and obviously can't be removed from).
			Assert.Throws<InvalidOperationException>( () => {
				heap.Remove( elem1 );
			} );

            // Insert 2 of the elements into the heap.
            heap.Push( elem2 );
            heap.Push( elem3 );

            // Expect Remove() to return false for elem1, indicating the element was removed
			// (since it doesn't belong to the heap and can't be found). This tests the if-else 
			// case for when the provided element isn't found in the heap.
            Assert.That( heap.Remove( elem1 ), Is.False );

            // Expect Remove() to return true for elem2, indicating the element was removed
			// (since it belongs to the heap and can be found). This tests the if-else case for 
			// when Count is 2 or greater.
            Assert.That( heap.Remove( elem2 ), Is.True );

            // Expect Remove() to return true for elem3, indicating the element was removed
			// (since it belongs to the heap and can be found). This tests the if-else case for 
			// when Count equals 1.
            Assert.That( heap.Remove( elem3 ), Is.True );
        }

        #endregion


        #region Private methods (these are testable as public methods in DEBUG builds)

		#if DEBUG
		[Test]
		public void SwapElements()
		{
			// Create a new heap.
			ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

			// Enqueue an element into the queue.
			var elem1 = new KeyValuePair<float, int>( 2f, 4 );
			heap.Push( elem1 );

			// Ensure that the element was inserted.
			Assert.That( heap.Count, Is.EqualTo( 1 ) );
			Assert.That( heap.Peek(), Is.EqualTo( elem1 ) );

			// Try to HeapSwapElements() while the queue only contains 1 element and expect an
			// InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				heap.SwapElements( 0, 1 );
			} );

			// Enqueue another element with higher priority than the last.
			var elem2 = new KeyValuePair<float, int>( 1f, 2 );
			heap.Push( elem2 );

			// Ensure that the element was inserted and that the 1st (higher priority) element is 
			// still at the root of the heap.
			Assert.That( heap.Count, Is.EqualTo( 2 ) );
			Assert.That( heap.Peek(), Is.EqualTo( elem1 ) );

			// Try to HeapSwapElements() with an invalid index1 and expect an
			// ArgumentOutOfRangeException to be thrown.
			Assert.Throws<ArgumentOutOfRangeException>( () => {
				heap.SwapElements( -1, 1 );
			} );

			// Try to HeapSwapElements() with an invalid index2 and expect an
			// ArgumentOutOfRangeException to be thrown.
			Assert.Throws<ArgumentOutOfRangeException>( () => {
				heap.SwapElements( 0, -1 );
			} );

			// Actually swap elements now.
			heap.SwapElements( 0, 1 );

			// Ensure that the elements were swapped.
			Assert.That( heap.Count, Is.EqualTo( 2 ) );
			Assert.That( heap.Peek(), Is.EqualTo( elem2 ) );
			Assert.That( heap.Contains( elem1 ), Is.True );
		}
		#endif


		#if DEBUG
		[Test]
		[Ignore]
		public void HeapifyBottomUp()
		{
			// TODO The HeapifyBottomUp() test is incomplete.

			// Create a new heap.
			ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

			// Execute several HeapifyBottomUp()s to test different tree operations on the heap.
			var index = 0;
			heap.HeapifyBottomUp( index );
		}
		#endif


		#if DEBUG
		[Test]
		[Ignore]
		public void HeapifyTopDown()
		{
			// TODO The HeapifyTopDown() test is incomplete.

			// Create a new heap.
			ConcurrentBinaryMinHeap<int> heap = new ConcurrentBinaryMinHeap<int>();

			// Execute several HeapifyBottomUp()s to test different tree operations on the heap.
			var index = 0;
			heap.HeapifyTopDown( index );
		}
		#endif

        #endregion
	}
}