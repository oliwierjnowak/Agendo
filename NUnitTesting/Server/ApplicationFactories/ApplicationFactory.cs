﻿using System.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;


namespace NUnitTesting.Server.ApplicationFactories
{
    public class ApplicationFactory : WebApplicationFactory<Program>
    {
        private IHost? _host;
        
        public ApplicationFactory()
        {
        }

        public string ServerAddress
        {
            get
            {
                EnsureServer();
                return ClientOptions.BaseAddress.ToString();
            }
        }


        protected override IHost CreateHost(IHostBuilder builder)
        {
            var testHost = builder.Build();
            
            builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

            _host = builder.Build();
            _host.Start();
            var server = _host.Services.GetRequiredService<IServer>();
            var addresses = server.Features.Get<IServerAddressesFeature>();

            ClientOptions.BaseAddress = addresses!.Addresses
                .Select(x => new Uri(x))
                .Last();
            testHost.Start();
            return testHost;

        }

        protected override void Dispose(bool disposing)
        {
            _host?.Dispose();
        }

        private void EnsureServer()
        {
            if (_host is null)
            {
                // This forces WebApplicationFactory to bootstrap the server
                using var _ = CreateDefaultClient();
            }
        }
    }
}
