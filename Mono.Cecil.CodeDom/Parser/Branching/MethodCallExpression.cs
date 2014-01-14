using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Collections;
using Mono.Cecil.CodeDom.Rocks;

namespace Mono.Cecil.CodeDom.Parser.Branching
{
	public class MethodCallExpression : CodeDomSingleInstructionExpression
	{
		private readonly List<CodeDomExpression> _parameters = new List<CodeDomExpression>();
		public const int InstancePos = 0;
		public const int ParamsPos = 1;
		private const int MaxNodes = ParamsPos;

		private readonly MergedList<CodeDomExpression> _mergedList = new MergedList<CodeDomExpression>();
		private readonly IFixedList<CodeDomExpression> _handlersContainer = new FixedList<CodeDomExpression>(MaxNodes);

		public MethodCallExpression(Context context, Instruction position, MethodReference ref_method, CodeDomExpression exp_instance, params CodeDomExpression[] exp_methodParams)
			: base(context, position)
		{
			// check parameters count at method and at "stack"
			if (ref_method.Parameters.Count != exp_methodParams.Length)
			{
				throw new ArgumentException("ref_method.Parameters.Count != exp_methodParams");
			}

			// check params types and method params types
			for (int i = 0; i < exp_methodParams.Length; i++)
			{
				if (!exp_methodParams[i].ReturnType.HardEquals(ref_method.Parameters[i].ParameterType))
				{				
//					throw new ArgumentException(
//							string.Format("Constructor parameter #{0} and stack item have different types (parameter: {1}, stack item: {2})", 
//							i, 
//							ref_method.Parameters[i].ParameterType, 
//							exp_methodParams[i].ReturnType
//					));				
				}
			}

			// setup parents
			foreach (var exp_parameter in exp_methodParams)
			{
				exp_parameter.ParentNode = this;
				_parameters.Add(exp_parameter);
			}

			// base class
			ReadsStack = exp_methodParams.Length;
			WritesStack = ref_method.ReturnType.MetadataType == MetadataType.Void ? 0 : 1;
			ReturnType = ref_method.ReturnType;
			Nodes = _mergedList;

			// this
			IsConstructor = false;
			MethodReference = ref_method;
			Parameters = new FixedListAdapter<CodeDomExpression>(_parameters);
			InstanceExpression = exp_instance;

			IsPropertySetter = MethodReference.Parameters.Count == 1 && MethodReference.ReturnType.MetadataType == MetadataType.Void && MethodReference.Name.StartsWith("set_");
			IsPropertyGetter = MethodReference.Parameters.Count == 0 && MethodReference.ReturnType.MetadataType != MetadataType.Void && MethodReference.Name.StartsWith("get_");


			// finish merged access setup
			_mergedList.AddList(_handlersContainer);
			_mergedList.AddList(Parameters);
		}

		public bool IsConstructor { get; protected set; }

		public bool IsPropertyGetter { get; private set; }

		public bool IsPropertySetter { get; private set; }

		public CodeDomExpression InstanceExpression 
		{            
			get { return _handlersContainer[InstancePos]; }
			private set { _handlersContainer[InstancePos] = value; value.ParentNode = this; }
		}

		public MethodReference MethodReference { get; protected set; }

		public IFixedList<CodeDomExpression> Parameters { get; protected set; }

		public override string ToString()
		{
			// Is property getter or setter
			if (IsPropertyGetter || IsPropertySetter)
			{
				var name = MethodReference.Name.Substring("get_".Length);
				return MethodReference.HasThis ? 
					string.Format("{0}.{1}{2}", InstanceExpression, name, FinalString) :
					string.Format("{0}.{1}{2}", MethodReference.DeclaringType, name, FinalString);
			}

			return MethodReference.HasThis ? 
				string.Format("{0}.{1}({2}){3}", InstanceExpression, MethodReference.Name, string.Join(", ", Parameters.Reverse()), FinalString) :
				string.Format("{0}.{1}({2}){3}", MethodReference.DeclaringType, MethodReference.Name, string.Join(", ", Parameters.Reverse()), FinalString);
		}
	}
}

namespace Mono.Cecil.CodeDom.Parser
{
	using Branching;

	public static partial class CodeDom
	{
		public static MethodCallExpression MethodCall(Context context, Instruction position, MethodReference ref_method, CodeDomExpression exp_instance, params CodeDomExpression[] exp_methodParams)
		{
			return new MethodCallExpression(context, position, ref_method, exp_instance, exp_methodParams);
		}
	}
}