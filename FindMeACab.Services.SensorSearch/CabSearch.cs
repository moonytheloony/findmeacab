// ***********************************************************************
// Assembly         : CabSearch
// Author           : rahulrai
// Created          : 11-23-2015
//
// Last Modified By : rahulrai
// Last Modified On : 12-21-2015
// ***********************************************************************
// <copyright file="CabSearch.cs" company="Rahul Rai">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace CabSearch
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Windows.ApplicationModel;
    using Windows.ApplicationModel.AppService;
    using Windows.ApplicationModel.Background;
    using Windows.ApplicationModel.VoiceCommands;
    using Windows.Devices.Geolocation;

    using FindMeACab.Basics.Model;
    using FindMeACab.Modules.BingMapsSharedModule;
    using FindMeACab.Modules.SearchSharedModule;

    #endregion

    /// <summary>
    /// Class CabSearch. This class cannot be inherited.
    /// </summary>
    public sealed class CabSearch : IBackgroundTask
    {
        #region Constants

        /// <summary>
        /// The bing API key
        /// </summary>
        private const string BingApiKey =
            "jDUmcyOiPXX8FZrvz4Z0~c4XBE-mSyv1-_X3xELgh1Q~AkvQ2mTw1MgGZXTCg8GZgwFOs2uewxR3r6ykIsUoEJ0HeXZ4vaAuJELO3LCM6mSm";

        /// <summary>
        /// The search service key
        /// </summary>
        private const string SearchServiceKey = "29DBEB81ABBDA405EBDDD895636EAAB7";

        /// <summary>
        /// The search service name
        /// </summary>
        private const string SearchServiceName = "findmeacab";

        #endregion

        #region Fields

        /// <summary>
        /// The deferral
        /// </summary>
        private BackgroundTaskDeferral deferral;

        /// <summary>
        /// The search client
        /// </summary>
        private SearchDocument searchClient;

        /// <summary>
        /// The voice command service connection
        /// </summary>
        private VoiceCommandServiceConnection voiceCommandServiceConnection;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Runs the specified task instance.
        /// </summary>
        /// <param name="taskInstance">The task instance.</param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                this.searchClient = new SearchDocument(SearchServiceName, SearchServiceKey);
            }
            catch (Exception ex)
            {
                await this.SendErrorMessageAsync(ex.Message);
                return;
            }

            this.deferral = taskInstance.GetDeferral();

            taskInstance.Canceled += (sender, reason) => this.deferral?.Complete();

            var triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

            if (triggerDetails?.Name != "CabSearch")
            {
                return;
            }

            this.voiceCommandServiceConnection =
                VoiceCommandServiceConnection.FromAppServiceTriggerDetails(triggerDetails);
            this.voiceCommandServiceConnection.VoiceCommandCompleted += (sender, args) => this.deferral?.Complete();
            var voicecommand = await this.voiceCommandServiceConnection.GetVoiceCommandAsync();

            switch (voicecommand.CommandName)
            {
                case "findCabInArea":
                    var area = voicecommand.Properties["area"][0];
                    await this.SendProgressMessageAsync($"Searching for cars in {area}");
                    await this.SearchCabsInArea(area);
                    break;
                case "findCabNearby":
                    await this.SendProgressMessageAsync($"Searching for cabs in 100 km radius.");
                    await this.SearchCabsNearby();
                    break;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches the cabs in area.
        /// </summary>
        /// <param name="area">The area.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private async Task SearchCabsInArea(string area)
        {
            var locationData = new LocationData(BingApiKey).GetBoundingBoxCoordinates($"{area},India").Result;
            var searchResult = this.searchClient.SearchDocuments<GpsSensorRecord>(
                "*",
                SearchDocument.FilterTextForLocationBounds("geoCoordinates", locationData));
            if (!searchResult.Any())
            {
                await this.SendErrorMessageAsync("No cabs available");
                return;
            }

            var tilelist = searchResult.Select(result => new VoiceCommandContentTile { ContentTileType = VoiceCommandContentTileType.TitleOnly, Title = result.VehicleId }).ToList();
            var successmessage = new VoiceCommandUserMessage();
            successmessage.DisplayMessage = successmessage.SpokenMessage = $"Found the following cabs in {area}...";
            var response = VoiceCommandResponse.CreateResponse(successmessage, tilelist);
            await this.voiceCommandServiceConnection.ReportSuccessAsync(response);
        }

        /// <summary>
        /// Searches the cabs nearby.
        /// </summary>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private async Task SearchCabsNearby()
        {
            var geolocator = new Geolocator();
            var pos = await geolocator.GetGeopositionAsync().AsTask();
            var locationPoint = new LocationPoint
            {
                Latitude = pos.Coordinate.Point.Position.Latitude,
                Longitude = pos.Coordinate.Point.Position.Longitude
            };
            var searchResult = this.searchClient.SearchDocuments<GpsSensorRecord>(
                "*",
                SearchDocument.FilterTextForDistanceFromPoint("geoCoordinates", locationPoint, 100));
            if (!searchResult.Any())
            {
                await this.SendErrorMessageAsync("No cabs available");
                return;
            }

            var tilelist = searchResult.Select(result => new VoiceCommandContentTile { ContentTileType = VoiceCommandContentTileType.TitleOnly, Title = result.VehicleId }).ToList();
            var successmessage = new VoiceCommandUserMessage();
            successmessage.DisplayMessage = successmessage.SpokenMessage = "Found the following cabs near you...";
            var response = VoiceCommandResponse.CreateResponse(successmessage, tilelist);
            await this.voiceCommandServiceConnection.ReportSuccessAsync(response);
        }

        /// <summary>
        /// Sends the error message asynchronous.
        /// </summary>
        /// <param name="errortext">The errortext.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private async Task SendErrorMessageAsync(string errortext)
        {
            var errormessage = new VoiceCommandUserMessage();
            errormessage.DisplayMessage = errormessage.SpokenMessage = errortext;
            var response = VoiceCommandResponse.CreateResponse(errormessage);
            await this.voiceCommandServiceConnection.ReportFailureAsync(response);
        }

        /// <summary>
        /// Sends the progress message asynchronous.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>System.Threading.Tasks.Task.</returns>
        private async Task SendProgressMessageAsync(string message)
        {
            var progressmessage = new VoiceCommandUserMessage();
            progressmessage.DisplayMessage = progressmessage.SpokenMessage = message;
            var response = VoiceCommandResponse.CreateResponse(progressmessage);
            await this.voiceCommandServiceConnection.ReportProgressAsync(response);
        }

        #endregion
    }
}