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
            queue.Enqueue( 1.0, 1 );
            queue.Enqueue( 3.0, 3 );
            queue.Enqueue( 2.0, 2 );

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
            queue.Enqueue( 1.0, 1 );

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


        [Test]
        public void PropertyNumQueuedItems()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that NumQueuedItems reports 0.
            Assert.That( queue.NumQueuedItems, Is.EqualTo( 0L ) );

            // Enqueue an element in the queue.
            queue.Enqueue( 1.0, 1 );

            // Ensure that NumQueuedItems reports 1.
            Assert.That( queue.NumQueuedItems, Is.EqualTo( 1L ) );

            // Enqueue an element in the queue.
            queue.Enqueue( 1.0, 1 );

            // Ensure that NumQueuedItems reports 2.
            Assert.That( queue.NumQueuedItems, Is.EqualTo( 2L ) );

			// Clear the queue.
			queue.Clear();

            // Ensure that NumQueuedItems reports 0.
            Assert.That( queue.NumQueuedItems, Is.EqualTo( 0L ) );
        }


        [Test]
        public void PropertyPriorityAdjustment()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that PriorityAdjustment reports 0.
            Assert.That( queue.PriorityAdjustment, Is.EqualTo( 0.0 ) );

            // Enqueue an element in the queue.
            queue.Enqueue( 1.0, 1 );

            // Ensure that PriorityAdjustment reports 1.
            Assert.That( queue.PriorityAdjustment, Is.EqualTo( 1L * ConcurrentPriorityQueue<int>.EPSILON ) );

            // Enqueue an element in the queue.
            queue.Enqueue( 1.0, 1 );

            // Ensure that PriorityAdjustment reports 2.
            Assert.That( queue.PriorityAdjustment, Is.EqualTo( 2L * ConcurrentPriorityQueue<int>.EPSILON ) );

			// Clear the queue.
			queue.Clear();

            // Ensure that PriorityAdjustment reports 0.
            Assert.That( queue.PriorityAdjustment, Is.EqualTo( 0.0 ) );
        }

        #endregion


        #region Constructors

        [Test]
		public void ConstructorParameterless()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Nothing to test about the queue, but check that NumQueuedItems inits to 0.
            Assert.That( queue.NumQueuedItems, Is.EqualTo( 0L ) );
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

            // Check that NumQueuedItems inits to 0.
            Assert.That( queue.NumQueuedItems, Is.EqualTo( 0L ) );
        }

        #endregion


        #region Public API

        [Test]
        public void Add()
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that the queue is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Call Add() to insert a new element to the queue as a KeyValuePair.
            queue.Add( new PriorityValuePair<int>( 1.0, 2 ) );

            // Expect a value of 2 on the first item to be removed after adding it.
            Assert.That( queue.Dequeue().Value, Is.EqualTo( 2 ) );
        }


        [Test]
        public void Clear() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Enqueue a few elements into the queue.
            queue.Enqueue( 1.0, 2 );
            queue.Enqueue( 3.0, 6 );
            queue.Enqueue( 2.0, 4 );

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
            PriorityValuePair<int> elem = new PriorityValuePair<int>( 1.0, 2 );

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
            PriorityValuePair<int>[] arrayCopy = new PriorityValuePair<int>[ 5 ];

            // Enqueue 3 elements into the queue.
			PriorityValuePair<int> elem = new PriorityValuePair<int>( 3.0, 6 );
            queue.Enqueue( 1.0, 2 );
            queue.Enqueue( elem );
            queue.Enqueue( 2.0, 4 );

            // Copy the queue data to the array, starting from index 1 (not 0).
            queue.CopyTo( arrayCopy, 1 );

            // Expect the first array index to be unset, but all the rest to be set.
			// Note: The order of elements after the first can't be guaranteed, because the heap 
			// implementing the queue internally doesn't store things in an exact linear order, 
			// but we can be sure that the elements aren't going to be equal to null because we 
			// set them.
            Assert.That( arrayCopy[ 0 ], Is.EqualTo( null ) );
            Assert.That( arrayCopy[ 1 ], Is.EqualTo( elem ) );
            Assert.That( arrayCopy[ 2 ], Is.Not.EqualTo( null ) );
            Assert.That( arrayCopy[ 3 ], Is.Not.EqualTo( null ) );
            Assert.That( arrayCopy[ 4 ], Is.EqualTo( null ) );
        }


        [Test]
        public void Dequeue() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that the heap is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );
			
			// Expect Dequeue() to return null for an empty heap.
            Assert.That( queue.Dequeue(), Is.EqualTo( null ) );

            // Ensure that the heap is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the heap.
            PriorityValuePair<int> elem = new PriorityValuePair<int>( 1.0, 2 );
            queue.Enqueue( elem );

            // Ensure that the element was inserted into the heap.
            Assert.That( queue.Count, Is.EqualTo( 1 ) );
            Assert.That( queue.Peek(), Is.EqualTo( elem ) );

            // Ensure that the PriorityAdjustment was incremented.
            Assert.That( queue.PriorityAdjustment, Is.EqualTo( ConcurrentPriorityQueue<int>.EPSILON ) );

            // Ensure that the returned element points to the same object we stored earlier.
            Assert.That( queue.Dequeue(), Is.EqualTo( elem ) );

            // Ensure that the element was removed from the heap.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Ensure that the PriorityAdjustment was reset once the queue became empty.
            Assert.That( queue.PriorityAdjustment, Is.EqualTo( 0.0 ) );

            // Enqueue 5 items with the same priority.
            PriorityValuePair<int> elem2 = new PriorityValuePair<int>( 2.0, 0 );
            PriorityValuePair<int> elem3 = new PriorityValuePair<int>( 2.0, 2 );
            PriorityValuePair<int> elem4 = new PriorityValuePair<int>( 2.0, 4 );
            PriorityValuePair<int> elem5 = new PriorityValuePair<int>( 2.0, 6 );
            PriorityValuePair<int> elem6 = new PriorityValuePair<int>( 2.0, 8 );
            queue.Enqueue( elem2 );
            queue.Enqueue( elem3 );
            queue.Enqueue( elem4 );
            queue.Enqueue( elem5 );
            queue.Enqueue( elem6 );

			//// Ensure that Dequeue() returns the items in the order they were enqueued.
			Assert.That( queue.Dequeue(), Is.EqualTo( elem2 ) );
			Assert.That( queue.Dequeue(), Is.EqualTo( elem3 ) );
			Assert.That( queue.Dequeue(), Is.EqualTo( elem4 ) );
			Assert.That( queue.Dequeue(), Is.EqualTo( elem5 ) );
			Assert.That( queue.Dequeue(), Is.EqualTo( elem6 ) );
        }


        [Test]
        public void EnqueueElement() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Ensure that the queue is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the queue.
            PriorityValuePair<int> elem = new PriorityValuePair<int>( 1.0, 2 );
            queue.Enqueue( elem );

            // Ensure that the element was inserted into the queue.
            Assert.That( queue.Peek(), Is.EqualTo( elem ) );

			// Store another element with higher priority and insert it as well.
			elem = new PriorityValuePair<int>( 2.0, 4 );
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
            queue.Enqueue( 1.0, 2 );

            // Ensure that the element was inserted into the queue.
            Assert.That( queue.Peek().Value, Is.EqualTo( 2 ) );

			// Store another element with higher priority and insert it as well.
            queue.Enqueue( 2.0, 4 );

            // Ensure that the element was inserted into the queue.
            Assert.That( queue.Peek().Value, Is.EqualTo( 4 ) );
        }


        [Test]
        public void GetEnumerator() 
		{
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Enqueue a few elements into the queue.
            queue.Enqueue( 1.0, 2 );
            queue.Enqueue( 3.0, 6 );
            queue.Enqueue( 2.0, 4 );

            // Use the enumerator of queue (using disposes it when we're finished).
            using ( IEnumerator< PriorityValuePair<int> > enumerator = queue.GetEnumerator() )
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

            // Ensure that the heap is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );
			
			// Expect Peek() to return null for an empty heap.
            Assert.That( queue.Peek(), Is.EqualTo( null ) );

            // Ensure that the queue is empty.
            Assert.That( queue.Count, Is.EqualTo( 0 ) );

            // Store an element and insert it into the queue.
            PriorityValuePair<int> elem1 = new PriorityValuePair<int>( 1.0, 2 );
            queue.Enqueue( elem1 );

            // Ensure that the element was inserted into the queue at the front.
            Assert.That( queue.Count, Is.EqualTo( 1 ) );
            Assert.That( queue.Peek(), Is.EqualTo( elem1 ) );

            // Ensure that the element was not removed from the heap.
            Assert.That( queue.Count, Is.EqualTo( 1 ) );

            // Insert another element with higher priority than the last.
            PriorityValuePair<int> elem2 = new PriorityValuePair<int>( 2.0, 4 );
            queue.Enqueue( elem2 );

            // Ensure that Peek() returns the new front element.
            Assert.That( queue.Peek(), Is.EqualTo( elem2 ) );

            // Insert another element with the same priority as the last.
            PriorityValuePair<int> elem3 = new PriorityValuePair<int>( 2.0, 6 );
            queue.Enqueue( elem3 );

            // Ensure that Peek() returns still returns the first value with that priority.
            Assert.That( queue.Peek(), Is.EqualTo( elem2 ) );

			// Remove the element from the queue.
			queue.Dequeue();

            // Ensure that Peek() returns now returns the other value with the same priorty.
            Assert.That( queue.Peek(), Is.EqualTo( elem3 ) );
        }


        [Test]
        public void Remove() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> queue = new ConcurrentPriorityQueue<int>();

            // Create and store a few elements.
            PriorityValuePair<int> elem1 = new PriorityValuePair<int>( 1.0, 2 );
            PriorityValuePair<int> elem2 = new PriorityValuePair<int>( 2.0, 4 );
            PriorityValuePair<int> elem3 = new PriorityValuePair<int>( 3.0, 6 );

            // Expect Remove() to return false for an empty queue.
            Assert.That( queue.Remove( elem1 ), Is.False );

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