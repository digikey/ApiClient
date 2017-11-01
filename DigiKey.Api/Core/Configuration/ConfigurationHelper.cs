﻿using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using DigiKey.Api.Core.Configuration.Interfaces;

namespace DigiKey.Api.Core.Configuration
{
    /// <summary>
    ///     Helper classes that wrapps up working with System.Configuration.Configuration
    /// </summary>
    /// <seealso cref="IConfigurationHelper" />
    [ExcludeFromCodeCoverage]
    public class ConfigurationHelper : IConfigurationHelper
    {
        /// <summary>
        ///     This object represents the config file
        /// </summary>
        protected System.Configuration.Configuration _config;

        /// <summary>
        ///     Updates the value for the specified key in the AppSettings of the Config file.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Update(string key, string value)
        {
            if (_config.AppSettings.Settings[key] == null)
            {
                _config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                _config.AppSettings.Settings[key].Value = value;
            }
        }

        /// <summary>
        ///     Gets the attribute or value of the key.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>string value of attribute</returns>
        public string GetAttribute(string attrName)
        {
            try
            {
                return _config.AppSettings.Settings[attrName] == null
                    ? null
                    : _config.AppSettings.Settings[attrName].Value;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        /// <summary>
        ///     Gets the boolean attribute or value.
        /// </summary>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>true of false</returns>
        public bool GetBooleanAttribute(string attrName)
        {
            try
            {
                var value = GetAttribute(attrName);
                return value != null && Convert.ToBoolean(value);
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        public void Save()
        {
            _config.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        ///     Refreshes the application settingses.
        /// </summary>
        public void RefreshAppSettings()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
