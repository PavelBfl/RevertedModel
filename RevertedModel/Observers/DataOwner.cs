using System;
using System.Collections.Generic;
using System.Text;

namespace RevertedModel.Observers
{
	public static class DataOwner
	{
		public static IDataOwner<TParams, TValue> Create<TParams, TValue>(Func<TParams, TValue> getter, Action<TParams, TValue> setter)
		{
			return new DataOwnerDelegate<TParams, TValue>(getter, setter);
		}

		private class DataOwnerDelegate<TParams, TValue> : IDataOwner<TParams, TValue>
		{
			public DataOwnerDelegate(Func<TParams, TValue> getter, Action<TParams, TValue> setter)
			{
				Getter = getter ?? throw new ArgumentNullException(nameof(getter));
				Setter = setter ?? throw new ArgumentNullException(nameof(setter));
			}

			private Func<TParams, TValue> Getter { get; }
			private Action<TParams, TValue> Setter { get; }

			public TValue GetData(TParams @params) => Getter(@params);

			public void SetData(TParams @params, TValue value) => Setter(@params, value);
		}
	}
}
