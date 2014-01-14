using System.Collections.Generic;

namespace Mono.Cecil.CodeDom
{
	public interface ICodeDomEditableGroup
	{
		List<CodeDomExpression> EditableNodes { get; }
	}
}

