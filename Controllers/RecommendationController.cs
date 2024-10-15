using Microsoft.AspNetCore.Mvc;
using RecommendationSystem.Models;

namespace RecommendationSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendationModel _model;

        public RecommendationController(RecommendationModel model)
        {
            _model = model;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRecommendations(int userId, [FromQuery] int count = 5)
        {
            var recommendations = await _model.GetRecommendationsAsync(userId, count);
            return Ok(recommendations);
        }

        [HttpPost("train")]
        public async Task<IActionResult> TrainModel()
        {
            await _model.TrainModelAsync();
            return Ok("Model trained successfully");
        }
    }
}
