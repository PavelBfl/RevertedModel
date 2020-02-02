using MockGenerators.DoubleGenerators;
using MockGenerators.Int32Generators;
using MockGenerators.StringGenerators;
using RevertedModelTests.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevertedModelTests
{
	public abstract class CollectionTestBase<T>
	{
		protected const int MIN_COUNT = 1;
		protected const int MAX_COUNT = 100;
		protected const int TESTS_COUNT = 10;

		public CollectionTestBase()
		{
			var tests = new List<T>();
			tests.AddRange(CreateTests(0));

			var countGenerator = new Int32RangeGenerator(13, MIN_COUNT, MAX_COUNT);
			foreach (var count in countGenerator.Take(TESTS_COUNT))
			{
				tests.AddRange(CreateTests(count));
			}
			Tests = tests;
		}

		protected IEnumerable<T> Tests { get; } = null;

		protected abstract IEnumerable<T> CreateTests(int count);

		protected void Testing(Action<T> testExecute)
		{
			var index = 0;
			foreach (var test in Tests)
			{
				testExecute(test);
				index++;
			}
		}
	}
}
