using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockGenerators;
using RevertedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevertedModelTests.Values
{
	class TrackValueTests<T> : ITrackValueTests
	{
		public TrackValueTests(T value)
		{
			Value = value;
		}

		public T Value { get; } = default;

		public void TrackValueConstructorTest()
		{
			var trackValue = new TrackValue<T>(Value);
			Assert.AreEqual(Value, trackValue.Value);
		}

		public void TrackValueValueTest()
		{
			var trackValue = new TrackValue<T>()
			{
				Value = Value,
			};
			Assert.AreEqual(Value, trackValue.Value);
		}
	}
}
