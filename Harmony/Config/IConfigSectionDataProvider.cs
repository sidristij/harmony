using System;

namespace Harmony.Sdk.Config
{
	/// <summary>
	/// Provides parser-side configuration representation
	/// </summary>
	public interface IConfigSectionDataProvider
	{
		/// <summary>
		/// Gets configuration entry by key and converts to destination type by given function
		/// </summary>
		/// <exception cref="System.ArgumentOutOfRangeException">Throws when key not found</exception>
		T Get<T>(string key, Func<T, string> converter);

		/// <summary>
		/// Gets configuration entry by key and converts to destination type by given function.
		/// If key not found, default value will be returned
		/// </summary>
		T Get<T>(string key, Func<T, string> converter, T defaultValue);

		/// <summary>
		/// Sets value by key using converter
		/// </summary>
		void Set<T>(string key, T val, Func<string, T> converter);
	}
}

