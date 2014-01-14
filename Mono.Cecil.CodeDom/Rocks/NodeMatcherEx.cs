using System;
using System.Diagnostics;
using System.Linq;

namespace Mono.Cecil.CodeDom.Rocks
{
	internal static class NodeMatcherEx
	{
		[DebuggerStepThrough]
		public static NodeMatcherEx.NodeMatcher ToMatcher(this CodeDomExpression node)
		{
			return new NodeMatcherEx.NodeMatcher(node);
		}

		internal struct NodeMatcher
		{
			private CodeDomExpression _node;
			private bool _ismatch;

			[DebuggerStepThrough]
			public NodeMatcher(CodeDomExpression node)
			{
				this = new NodeMatcherEx.NodeMatcher();
				this._node = node;
				this._ismatch = node != null;
			}

			[DebuggerStepThrough]
			public static implicit operator bool(NodeMatcherEx.NodeMatcher matcher)
			{
				return matcher._ismatch;
			}

			public NodeMatcherEx.NodeMatcher Match<T>(out T typedNode, Predicate<T> predicate) where T : CodeDomExpression
			{
				typedNode = default (T);
				if (!this._ismatch)
					return this;
				typedNode = this._node as T;
				return (typedNode == null || predicate != null && !predicate(typedNode)) ? this.Fail() : this;
			}

			public NodeMatcherEx.NodeMatcher Match<T>(Predicate<T> predicate) where T : CodeDomExpression
			{
				T typedNode;
				return this.Match<T>(out typedNode, predicate);
			}

			public NodeMatcherEx.NodeMatcher Match<T>(out T typedNode) where T : CodeDomExpression
			{
				return this.Match<T>(out typedNode, (Predicate<T>) null);
			}

			public NodeMatcherEx.NodeMatcher Match<T>() where T : CodeDomExpression
			{
				T typedNode;
				return this.Match<T>(out typedNode, (Predicate<T>) null);
			}

			/*
			public NodeMatcherEx.NodeMatcher MatchLocalVariableReference(ILocalVariable variable)
			{
				if (!this._ismatch)
					return this;
				ILocalVariableReferenceExpression referenceExpression = this._node as ILocalVariableReferenceExpression;
				if (referenceExpression == null || referenceExpression.Variable != variable)
					return this.Fail();
				else
					return this;
			}

			public NodeMatcherEx.NodeMatcher MatchLocalVariableReference(out ILocalVariable variable)
			{
				variable = (ILocalVariable) null;
				if (!this._ismatch)
					return this;
				ILocalVariableReferenceExpression referenceExpression = this._node as ILocalVariableReferenceExpression;
				if (referenceExpression == null)
					return this.Fail();
				variable = referenceExpression.Variable;
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchParameterReference(IMethodParameter parameter)
			{
				if (!this._ismatch)
					return this;
				IParameterReferenceExpression referenceExpression = this._node as IParameterReferenceExpression;
				if (referenceExpression == null || referenceExpression.Parameter != parameter)
					return this.Fail();
				else
					return this;
			}

			public NodeMatcherEx.NodeMatcher MatchParameterReference(out IMethodParameter parameter)
			{
				parameter = (IMethodParameter) null;
				if (!this._ismatch)
					return this;
				IParameterReferenceExpression referenceExpression = this._node as IParameterReferenceExpression;
				if (referenceExpression == null)
					return this.Fail();
				parameter = referenceExpression.Parameter;
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchLiteralAgainstValue([NotNull] object value)
			{
				if (value == null)
					throw new ArgumentNullException("value");
				if (!this._ismatch)
					return this;
				ILiteralExpression literalExpression = this._node as ILiteralExpression;
				if (literalExpression == null)
					return this.Fail();
				this._ismatch = literalExpression.Value.ElementType == MetadataEx.ToElementType(value.GetType()) && object.Equals(literalExpression.Value.Value, value);
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchLiteralAgainstConstant([NotNull] Constant value)
			{
				if (value == null)
					throw new ArgumentNullException("value");
				if (!this._ismatch)
					return this;
				ILiteralExpression literalExpression = this._node as ILiteralExpression;
				if (literalExpression == null)
					return this.Fail();
				this._ismatch = literalExpression.Value.Equals(value);
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchLiteral<T>(out T value)
			{
				value = default (T);
				if (!this._ismatch)
					return this;
				ILiteralExpression literalExpression = this._node as ILiteralExpression;
				if (literalExpression == null || !literalExpression.Value.Is<T>())
					return this.Fail();
				value = literalExpression.Value.As<T>();
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchNullLiteral()
			{
				if (!this._ismatch)
					return this;
				ILiteralExpression literalExpression = this._node as ILiteralExpression;
				if (literalExpression == null || !literalExpression.Value.IsNull)
					return this.Fail();
				else
					return this;
			}

			public NodeMatcherEx.NodeMatcher MatchExpressionStatement<T>(out IExpressionStatement expressionStatement, Predicate<T> predicate) where T : class, IExpression
			{
				expressionStatement = (IExpressionStatement) null;
				if (!this._ismatch)
					return this;
				expressionStatement = this._node as IExpressionStatement;
				if (expressionStatement == null)
					return this.Fail();
				T obj = expressionStatement.Expression as T;
				if ((object) obj == null)
					return this.Fail();
				this._ismatch = predicate(obj);
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchExpressionStatement<T>(Predicate<T> predicate) where T : class, IExpression
			{
				IExpressionStatement expressionStatement;
				return this.MatchExpressionStatement<T>(out expressionStatement, predicate);
			}

			public NodeMatcherEx.NodeMatcher MatchExpressionStatement<T>(out T expression, Predicate<T> predicate) where T : class, IExpression
			{
				expression = default (T);
				if (!this._ismatch)
					return this;
				IExpressionStatement expressionStatement = this._node as IExpressionStatement;
				if (expressionStatement == null)
					return this.Fail();
				expression = expressionStatement.Expression as T;
				if ((object) expression == null || predicate != null && !predicate(expression))
					return this.Fail();
				else
					return this;
			}

			public NodeMatcherEx.NodeMatcher MatchExpressionStatement<T>(out T expression) where T : class, IExpression
			{
				return this.MatchExpressionStatement<T>(out expression, (Predicate<T>) null);
			}

			public NodeMatcherEx.NodeMatcher MatchTypeCast<T>(bool isMandatory, IMetadataType expectedType, out ITypeCastExpression typeCast, Predicate<T> predicate) where T : class, IExpression
			{
				typeCast = (ITypeCastExpression) null;
				if (!this._ismatch)
					return this;
				typeCast = this._node as ITypeCastExpression;
				INode node;
				if (typeCast != null && (expectedType == null || MetadataTypeEx.IsEqualTo(typeCast.TargetType, expectedType)))
				{
					node = (INode) typeCast.Argument;
				}
				else
				{
					if (isMandatory)
						return this.Fail();
					node = this._node;
				}
				T obj = node as T;
				if ((object) obj == null || predicate != null && !predicate(obj))
					return this.Fail();
				else
					return this;
			}

			public NodeMatcherEx.NodeMatcher MatchTypeCast<T>(bool isMandatory, IMetadataType expectedType, Predicate<T> predicate) where T : class, IExpression
			{
				ITypeCastExpression typeCast;
				return this.MatchTypeCast<T>(isMandatory, expectedType, out typeCast, predicate);
			}

			public NodeMatcherEx.NodeMatcher MatchTypeCast<T>(bool isMandatory, out ITypeCastExpression typeCast, Predicate<T> predicate) where T : class, IExpression
			{
				return this.MatchTypeCast<T>(isMandatory, (IMetadataType) null, out typeCast, predicate);
			}

			public NodeMatcherEx.NodeMatcher MatchTypeCast<T>(bool isMandatory, IMetadataType expectedType, out T expression, Predicate<T> predicate) where T : class, IExpression
			{
				expression = default (T);
				if (!this._ismatch)
					return this;
				ITypeCastExpression typeCastExpression = this._node as ITypeCastExpression;
				INode node;
				if (typeCastExpression != null && (expectedType == null || MetadataTypeEx.IsEqualTo(typeCastExpression.TargetType, expectedType)))
				{
					node = (INode) typeCastExpression.Argument;
				}
				else
				{
					if (isMandatory)
						return this.Fail();
					node = this._node;
				}
				expression = node as T;
				if ((object) expression == null || predicate != null && !predicate(expression))
					return this.Fail();
				else
					return this;
			}

			public NodeMatcherEx.NodeMatcher MatchTypeCast<T>(bool isMandatory, out T expression, Predicate<T> predicate) where T : class, IExpression
			{
				return this.MatchTypeCast<T>(isMandatory, (IMetadataType) null, out expression, predicate);
			}

			public NodeMatcherEx.NodeMatcher MatchTypeCast<T>(bool isMandatory, IMetadataType expectedType, out T expression) where T : class, IExpression
			{
				return this.MatchTypeCast<T>(isMandatory, expectedType, out expression, (Predicate<T>) null);
			}

			public NodeMatcherEx.NodeMatcher MatchTypeCast<T>(bool isMandatory, IMetadataType expectedType, out ITypeCastExpression typeCast, out T expression) where T : class, IExpression
			{
				typeCast = (ITypeCastExpression) null;
				expression = default (T);
				if (!this._ismatch)
					return this;
				typeCast = this._node as ITypeCastExpression;
				INode node;
				if (typeCast != null && (expectedType == null || MetadataTypeEx.IsEqualTo(typeCast.TargetType, expectedType)))
				{
					node = (INode) typeCast.Argument;
				}
				else
				{
					if (isMandatory)
						return this.Fail();
					node = this._node;
				}
				expression = node as T;
				if ((object) expression == null)
					return this.Fail();
				else
					return this;
			}

			public NodeMatcherEx.NodeMatcher MatchTypeCast<T>(bool isMandatory, out ITypeCastExpression typeCast, out T expression) where T : class, IExpression
			{
				return this.MatchTypeCast<T>(isMandatory, (IMetadataType) null, out typeCast, out expression);
			}

			public NodeMatcherEx.NodeMatcher MatchBox<T>(bool isMandatory, IMetadataType type, out T expression, Predicate<T> predicate) where T : class, IExpression
			{
				expression = default (T);
				if (!this._ismatch)
					return this;
				IBoxExpression boxExpression = this._node as IBoxExpression;
				INode node;
				if (boxExpression != null && MetadataTypeEx.IsEqualTo(boxExpression.SourceType, type))
				{
					node = (INode) boxExpression.Argument;
				}
				else
				{
					if (isMandatory)
						return this.Fail();
					node = this._node;
				}
				expression = node as T;
				if ((object) expression == null || predicate != null && !predicate(expression))
					return this.Fail();
				else
					return this;
			}

			public NodeMatcherEx.NodeMatcher MatchBox<T>(bool isMandatory, IMetadataType type, out T expression) where T : class, IExpression
			{
				return this.MatchBox<T>(isMandatory, type, out expression, (Predicate<T>) null);
			}

			public NodeMatcherEx.NodeMatcher MatchBox<T>(bool isMandatory, IMetadataType type, Predicate<T> predicate) where T : class, IExpression
			{
				T expression;
				return this.MatchBox<T>(isMandatory, type, out expression, predicate);
			}

			public NodeMatcherEx.NodeMatcher MatchBox()
			{
				if (!this._ismatch)
					return this;
				IBoxExpression boxExpression = this._node as IBoxExpression;
				if (boxExpression == null)
					return this.Fail();
				this._node = (INode) boxExpression.Argument;
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchRef()
			{
				if (!this._ismatch)
					return this;
				IRefExpression refExpression = this._node as IRefExpression;
				if (refExpression == null)
					return this.Fail();
				this._node = (INode) refExpression.Argument;
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchDeref()
			{
				if (!this._ismatch)
					return this;
				IDerefExpression derefExpression = this._node as IDerefExpression;
				if (derefExpression == null)
					return this.Fail();
				this._node = (INode) derefExpression.Argument;
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchOptionalNegation(out bool negate)
			{
				negate = false;
				if (!this._ismatch)
					return this;
				IUnaryOperationExpression operationExpression = this._node as IUnaryOperationExpression;
				if (operationExpression == null || operationExpression.OperationType != OperationType.LogicalNeg)
					return this;
				negate = true;
				this._node = (INode) operationExpression.Argument;
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchEqualityComparison<T>(out bool negate, Predicate<T> predicate) where T : class, IAbstractBinaryOperationExpression
			{
				this = this.MatchOptionalNegation(out negate);
				if (!this._ismatch)
					return this;
				T obj = this._node as T;
				if ((object) obj == null || !OperationTypeEx.IsAnyOf(obj.OperationType, OperationType.Equal, OperationType.NotEqual))
					return this.Fail();
				// ISSUE: explicit reference operation
				// ISSUE: variable of a reference type
				bool& local = @negate;
				// ISSUE: explicit reference operation
				int num = ^local ^ obj.OperationType == OperationType.NotEqual ? 1 : 0;
				// ISSUE: explicit reference operation
				^local = num != 0;
				if (!predicate(obj))
					return this.Fail();
				else
					return this;
			}
			*/

			public NodeMatcherEx.NodeMatcher MatchNullRightSibling()
			{
				if (!_ismatch)
					return this;
				_ismatch = _node.GetRightSibling() == null;
				return this;
			}

			public NodeMatcherEx.NodeMatcher MatchNullLeftSibling()
			{
				if (!_ismatch)
					return this;
				_ismatch = _node.GetLeftSibling() == null;
				return this;
			}

			public NodeMatcherEx.NodeMatcher ToParent()
			{
				if (!_ismatch)
					return this;

				if (_node.ParentNode == null)
					return Fail();

				_node = _node.ParentNode;
				return this;
			}

			public NodeMatcherEx.NodeMatcher ToLeftSibling()
			{
				if (!_ismatch)
					return this;

				var leftSibling = _node.GetLeftSibling();
				if (leftSibling == null)
					return Fail();

				_node = leftSibling;
				return this;
			}

			public NodeMatcherEx.NodeMatcher ToRightSibling()
			{
				if (!_ismatch)
					return this;

				var rightSibling = _node.GetRightSibling();

				if (rightSibling == null)
					return Fail();

				_node = rightSibling;
				return this;
			}

			public NodeMatcherEx.NodeMatcher ToFirstStatement()
			{
				if (!_ismatch)
					return this;

				var blockStatement = _node as CodeDomGroupExpression;
				if (blockStatement == null)
					return this;

				var first = blockStatement.Nodes.FirstOrDefault();
				if (first == null)
					return Fail();

				_node = first;
				return this;
			}

			public NodeMatcherEx.NodeMatcher ToLastStatement()
			{
				if (!_ismatch)
					return this;

				var blockStatement = _node as CodeDomGroupExpression;
				if (blockStatement == null)
					return this;

				var last = blockStatement.Nodes.LastOrDefault();
				if (last == null)
					return Fail();

				_node = last;
				return this;
			}

			public NodeMatcherEx.NodeMatcher ToOnlyStatement()
			{
				if (!_ismatch)
					return this;

				var blockStatement = _node as CodeDomGroupExpression;
				if (blockStatement == null)
					return this;

				if (blockStatement.Nodes.Count != 1)
					return Fail();

				_node = blockStatement.Nodes.First();
				return this;
			}

			private NodeMatcherEx.NodeMatcher Fail()
			{
				_ismatch = false;
				return this;
			}
		}
	}
}
