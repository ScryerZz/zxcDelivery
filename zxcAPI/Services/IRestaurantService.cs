using zxcAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace zxcAPI.Services
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetAllRestaurants(int page, int pageSize, string search);
        Task<Restaurant> GetRestaurantById(int id);
        Task<Restaurant> CreateRestaurant(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurant(Restaurant restaurant);
        Task<bool> DeleteRestaurant(int id);
    }
}