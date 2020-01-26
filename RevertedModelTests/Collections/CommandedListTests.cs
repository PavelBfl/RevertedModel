using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockGenerators.DoubleGenerators;
using MockGenerators.Int32Generators;
using MockGenerators.StringGenerators;
using RevertedModel.Collections;
using RevertedModelTests;
using RevertedModelTests.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RevertedModel.Collections.Tests
{
	[TestClass()]
	public class CommandedListTests : CollectionTestBase<ICommandedListTests>
	{
		protected override IEnumerable<ICommandedListTests> CreateTests(int count)
		{
			return new ICommandedListTests[]
			{
				new CommandedListTest<int>(new Int32Generator(count), count),
				new CommandedListTest<double>(new DoubleGenerator(count), count),
				new CommandedListTest<string>(new StringGenerator(count), count),
			};
		}

		[TestMethod()]
		public void IndexOfTest()
		{
			Testing(test => test.IndexOfTest());
		}

		[TestMethod()]
		public void RemoveAtTest()
		{
			Testing(test => test.RemoveAtTest());
		}

		[TestMethod()]
		public void AddTest()
		{
			Testing(test => test.AddTest());
		}

		[TestMethod()]
		public void ClearTest()
		{
			Testing(test => test.ClearTest());
		}

		[TestMethod()]
		public void ContainsTest()
		{
			Testing(test => test.ContainsTest());
		}

		[TestMethod()]
		public void CopyToTest()
		{
			Testing(test => test.CopyToTest());
		}

		[TestMethod()]
		public void RemoveTest()
		{
			Testing(test => test.RemoveTest());
		}
	}
}