using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using MercadonaTests.Authentication;

namespace Mercadona.Tests
{
    public class AccesTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AccesTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AccesAdminInBackOffice()
        {

            // On simule les actions d'un administrateur
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("AdminTest")
                        .AddScheme<AuthenticationSchemeOptions, AdminTestAuthenticationHandler>("AdminTest", options => { });
                });
            }).CreateClient();
            client.DefaultRequestHeaders.Add("admin", "admin@admin.com");

            // On fait appel a la page des produits
            var response = await client.GetAsync("/Products");

            // La page doit s'afficher
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AccessUserInBackOffice()
        {
            // On simule les actions d'une personne connectée mais non admin
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, UserTestAuthenticationHandler>("Test", options => { });
                });
            }).CreateClient();
            client.DefaultRequestHeaders.Add("user", "user@user.com");

            var response = await client.GetAsync("/Products");

            // L'accès doit être refusé
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task AccessInBackOfficeWithoutAccount()
        {
      
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/Products");

            // On doit être redirigé vers la page de connexion
            Assert.StartsWith("/Identity/Account/Login", response?.RequestMessage?.RequestUri?.AbsolutePath);
        }
    }
}
