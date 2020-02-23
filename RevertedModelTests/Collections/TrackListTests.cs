using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevertedModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevertedModel.Collections.Tests
{
	[TestClass()]
	public class TrackListTests
	{
		private const int UNDEFINED_INDEX = -1;
		private const int MISSING_VALUE = -1;

		private static TrackDispatcher CustomDispatcher()
		{
			return new TrackDispatcher(new TokenOffsetDispatcher());
		}

		[TestMethod()]
		public void TrackList_EmptyConstructor_DefaultDispatcher()
		{
			var list = new TrackList<object>();

			Assert.AreEqual(list.TrackDispatcher, TrackDispatcher.Default);
		}

		[TestMethod()]
		public void TrackList_InitDispatcher_InitiateDispatcher()
		{
			var dispatcher = CustomDispatcher();
			var list = new TrackList<object>(dispatcher);

			Assert.AreEqual(list.TrackDispatcher, dispatcher);
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TrackList_NullDispatcher_ArgumentNullException()
		{
			new TrackList<object>((TrackDispatcher)null);
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4 }, DisplayName = "Values")]
		[DataRow(new int[] { }, DisplayName = "Empty")]
		public void TrackList_InitItems_InitiateItems(int[] items)
		{
			var list = new TrackList<int>(items);

			Assert.IsTrue(list.SequenceEqual(items));
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TrackList_NullItems_ArgumentNullException()
		{
			new TrackList<int>((IEnumerable<int>)null);
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4 }, DisplayName = "Values")]
		public void IndexOf_ExistsValue_Index(int[] values)
		{
			var list = new TrackList<int>(values);
			for (int i = 0; i < list.Count; i++)
			{
				var value = list[i];
				var index = list.IndexOf(value);
				Assert.AreEqual(index, i);
			}
		}
		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4 }, DisplayName = "Values")]
		[DataRow(new int[] { }, DisplayName = "Empty")]
		public void IndexOf_NotExistsValue_UndefinedIndex(int[] values)
		{
			var list = new TrackList<int>(values);

			var index = list.IndexOf(MISSING_VALUE);
			Assert.AreEqual(index, UNDEFINED_INDEX);
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3 }, 0, DisplayName = "First")]
		[DataRow(new int[] { 0, 1, 2, 3 }, 2, DisplayName = "Any")]
		[DataRow(new int[] { 0, 1, 2, 3 }, 3, DisplayName = "Last")]
		public void RemoveAt_ContainsIndex_WithoutItem(int[] items, int removetIndex)
		{
			var originalList = new List<int>(items);
			var list = new TrackList<int>(items);

			originalList.RemoveAt(removetIndex);
			list.RemoveAt(removetIndex);
			Assert.IsTrue(list.SequenceEqual(originalList));
		}

		[TestMethod()]
		[DataRow(new[] { 0, 1, 2, 3 }, MISSING_VALUE)]
		[DataRow(new[] { 0, 1, 2, 3 }, 5)]
		[DataRow(new[] { 0, 1, 2, 3 }, 100)]
		[DataRow(new int[] { }, MISSING_VALUE)]
		[DataRow(new int[] { }, 1)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RemoveAt_MissingIndex_ArgumentOutOfRangeException(int[] items, int removetIndex)
		{
			var list = new TrackList<int>(items);
			list.RemoveAt(removetIndex);
		}

		[TestMethod()]
		[DataRow(new[] { 0, 1, 2, 3, 4, 4 })]
		public void Add_InitItem_ContainsItem(int[] items)
		{
			var list = new TrackList<int>();
			for (int i = 0; i < items.Length; i++)
			{
				var addedValue = items[i];
				list.Add(addedValue);
				var containsValue = list[list.Count - 1];
				Assert.AreEqual(addedValue, containsValue);
			}
		}

		[TestMethod()]
		public void Clear_Empty_CountZero()
		{
			var list = new TrackList<int>();

			list.Clear();

			Assert.AreEqual(list.Count, 0);
		}

		[TestMethod()]
		[DataRow(new[] { 0, 1, 2, 3, 4 })]
		[DataRow(new int[] { })]
		public void Clear_InitItems_CountZero(int[] items)
		{
			var list = new TrackList<int>(items);

			list.Clear();

			Assert.AreEqual(list.Count, 0);
		}

		[TestMethod()]
		[DataRow(new[] { 0, 1, 2, 3, 4, 5 }, 0, DisplayName = "First")]
		[DataRow(new[] { 0, 1, 2, 3, 4, 5 }, 3, DisplayName = "Any")]
		[DataRow(new[] { 0, 1, 2, 3, 4, 5 }, 5, DisplayName = "Last")]
		public void Contains_InitItems_Contains(int[] items, int containItem)
		{
			var list = new TrackList<int>(items);
			Assert.IsTrue(list.Contains(containItem));
		}
		[TestMethod()]
		[DataRow(new[] { 0, 1, 2, 3, 4, 5 }, -1, DisplayName = "Before")]
		[DataRow(new[] { 0, 1, 2, 3, 4, 5 }, 7, DisplayName = "After")]
		[DataRow(new int[] { }, -1, DisplayName = "Empty Before")]
		[DataRow(new int[] { }, 0, DisplayName = "Empty After")]
		public void Contains_InitItems_NotContains(int[] items, int containItem)
		{
			var list = new TrackList<int>(items);
			Assert.IsFalse(list.Contains(containItem));
		}

		[TestMethod()]
		[DataRow(new[] { 0, 1, 2, 3, 4 }, 0, new[] { 0, 0, 0, 0, 0 })]
		[DataRow(new[] { 0, 1, 2, 3, 4 }, 2, new[] { 0, 0, 0, 0, 0, 0, 0 })]
		[DataRow(new[] { 0, 1, 2, 3, 4 }, 2, new[] { 0, 0, 0, 0, 0, 0, 0, 0 })]
		[DataRow(new int[] { }, 0, new int[] { })]
		public void CopyTo_InitItems_InitiateBuffer(int[] items, int arrayIndex, int[] buffer)
		{
			var list = new TrackList<int>(items);

			list.CopyTo(buffer, arrayIndex);
			Assert.IsTrue(list.SequenceEqual(buffer.Skip(arrayIndex).Take(list.Count)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CopyTo_EmptyItems_NullBuffer()
		{
			var list = new TrackList<int>();
			list.CopyTo(null, 0);
		}


		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4 }, 0, DisplayName = "First")]
		[DataRow(new int[] { 0, 1, 2, 3, 4 }, 2, DisplayName = "Any")]
		[DataRow(new int[] { 0, 1, 2, 3, 4 }, 4, DisplayName = "Last")]
		public void Remove_ContainsItem_Success(int[] items, int value)
		{
			var list = new TrackList<int>(items);
			var result = list.Remove(value);
			Assert.IsTrue(result);
		}

		[TestMethod()]
		[DataRow(new int[] { 0, 1, 2, 3, 4 }, -1)]
		[DataRow(new int[] { }, -1)]
		public void Remove_NotContainsItem_Fail(int[] items, int value)
		{
			var list = new TrackList<int>(items);
			var result = list.Remove(value);
			Assert.IsFalse(result);
		}

		private class TokenOffsetDispatcher : IOffsetTokenDispatcher
		{
			public int CurretTokken { get; private set; } = 0;

			public IComparable CreateToken()
			{
				return CurretTokken++;
			}
		}
	}
}