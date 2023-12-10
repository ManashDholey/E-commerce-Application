using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public static class AppIdentityDBContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Manash",
                    Email = "manash@test.com",
                    UserName = "manash@test.com",
                    Address = new Address
                    {
                        FirstName = "Manash",
                        LastName = "Dholey",
                        Street = "10 The street",
                        City = "Hooghly",
                        State = "WB",
                        Zipcode = "712224"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}