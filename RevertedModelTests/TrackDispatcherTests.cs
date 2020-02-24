using Microsoft.VisualStudio.TestTools.UnitTesting;
using RevertedModel;
using RevertedModelTests;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Tests
{
	[TestClass()]
	public class TrackDispatcherTests
	{
		[TestMethod()]
		public void TrackDispatcher_InitOffsetTokenDispatcher_InitiateOffsetTokenDispatcher()
		{
			var offsetTokenDispatcher = new OffsetTokenDispatcher();
			var dispatcher = new TrackDispatcher(offsetTokenDispatcher);

			Assert.AreEqual(dispatcher.OffsetTokenDispatcher, offsetTokenDispatcher);
		}
		[TestMethod()]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TrackDispatcher_InitNullOffsetTokenDispatcher_ArgumentNullException()
		{
			new TrackDispatcher(null);
		}

		[TestMethod()]
		public void Disable_Disable_TokenNotNull()
		{
			var dispatcher = new TrackDispatcher(new OffsetTokenDispatcher());

			using (var disableToken = dispatcher.Disable())
			{
				Assert.IsNotNull(disableToken);
			}
		}
		[TestMethod()]
		public void Disable_Disable_IsEnableFalse()
		{
			var dispatcher = new TrackDispatcher(new OffsetTokenDispatcher());

			using (dispatcher.Disable())
			{
				Assert.IsFalse(dispatcher.IsEnable);
			}
		}

		[TestMethod()]
		public void Offset_RollBackStart_BaseValue()
		{
			const int BASE_VALUE = 0;
			const int CHANGE_VALUE = 1;

			var offsetTokenDispatcher = new OffsetTokenDispatcher();
			var offsetToken = offsetTokenDispatcher.CurrentToken;
			var dispatcher = new TrackDispatcher(offsetTokenDispatcher);

			var trackValue = new TrackValue<int>(BASE_VALUE, dispatcher);
			trackValue.Value = CHANGE_VALUE;

			dispatcher.Offset(offsetToken);

			Assert.AreEqual(trackValue.Value, BASE_VALUE);
		}
	}
}