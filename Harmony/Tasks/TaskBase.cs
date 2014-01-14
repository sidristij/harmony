using System;

namespace Harmony.Sdk
{
	/// <summary>
	/// TaskBase class is base class for pipe task. 
	/// </summary>
	public abstract class TaskBase
	{                
		public String Name { get; private set; }

		public bool AllowsRunInParallel { get; protected set; }

		protected TaskBase(String name)
		{
			Name = name;
			AllowsRunInParallel = false;
		}

		public abstract void Run();

		public override string ToString()
		{
			return Name;
		}
	}
}

