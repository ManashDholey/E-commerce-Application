using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceEvtensions 
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services
        ,IConfiguration config){
           
         services.AddDbContext<AppIdentityDbContext>(opt =>{
            opt.UseSqlite(config.GetConnectionString("IdentityConnection"));
         });
         services.AddIdentityCore<AppUser>(options =>{
             // Password settings
             // options.Password.RequireDigit = true;
             // options.Password.RequiredLength = 8;
             // options.Password.RequireNonAlphanumeric = true;
             // options.Password.RequireUppercase = true;
             // options.Password.RequireLowercase = true;

             // // Lockout settings
             // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
             // options.Lockout.MaxFailedAccessAttempts = 5;
             // options.Lockout.AllowedForNewUsers = true;

             // // User settings
             // options.User.RequireUniqueEmail = true;

             // // Sign in settings
             // options.SignIn.RequireConfirmedEmail = true;
             // options.SignIn.RequireConfirmedAccount = false;
             //builder.Services.AddAuthentication(x =>
             //{
             //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

             //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

             //})

         }).AddEntityFrameworkStores<AppIdentityDbContext>()
         .AddSignInManager<SignInManager<AppUser>>();


         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AppSettings:Secret"])),
                        //IssuerSigningKey = Encoding.ASCII.GetBytes(config["Token:Key"]),
                    //ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = false,
                        ValidateAudience = false,
                        SaveSigninToken =true,
                        
                    };
                });
         services.AddAuthorization();
          return services;
        }
    }
}