using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevertedModel.Collections;
using RevertedModelTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Collections.Tests
{
	[TestClass()]
	public class TrackDictionaryTests
	{
		private static TrackDictionary<T, T> CreateKeys<T>(IEnumerable<T> keys)
		{
			var result = new TrackDictionary<T, T>();
			foreach (var key in keys)
			{
				result.Add(key, default);
			}
			return result;
		}
		private static TrackDictionary<T, T> CreatePairs<T>(IEnumerable<T> pairs)
		{
			var result = new TrackDictionary<T, T>();
			foreach (var pair in pairs)
			{
				result.Add(pair, pair);
			}
			return result;
		}

		[TestMethod()]
		public void TrackDictionary_Default_DefaultDispatcher()
		{
			var dictionary = new TrackDictionary<int, int>();

			Assert.AreEqual(dictionary.TrackDispatcher, TrackDispatcher.Default);
		}
		[TestMethod()]
		public void TrackDictionary_Default_EmptyItems()
		{
			var dictionary = new TrackDictionary<int, int>();

			Assert.AreEqual(dictionary.Count, 0);
		}

		[TestMethod()]
		public void TrackDictionaryTest_InitDispatcher_InitiateDispatcher()
		{
			var dispatcher = new TrackDispatcher(new OffsetTokenDispatcher());
			var dictionary = new TrackDictionary<int, int>(dispatcher);

			Assert.AreEqual(dictionary.TrackDispatcher, dispatcher);
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TrackDictionaryTest_InitNullDispatcher_ArgumentNullException()
		{
			new TrackDictionary<int, int>(null);
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 })]
		public void ContainsKey_ExistsKey_True(int[] keys)
		{
			var dictionary = CreateKeys(keys);

			foreach (var key in keys)
			{
				Assert.IsTrue(dictionary.ContainsKey(key));
			}
		}
		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 })]
		public void ContainsKey_NotExistsKey_False(int[] keys)
		{
			var dictionary = new TrackDictionary<int, int>();

			foreach (var key in keys)
			{
				Assert.IsFalse(dictionary.ContainsKey(key));
				dictionary.Add(key, 0);
			}
		}
		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 }, -1)]
		[DataRow(new int[] { }, -1)]
		public void ContainsKey_NotExistsTargetKey_False(int[] keys, int excludeKey)
		{
			var dictionary = CreateKeys(keys);

			Assert.IsFalse(dictionary.ContainsKey(excludeKey));
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 })]
		public void Add_InitPair_IncrementCount(int[] keys)
		{
			var dictionary = new TrackDictionary<int, int>();

			for (int i = 0; i < keys.Length; i++)
			{
				var key = keys[i];
				dictionary.Add(key, 0);
				Assert.AreEqual(dictionary.Count, i + 1);
			}
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 0 })]
		[DataRow(new int[] { 0, 1, 1 })]
		[ExpectedException(typeof(ArgumentException))]
		public void Add_InitItems_ArgumentException(int[] keys)
		{
			var dictionary = new TrackDictionary<int, int>();

			for (int i = 0; i < keys.Length; i++)
			{
				var key = keys[i];
				dictionary.Add(key, default);
			}
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 })]
		public void TryGetValue_GetExstsValue_True(int[] keys)
		{
			var dictionary = CreateKeys(keys);

			foreach (var key in keys)
			{
				Assert.IsTrue(dictionary.TryGetValue(key, out _));
			}
		}
		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 })]
		public void TryGetValue_GetExstsValue_Value(int[] pairs)
		{
			var dictionary = CreatePairs(pairs);

			foreach (var pair in pairs)
			{
				dictionary.TryGetValue(pair, out var value);

				Assert.AreEqual(pair, value);
			}
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 })]
		[DataRow(new int[] { })]
		public void Clear_InitItems_CountZero(int[] keys)
		{
			var dictionary = CreateKeys(keys);

			dictionary.Clear();

			Assert.AreEqual(dictionary.Count, 0);
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 })]
		public void Remove_RemoveUncludeKeys_True(int[] keys)
		{
			var dictionary = CreateKeys(keys);

			foreach (var key in keys)
			{
				var result = dictionary.Remove(key);
				Assert.IsTrue(result);
			}
		}
		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4, 5 }, -1)]
		[DataRow(new int[] { }, -1)]
		public void Remove_RemoveExcludeKeys_False(int[] keys, int excludeKey)
		{
			var dictionary = CreateKeys(keys);

			Assert.IsFalse(dictionary.Remove(excludeKey));
		}
	}
}