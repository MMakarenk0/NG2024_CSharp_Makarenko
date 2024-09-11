using AutoMapper;
using BLL.Models.AddEntityModels;
using BLL.Models.UpdateEntityModels;
using BLL.Services.Interfaces;
using DataLayer.Data.Infrastructure;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;

namespace IntegrationTests
{
    public class ItemControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ItemControllerTests(WebApplicationFactory<Program> factory)
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
            var response = await _client.GetAsync("/Item");

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
                var item = new Item { Description = "Test Item", Weight = 10.0f };
                dbContext.Items.Add(item);
                await dbContext.SaveChangesAsync();
                id = item.Id;
            }

            // Act
            var response = await _client.GetAsync($"/Item/{id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetById_InvalidId_ShouldReturnNotFoundResponse()
        {
            // Act
            var response = await _client.GetAsync("/Item/" + Guid.NewGuid());

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetBySenderPhone_ValidPhone_ShouldReturnOkResponse()
        {
            // Arrange
            var phone = "1234567890";
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                var client = new Client { Name = "Name", Phone = phone };
                dbContext.Clients.Add(client);
                var item = new Item { SenderId = client.Id, Description = "Test Item", Weight = 10.0f };
                dbContext.Items.Add(item);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync($"/Item/sender/{phone}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetBySenderPhone_InvalidPhone_ShouldReturnNotFoundResponse()
        {
            // Act
            var response = await _client.GetAsync("/Item/sender/0000000000");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetByReceiverPhone_ValidPhone_ShouldReturnOkResponse()
        {
            // Arrange
            var phone = "0987654321";
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                var client = new Client { Name = "Name", Phone = phone };
                dbContext.Clients.Add(client);
                var item = new Item { ReceiverId = client.Id, Description = "Test Item", Weight = 10.0f };
                dbContext.Items.Add(item);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync($"/Item/receiver/{phone}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetByReceiverPhone_InvalidPhone_ShouldReturnNotFoundResponse()
        {
            // Act
            var response = await _client.GetAsync("/Item/receiver/0000000000");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetByFilters_ShouldReturnOkResponse()
        {
            // Arrange
            var filters = new
            {
                description = "Laptop",
                minWeight = 1.0f,
                maxWeight = 10.0f,
                categoryIds = new List<Guid> { Guid.NewGuid() },
                startDate = DateTime.UtcNow.AddDays(-1),
                endDate = DateTime.UtcNow
            };

            // Act
            var response = await _client.GetAsync($"/Item/filters?description={filters.description}&minWeight={filters.minWeight}&maxWeight={filters.maxWeight}&categoryIds={string.Join(",", filters.categoryIds)}&startDate={filters.startDate}&endDate={filters.endDate}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetByStorage_ValidId_ShouldReturnOkResponse()
        {
            // Arrange
            Guid storageId;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                var item = new Item { StorageId = storageId = Guid.NewGuid(), Description = "Test Item", Weight = 10.0f };
                dbContext.Items.Add(item);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync($"/Item/storage/{storageId}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task UpdateItemStatus_ValidId_ShouldReturnOkResponse()
        {
            // Arrange
            Guid itemId;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                var item = new Item { Id = itemId = Guid.NewGuid(), Description = "Test Item", Weight = 10.0f };
                dbContext.Items.Add(item);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.PutAsync($"/Item/status/{itemId}?isRecieved=true", null);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Add_ValidModel_ShouldReturnCreatedResponse()
        {
            // Arrange
            var model = new SaveItemModel
            {
                Description = "New Item",
                Weight = 10.0f,
                Price = 100.0m
            };

            // Act
            var response = await _client.PostAsync("/Item", GetPayload(model));

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Add_InvalidModel_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var model = new SaveItemModel(); // Invalid model

            // Act
            var response = await _client.PostAsync("/Item", GetPayload(model));

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
                var item = new Item { Description = "Old Item", Weight = 5.0f };
                dbContext.Items.Add(item);
                await dbContext.SaveChangesAsync();
                id = item.Id;

                var model = new UpdateItemModel
                {
                    Id = id,
                    Description = "Updated Item",
                    Weight = 10.0f,
                    Price = 150.0m
                };

                // Act
                var response = await _client.PutAsync("/Item", GetPayload(model));

                // Assert
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Update_InvalidModel_ShouldReturnNotFoundResponse()
        {
            // Arrange
            var model = new UpdateItemModel
            {
                Id = Guid.NewGuid(),
                Description = "Updated Item",
                Weight = 10.0f,
                Price = 150.0m
            };

            // Act
            var response = await _client.PutAsync("/Item", GetPayload(model));

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ValidId_ShouldReturnNoContentResponse()
        {
            // Arrange
            Guid itemId;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                var item = new Item
                {
                    Id = itemId = Guid.NewGuid(),
                    Description = "Item to Delete",
                    Weight = 5.0f
                };
                dbContext.Items.Add(item);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.DeleteAsync($"/Item/{itemId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_InvalidId_ShouldReturnNotFoundResponse()
        {
            // Act
            var response = await _client.DeleteAsync($"/Item/{Guid.NewGuid()}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
