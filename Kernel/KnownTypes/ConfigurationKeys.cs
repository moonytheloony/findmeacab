// ***********************************************************************
// Assembly         : CloudSearch.Basics.Kernel
// Author           : rahulrai
// Created          : 12-14-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-16-2015
// ***********************************************************************
// <copyright file="ConfigurationKeys.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Kernel.KnownTypes
{
    /// <summary>
    /// Class ConfigurationKeys.
    /// </summary>
    public static class ConfigurationKeys
    {
        #region Constants

        /// <summary>
        /// The event hub connection string
        /// </summary>
        public const string EventHubConnectionString = nameof(EventHubConnectionString);

        /// <summary>
        /// The event hub name
        /// </summary>
        public const string EventHubName = nameof(EventHubName);

        /// <summary>
        /// The bing maps key
        /// </summary>
        public const string BingMapsKey = nameof(BingMapsKey);

        /// <summary>
        /// The search service name
        /// </summary>
        public const string SearchServiceName = nameof(SearchServiceName);

        /// <summary>
        /// The search service key
        /// </summary>
        public const string SearchServiceKey = nameof(SearchServiceKey);

        public const string IsEnabled = nameof(IsEnabled);

        #endregion
    }
}