using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zxcAPI.Models;
using zxcAPI.Services;
using System.Threading.Tasks;

namespace zxcAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRestaurants(int page = 1, int pageSize = 10, string search = "")
        {
            var restaurants = await _restaurantService.GetAllRestaurants(page, pageSize, search);
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant(int id)
        {
            var restaurant = await _restaurantService.GetRestaurantById(id);
            if (restaurant == null) return NotFound();
            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateRestaurant([FromBody] Restaurant restaurant)
        {
            var created = await _restaurantService.CreateRestaurant(restaurant);
            return CreatedAtAction(nameof(GetRestaurant), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] Restaurant restaurant)
        {
            if (id != restaurant.Id) return BadRequest();
            var updated = await _restaurantService.UpdateRestaurant(restaurant);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var deleted = await _restaurantService.DeleteRestaurant(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}