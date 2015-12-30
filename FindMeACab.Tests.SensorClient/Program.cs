// ***********************************************************************
// Assembly         : FindMeACab.Tests.SensorClient
// Author           : rahulrai
// Created          : 11-24-2015
//
// Last Modified By : rahulrai
// Last Modified On : 11-29-2015
// ***********************************************************************
// <copyright file="Program.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FindMeACab.Tests.SensorClient
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Text;
    using System.Threading;

    using FindMeACab.Basics.Kernel.KnownTypes;
    using FindMeACab.Basics.Model;
    using FindMeACab.Modules.SearchSharedModule;
    using FindMeACab.Tests.Shared;

    using Microsoft.ServiceBus.Messaging;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    ///     Class Program.
    /// </summary>
    internal static class Program
    {
        #region Static Fields

        /// <summary>
        ///     The connection string
        /// </summary>
        private static readonly string ConnectionString =
            ConfigurationManager.AppSettings[ConfigurationKeys.EventHubConnectionString];

        /// <summary>
        ///     The event hub name
        /// </summary>
        private static readonly string EventHubName = ConfigurationManager.AppSettings[ConfigurationKeys.EventHubName];

        #endregion

        #region Methods

        /// <summary>
        ///     Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            while (!bool.Parse(ConfigurationManager.AppSettings[ConfigurationKeys.IsEnabled]))
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }

            SendPositionDataToHub();
        }

        /// <summary>
        ///     Sending the random messages.
        /// </summary>
        private static void SendPositionDataToHub()
        {
            var carsAndSeedPosition = TestDataGenerator.GetCarsAndSeedPosition();
            while (bool.Parse(ConfigurationManager.AppSettings[ConfigurationKeys.IsEnabled]))
            {
                MoveCars(carsAndSeedPosition);
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        private static void MoveCars(IEnumerable<VehicleCoordinates> carsAndSeedPosition)
        {
            var randomiser = new Random();
            var allowedIncrements = new List<double> { 0.010000, 0.020000, 0.030000, 0.040000 };
            var eventHubClient = EventHubClient.CreateFromConnectionString(ConnectionString, EventHubName);
            while (bool.Parse(ConfigurationManager.AppSettings[ConfigurationKeys.IsEnabled]))
            {
                foreach (var car in carsAndSeedPosition)
                {
                    if (!bool.Parse(ConfigurationManager.AppSettings[ConfigurationKeys.IsEnabled]))
                    {
                        return;
                    }

                    var moveBy = allowedIncrements[randomiser.Next(allowedIncrements.Count)];
                    if (car.Latitude + moveBy > car.LatitudeMax || car.Longitude + moveBy > car.LongitudeMax)
                    {
                        //// The car has traversed the diagonal. Moving it back to initial position.
                        car.Latitude = car.LatitudeMin;
                        car.Longitude = car.LongitudeMin;
                    }
                    else
                    {
                        car.Latitude += moveBy;
                        car.Longitude += moveBy;
                    }

                    var gpsMessage =
                        JsonConvert.SerializeObject(
                            new GpsSensorRecord(
                                car.RegistrationNumber,
                                new
                                {
                                    type = "Point",
                                    coordinates =
                                            new[] { Convert.ToDouble(car.Longitude), Convert.ToDouble(car.Latitude) }
                                }));
                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, gpsMessage);
                    eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(gpsMessage)));
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
            }
        }

        #endregion
    }
}