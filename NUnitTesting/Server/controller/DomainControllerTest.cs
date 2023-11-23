﻿using Agendo.AuthAPI.Model;
using Agendo.Server.Controllers;
using Agendo.Server.Persistance;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnitTesting.Server.ApplicationFactories;

namespace NUnitTesting.Server.controller
{
    [TestFixture]
    internal class DomainControllerTest : PlaywrightTest
    {
        private IAPIRequestContext Request = null;
        private DomainApplicationFactory _applicationFactory;
        public readonly HttpClient _client;

        public DomainControllerTest()
        {
            _applicationFactory = new DomainApplicationFactory();
            _client = _applicationFactory.CreateClient();
        }

        [SetUp]
        public async Task SetUpAPITesting()
        {            
            await CreateAPIRequestContext();
        }

        private async Task CreateAPIRequestContext()
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
            //ArrangeRepository
            var x = await _client.GetAsync("api/domain/string");
            var request = await Request.GetAsync("string");


            Assert.True(request.Ok);
        }

        [Test]
        public async Task AuthDomainGet719()
        {
            var request = await Request.GetAsync("api/domain");


            Assert.True(request.Ok);
        }

    }
}
