using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

using MyWebApp.Interfaces;
using System.Net;

namespace MyWebApp.Tests;

/* Reminder
 * These are considered to be integration tests
 * Because they test the end-to-end request flow.
 */
public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory;

  public EndpointTests(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task GetRoot_HasExpectedResponse()
  {
    // Arrange
    // Create mock service and register it to the service container
    // Assumption: there is a fake DB returning the welcome message
    // The mock overrides it (there isn't actually a db, yet)
    var mockService = new Mock<IWelcomeService>();

    mockService.Setup(service => service.GetWelcomeMessage())
      .Returns("Welcome! Jane Doe");

    var client = _factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
      {
        services.AddSingleton(mockService.Object);
      });
    }).CreateClient();

    // Act - Invoke the welcome page
    var response = await client.GetAsync("/");
    var responseString = await response.Content.ReadAsStringAsync();

    // Assert - Return value of endpoint is as expected
    // Note: IRL the value check here isn't that useful.
    // The mock decouples this test from the implementation
    // of the WelcomeService implementation
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    Assert.Equal("Welcome! Jane Doe", responseString);
  }

  [Fact]
  public async Task GetAbout_HasExpectedResponse()
  {
    // Arrange - Initialize the client
    var client = _factory.CreateClient();

    // Act - Invoke the about page
    var response = await client.GetAsync("/about");
    var responseString = await response.Content.ReadAsStringAsync();

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    Assert.Contains("Contoso", responseString);
  }
}
