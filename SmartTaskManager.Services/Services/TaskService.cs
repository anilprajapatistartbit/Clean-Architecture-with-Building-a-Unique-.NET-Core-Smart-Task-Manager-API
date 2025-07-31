using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTaskManager.Services.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repo;

        public TaskService(ITaskRepository repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<TaskItem>> GetAllTasksAsync() => _repo.GetAllAsync();
        public Task<TaskItem?> GetTaskByIdAsync(int id) => _repo.GetByIdAsync(id);
        public Task<TaskItem> CreateTaskAsync(TaskItem task) => _repo.AddAsync(task);
        public Task UpdateTaskAsync(TaskItem task) => _repo.UpdateAsync(task);
        public Task DeleteTaskAsync(int id) => _repo.DeleteAsync(id);
        public Task<IEnumerable<TaskItem>> GetTasksByProjectIdAsync(int projectId) => _repo.GetByProjectIdAsync(projectId);
    }
}