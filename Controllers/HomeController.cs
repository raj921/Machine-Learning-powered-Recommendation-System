using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendationSystem.Data;
using RecommendationSystem.Models;
using System.Diagnostics;

namespace RecommendationSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var userCount = await _dbContext.Users.CountAsync();
            var itemCount = await _dbContext.Items.CountAsync();
            var interactionCount = await _dbContext.Interactions.CountAsync();

            ViewData["UserCount"] = userCount;
            ViewData["ItemCount"] = itemCount;
            ViewData["InteractionCount"] = interactionCount;

            return View();
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
