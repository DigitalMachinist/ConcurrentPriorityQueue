using System;
using Axon.Collections;

namespace ConcurrentPriorityQueueExample
{
	class Program
	{
		static void Main( string[] args )
		{
			ConcurrentPriorityQueue<string> queue = new ConcurrentPriorityQueue<string>();
			
			queue.Enqueue( 1000.0, "This " );
			queue.Enqueue( 1000.0, "should " );
			queue.Enqueue( 1000.0, "form " );
			queue.Enqueue( 1000.0, "a " );
			queue.Enqueue( 1000.0, "complete " );
			queue.Enqueue( 1000.0, "and " );
			queue.Enqueue( 1000.0, "understandable " );
			queue.Enqueue( 1000.0, "sentence." );
			
			int numIterations = queue.Count;
			for ( int x = 0; x < numIterations; x++ )
			{
				Console.WriteLine( "ITERATION " + ( x + 1 ) );
				Console.WriteLine( "" );

				// Print out the current state of the heap
				PriorityValuePair<string>[] heapArray = new PriorityValuePair<string>[ queue.Count ];
				queue.CopyTo( heapArray, 0 );
				for ( int i = 0; i < heapArray.Length; i++ )
				{
					Console.WriteLine( heapArray[ i ].Value + ", " + heapArray[ i ].Priority );
				}

				// Dequeue the next element
				PriorityValuePair<string> dequeued = queue.Dequeue();
				Console.WriteLine( "" );
				Console.WriteLine( "DEQUEUED: " + dequeued.Value + ", " + dequeued.Priority );
				Console.WriteLine( "" );
			}

			Console.ReadLine();
		}
	}
}
