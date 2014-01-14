using System;
using System.Xml;

namespace Harmony.Sdk.Config
{
	/// <summary>
	/// Provides Xml parser for reading configuration sections
	/// </summary>
	[ConfigOverlaps(ConfigOverlappingAction.Allow)]
	public class XmlConfigDataProvider : IConfigSectionDataProvider
	{
		private XmlNode _node;

		public XmlConfigDataProvider(XmlNode node)
		{
			_node = node;
		}

		#region IConfigDataProvider implementation

		public T Get<T>(string key, Func<T, string> converter)
		{
			return default(T);
		}

		public T Get<T>(string key, Func<T, string> converter, T defaultValue)
		{
			return default(T);
		}

		public void Set<T>(string key, T val, Func<string, T> converter)
		{

		}

		#endregion
	}
}

