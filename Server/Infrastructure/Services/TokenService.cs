using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly AppSettings _appSettings;

        public TokenService(IConfiguration config, IOptions<AppSettings> appSettings)
        {
            _config = config;
            var d=_config["Token:Key"];
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            _appSettings = appSettings.Value;
        }
        
        public async Task<string> CreateToken(AppUser user)
        {
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim(ClaimTypes.GivenName, user.DisplayName),
            //    new Claim("Id",user.Id) 
            //};

            //var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims),
            //    Expires = DateTime.Now.AddDays(7),
            //    SigningCredentials = creds,
            //    Issuer = _config["Token:Issuer"]
            //};

            //var tokenHandler = new JwtSecurityTokenHandler();

            //var token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);
            var token = await generateJwtToken(user);
            return token;
        }
       
        // helper methods
        private async Task<string> generateJwtToken(AppUser user)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, user.Email),
                                                         new Claim(ClaimTypes.GivenName, user.DisplayName),
                                                         new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
        public ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            // Add any other validation parameters as needed
        };

        try
        {
            if(!string.IsNullOrEmpty(token)){
            //var d = token.Replace("Bearer ","");
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            return principal;
            }
            else{
                return null;
            }
        }
        catch (Exception ex)
        {
            // Token validation failed
            return null;
        }
    }
    }
}