using RevertedModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModelTests
{
	class OffsetTokenDispatcher : ITrackTokenProvider
	{
		public int CurrentToken { get; private set; } = 0;

		public object CreateToken()
		{
			return CurrentToken++;
		}
	}
}
