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
    public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public EmployeeControllerTests(WebApplicationFactory<Program> factory)
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
            var response = await _client.GetAsync("/Employee");

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
                id = dbContext.Employees.First().Id;
            }

            // Act
            var response = await _client.GetAsync($"/Employee/{id}");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetByStorage_ValidStorageId_ShouldReturnOkResponse()
        {
            // Arrange
            Guid storageId;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
                storageId = dbContext.Storages.First().Id;
            }

            // Act
            var response = await _client.GetAsync($"/Employee/{storageId}/storage");

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Add_ValidModel_ShouldReturnCreatedResponse()
        {
            // Arrange
            var model = new SaveEmployeeModel
            {
                Name = "Test Employee",
                Phone = "1234567890",
                Salary = 5000,
                Position = "Manager",
            };

            // Act
            var response = await _client.PostAsync("/Employee", GetPayload(model));

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Add_InvalidModel_ShouldReturnBadRequestResponse()
        {
            // Arrange
            var model = new SaveEmployeeModel();

            // Act
            var response = await _client.PostAsync("/Employee", GetPayload(model));

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
                var employee = dbContext.Employees.First(e => e.Name == "Test Employee");

                var model = new UpdateEmployeeModel
                {
                    Id = employee.Id,
                    Name = "Updated Employee",
                    Phone = "0987654321",
                    Salary = 6000,
                    Position = "Senior Manager",
                    StorageId = employee.StorageId
                };

                // Act
                var response = await _client.PutAsync("/Employee", GetPayload(model));

                // Assert
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_ValidId_ShouldReturnNoContentResponse()
        {
            // Arrange
            Guid employeeId;
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();

                var employee = new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Employee",
                    Phone = "1234567890",
                    Salary = 5000,
                    Position = "Manager"
                };
                await dbContext.Employees.AddAsync(employee);
                await dbContext.SaveChangesAsync();

                employeeId = employee.Id;
            }

            // Act
            var response = await _client.DeleteAsync($"/Employee/{employeeId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
