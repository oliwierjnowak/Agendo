using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Legacy;
using NUnitTesting.Server.ApplicationFactories;
using Agendo.Shared.DTOs;
using Newtonsoft.Json;


namespace NUnitTesting.Server.controller
{
    [TestFixture]
    internal class DailyScheduleControllerTest : PlaywrightTest
    {
        private IAPIRequestContext Request = null;

        private async Task CreateAPIRequestContext(ApplicationFactory _applicationFactory, string token)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + token);

            Request = await this.Playwright.APIRequest.NewContextAsync(new()
            {
                // All requests we send go to this API endpoint.
                BaseURL = _applicationFactory.ServerAddress,
                ExtraHTTPHeaders = headers
            });
        }

        private async Task CreateAPIRequestContext719(ApplicationFactory _applicationFactory)
        {
            string token =
                "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRW1tZXR0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiNzE5IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwOS8wOS9pZGVudGl0eS9jbGFpbXMvYWN0b3IiOiIxIiwiZXhwIjoyNTI0NjA0NDAwfQ.VeJRyuaJz2vcSfReKuQ8xYCM7Z4U4TrpC-JDT4KvrX5UDhmy0vgEpZX_i2JgMz6r8KdI2Y5aeUWvJbaG-HtJtA";
            await CreateAPIRequestContext(_applicationFactory, token);
        }



        [Test]
        [TestCase(
            "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRW1tZXR0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiNzE5IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwOS8wOS9pZGVudGl0eS9jbGFpbXMvYWN0b3IiOiIxIiwiZXhwIjoyNTI0NjA0NDAwfQ.VeJRyuaJz2vcSfReKuQ8xYCM7Z4U4TrpC-JDT4KvrX5UDhmy0vgEpZX_i2JgMz6r8KdI2Y5aeUWvJbaG-HtJtA")]
        [TestCase(
            "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiS2V0dGVsIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMTAwMCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDkvMDkvaWRlbnRpdHkvY2xhaW1zL2FjdG9yIjoiMiIsImV4cCI6NDg2Mjk5OTE0Nn0.0TmxtzHnxvqreb2GX2sX04zAqSqCrtB33Ftt9n1fL1HlP1q2vuK3EklHXtyn2xECCmDOLcU-mmeZ6O6aPYAExw")]
        public async Task GetAllAsyncWithFactoryAsync(string token)
        {
            ApplicationFactory _applicationFactory = new ApplicationFactory();
            HttpClient _client = _applicationFactory.CreateClient();


            await CreateAPIRequestContext(_applicationFactory, token);

            //Arrange
            var x = await _client.GetAsync("api/domain/string");
            var request = await Request.GetAsync("string");


            ClassicAssert.True(request.Ok);
            await _applicationFactory.DisposeAsync();
        }

        [Test]
        [TestCase(
            "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRW1tZXR0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiNzE5IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwOS8wOS9pZGVudGl0eS9jbGFpbXMvYWN0b3IiOiIxIiwiZXhwIjoyNTI0NjA0NDAwfQ.VeJRyuaJz2vcSfReKuQ8xYCM7Z4U4TrpC-JDT4KvrX5UDhmy0vgEpZX_i2JgMz6r8KdI2Y5aeUWvJbaG-HtJtA")]
        [TestCase(
            "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiS2V0dGVsIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMTAwMCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDkvMDkvaWRlbnRpdHkvY2xhaW1zL2FjdG9yIjoiMiIsImV4cCI6NDg2Mjk5OTE0Nn0.0TmxtzHnxvqreb2GX2sX04zAqSqCrtB33Ftt9n1fL1HlP1q2vuK3EklHXtyn2xECCmDOLcU-mmeZ6O6aPYAExw")]
        public async Task GetSingleAsync(string token)
        {
            ApplicationFactory _applicationFactory = new ApplicationFactory();
            HttpClient _client = _applicationFactory.CreateClient();


            await CreateAPIRequestContext(_applicationFactory, token);

            // Act
            // int shiftId = 1;
            var request = await Request.GetAsync("api/DailySchedule/1");
            ClassicAssert.True(request.Ok);

            var issuesJsonResponse = await request.JsonAsync();
            ClassicAssert.NotNull(issuesJsonResponse);
            // Assert
            var responseDataString = request.ToString();
            var responseData = JsonConvert.DeserializeObject<DailyScheduleDTO>(issuesJsonResponse.ToString());

            Assert.That(responseData, Is.EqualTo(new DailyScheduleDTO
            {
                Nr = 1,
                Name = "empty",
                Hours = 0,
                Color = "#e9c46a"
            }));
            // Cleanup
            await _applicationFactory.DisposeAsync();

        }


        [Test]
        public async Task AddNewShift()
        {
            ApplicationFactory _applicationFactory = new ApplicationFactory();
            HttpClient _client = _applicationFactory.CreateClient();


            await CreateAPIRequestContext719(_applicationFactory);


            var dailyScheduleDTO = new DailyScheduleDTO
            {
                Name = "TestShift",
                Hours = 8,
                Color = "#e9c46a"
            };

            var request = await Request.PostAsync("/api/DailySchedule", new() {DataObject = dailyScheduleDTO});

            ClassicAssert.True(request.Ok);
            await _applicationFactory.DisposeAsync();


        }
    }
}
