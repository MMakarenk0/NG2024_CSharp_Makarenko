using BLL.Models.GetEntityModels;
using BLL.Models.UpdateEntityModels;
using DataLayer.Data.Infrastructure;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace IntegrationTests
{
    public class ManagerControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ManagerControllerTests(WebApplicationFactory<Program> factory)
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
            var response = await _client.GetAsync("/Manager");

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
                id = dbContext.Managers.First().Id;
            }

            // Act
            var response = await _client.GetAsync($"/Manager/{id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Add_ValidModel_ShouldReturnCreatedResponse()
        {
            // Arrange
            var model = new SaveManagerModel
            {
                Name = "Test Manager",
                StorageId = null
            };

            // Act
            var response = await _client.PostAsync("/Manager", GetPayload(model));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Add_InvalidModel_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var model = new SaveManagerModel();

            // Act
            var response = await _client.PostAsync("/Manager", GetPayload(model));

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
                var manager = new Manager { Name = "Old Manager" };
                dbContext.Managers.Add(manager);
                await dbContext.SaveChangesAsync();
                id = manager.Id;

                var model = new UpdateManagerModel
                {
                    Id = id,
                    Name = "Updated Manager",
                    StorageId = null
                };

                // Act
                var response = await _client.PutAsync("/Manager", GetPayload(model));

                // Assert
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_ValidId_ShouldReturnNoContentResponse()
        {
            // Arrange
            Guid managerId;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                var manager = new Manager { Name = "Manager to Delete" };
                dbContext.Managers.Add(manager);
                await dbContext.SaveChangesAsync();

                managerId = manager.Id;
            }

            // Act
            var response = await _client.DeleteAsync($"/Manager/{managerId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
