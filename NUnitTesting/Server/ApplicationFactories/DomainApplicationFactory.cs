using Agendo.Server.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Agendo.AuthAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;

namespace NUnitTesting.Server.ApplicationFactories
{
    internal class DomainApplicationFactory : WebApplicationFactory<Program>
    {
        private IHost? _host;

        public DomainApplicationFactory()
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

            builder.ConfigureServices(services => {
                services.RemoveAll(typeof(IDomainService));
                services.RemoveAll(typeof(IAuthService));


                var service = ArrangeRepository();
                services.AddSingleton(typeof(IDomainService), service.Object);

            });
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


        private Mock<IDomainService> ArrangeRepository()
        {
            var mockService = new Mock<IDomainService>();

            List<Agendo.Server.Models.DomainDTO> data = new List<Agendo.Server.Models.DomainDTO>() {
               new Agendo.Server.Models.DomainDTO{Nr = 1, Name ="Oliwier Nowak" },
               new Agendo.Server.Models.DomainDTO{Nr = 2, Name ="Anton Schubhart" },
               new Agendo.Server.Models.DomainDTO{Nr = 3, Name ="Philipp Schaffer" },
            };

            mockService.Setup(r => r.GetAllAsync(1)).ReturnsAsync(data);

            return mockService;
        }
    }
}
