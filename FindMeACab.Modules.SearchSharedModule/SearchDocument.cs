// ***********************************************************************
// Assembly         : CloudSearch.Modules.SearchSharedModule
// Author           : rahulrai
// Created          : 12-14-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-16-2015
// ***********************************************************************
// <copyright file="SearchDocument.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FindMeACab.Modules.SearchSharedModule
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    using FindMeACab.Basics.Model;

    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;

    #endregion

    /// <summary>
    ///     Class SearchDocument.
    /// </summary>
    public class SearchDocument : SearchBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SearchBase" /> class.
        /// </summary>
        /// <param name="searchServiceName">Name of the search service.</param>
        /// <param name="apiKey">The API key.</param>
        public SearchDocument(string searchServiceName, string apiKey)
            : base(searchServiceName, apiKey)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Filters the text for location bounds.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="boundingbox">The boundingbox.</param>
        /// <returns>System.String.</returns>
        public static string FilterTextForLocationBounds(string fieldName, List<double> boundingbox)
        {
            var minLatitude = boundingbox[0];
            var minLongitude = boundingbox[1];
            var maxLatitude = boundingbox[2];
            var maxLongitude = boundingbox[3];
            var filter =
                $"geo.intersects({fieldName}, geography'POLYGON(({maxLongitude} {minLatitude}, {maxLongitude} {maxLatitude}, {minLongitude} {maxLatitude}, {minLongitude} {minLatitude}, {maxLongitude} {minLatitude}))')";
            return filter;
        }

        /// <summary>
        ///     Searches the documents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchText">The search text.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>IList&lt;T&gt;.</returns>
        public IList<T> SearchDocuments<T>(string searchText, string filter = null) where T : class
        {
            var sp = new SearchParameters();
            if (!string.IsNullOrEmpty(filter))
            {
                sp.Filter = filter;
            }

            var response = this.IndexClient.Documents.Search<T>(searchText, sp);

            return response.Select(result => (result.Document)).ToList();
        }

        /// <summary>
        ///     Uploads the documents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documents">The documents.</param>
        public void UploadDocuments<T>(IEnumerable<T> documents) where T : class
        {
            try
            {
                this.IndexClient.Documents.Index(
                    IndexBatch.Create(documents.Select<T, IndexAction<T>>(IndexAction.Create)));
            }
            catch (IndexBatchException e)
            {
            }
        }

        /// <summary>
        /// Filters the text for distance from point.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="locationPoint">The location point.</param>
        /// <param name="distanceFromPoint">The distance from point.</param>
        /// <returns>System.String.</returns>
        public static string FilterTextForDistanceFromPoint(string fieldName, LocationPoint locationPoint, int distanceFromPoint)
        {
            var filter = $"geo.distance({fieldName}, geography'POINT({locationPoint.Longitude} {locationPoint.Latitude})') le {distanceFromPoint}";
            return filter;
        }

        #endregion
    }
}