using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using SmartTaskManager.Core.Models;

public class ProjectsApiTests : IClassFixture<WebApplicationFactory<SmartTaskManager.Api.Program>>
{
    private readonly HttpClient _client;

    public ProjectsApiTests(WebApplicationFactory<SmartTaskManager.Api.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostProject_ReturnsCreated()
    {
        var project = new Project { Name = "IntegrationTest" };
        var response = await _client.PostAsJsonAsync("/api/projects", project);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<Project>();
        Assert.Equal("IntegrationTest", created!.Name);
    }
}