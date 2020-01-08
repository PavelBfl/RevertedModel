using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevertedModel.Collections
{
	public abstract class CommandedCollection<T> : CommandedObject
	{
		public CommandedCollection(CommandDispatcher commandDispatcher)
			: base(commandDispatcher)
		{

		}

	}
}
