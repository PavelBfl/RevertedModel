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
	class CommandedDictionaryTestOfType<TKey, TValue> : ICommandedDictionaryTest
	{
		public CommandedDictionaryTestOfType(IValueGenerator<TKey> keyGenerator, IValueGenerator<TValue> valueGenerator, int count)
		{
			KeyGenerator = keyGenerator;
			ValueGenerator = valueGenerator;
			Count = count;
		}

		public IValueGenerator<TKey> KeyGenerator { get; } = null;
		public IValueGenerator<TValue> ValueGenerator { get; } = null;
		public TrackDispatcher CommandDispatcher { get; } = new TrackDispatcher(new DefaultOffsetTokenDispatcher());
		public int Count { get; } = 0;

		private IEnumerable<KeyValuePair<TKey, TValue>> GetPairs()
		{
			return KeyGenerator
				.Zip(
					ValueGenerator,
					(key, value) => KeyValuePair.Create(key, value)
				).Take(Count);
		}

		public void CommandedDictionaryTest()
		{
			var testDictionary = new TrackDictionary<TKey, TValue>(CommandDispatcher);

			Assert.IsTrue(testDictionary.Count == 0);
		}

		public void ContainsKeyTest()
		{
			var originalDictionary = new Dictionary<TKey, TValue>();
			var testDictionary = new TrackDictionary<TKey, TValue>(CommandDispatcher);
			foreach (var pair in GetPairs())
			{
				Assert.AreEqual(originalDictionary.ContainsKey(pair.Key), testDictionary.ContainsKey(pair.Key));
				originalDictionary.Add(pair.Key, pair.Value);
				testDictionary.Add(pair.Key, pair.Value);
				Assert.AreEqual(originalDictionary.ContainsKey(pair.Key), testDictionary.ContainsKey(pair.Key));
			}
		}

		public void AddTest()
		{
			var originalDictionary = new Dictionary<TKey, TValue>();
			var testDictionary = new TrackDictionary<TKey, TValue>(CommandDispatcher);
			foreach (var pair in GetPairs())
			{
				originalDictionary.Add(pair.Key, pair.Value);
				testDictionary.Add(pair.Key, pair.Value);
				Assert.AreEqual(originalDictionary.Count, testDictionary.Count);
			}
		}

		public void TryGetValueTest()
		{
			var originalDictionary = new Dictionary<TKey, TValue>();
			var testDictionary = new TrackDictionary<TKey, TValue>(CommandDispatcher);
			foreach (var pair in GetPairs())
			{
				Assert.AreEqual(
					originalDictionary.TryGetValue(pair.Key, out var originalValue),
					testDictionary.TryGetValue(pair.Key, out var testValue)
				);
				Assert.AreEqual(originalValue, testValue);

				originalDictionary.Add(pair.Key, pair.Value);
				testDictionary.Add(pair.Key, pair.Value);

				Assert.AreEqual(
					originalDictionary.TryGetValue(pair.Key, out originalValue),
					testDictionary.TryGetValue(pair.Key, out testValue)
				);
				Assert.AreEqual(originalValue, testValue);
			}
		}

		public void ClearTest()
		{
			var testDictionary = new TrackDictionary<TKey, TValue>(CommandDispatcher);
			foreach (var pair in GetPairs())
			{
				testDictionary.Add(pair.Key, pair.Value);
			}
			testDictionary.Clear();
			Assert.IsTrue(testDictionary.Count == 0);
		}

		public void RemoveTest()
		{
			var originalDictionary = new Dictionary<TKey, TValue>();
			var testDictionary = new TrackDictionary<TKey, TValue>(CommandDispatcher);
			foreach (var pair in GetPairs())
			{
				originalDictionary.Add(pair.Key, pair.Value);
				testDictionary.Add(pair.Key, pair.Value);
			}

			foreach (var key in KeyGenerator.Take(Count).Concat(originalDictionary.Keys))
			{
				originalDictionary.Remove(key);
				testDictionary.Remove(key);
				Assert.AreEqual(originalDictionary.Count, testDictionary.Count);
			}
		}
	}
}
