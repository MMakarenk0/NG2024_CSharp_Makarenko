using BLL.Models.AddEntityModels;
using BLL.Models.UpdateEntityModels;
using DataLayer.Data.Infrastructure;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace IntegrationTests
{
    public class ClientControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ClientControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        private StringContent GetPayload(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResponse()
        {
            // Act
            var response = await _client.GetAsync("/Client");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetById_ValidId_ShouldReturnOkResponse()
        {
            // Arrange
            Guid id;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                id = dbContext.Clients.First().Id;
            }

            // Act
            var response = await _client.GetAsync($"/Client/{id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Add_ValidModel_ShouldReturnCreatedResponse()
        {
            // Arrange
            var model = new SaveClientModel
            {
                Name = "Test Client",
                Phone = "1234567890"
            };

            // Act
            var response = await _client.PostAsync("/Client", GetPayload(model));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Add_InvalidModel_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var model = new SaveClientModel();

            // Act
            var response = await _client.PostAsync("/Client", GetPayload(model));

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ValidModel_ShouldReturnOkResponse()
        {
            // Arrange
            Guid id;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                var client = dbContext.Clients.First(c => c.Name == "Test Client");

                var model = new UpdateClientModel
                {
                    Id = client.Id,
                    Name = "Updated Client",
                    Phone = "0987654321"
                };

                // Act
                var response = await _client.PutAsync("/Client", GetPayload(model));

                // Assert
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_ValidId_ShouldReturnNoContentResponse()
        {
            // Arrange
            Guid clientId;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();

                var client = new Client
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Client",
                    Phone = "1234567890"
                };
                await dbContext.Clients.AddAsync(client);
                await dbContext.SaveChangesAsync();

                clientId = client.Id;
            }

            // Act
            var response = await _client.DeleteAsync($"/Client/{clientId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
