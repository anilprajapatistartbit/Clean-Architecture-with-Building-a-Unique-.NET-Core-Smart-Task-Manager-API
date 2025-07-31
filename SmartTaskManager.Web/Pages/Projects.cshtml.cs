using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartTaskManager.Core.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ProjectsModel : PageModel
{
    public List<Project> Projects { get; set; } = new();

    public async Task OnGetAsync()
    {
        using var http = new HttpClient();
        Projects = await http.GetFromJsonAsync<List<Project>>("https://localhost:5001/api/projects");
    }

    public async Task OnPostAsync()
    {
        var name = Request.Form["Name"];
        var desc = Request.Form["Description"];
        using var http = new HttpClient();
        await http.PostAsJsonAsync("https://localhost:5001/api/projects", new Project { Name = name, Description = desc });
        await OnGetAsync();
    }
}