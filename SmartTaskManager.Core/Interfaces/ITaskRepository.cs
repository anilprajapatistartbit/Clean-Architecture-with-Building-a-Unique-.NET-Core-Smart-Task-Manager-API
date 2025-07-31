using SmartTaskManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTaskManager.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(int id);
        Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId);
    }
}