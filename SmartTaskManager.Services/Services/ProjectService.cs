using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTaskManager.Services.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _repo;

        public ProjectService(IProjectRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<Project>> GetAllProjectsAsync() => _repo.GetAllAsync();
        public Task<Project?> GetProjectByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Project> CreateProjectAsync(Project project) => _repo.AddAsync(project);
        public Task UpdateProjectAsync(Project project) => _repo.UpdateAsync(project);
        public Task DeleteProjectAsync(int id) => _repo.DeleteAsync(id);
    }
}