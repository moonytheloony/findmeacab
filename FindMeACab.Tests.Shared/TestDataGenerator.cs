// ***********************************************************************
// Assembly         : FindMeACab.Tests.Shared
// Author           : rahulrai
// Created          : 12-18-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-18-2015
// ***********************************************************************
// <copyright file="TestDataGenerator.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FindMeACab.Tests.Shared
{
    #region

    using System.Collections.Generic;
    using System.Configuration;

    using FindMeACab.Basics.Kernel.KnownTypes;
    using FindMeACab.Modules.BingMapsSharedModule;

    #endregion

    /// <summary>
    /// Class TestDataGenerator.
    /// </summary>
    public static class TestDataGenerator
    {
        #region Static Fields

        /// <summary>
        /// The vehicle and location
        /// </summary>
        private static readonly Dictionary<string, string> VehicleAndLocation;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="TestDataGenerator"/> class.
        /// </summary>
        static TestDataGenerator()
        {
            VehicleAndLocation = new Dictionary<string, string>
                                     {
                                         { "DelhiCar1", "Delhi, India" },
                                         { "DelhiCar2", "Delhi, India" },
                                         { "GurgaonCar1", "Gurgaon, Haryana, India" },
                                         { "GurgaonCar2", "Gurgaon, Haryana, India" },
                                         { "HyderabadCar1", "Hyderabad, Telangana, India" },
                                         { "HyderabadCar2", "Hyderabad, Telangana, India" }
                                     };
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the cars and seed position.
        /// </summary>
        /// <returns>IEnumerable&lt;VehicleCoordinates&gt;.</returns>
        public static IEnumerable<VehicleCoordinates> GetCarsAndSeedPosition()
        {
            var vehicleCoordinates = new List<VehicleCoordinates>();
            var locationData = new LocationData(ConfigurationManager.AppSettings[ConfigurationKeys.BingMapsKey]);
            foreach (var vehicle in VehicleAndLocation)
            {
                var boundBox = locationData.GetBoundingBoxCoordinates(vehicle.Value).Result;
                var vehicleToAdd = new VehicleCoordinates
                {
                    RegistrationNumber = vehicle.Key,
                    LatitudeMax = boundBox[2],
                    LongitudeMax = boundBox[3]
                };
                vehicleToAdd.Latitude = vehicleToAdd.LatitudeMin = boundBox[0];
                vehicleToAdd.Longitude = vehicleToAdd.LongitudeMin = boundBox[1];
                vehicleCoordinates.Add(vehicleToAdd);
            }

            return vehicleCoordinates;
        }

        #endregion
    }
}