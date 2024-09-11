using BLL.Models.AddEntityModels;
using BLL.Models.UpdateEntityModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using DataLayer.Data.Infrastructure;
using DataLayer.Entities;

namespace IntegrationTests;

public class CategoryControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CategoryControllerTests(WebApplicationFactory<Program> factory)
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
        var response = await _client.GetAsync("/Category");

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
            var scopedDbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
            id = scopedDbContext.Categories.First().Id;
        }

        // Act
        var response = await _client.GetAsync($"/Category/{id}");

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task GetById_InvalidId_ShouldReturnNoContentResponse()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/Category/{id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Add_ValidModel_ShouldReturnCreatedResponse()
    {
        // Arrange
        var model = new SaveCategoryModel
        {
            Id = Guid.NewGuid(),
            Description = "Test",
        };

        // Act
        var response = await _client.PostAsync("/Category", GetPayload(model));

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Add_InvalidModel_ShouldReturnBadRequestResponse()
    {
        // Arrange
        var model = new SaveCategoryModel(); // Missing required properties

        // Act
        var response = await _client.PostAsync("/Category", GetPayload(model));

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
            var scopedDbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
            var category = scopedDbContext.Categories.First(c => c.Description == "Test");
            id = category.Id;
        }

        var model = new UpdateCategoryModel
        {
            Id = id,
            Description = "New Test",
        };

        // Act
        var response = await _client.PutAsync("/Category", GetPayload(model));

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Update_InvalidModel_ShouldReturnNotFoundResponse()
    {
        // Arrange
        var model = new UpdateCategoryModel
        {
            Id = Guid.NewGuid(),
            Description = "Test"
        }; // Non-existent ID

        // Act
        var response = await _client.PutAsync("/Category", GetPayload(model));

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ValidId_ShouldReturnNoContentResponse()
    {
        // Arrange
        Guid id;
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedDbContext = scope.ServiceProvider.GetRequiredService<DeliveryServiceDbContext>();
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Description = "Test",
            };
            await scopedDbContext.Categories.AddAsync(category);
            await scopedDbContext.SaveChangesAsync();
            id = category.Id;
        }

        // Act
        var response = await _client.DeleteAsync($"/Category/{id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_InvalidId_ShouldReturnNoContentResponse()
    {
        // Act
        var response = await _client.DeleteAsync($"/Category/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }
}
