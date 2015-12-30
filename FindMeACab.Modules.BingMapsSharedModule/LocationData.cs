// ***********************************************************************
// Assembly         : CloudSearch.Modules.BingMapsSharedModule
// Author           : rahulrai
// Created          : 12-14-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-16-2015
// ***********************************************************************
// <copyright file="LocationData.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FindMeACab.Modules.BingMapsSharedModule
{
    #region

    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;

    using Newtonsoft.Json.Linq;

    #endregion

    /// <summary>
    /// Class LocationData.
    /// </summary>
    public class LocationData
    {
        #region Constants

        /// <summary>
        /// The bing maps base URI
        /// </summary>
        private const string BingMapsBaseUri = "http://dev.virtualearth.net/REST/v1/Locations/{0}?maxResults=1&key={1}";

        #endregion

        #region Static Fields

        /// <summary>
        /// The bing maps key
        /// </summary>
        private readonly string bingMapsKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationData"/> class.
        /// </summary>
        /// <param name="bingMapsKey">The bing maps key.</param>
        public LocationData(string bingMapsKey)
        {
            this.bingMapsKey = bingMapsKey;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the bounding box coordinates.
        /// </summary>
        /// <param name="locationName">Name of the location.</param>
        /// <returns>Task&lt;List&lt;System.Double&gt;&gt;.</returns>
        public async Task<List<double>> GetBoundingBoxCoordinates(string locationName)
        {
            var encodedLocation = HttpUtility.UrlEncode(locationName);
            var requestUri = string.Format(BingMapsBaseUri, encodedLocation, this.bingMapsKey);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(requestUri);
            dynamic data = JObject.Parse(content);
            return data.resourceSets[0].resources[0].bbox.ToObject<List<double>>();
        }

        /// <summary>
        /// Gets the location coordinates.
        /// </summary>
        /// <param name="locationName">Name of the location.</param>
        /// <returns>Task&lt;List&lt;System.String&gt;&gt;.</returns>
        public async Task<List<string>> GetLocationCoordinates(string locationName)
        {
            var encodedLocation = HttpUtility.UrlEncode(locationName);
            var requestUri = string.Format(BingMapsBaseUri, encodedLocation, this.bingMapsKey);
            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync(requestUri);
            dynamic data = JObject.Parse(content);
            return data.resourceSets[0].resources[0].geocodePoints[0].coordinates.ToObject<List<string>>();
        }

        #endregion
    }
}