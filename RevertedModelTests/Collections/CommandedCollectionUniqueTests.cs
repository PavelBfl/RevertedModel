using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockGenerators.DoubleGenerators;
using MockGenerators.Int32Generators;
using MockGenerators.StringGenerators;
using RevertedModel.Collections;
using RevertedModelTests;
using RevertedModelTests.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Collections.Tests
{
	[TestClass()]
	public class CommandedCollectionUniqueTests : CollectionTestBase<ICommandedCollectionUniqueTests>
	{
		protected override IEnumerable<ICommandedCollectionUniqueTests> CreateTests(int count)
		{
			return new ICommandedCollectionUniqueTests[]
			{
				new CommandedCollectionUniqueTestsOfType<int>(new Int32Generator(count), count),
				new CommandedCollectionUniqueTestsOfType<double>(new DoubleGenerator(count), count),
				new CommandedCollectionUniqueTestsOfType<string>(new StringGenerator(count), count),
			};
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
		public void RemoveTest()
		{
			Testing(test => test.RemoveTest());
		}
		[TestMethod()]
		public void ContainsTest()
		{
			Testing(test => test.ContainsTest());
		}
	}
}