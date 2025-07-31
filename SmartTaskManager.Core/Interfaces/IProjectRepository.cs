using SmartTaskManager.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTaskManager.Core.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<Project> AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(int id);
    }
}