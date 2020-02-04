using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Exceptions
{
	public class OffsetTokenLessCurrentExeption : TrackModelExeption
	{
		private const string MESSAGE = "Созданый токен сдвига меньше текущего";

		public OffsetTokenLessCurrentExeption()
			: base(MESSAGE)
		{

		}
	}
}
