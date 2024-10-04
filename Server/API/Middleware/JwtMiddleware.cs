using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly ITokenService _tokenService;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, ITokenService tokenService)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _tokenService = tokenService;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            if (ShouldSkipAuthentication(context))
            {
                await _next(context);
                return;
            }
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachUserToContext(context, userService, token);

            await _next(context);
        }
        //private void AttachUserToContext(HttpContext context, string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var key = Encoding.ASCII.GetBytes(_appSettings.Secret ?? "");
        //        tokenHandler.ValidateToken(token, new TokenValidationParameters
        //        {
        //            ValidateIssuerSigningKey = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(key),
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        //            ClockSkew = TimeSpan.Zero
        //        }, out SecurityToken validatedToken);
        //        var jwtToken = (JwtSecurityToken)validatedToken;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //}
        private bool ShouldSkipAuthentication(HttpContext context)
        {
            // Get endpoint information
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            if (endpoint == null)
            {
                return false; // Fallback if endpoint info is unavailable
            }

            // Check if the endpoint has the AllowAnonymous attribute
            var allowAnonymous = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null;
            return allowAnonymous;
        }
        private async Task attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                //var tokenHandler = new JwtSecurityTokenHandler();
                //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                //tokenHandler.ValidateToken(token, new TokenValidationParameters
                //{
                //    ValidateIssuerSigningKey = true,
                //    IssuerSigningKey = new SymmetricSecurityKey(key),
                //    ValidateIssuer = false,
                //    ValidateAudience = false,
                //    // set clock skew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                //    ClockSkew = TimeSpan.Zero
                //}, out SecurityToken validatedToken);

                //var jwtToken = (JwtSecurityToken)validatedToken;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_appSettings.Secret ?? "");
              var Principal =  tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                context.User = Principal;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;

                //Attach user to context on successful JWT validation
                context.Items["User"] = await userService.GetById(userId);
            }
            catch(SecurityTokenExpiredException)
            {
                throw new UnauthorizedAccessException("Token has expired");
                //Do nothing if JWT validation fails
                // user is not attached to context so the request won't have access to secure routes
            }
            catch (SecurityTokenValidationException)
            {
                throw new UnauthorizedAccessException("Token validation failed");
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Token validation failed");
            }
        }
    }
}
