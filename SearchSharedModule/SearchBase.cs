// ***********************************************************************
// Assembly         : CloudSearch.Modules.SearchSharedModule
// Author           : rahulrai
// Created          : 12-14-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-16-2015
// ***********************************************************************
// <copyright file="SearchBase.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace SearchSharedModule
{
    #region

    using Kernel.KnownTypes;

    using Microsoft.Azure.Search;

    #endregion

    /// <summary>
    /// Class SearchBase.
    /// </summary>
    public abstract class SearchBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBase"/> class.
        /// </summary>
        /// <param name="searchServiceName">Name of the search service.</param>
        /// <param name="apiKey">The API key.</param>
        protected SearchBase(string searchServiceName, string apiKey)
        {
            this.SearchServiceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));

            this.IndexClient = this.SearchServiceClient.Indexes.GetClient(KeyNames.IndexName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the index client.
        /// </summary>
        /// <value>The index client.</value>
        protected SearchIndexClient IndexClient { get; }

        /// <summary>
        /// Gets the search service client.
        /// </summary>
        /// <value>The search service client.</value>
        protected SearchServiceClient SearchServiceClient { get; }

        #endregion
    }
}