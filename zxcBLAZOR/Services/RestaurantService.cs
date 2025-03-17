using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using zxcBLAZOR.Models;

namespace zxcBLAZOR.Services
{
    public class RestaurantService
    {
        private readonly HttpClient _httpClient;

        public RestaurantService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Restaurant>> GetRestaurants(int page = 1, int pageSize = 10, string search = "")
        {
            return await _httpClient.GetFromJsonAsync<List<Restaurant>>($"api/restaurants?page={page}&pageSize={pageSize}&search={search}");
        }

        public async Task<Restaurant> GetRestaurant(int id)
        {
            return await _httpClient.GetFromJsonAsync<Restaurant>($"api/restaurants/{id}");
        }

        public async Task<Restaurant> CreateRestaurant(Restaurant restaurant)
        {
            var response = await _httpClient.PostAsJsonAsync("api/restaurants", restaurant);
            return await response.Content.ReadFromJsonAsync<Restaurant>();
        }

        public async Task<Restaurant> UpdateRestaurant(Restaurant restaurant)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/restaurants/{restaurant.Id}", restaurant);
            return await response.Content.ReadFromJsonAsync<Restaurant>();
        }

        public async Task DeleteRestaurant(int id)
        {
            await _httpClient.DeleteAsync($"api/restaurants/{id}");
        }
    }
}