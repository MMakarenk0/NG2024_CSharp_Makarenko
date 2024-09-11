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
    public class StorageControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public StorageControllerTests(WebApplicationFactory<Program> factory)
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
            var response = await _client.GetAsync("/Storage");

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
                id = dbContext.Storages.First().Id;
            }

            // Act
            var response = await _client.GetAsync($"/Storage/{id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Add_ValidModel_ShouldReturnCreatedResponse()
        {
            // Arrange
            var model = new SaveStorageModel
            {
                Address = "123 Main St",
                Number = 1,
                DirectorId = null,
                EmployeeIds = new List<Guid>(), 
                ItemIds = new List<Guid>() 
            };

            // Act
            var response = await _client.PostAsync("/Storage", GetPayload(model));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Add_InvalidModel_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var model = new SaveStorageModel(); 

            // Act
            var response = await _client.PostAsync("/Storage", GetPayload(model));

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
                var storage = new Storage { Address = "Old Address", Number = 1 };
                dbContext.Storages.Add(storage);
                await dbContext.SaveChangesAsync();
                id = storage.Id;

                var model = new UpdateStorageModel
                {
                    Id = id,
                    Address = "Updated Address",
                    Number = 2,
                    DirectorId = null, 
                    EmployeeIds = new List<Guid>(), 
                    ItemIds = new List<Guid>() 
                };

                // Act
                var response = await _client.PutAsync("/Storage", GetPayload(model));

                // Assert
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_ValidId_ShouldReturnNoContentResponse()
        {
            // Arrange
            Guid storageId;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                var storage = new Storage { Address = "Address to Delete", Number = 1 };
                dbContext.Storages.Add(storage);
                await dbContext.SaveChangesAsync();

                storageId = storage.Id;
            }

            // Act
            var response = await _client.DeleteAsync($"/Storage/{storageId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
