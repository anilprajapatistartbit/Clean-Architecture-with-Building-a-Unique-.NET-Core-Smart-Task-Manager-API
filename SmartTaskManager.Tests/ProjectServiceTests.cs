using Xunit;
using Moq;
using SmartTaskManager.Services.Services;
using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProjectServiceTests
{
    [Fact]
    public async Task GetAllProjectsAsync_ReturnsProjects()
    {
        var mockRepo = new Mock<IProjectRepository>();
        mockRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Project> { new Project { Name = "Test" } });

        var service = new ProjectService(mockRepo.Object);
        var result = await service.GetAllProjectsAsync();

        Assert.Single(result);
    }
}