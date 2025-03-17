using zxcAPI.Models;
using System.Threading.Tasks;

namespace zxcAPI.Services
{
    public interface IUserService
    {
        Task<User> Register(User user);
        Task<string> Login(string username, string password);
    }
}