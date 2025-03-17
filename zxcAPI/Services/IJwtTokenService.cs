using zxcAPI.Models;

namespace zxcAPI.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}