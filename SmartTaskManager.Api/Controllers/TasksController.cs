using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskManager.Core.Models;
using SmartTaskManager.Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _service;

        public TasksController(TaskService service)
        {
            _service = service;
        }

        // Filtering, sorting, pagination
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            string? search = null,
            string? sortBy = "Title",
            bool desc = false,
            int page = 1,
            int pageSize = 10)
        {
            var tasks = await _service.GetAllTasksAsync();

            // Filtering
            if (!string.IsNullOrEmpty(search))
                tasks = tasks.Where(t => t.Title.Contains(search, StringComparison.OrdinalIgnoreCase));

            // Sorting
            tasks = sortBy?.ToLower() switch
            {
                "title" => desc ? tasks.OrderByDescending(t => t.Title) : tasks.OrderBy(t => t.Title),
                "id" => desc ? tasks.OrderByDescending(t => t.Id) : tasks.OrderBy(t => t.Id),
                "iscompleted" => desc ? tasks.OrderByDescending(t => t.IsCompleted) : tasks.OrderBy(t => t.IsCompleted),
                _ => tasks
            };

            // Pagination
            var total = tasks.Count();
            var items = tasks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Items = items
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var task = await _service.GetTaskByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TaskItem task)
        {
            var created = await _service.CreateTaskAsync(task);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TaskItem task)
        {
            if (id != task.Id) return BadRequest();
            await _service.UpdateTaskAsync(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteTaskAsync(id);
            return NoContent();
        }

        [HttpGet("project/{projectId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByProject(
            int projectId, 
            string? search = null,
            string? sortBy = "Title",
            bool desc = false,
            int page = 1,
            int pageSize = 10)
        {
            var tasks = await _service.GetTasksByProjectIdAsync(projectId);

            if (!string.IsNullOrEmpty(search))
                tasks = tasks.Where(t => t.Title.Contains(search, StringComparison.OrdinalIgnoreCase));

            tasks = sortBy?.ToLower() switch
            {
                "title" => desc ? tasks.OrderByDescending(t => t.Title) : tasks.OrderBy(t => t.Title),
                "id" => desc ? tasks.OrderByDescending(t => t.Id) : tasks.OrderBy(t => t.Id),
                "iscompleted" => desc ? tasks.OrderByDescending(t => t.IsCompleted) : tasks.OrderBy(t => t.IsCompleted),
                _ => tasks
            };

            var total = tasks.Count();
            var items = tasks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Items = items
            });
        }
    }
}