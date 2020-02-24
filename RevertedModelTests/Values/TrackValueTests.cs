using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevertedModel;
using RevertedModelTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Tests
{
	[TestClass()]
	public class TrackValueTests
	{
		[TestMethod()]
		public void TrackValue_Default_DefaultDispatcher()
		{
			var value = new TrackValue<int>();

			Assert.AreEqual(value.TrackDispatcher, TrackDispatcher.Default);
		}

		[TestMethod()]
		public void TrackValue_InitDispatcher_InitiateDispatcher()
		{
			var dispatcher = new TrackDispatcher(new OffsetTokenDispatcher());
			var value = new TrackValue<int>(dispatcher);

			Assert.AreEqual(value.TrackDispatcher, dispatcher);
		}

		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TrackValue_InitDispatcher_ArgumentNullException()
		{
			new TrackValue<int>(null);
		}

		[TestMethod()]
		[DataRow(0)]
		public void TrackValue_InitValue_InitiateValue(int value)
		{
			var trackTalue = new TrackValue<int>(value);

			Assert.AreEqual(trackTalue.Value, value);
		}
		[TestMethod()]
		[DataRow(0)]
		public void TrackValue_InitValue_DefaultDispatcher(int value)
		{
			var trackTalue = new TrackValue<int>(value);

			Assert.AreEqual(trackTalue.TrackDispatcher, TrackDispatcher.Default);
		}
		[TestMethod()]
		[DataRow(0)]
		public void TrackValue_InitValueAndDispatcher_InitiateValue(int value)
		{
			var dispatcher = new TrackDispatcher(new OffsetTokenDispatcher());
			var trackTalue = new TrackValue<int>(value, dispatcher);

			Assert.AreEqual(trackTalue.Value, value);
		}
		[TestMethod()]
		[DataRow(0)]
		public void TrackValue_InitValueAndDispatcher_InitiateDispatcher(int value)
		{
			var dispatcher = new TrackDispatcher(new OffsetTokenDispatcher());
			var trackTalue = new TrackValue<int>(value, dispatcher);

			Assert.AreEqual(trackTalue.TrackDispatcher, dispatcher);
		}
		[TestMethod()]
		[DataRow(0)]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TrackValue_InitValueAndNullDispatcher_ArgumentNullException(int value)
		{
			new TrackValue<int>(value, null);
		}

		[TestMethod()]
		[DataRow(0)]
		public void PropertyValue_SetValue_InitiateValue(int value)
		{
			var trackValue = new TrackValue<int>();

			trackValue.Value = value;
			Assert.AreEqual(trackValue.Value, value);
		}
	}
}