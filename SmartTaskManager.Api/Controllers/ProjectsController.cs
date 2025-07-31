using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskManager.Core.Models;
using SmartTaskManager.Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SmartTaskManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _service;

        public ProjectsController(ProjectService service)
        {
            _service = service;
        }

        // Advanced filtering, sorting, pagination
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(
            string? search = null, 
            string? sortBy = "Name", 
            bool desc = false, 
            int page = 1, 
            int pageSize = 10)
        {
            var projects = await _service.GetAllProjectsAsync();

            // Filtering
            if (!string.IsNullOrEmpty(search))
                projects = projects.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) 
                                            || (p.Description != null && p.Description.Contains(search, StringComparison.OrdinalIgnoreCase)));

            // Sorting
            projects = sortBy?.ToLower() switch
            {
                "name" => desc ? projects.OrderByDescending(p => p.Name) : projects.OrderBy(p => p.Name),
                "id" => desc ? projects.OrderByDescending(p => p.Id) : projects.OrderBy(p => p.Id),
                _ => projects
            };

            // Pagination
            var total = projects.Count();
            var items = projects.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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
            var project = await _service.GetProjectByIdAsync(id);
            return project == null ? NotFound() : Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Project project)
        {
            var created = await _service.CreateProjectAsync(project);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Project project)
        {
            if (id != project.Id) return BadRequest();
            await _service.UpdateProjectAsync(project);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}