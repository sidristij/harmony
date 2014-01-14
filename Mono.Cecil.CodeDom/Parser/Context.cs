using System;
using Mono.Cecil.Cil;
using Mono.Cecil.CodeDom.Rocks;
using Mono.Cecil.CodeDom.Parser;
using System.Collections.Generic;

namespace Mono.Cecil.CodeDom
{
	/// <summary>
	/// Cecil tree modification context. Should be used for making references to target types from usings.
	/// </summary>
	public class Context
	{
		private readonly Dictionary<Instruction, CodeDomExpression> _map;

		public Context(MethodDefinition method, CodeDomParserBase parser)
		{
			Method = method;
			Parser = parser;
			_map = new Dictionary<Instruction, CodeDomExpression>();
		}

		public void SetExpression(Instruction instruction, CodeDomExpression expression)
		{
			_map[instruction] = expression;
		}

		public CodeDomExpression GetExpression(Instruction instruction)
		{
			return _map[instruction];
		}

		public ModuleDefinition Module { get { return Method.Module; } }

		public MethodDefinition Method { get; protected set; }

		public CodeDomParserBase Parser { get; protected set; }

		#region MethodRef

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef(Action method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1>(Action<T1> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2>(Action<T1, T2> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3>(Action<T1, T2, T3> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1>(Func<T1> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2>(Func<T1, T2> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3>(Func<T1, T2, T3> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4>(Func<T1, T2, T3, T4> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7> method)
		{
			return MethodRef.Of(Module, method);
		}

		/// <summary>
		/// Imports method, covered by <paramref name="method"/> parameter
		/// </summary>
		/// <param name="method">Method to be imported</param>
		public MethodReference MakeRef<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8> method)
		{
			return MethodRef.Of(Module, method);
		}

		#endregion

		#region MethodDef

		public MethodDefinition Resolve(Action method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1>(Action<T1> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2>(Action<T1, T2> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3>(Action<T1, T2, T3> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1>(Func<T1> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2>(Func<T1, T2> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3>(Func<T1, T2, T3> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4>(Func<T1, T2, T3, T4> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7> method)
		{
			return MethodDef.Of(Module, method);
		}

		public MethodDefinition Resolve<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8> method)
		{
			return MethodDef.Of(Module, method);
		}

		#endregion

		#region Typeof

		public TypeReference MakeRef<T>()
		{
			return Module.Import(typeof(T));
		}

		public TypeReference MakeRef(Type type)
		{
			return Module.Import(type);
		}

		public TypeReference MakeRef(FieldReference field)
		{
			return Module.Import(field.FieldType);
		}

		public TypeReference MakeRef(ParameterReference param)
		{
			return Module.Import(param.ParameterType);
		}

		public TypeReference MakeRef(VariableReference variable)
		{
			return Module.Import(variable.VariableType);
		}

		public TypeReference MakeRef(TypeReference type)
		{
			return Module.Import(type);
		}

		#endregion

		#region TypeDef

		public TypeDefinition Resolve(TypeReference ref_type)
		{
			return ref_type.Resolve();
		}

		public TypeDefinition Resolve<T>()
		{
			return MakeRef<T>().Resolve();
		}

		public TypeDefinition Resolve(Type type)
		{
			return MakeRef(type).Resolve();
		}

		#endregion
	}
}

