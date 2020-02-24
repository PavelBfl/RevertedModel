using RevertedModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModelTests
{
	class OffsetTokenDispatcher : IOffsetTokenDispatcher
	{
		public int CurrentToken { get; private set; } = 0;

		public IComparable CreateToken()
		{
			return CurrentToken++;
		}
	}
}
