// ***********************************************************************
// Assembly         : FindMeACab.Basics.Model
// Author           : rahulrai
// Created          : 11-26-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-18-2015
// ***********************************************************************
// <copyright file="GpsSensorRecord.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FindMeACab.Basics.Model
{
    /// <summary>
    ///     Class GpsSensorRecord.
    /// </summary>
    public class GpsSensorRecord
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GpsSensorRecord"/> class.
        /// </summary>
        /// <param name="vehicleId">The vehicle identifier.</param>
        /// <param name="geoCoordinates">The geo coordinates.</param>
        public GpsSensorRecord(string vehicleId, dynamic geoCoordinates)
        {
            this.VehicleId = vehicleId;
            this.GeoCoordinates = geoCoordinates;
            this.IsDeleted = false.ToString();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the geo coordinates.
        /// </summary>
        /// <value>The geo coordinates.</value>
        public dynamic GeoCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the is deleted.
        /// </summary>
        /// <value>The is deleted.</value>
        public string IsDeleted { get; set; }

        /// <summary>
        ///     Gets or sets the vehicle identifier.
        /// </summary>
        /// <value>The vehicle identifier.</value>
        public string VehicleId { get; set; }

        #endregion
    }
}