// ***********************************************************************
// Assembly         : FindMeACab.Tests.Shared
// Author           : rahulrai
// Created          : 12-18-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-18-2015
// ***********************************************************************
// <copyright file="VehicleCoordinates.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FindMeACab.Tests.Shared
{
    /// <summary>
    ///     Class VehicleCoordinates.
    /// </summary>
    public class VehicleCoordinates
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        public double LatitudeMax { get; set; }

        public double LatitudeMin { get; set; }

        /// <summary>
        ///     Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        public double LongitudeMax { get; set; }

        public double LongitudeMin { get; set; }

        /// <summary>
        ///     Gets or sets the registration number.
        /// </summary>
        /// <value>The registration number.</value>
        public string RegistrationNumber { get; set; }

        #endregion
    }
}