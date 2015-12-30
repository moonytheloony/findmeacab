// ***********************************************************************
// Assembly         : CloudSearch.Modules.SearchSharedModule
// Author           : rahulrai
// Created          : 12-14-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-16-2015
// ***********************************************************************
// <copyright file="SearchIndex.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace SearchSharedModule
{
    #region

    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;

    #endregion

    /// <summary>
    /// Class SearchIndex.
    /// </summary>
    public class SearchIndex : SearchBase
    {
        #region Fields

        /// <summary>
        /// The service client
        /// </summary>
        private readonly SearchServiceClient serviceClient;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchIndex" /> class.
        /// </summary>
        /// <param name="searchServiceName">Name of the search service.</param>
        /// <param name="searchServiceKey">The search service key.</param>
        public SearchIndex(string searchServiceName, string searchServiceKey)
            : base(searchServiceName, searchServiceKey)
        {
            this.serviceClient = this.SearchServiceClient;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates the index if not exists.
        /// </summary>
        /// <param name="indexToCreate">The index to create.</param>
        public void CreateIndexIfNotExists(Index indexToCreate)
        {
            if (!this.serviceClient.Indexes.Exists(indexToCreate.Name))
            {
                this.serviceClient.Indexes.Create(indexToCreate);
            }
        }

        /// <summary>
        /// Deletes the index if exists.
        /// </summary>
        /// <param name="indexName">Name of the index.</param>
        public void DeleteIndexIfExists(string indexName)
        {
            if (this.serviceClient.Indexes.Exists(indexName))
            {
                this.serviceClient.Indexes.Delete(indexName);
            }
        }

        #endregion
    }
}