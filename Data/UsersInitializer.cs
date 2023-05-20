using Feipder.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Feipder.Data
{
    public static class UsersInitializer
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@admin.ru";
            var adminLogin = "admin";
            var adminPhone = "88005553535";
            var adminPassword = "123123";

            var guestEmail = "guest@guest.ru";
            var guestLogin = "guest";
            var guestPhone = "88005553536";
            var guestPassword = "123123";

            if(await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            if (await roleManager.FindByNameAsync("guest") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("guest"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User {
                    Email = adminEmail,
                    UserName = adminEmail,
                    PhoneNumber = adminPhone,
                    FirstName = "Админ",
                    LastName = "Админский",
                    Basket = new Basket()
                };
                var result = await userManager.CreateAsync(admin);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await userManager.FindByNameAsync(guestEmail) == null)
            {
                var guest = new User
                {
                    Email = guestEmail,
                    UserName = guestLogin,
                    PhoneNumber = guestPhone,
                    FirstName = "Гость",
                    LastName = "Гостянский",
                    Basket = new Basket()
                };
                var result = await userManager.CreateAsync(guest);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(guest, "guest");
                }
            }
        }
    }
}
