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
	public class CommandedDictionaryTests : CollectionTestBase<ICommandedDictionaryTest>
	{
		protected override IEnumerable<ICommandedDictionaryTest> CreateTests(int count)
		{
			var int32Generator = new Int32Generator(count);
			var doubleGenerator = new DoubleGenerator(count);
			var stringGenerator = new StringGenerator(count);
			var int32UnicueGenerator = new Int32UniqueGenerator(int32Generator);
			var doubleUnicueGenerator = new DoubleUniqueGenerator(doubleGenerator, new DoubleComparer(0.00001));
			var stringunicueGenerator = new StringUniqueGenerator(stringGenerator);
			return new ICommandedDictionaryTest[]
			{
				new CommandedDictionaryTestOfType<int, int>(int32UnicueGenerator, int32Generator, count),
				new CommandedDictionaryTestOfType<int, double>(int32UnicueGenerator, doubleGenerator, count),
				new CommandedDictionaryTestOfType<int, string>(int32UnicueGenerator, stringGenerator, count),
				new CommandedDictionaryTestOfType<double, int>(doubleUnicueGenerator, int32Generator, count),
				new CommandedDictionaryTestOfType<double, double>(doubleUnicueGenerator, doubleGenerator, count),
				new CommandedDictionaryTestOfType<double, string>(doubleUnicueGenerator, stringGenerator, count),
				new CommandedDictionaryTestOfType<string, int>(stringunicueGenerator, int32Generator, count),
				new CommandedDictionaryTestOfType<string, double>(stringunicueGenerator, doubleGenerator, count),
				new CommandedDictionaryTestOfType<string, string>(stringunicueGenerator, stringGenerator, count),
			};
		}

		[TestMethod()]
		public void CommandedDictionaryTest()
		{
			Testing(test => test.CommandedDictionaryTest());
		}

		[TestMethod()]
		public void ContainsKeyTest()
		{
			Testing(test => test.ContainsKeyTest());
		}

		[TestMethod()]
		public void AddTest()
		{
			Testing(test => test.AddTest());
		}

		[TestMethod()]
		public void TryGetValueTest()
		{
			Testing(test => test.TryGetValueTest());
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
	}
}