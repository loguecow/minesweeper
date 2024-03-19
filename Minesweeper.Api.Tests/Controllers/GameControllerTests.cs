using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Minesweeper.Api.Tests.Controllers;
public class GameControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public GameControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Post_ValidRequest_ShouldReturn201Created()
    {
        // Arrange
        var client = _factory.CreateClient();
        using StringContent jsonContent = new(
            JsonSerializer.Serialize(new
            {
                level = 0,
                userName= "D. Duck",
                userId = Guid.NewGuid()
            }),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync("/games", jsonContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        response.Content.Headers?.ContentLocation?.ToString().Should().Contain("/games/");
        response.Content.Headers?.ContentType?.ToString().Should().Be("application/json; charset=utf-8");
    }

    [Fact]
    public async Task Get_GameDoesNotExits_ShouldReturn404NotFound()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/games/00000000-0000-0000-0000-000000000000");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
