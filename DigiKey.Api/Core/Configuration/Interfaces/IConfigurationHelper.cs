using System;

namespace DigiKey.Api.Core.Configuration.Interfaces
{
    /// <summary>
    ///     Interface for System.Configuration.Configuration helper class
    /// </summary>
    public interface IConfigurationHelper
    {
        /// <summary>
        ///     Updates the value for the specified key in the AppSettings of the Config file.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Update(string key, string value);

        /// <summary>
        ///     Gets the attribute or value of the key.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>string value of attribute</returns>
        string GetAttribute(string attrName);

        /// <summary>
        ///     Gets the boolean attribute or value.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>true of false</returns>
        bool GetBooleanAttribute(string attrName);

        /// <summary>
        ///     Saves changes to the Config file
        /// </summary>
        void Save();

        /// <summary>
        ///     Refreshes the application settingses.
        /// </summary>
        void RefreshAppSettings();
    }
}
