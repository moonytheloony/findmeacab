// ***********************************************************************
// Assembly         : CloudSearch.Tests.SearchTests
// Author           : rahulrai
// Created          : 12-02-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-16-2015
// ***********************************************************************
// <copyright file="SearchDocumentTests.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FindMeACab.Tests.SearchTests
{
    using System.Configuration;
    using System.Linq;

    using FindMeACab.Basics.Kernel.KnownTypes;
    using FindMeACab.Basics.Model;
    using FindMeACab.Modules.BingMapsSharedModule;
    using FindMeACab.Modules.SearchSharedModule;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class SearchDocumentTests.
    /// </summary>
    [TestClass()]
    public class SearchDocumentTests
    {
        /// <summary>
        /// Searches the documents test.
        /// </summary>
        [TestMethod()]
        public void SearchDocumentsTest()
        {
            var locationData = new LocationData(ConfigurationManager.AppSettings[ConfigurationKeys.BingMapsKey]);
            var searchServiceName = ConfigurationManager.AppSettings[ConfigurationKeys.SearchServiceName];
            var searchServiceKey = ConfigurationManager.AppSettings[ConfigurationKeys.SearchServiceKey];
            var searchClient = new SearchDocument(searchServiceName, searchServiceKey);
            var data = locationData.GetBoundingBoxCoordinates("Delhi,India").Result;
            var searchResult = searchClient.SearchDocuments<GpsSensorRecord>("*", SearchDocument.FilterTextForLocationBounds("geoCoordinates", data));
            var locationPoint = new LocationPoint { Latitude = 28.644979476928711, Longitude = 77.2164306640625 };
            var searchByDistance = searchClient.SearchDocuments<GpsSensorRecord>("*", SearchDocument.FilterTextForDistanceFromPoint("geoCoordinates", locationPoint, 100));
            Assert.IsTrue(searchResult.Any());
            Assert.IsTrue(searchByDistance.Any());

            var dataUsa = locationData.GetBoundingBoxCoordinates("Washington,USA").Result;
            var searchResultUsa = searchClient.SearchDocuments<GpsSensorRecord>("*", SearchDocument.FilterTextForLocationBounds("geoCoordinates", dataUsa));
            Assert.IsTrue(searchResultUsa.Any());
        }
    }
}