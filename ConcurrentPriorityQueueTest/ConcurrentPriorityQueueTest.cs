using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Axon.Collections
{
    [TestFixture]
    public class ConcurrentPriorityQueueTest
    {
        #region Instance members

        [Test]
        public void PropertyCapacity()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>( 15 );

            // Ensure that Capacity reports 15.
            Assert.That( queue.Capacity, Is.EqualTo( 15 ) );

            // Intentionally over-fill the queue to force it to resize.
            for ( int i = 0; i < 16; i++ )
            {
                queue.Enqueue( 1f, 1 );
            }

            // Ensure that Capacity is now greater than 15.
            Assert.That( queue.Capacity, Is.GreaterThan( 15 ) );
        }


        [Test]
        public void PropertyCount()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that Count reports 0.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Enqueue 3 elements in the queue.
            queue.Enqueue( 1f, 1 );
            queue.Enqueue( 3f, 3 );
            queue.Enqueue( 2f, 2 );

            // Ensure that Count now reports 3.
            Assert.That( queue.Count, Is.EqualTo( 3 ) );
        }


        [Test]
        public void PropertyIsEmpty()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that IsEmpty reports TRUE.
            Assert.That( queue.IsEmpty, Is.True );

            // Enqueue an element in the queue.
            queue.Enqueue( 1f, 1 );

            // Ensure that IsEmpty now reports FALSE.
            Assert.That( queue.IsEmpty, Is.False );
        }


        [Test]
        public void PropertyIsReadOnly()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that IsReadOnly always reports FALSE.
            Assert.That( queue.IsReadOnly, Is.False );
        }

        #endregion


        #region Constructors

        [Test]
		public void ConstructorParameterless()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Nothing to test here. The following explicitly passes this test:
            Assert.That( true, Is.True );
        }


        [Test]
        public void ConstructorInitialSize()
        {
            // Try to create a priority queue with a negative initial size and expect an 
			// ArgumentOutOfRangeException to be thrown.
			Assert.Throws<ArgumentOutOfRangeException>( () => {
				new ConcurrentPriorityQueue<int>( -10 );
			} );

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>( 15 );

            // Ensure that Capacity reports 15.
            Assert.That( queue.Capacity, Is.EqualTo( 15 ) );
        }

        #endregion


        #region Public API

        [Test]
        public void Add()
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Try to Dequeue() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				queue.Dequeue();
			} );

            // Call Add() to insert a new element to the queue as a KeyValuePair.
            queue.Add( new KeyValuePair<float, int>( 1f, 2 ) );

            // Expect a value of 2 on the first item to be removed after adding it.
            Assert.That( queue.DequeueValue(), Is.EqualTo( 2 ) );
        }


        [Test]
        public void Clear() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Enqueue a few elements into the queue.
            queue.Enqueue( 1f, 2 );
            queue.Enqueue( 3f, 6 );
            queue.Enqueue( 2f, 4 );

            // Ensure that 3 elements have been added to the queue.
            Assert.That( queue.Count, Is.EqualTo( 3 ) );

            // Clear the queue.
            queue.Clear();

            // Ensure that all of the elements have been removed.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );
        }


        [Test]
        public void Contains() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Create and store a new element.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );

            // Ensure the queue contains the element.
            Assert.That( queue.Contains( elem ), Is.False );

            // Enqueue it in the queue.
            queue.Enqueue( elem );

            // Ensure the queue now contains the element.
            Assert.That( queue.Contains( elem ), Is.True );
        }


        [Test]
        public void CopyTo() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Create a new array of size 5.
            KeyValuePair<float, int>[] arrayCopy = new KeyValuePair<float, int>[ 5 ];

            // Enqueue 3 elements into the queue.
            queue.Enqueue( 1f, 2 );
            queue.Enqueue( 3f, 6 );
            queue.Enqueue( 2f, 4 );

            // Copy the queue data to the array, starting from index 1 (not 0).
            queue.CopyTo( arrayCopy, 1 );

            // Expect the first array index to be unset, but all the rest to be set.
			// Note: The order of elements after the first can't be guaranteed, because the heap 
			// implementing the queue internally doesn't store things in an exact linear order, 
			// but we can be sure that the elements aren't going to be equal to a default 
			// KeyValuePair because we set them.
            Assert.That( arrayCopy[ 0 ], Is.EqualTo( default( KeyValuePair<float, int> ) ) );
            Assert.That( arrayCopy[ 1 ], Is.EqualTo( new KeyValuePair<float, int>( 3f, 6 ) ) );
            Assert.That( arrayCopy[ 2 ], Is.Not.EqualTo( default( KeyValuePair<float, int> ) ) );
            Assert.That( arrayCopy[ 3 ], Is.Not.EqualTo( default( KeyValuePair<float, int> ) ) );
            Assert.That( arrayCopy[ 4 ], Is.EqualTo( default( KeyValuePair<float, int> ) ) );
        }


        [Test]
        public void Dequeue() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Try to Dequeue() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				queue.Dequeue();
			} );

            // Ensure that the heap is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );
            queue.Enqueue( elem );

            // Ensure that the element was inserted into the heap.
            Assert.That( queue.Count, Is.EqualTo( 1 ) );
            Assert.That( queue.Peek(), Is.EqualTo( elem ) );

            // Ensure that the returned element points to the same object we stored earlier.
            Assert.That( queue.Dequeue(), Is.EqualTo( elem ) );

            // Ensure that the element was removed from the heap.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );
        }


        [Test]
        public void DequeueValue() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Try to DequeueValue() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				queue.DequeueValue();
			} );

            // Ensure that the heap is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );
            queue.Enqueue( elem );

            // Ensure that the element was inserted into the heap.
            Assert.That( queue.Count, Is.EqualTo( 1 ) );
            Assert.That( queue.Peek(), Is.EqualTo( elem ) );

            // Ensure that the value of the enqueued element is returned.
            Assert.That( queue.DequeueValue(), Is.EqualTo( 2 ) );

            // Ensure that the element was removed from the heap.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );
        }


        [Test]
        public void EnqueueElement() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that the queue is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the queue.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 1f, 2 );
            queue.Enqueue( elem );

            // Ensure that the element was inserted into the queue.
            Assert.That( queue.Peek(), Is.EqualTo( elem ) );

			// Store another element with higher priority and insert it as well.
			elem = new KeyValuePair<float, int>( 2f, 4 );
            queue.Enqueue( elem );

            // Ensure that the element was inserted into the queue and is at the front.
            Assert.That( queue.Peek(), Is.EqualTo( elem ) );
        }


        [Test]
        public void EnqueuePriorityValue() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that queue is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the queue.
            queue.Enqueue( 1f, 2 );

            // Ensure that the element was inserted into the queue.
            Assert.That( queue.PeekValue(), Is.EqualTo( 2 ) );

			// Store another element with higher priority and insert it as well.
            queue.Enqueue( 2f, 4 );

            // Ensure that the element was inserted into the queue.
            Assert.That( queue.PeekValue(), Is.EqualTo( 4 ) );
        }


        [Test]
        public void GetEnumerator() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Enqueue a few elements into the queue.
            queue.Enqueue( 1f, 2 );
            queue.Enqueue( 3f, 6 );
            queue.Enqueue( 2f, 4 );

            // Use the enumerator of queue (using disposes it when we're finished).
            using ( IEnumerator< KeyValuePair<float, int> > enumerator = queue.GetEnumerator() )
            {
                // Expect the first element to have the highest priority, and expect MoveNext() to 
				// return true until the last element. After the end of the heap is reached, it 
				// then returns false.
				// Note: Since the heap implementing the queue internally doesn't guarantee the 
				// order of elements after the first, we can only be certain of the root element
				// and after that we really can't be sure of the order -- just the length.
                Assert.That( enumerator.MoveNext(), Is.True );
                Assert.That( enumerator.Current.Value, Is.EqualTo( 6 ) );
                Assert.That( enumerator.MoveNext(), Is.True );
                Assert.That( enumerator.MoveNext(), Is.True );
                Assert.That( enumerator.MoveNext(), Is.False );
            }
        }


        [Test]
        public void Peek() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Try to Peek() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				queue.Peek();
			} );

            // Ensure that the queue is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the queue.
            KeyValuePair<float, int> elem1 = new KeyValuePair<float, int>( 1f, 2 );
            queue.Enqueue( elem1 );

            // Ensure that the element was inserted into the queue at the front.
            Assert.That( queue.Count, Is.EqualTo( 1 ) );
            Assert.That( queue.Peek(), Is.EqualTo( elem1 ) );

            // Insert another element with higher priority than the last.
            KeyValuePair<float, int> elem2 = new KeyValuePair<float, int>( 2f, 4 );
            queue.Enqueue( elem2 );

            // Ensure that Peak() returns the new front element.
            Assert.That( queue.Peek(), Is.EqualTo( elem2 ) );
        }


        [Test]
        public void PeekValue() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Try to Peek() and expect an InvalidOperationException to be thrown.
			Assert.Throws<InvalidOperationException>( () => {
				queue.Peek();
			} );

            // Ensure that the queue is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the queue.
            queue.Enqueue( new KeyValuePair<float, int>( 1f, 2 ) );

            // Ensure that the element was inserted into the queue at the front.
            Assert.That( queue.Count, Is.EqualTo( 1 ) );
            Assert.That( queue.PeekValue(), Is.EqualTo( 2 ) );

            // Insert another element with higher priority than the last.
            queue.Enqueue( new KeyValuePair<float, int>( 2f, 4 ) );

            // Ensure that Peak() returns the new front element's value.
            Assert.That( queue.PeekValue(), Is.EqualTo( 4 ) );
        }


        [Test]
        public void Remove() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Create and store a few elements.
            KeyValuePair<float, int> elem1 = new KeyValuePair<float, int>( 1f, 2 );
            KeyValuePair<float, int> elem2 = new KeyValuePair<float, int>( 2f, 4 );
            KeyValuePair<float, int> elem3 = new KeyValuePair<float, int>( 3f, 6 );

            // Expect Remove() to return false, indicating no element was removed (since the 
			// heap is empty and obviously can't be removed from).
			Assert.Throws<InvalidOperationException>( () => {
				queue.Remove( elem1 );
			} );

            // Enqueue 2 of the elements into the heap.
            queue.Enqueue( elem2 );
            queue.Enqueue( elem3 );

            // Expect Remove() to return false for elem1, indicating the element was removed
			// (since it doesn't belong to the heap and can't be found). This tests the if-else 
			// case for when the provided element isn't found in the heap.
            Assert.That( queue.Remove( elem1 ), Is.False );

            // Expect Remove() to return true for elem2, indicating the element was removed
			// (since it belongs to the heap and can be found). This tests the if-else case for 
			// when Count is 2 or greater.
            Assert.That( queue.Remove( elem2 ), Is.True );

            // Expect Remove() to return true for elem3, indicating the element was removed
			// (since it belongs to the heap and can be found). This tests the if-else case for 
			// when Count equals 1.
            Assert.That( queue.Remove( elem3 ), Is.True );
        }

        #endregion
    }
}