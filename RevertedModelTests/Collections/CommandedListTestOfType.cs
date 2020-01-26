using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockGenerators;
using MockGenerators.Int32Generators;
using RevertedModel;
using RevertedModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevertedModelTests.Collections
{
	class CommandedListTest<T> : ICommandedListTests
	{
		private const int MAX_COUNT = 1000;

		public CommandedListTest(IValueGenerator<T> itemsGenerator, int count)
		{
			ItemsGenerator = itemsGenerator ?? throw new NullReferenceException();
			Count = count >= 0 ? count : throw new ArgumentOutOfRangeException();
		}

		public IValueGenerator<T> ItemsGenerator { get; } = null;
		public CommandDispatcher CommandDispatcher { get; } = new CommandDispatcher(new DefaultOffsetTokenDispatcher());
		public int Count { get; } = 0;

		public void AddTest()
		{
			var originalList = new List<T>();
			var testList = new CommandedList<T>(CommandDispatcher);

			foreach (var value in ItemsGenerator.Take(MAX_COUNT))
			{
				originalList.Add(value);
				testList.Add(value);
				Assert.AreEqual(originalList.Count, testList.Count);
			}
		}

		public void ClearTest()
		{
			var testList = new CommandedList<T>(CommandDispatcher);
			foreach (var value in ItemsGenerator.Take(Count))
			{
				testList.Add(value);
			}
			testList.Clear();
			Assert.AreEqual(testList.Count, 0);
		}

		public void ContainsTest()
		{
			var originalList = new List<T>();
			var testList = new CommandedList<T>(CommandDispatcher);

			foreach (var value in ItemsGenerator.Take(Count))
			{
				Assert.AreEqual(originalList.Contains(value), testList.Contains(value));
				originalList.Add(value);
				testList.Add(value);
				Assert.AreEqual(originalList.Contains(value), testList.Contains(value));
			}
		}

		public void CopyToTest()
		{
			var originalList = new List<T>();
			var testList = new CommandedList<T>(CommandDispatcher);

			foreach (var value in ItemsGenerator.Take(Count))
			{
				originalList.Add(value);
				testList.Add(value);
			}

			var originalBuffer = new T[originalList.Count];
			var testBuffer = new T[testList.Count];
			originalList.CopyTo(originalBuffer, 0);
			testList.CopyTo(testBuffer, 0);
			Assert.IsTrue(Enumerable.SequenceEqual(originalBuffer, testList));
		}

		public void IndexOfTest()
		{
			var originalList = new List<T>();
			var testList = new CommandedList<T>(CommandDispatcher);

			foreach (var value in ItemsGenerator.Take(Count))
			{
				Assert.AreEqual(originalList.IndexOf(value), testList.IndexOf(value));
				originalList.Add(value);
				testList.Add(value);
				Assert.AreEqual(originalList.IndexOf(value), testList.IndexOf(value));
			}
		}

		public void RemoveAtTest()
		{
			var originalList = new List<T>();
			var testList = new CommandedList<T>(CommandDispatcher);

			foreach (var value in ItemsGenerator.Take(Count))
			{
				originalList.Add(value);
				testList.Add(value);
			}

			var indexGenerator = new Int32Generator(0);
			while (originalList.Any())
			{
				var index = indexGenerator.First() % originalList.Count;
				originalList.RemoveAt(index);
				testList.RemoveAt(index);
				Assert.IsTrue(originalList.SequenceEqual(testList));
			}
		}

		public void RemoveTest()
		{
			var originalList = new List<T>();
			var testList = new CommandedList<T>(CommandDispatcher);

			foreach (var value in ItemsGenerator.Take(Count))
			{
				originalList.Add(value);
				testList.Add(value);
			}

			foreach (var value in ItemsGenerator.Take(Count).Concat(originalList.ToArray()))
			{
				originalList.Remove(value);
				testList.Remove(value);
				Assert.IsTrue(originalList.SequenceEqual(testList));
			}
		}
	}
}
