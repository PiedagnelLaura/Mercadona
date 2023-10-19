using MercadonaTests.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace MercadonaMSTest
{
    [TestClass]
    public class AccessTest
    {
        private WebApplicationFactory<Program> _factory;

        [TestInitialize]
        public void Initialize()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [TestMethod]
        public async Task AccesAdminInBackOffice()
        {
            // Arrange
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

            // Act
            // On fait appel a la page des produits
            var response = await client.GetAsync("/Products");

            // Assert
            // La page doit s'afficher
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task AccessUserInBackOffice()
        {
            // Arrange
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

            // Act
            var response = await client.GetAsync("/Products");

            // Assert
            // L'accès doit être refusé
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public async Task AccessInBackOfficeWithoutAccount()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Products");

            // Assert
            // On doit être redirigé vers la page de connexion
            StringAssert.StartsWith("/Identity/Account/Login", response?.RequestMessage?.RequestUri?.AbsolutePath);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _factory.Dispose();
        }
    }
}