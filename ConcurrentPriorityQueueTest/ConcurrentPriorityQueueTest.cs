using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axon.Collections
{
    [TestClass]
    public class ConcurrentPriorityQueueTest
    {


        #region Instance members


        [TestMethod]
        public void PropertyCapacity()
        {
            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>( 15 );

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
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

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
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

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
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

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
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Nothing to test here. The following explicitly passes this test:
            Assert.That( true, Is.True );
        }


        [TestMethod]
        public
        void
        ConstructorInitialSize()
        {
            // Declare a priority queue var, but don't assign it yet.
            ConcurrentPriorityQueue<int> instance;

            // Try to create a priority queue with a negative initial size and expect
            // an InvalidOperationException to be thrown.
            try {
                instance = new ConcurrentPriorityQueue<int>( -10 );
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e ) {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>( 15 );

            // Ensure that Capacity reports 15.
            System.Assert.AreEqual( instance.Capacity, 15 );
        }


        #endregion





        #region Public API


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
        public void Dequeue() {

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

            // Enqueue a few elements in the queue, and store one for later.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 3f, 6 );
            instance.Enqueue( 1f, 2 );
            instance.Add( elem );
            instance.Enqueue( 2f, 4 );

            // Test that Dequeue() returns the peak of the heap.
            Assert.That( instance.Dequeue(), Is.EqualTo( elem ) );

            // Check that Dequeue() removed the element.
            Assert.That( instance.Count, Is.EqualTo( 2 ) );
        }


        [TestMethod]
        public void DequeueValue() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Try to DequeueValue() and expect an InvalidOperationException to be thrown.
            try {
                instance.DequeueValue();
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e ) {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Enqueue a few elements in the queue.
            instance.Enqueue( 1f, 2 );
            instance.Enqueue( 3f, 6 );
            instance.Enqueue( 2f, 4 );

            // Test that DequeueValue() returns the peak of the heap.
            Assert.That( instance.DequeueValue(), Is.EqualTo( 6 ) );

            // Check that DequeueValue() removed the element.
            Assert.That( instance.Count, Is.EqualTo( 2 ) );
        }


        [TestMethod]
        public void Enqueue() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Enqueue a few elements in the queue.
            instance.Enqueue( 1f, 2 );
            instance.Enqueue( 3f, 6 );
            instance.Enqueue( 2f, 4 );

            // Ensure that the queue dequeues the elements in the correct order.
            Assert.That( instance.DequeueValue(), Is.EqualTo( 6 ) );
            Assert.That( instance.DequeueValue(), Is.EqualTo( 4 ) );
            Assert.That( instance.DequeueValue(), Is.EqualTo( 2 ) );
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
        public void Peek() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Try to Peek() and expect an InvalidOperationException to be thrown.
            try {
                instance.Peek();
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e ) {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Enqueue a few elements in the queue, and store one for later.
            KeyValuePair<float, int> elem = new KeyValuePair<float, int>( 3f, 6 );
            instance.Enqueue( 1f, 2 );
            instance.Add( elem );
            instance.Enqueue( 2f, 4 );

            // Test that Peek() returns the peak of the heap.
            Assert.That( instance.Peek(), Is.EqualTo( elem ) );

            // Check that Peek() didn't remove the element.
            Assert.That( instance.Count, Is.EqualTo( 3 ) );
        }


        [TestMethod]
        public void PeekValue() {

            // Create a new priority queue.
            ConcurrentPriorityQueue<int> instance = new ConcurrentPriorityQueue<int>();

            // Try to PeekValue() and expect an InvalidOperationException to be thrown.
            try {
                instance.PeekValue();
                Assert.Fail( "Expected exception was not thrown!" );
            }
            catch ( InvalidOperationException e ) {}
            catch ( Exception e ) {
                Assert.Fail( "Incorrect exception type thrown!" );
            }

            // Enqueue a few elements in the queue.
            instance.Enqueue( 1f, 2 );
            instance.Enqueue( 3f, 6 );
            instance.Enqueue( 2f, 4 );

            // Test that PeekValue() returns the peak of the heap.
            Assert.That( instance.PeekValue(), Is.EqualTo( 6 ) );

            // Check that PeekValue() didn't remove the element.
            Assert.That( instance.Count, Is.EqualTo( 3 ) );
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


    }
}