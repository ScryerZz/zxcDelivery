using Microsoft.EntityFrameworkCore;
using zxcAPI.Data;
using zxcAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zxcAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly AppDbContext _context;

        public RestaurantService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Restaurant>> GetAllRestaurants(int page, int pageSize, string search)
        {
            var query = _context.Restaurants.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r => r.Name.Contains(search) || r.Address.Contains(search));
            }
            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantById(int id)
        {
            return await _context.Restaurants.FindAsync(id);
        }

        public async Task<Restaurant> CreateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> UpdateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<bool> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null) return false;
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}