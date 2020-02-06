using MockGenerators.Int32Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RevertedModelTests
{
	public abstract class TestBaseOfType<T>
	{
		private const int DEFAULT_ROOT_SEED = 13;
		private const int TESTS_COUNT = 10;

		public TestBaseOfType()
		{
			var tests = new List<T>();
			foreach (var seed in new Int32Generator(DEFAULT_ROOT_SEED).Take(TESTS_COUNT))
			{
				tests.AddRange(CreateTests(seed));
			}
			Tests = tests;
		}

		protected abstract IEnumerable<T> CreateTests(int seed);

		private IEnumerable<T> Tests { get; } = null;
		protected void Testing(Action<T> testExecute)
		{
			foreach (var test in Tests)
			{
				testExecute(test);
			}
		}
	}
}
