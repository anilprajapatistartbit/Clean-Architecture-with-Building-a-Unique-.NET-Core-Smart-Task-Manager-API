using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Core.Models;
using SmartTaskManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SmartTaskManager.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync() =>
            await _context.Projects.Include(p => p.Tasks).ToListAsync();

        public async Task<Project?> GetByIdAsync(int id) =>
            await _context.Projects.Include(p => p.Tasks)
                                   .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Project> AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}