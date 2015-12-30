// ***********************************************************************
// Assembly         : CloudSearch.Tests.BingMapsTests
// Author           : rahulrai
// Created          : 11-28-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-16-2015
// ***********************************************************************
// <copyright file="LocationDataTests.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FindMeACab.Tests.BingMapsTests
{
    #region

    using System.Configuration;

    using FindMeACab.Basics.Kernel.KnownTypes;
    using FindMeACab.Modules.BingMapsSharedModule;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    /// <summary>
    ///     Class LocationDataTests.
    /// </summary>
    [TestClass]
    public class LocationDataTests
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Gets the bounding box coordinates test.
        /// </summary>
        [TestMethod]
        public void GetBoundingBoxCoordinatesTest()
        {
            var locationData = new LocationData(ConfigurationManager.AppSettings[ConfigurationKeys.BingMapsKey]);
            var data = locationData.GetBoundingBoxCoordinates("India").Result;
            Assert.IsTrue(null != data);

            var data1 = locationData.GetBoundingBoxCoordinates("USA").Result;
            Assert.IsTrue(null != data1);
        }

        /// <summary>
        ///     Gets the location coordinates test.
        /// </summary>
        [TestMethod]
        public void GetLocationCoordinatesTest()
        {
            var locationData = new LocationData(ConfigurationManager.AppSettings[ConfigurationKeys.BingMapsKey]);
            var data = locationData.GetLocationCoordinates("Delhi").Result;
            Assert.IsTrue(null != data);

            var data1 = locationData.GetLocationCoordinates("Hyderabad").Result;
            Assert.IsTrue(null != data1);
        }

        #endregion
    }
}