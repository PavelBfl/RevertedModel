using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockGenerators;
using RevertedModel;
using RevertedModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevertedModelTests.Collections
{
	public class CommandedCollectionUniqueTestsOfType<T> : ICommandedCollectionUniqueTests
	{
		public CommandedCollectionUniqueTestsOfType(IValueGenerator<T> valueGenerator, int count)
		{
			ValueGenerator = valueGenerator;
			Count = count;
		}

		public IValueGenerator<T> ValueGenerator { get; } = null;
		public int Count { get; } = 0;
		public TrackDispatcher CommandDispatcher { get; } = new TrackDispatcher(new DefaultOffsetTokenDispatcher());

		public void AddTest()
		{
			var originalCollection = new HashSet<T>();
			var testCollection = new TrackCollectionUnique<T>(CommandDispatcher);

			foreach (var value in ValueGenerator.Take(Count))
			{
				originalCollection.Add(value);
				testCollection.Add(value);
				Assert.AreEqual(originalCollection.Count, testCollection.Count);
			}
		}
		public void ClearTest()
		{
			var testCollection = new TrackCollectionUnique<T>(CommandDispatcher);
			foreach (var value in ValueGenerator.Take(Count))
			{
				testCollection.Add(value);
			}
			testCollection.Clear();
			Assert.IsTrue(testCollection.Count == 0);
		}
		public void ContainsTest()
		{
			var originalCollection = new HashSet<T>();
			var testCollection = new TrackCollectionUnique<T>(CommandDispatcher);

			foreach (var value in ValueGenerator.Take(Count))
			{
				originalCollection.Add(value);
				testCollection.Add(value);
			}

			foreach (var value in originalCollection.Concat(ValueGenerator.Take(Count + 1)))
			{
				Assert.AreEqual(originalCollection.Contains(value), testCollection.Contains(value));
			}
		}
		public void RemoveTest()
		{
			var originalCollection = new HashSet<T>();
			var testCollection = new TrackCollectionUnique<T>(CommandDispatcher);

			foreach (var value in ValueGenerator.Take(Count))
			{
				originalCollection.Add(value);
				testCollection.Add(value);
			}

			foreach (var value in originalCollection.ToArray())
			{
				Assert.AreEqual(originalCollection.Count, testCollection.Count);
				originalCollection.Remove(value);
				testCollection.Remove(value);
				Assert.AreEqual(originalCollection.Count, testCollection.Count);
			}
		}
	}
}
