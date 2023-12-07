﻿using Agendo.Server.Models;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Legacy;
using NUnitTesting.Server.ApplicationFactories;
using System.Text.Json;

namespace NUnitTesting.Server.controller
{
    [TestFixture]
    internal class DomainControllerTest : PlaywrightTest
    {
        private IAPIRequestContext Request = null;
     

   

        private async Task CreateAPIRequestContext(DomainApplicationFactory _applicationFactory)
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
        public async Task GetAllAsyncWithFactoryAsync()
        {
            DomainApplicationFactory _applicationFactory = new DomainApplicationFactory();
            HttpClient _client= _applicationFactory.CreateClient();


            await CreateAPIRequestContext(_applicationFactory);

            //ArrangeRepository
            var x = await _client.GetAsync("api/domain/string");
            var request = await Request.GetAsync("string");


            ClassicAssert.True(request.Ok);
        }

        [Test]
        public async Task AuthDomainGet719()
        {
            DomainApplicationFactory _applicationFactory = new DomainApplicationFactory();
            HttpClient _client = _applicationFactory.CreateClient();


            await CreateAPIRequestContext(_applicationFactory);

            var request = await Request.GetAsync("api/domain");


           ClassicAssert.True(request.Ok);

            var issuesJsonResponse = await request.JsonAsync();

            ClassicAssert.NotNull(issuesJsonResponse);

            var list = ConvertJsonElementToList(issuesJsonResponse.Value);
            ClassicAssert.AreEqual(list, new List<Agendo.Server.Models.DomainDTO>() {
               new Agendo.Server.Models.DomainDTO{Nr = 1, Name ="Oliwier Nowak" },
               new Agendo.Server.Models.DomainDTO{Nr = 2, Name ="Anton Schubhart" },
               new Agendo.Server.Models.DomainDTO{Nr = 3, Name ="Philipp Schaffer" },
            });

            ClassicAssert.AreEqual(1, 3);
        }


        static List<DomainDTO> ConvertJsonElementToList(JsonElement root)
        {
            List<DomainDTO> domainDTOList = new List<DomainDTO>();

            // Iterate through the array elements
            foreach (JsonElement element in root.EnumerateArray())
            {
                // Deserialize each element to DomainDTO
                DomainDTO domainDTO = JsonSerializer.Deserialize<DomainDTO>(element.GetRawText(), new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Use CamelCase naming policy to match the JSON property names
                });

                // Add the deserialized object to the list
                domainDTOList.Add(domainDTO);
            }

            return domainDTOList;
        }

    }
}
