using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mono.Cecil.CodeDom.Rocks.Cil
{
	public static class ModuleDefinitionEx
	{
		public static TypeReference Import(this ModuleDefinition self, MetadataType metadataType)
		{
			switch (metadataType)
			{
				case MetadataType.Boolean:
					return self.Import(typeof (bool));

				case MetadataType.Byte:
					return self.Import(typeof (Byte));
					
				case MetadataType.SByte:
					return self.Import(typeof (SByte));

				case MetadataType.Int16:
					return self.Import(typeof (Int16));

				case MetadataType.UInt16:
					return self.Import(typeof (UInt16));

				case MetadataType.Int32:
					return self.Import(typeof (Int32));

				case MetadataType.UInt32:
					return self.Import(typeof (UInt32));

				case MetadataType.Int64:
					return self.Import(typeof (Int64));

				case MetadataType.UInt64:
					return self.Import(typeof (UInt64));

				case MetadataType.IntPtr:
					return self.Import(typeof (IntPtr));

				case MetadataType.UIntPtr:
					return self.Import(typeof (UIntPtr));
					
				case MetadataType.Single:
					return self.Import(typeof (Single));
					
				case MetadataType.Double:
					return self.Import(typeof (Double));

				case MetadataType.Char:
					return self.Import(typeof (Char));

				case MetadataType.Array:
					return self.Import(typeof (Array));

				case MetadataType.Void:
					return self.Import(typeof (void));
			}

			return null;
		}
	}
}
