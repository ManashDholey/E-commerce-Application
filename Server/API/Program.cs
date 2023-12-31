
using API.Extensions;
using API.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
// builder.Services.AddSwaggerGen();
// builder.Services.AddDbContext<StoreContext>(opt=> {opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));});
// builder.Services.AddScoped<IProductRepository,ProductRepository>();
// builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
// builder.Services.AddAutoMapper(typeof(Program));
// builder.Services.Configure<ApiBehaviorOptions>(options => {
//     options.InvalidModelStateResponseFactory = ActionContext =>
//     {
//         var error =ActionContext.ModelState.Where(e => e.Value?.Errors.Count >0)
//         .SelectMany(x => x.Value.Errors)
//         .Select(x => x.ErrorMessage).ToArray();
//         var errorResponse= new ApiValidationErrorResponse{
//             Errors=error
//         };
//         return new BadRequestObjectResult(errorResponse);
//      };
// });
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithRedirects("/error/{0}");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerDocumentation();
app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast =  Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
 var identityContext = services.GetRequiredService<AppIdentityDbContext>();
 var userManager = services.GetRequiredService<UserManager<AppUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSheed.SeedAsync(context);
    await identityContext.Database.MigrateAsync();
    await AppIdentityDBContextSeed.SeedUsersAsync(userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}
app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
