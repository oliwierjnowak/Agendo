using System.Text;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Legacy;
using NUnitTesting.Server.ApplicationFactories;
using System.Text.Json;
using System.Text.Json.Serialization;
using Agendo.Shared.DTOs;
using Agendo.Shared.Form.CreateEmployeeShift;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NUnitTesting.Server.controller
{
    [TestFixture]

    internal class ShiftControllerTest : PlaywrightTest
    {
        private IAPIRequestContext Request = null;
        private string jwtToken = null;

        private async Task CreateAPIRequestContext1000(ApplicationFactory _applicationFactory)
        {

            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiS2V0dGVsIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiMTAwMCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDkvMDkvaWRlbnRpdHkvY2xhaW1zL2FjdG9yIjoiMiIsImV4cCI6NDg2Mjk5OTE0Nn0.0TmxtzHnxvqreb2GX2sX04zAqSqCrtB33Ftt9n1fL1HlP1q2vuK3EklHXtyn2xECCmDOLcU-mmeZ6O6aPYAExw");

            Request = await this.Playwright.APIRequest.NewContextAsync(new()
            {
                // All requests we send go to this API endpoint.
                BaseURL = _applicationFactory.ServerAddress,
                ExtraHTTPHeaders = headers
            });
        }
        protected async Task CreateAPIRequestContext719(ApplicationFactory _applicationFactory)
        {

            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", "Bearer " + "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRW1tZXR0IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiNzE5IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwOS8wOS9pZGVudGl0eS9jbGFpbXMvYWN0b3IiOiIxIiwiZXhwIjoyNTI0NjA0NDAwfQ.VeJRyuaJz2vcSfReKuQ8xYCM7Z4U4TrpC-JDT4KvrX5UDhmy0vgEpZX_i2JgMz6r8KdI2Y5aeUWvJbaG-HtJtA");

            Request = await this.Playwright.APIRequest.NewContextAsync(new()
            {
                // All requests we send go to this API endpoint.
                BaseURL = _applicationFactory.ServerAddress,
                ExtraHTTPHeaders = headers
            });
        }



        [Test]
        public async Task GetSingle()
        {
            // Arrange
            ApplicationFactory _applicationFactory = new ApplicationFactory();
            HttpClient _client = _applicationFactory.CreateClient();

            // Act
            await CreateAPIRequestContext1000(_applicationFactory);
            var request = await Request.GetAsync("api/shift/1");
            ClassicAssert.True(request.Ok);
            // Assert


            var issuesJsonResponse = await request.JsonAsync();
            ClassicAssert.NotNull(issuesJsonResponse);

            var responseData = JsonConvert.DeserializeObject<List<EmployeeShiftDTO>>(issuesJsonResponse.ToString());

            List<DomainDTO> domains = new List<DomainDTO>()
            {
                new DomainDTO {Nr = 1, Name = "Emmett"},
                new DomainDTO {Nr = 2, Name = "Kettel"},
                new DomainDTO {Nr = 3, Name = "Bachs"},
                new DomainDTO {Nr = 4, Name = "Gartell"},
                new DomainDTO {Nr = 5, Name = "Tante"},
            };
            Assert.That(responseData, Is.EqualTo(new List<EmployeeShiftDTO>()
            {


            }));
            await _applicationFactory.DisposeAsync();
        }

        [Test]
        public async Task GetMultiple()
        {
            ApplicationFactory _applicationFactory = new ApplicationFactory();
            HttpClient _client = _applicationFactory.CreateClient();

            await CreateAPIRequestContext719(_applicationFactory);

            var x = await _client.GetAsync("api/Shift/string");
            var request = await Request.GetAsync("string");

            ClassicAssert.True(request.Ok);
            await _applicationFactory.DisposeAsync();
        }


        // MANAGE EMPS ENDPOINT needs to be fixed so test can be written

        /*
        [Test]
        public async Task ManageEmps()
        {
            ApplicationFactory _applicationFactory = new ApplicationFactory();
            HttpClient _client = _applicationFactory.CreateClient();

            await CreateAPIRequestContext719(_applicationFactory);

            var empShift = new CreateMultipleEmpShift
            {
                AddedDomains = new[] {1,2,3},
                RemovedDomains = new []{5},
                ShiftDate = default,
                ShiftNr = 1
            };

            // Serialize empShift to JSON
             var empShiftJson = JsonSerializer.Serialize(empShift);
            var content = new StringContent(empShiftJson, Encoding.UTF8, "application/json");

            // Act
            var response = await Request.PutAsync("api/Shift", new () {DataObject = content});
            
            // Assert
            ClassicAssert.True(response.Ok);
            
            
            await _applicationFactory.DisposeAsync();
            */
    }
}  
        
        
        

        
