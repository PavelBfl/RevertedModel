using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockGenerators.DoubleGenerators;
using MockGenerators.Int32Generators;
using MockGenerators.StringGenerators;
using RevertedModel;
using RevertedModelTests;
using RevertedModelTests.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevertedModel.Tests
{
	[TestClass()]
	public class TrackValueTests : TestBaseOfType<ITrackValueTests>
	{
		protected override IEnumerable<ITrackValueTests> CreateTests(int seed)
		{
			return new ITrackValueTests[]
			{
				new TrackValueTests<int>(new Int32Generator(seed).First()),
				new TrackValueTests<double>(new DoubleGenerator(seed).First()),
				new TrackValueTests<string>(new StringGenerator(seed).First()),
			};
		}

		[TestMethod()]
		public void TrackValueTest()
		{
			Testing(test => test.TrackValueConstructorTest());
		}

		[TestMethod()]
		public void TrackValueValueTest()
		{
			Testing(test => test.TrackValueValueTest());
		}
	}
}