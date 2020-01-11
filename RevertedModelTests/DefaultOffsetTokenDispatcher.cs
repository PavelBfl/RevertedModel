using RevertedModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModelTests
{
	class DefaultOffsetTokenDispatcher : IOffsetTokenDispatcher
	{
		public int CurrentToken { get; private set; } = 0;

		public IComparable CreateToken()
		{
			return CurrentToken++;
		}
	}
}
