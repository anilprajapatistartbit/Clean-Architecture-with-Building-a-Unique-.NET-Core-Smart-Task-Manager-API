using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Core.Models;
using SmartTaskManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartTaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync() =>
            await _context.Tasks.Include(t => t.Project).ToListAsync();

        public async Task<TaskItem?> GetByIdAsync(int id) =>
            await _context.Tasks.Include(t => t.Project)
                                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId) =>
            await _context.Tasks.Where(t => t.ProjectId == projectId).ToListAsync();
    }
}