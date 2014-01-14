using System;

namespace Mono.Cecil.CodeDom
{
	internal static class TypeSwitchEx
	{
		public static ActionSwitch<TBase> TypeSwitch<TBase>([NotNull] this TBase target) where TBase : class
		{
			if (target == null)
				throw new ArgumentNullException("target");

			return new ActionSwitch<TBase>(target);
		}

		public static FuncSwitch<TBase, TResult> TypeSwitch<TBase, TResult>([NotNull] this TBase target) where TBase : class
		{
			if (target == null)
				throw new ArgumentNullException("target");

			return new FuncSwitch<TBase, TResult>(target);
		}

		public struct ActionSwitch<TBase> where TBase : class
		{
			private TBase myTarget;

			public ActionSwitch(TBase target)
			{
				myTarget = target;
			}

			public ActionSwitch<TBase> Case<TConcrete>(Action<TConcrete> action) where TConcrete : class, TBase
			{
				if (myTarget != null)
				{
					var concrete = myTarget as TConcrete;
					if (concrete != null)
					{
						action(concrete);
						myTarget = default (TBase);
					}
				}
				return this;
			}

			public void Default(Action<TBase> action)
			{
				if (myTarget == null)
					return;
				action(myTarget);
				myTarget = default (TBase);
			}

			public void EnsureHandled()
			{
				if (myTarget != null)
					throw new ArgumentException(string.Format("Unexpected type: {0}.", myTarget.GetType()));
			}
		}

		public struct FuncSwitch<TBase, TResult> where TBase : class
		{
			private TBase myTarget;
			private TResult myResult;

			public FuncSwitch(TBase target)
			{
				this = new FuncSwitch<TBase, TResult>();
				myTarget = target;
			}

			public FuncSwitch<TBase, TResult> Case<TConcrete>(Func<TConcrete, TResult> func) where TConcrete : class, TBase
			{
				if (myTarget != null)
				{
					var concrete = myTarget as TConcrete;
					if (concrete != null)
					{
						myResult = func(concrete);
						myTarget = default (TBase);
					}
				}
				return this;
			}

			public ResultHolder<TResult> Default(Func<TBase, TResult> func)
			{
				if (myTarget != null)
				{
					myResult = func(myTarget);
					myTarget = default (TBase);
				}
				return new ResultHolder<TResult>(myResult);
			}

			public TResult ToResult()
			{
				if (myTarget != null)
					throw new ArgumentException(string.Format("Unexpected type: {0}.", myTarget.GetType()));

				return myResult;
			}
		}

		public struct ResultHolder<TResult>
		{
			private readonly TResult myResult;

			public ResultHolder(TResult result)
			{
				myResult = result;
			}

			public TResult ToResult()
			{
				return myResult;
			}
		}
	}
}

