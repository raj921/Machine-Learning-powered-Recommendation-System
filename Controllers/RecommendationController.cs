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
        public IActionResult GetRecommendations(int userId, [FromQuery] int count = 5)
        {
            var recommendations = _model.GetRecommendations(userId, count);
            return Ok(recommendations);
        }
    }
}