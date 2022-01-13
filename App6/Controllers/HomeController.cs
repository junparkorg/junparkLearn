using App6.Models;
using App6.Util;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        // Get Todo List
        public async Task<IActionResult> Index()
        {
            Todo todo = await GetTodoAsync();

            ViewBag.BuildTime = BuildInfo.GetLinkerTime(Assembly.GetEntryAssembly());

            return View(todo);
        }

        private async Task<Todo> GetTodoAsync(CancellationToken cancellationToken = default)
        {
            var httpclient = _httpClientFactory.CreateClient();
            var response = 
                await httpclient.GetAsync("https://jsonplaceholder.typicode.com/todos/1", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Something went wrong!!");
            }

            return await response.Content.ReadFromJsonAsync<Todo>();
        }

        public async Task<IActionResult> TodoList()
        {
            List<Todo> todoList = await GetTodoListAsync(); ;
            return View(todoList);
        }

        private async Task<List<Todo>> GetTodoListAsync(CancellationToken cancellationToken = default)
        {
            var httpclient = _httpClientFactory.CreateClient();
            var list = await httpclient.GetFromJsonAsync<List<Todo>>("https://jsonplaceholder.typicode.com/todos",cancellationToken);

            if(list is null) throw new Exception("Something went wrong!!");
            return list;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}