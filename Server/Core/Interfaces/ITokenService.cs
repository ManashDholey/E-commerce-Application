using System.Security.Claims;
using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
         ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
    }
}