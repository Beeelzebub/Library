using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Library.Models;
using Library.Data;

namespace Library
{
    public class DataInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dataManager)
        {
            string adminLogin = "admin";
            string password = "admin";
            
            //if (dataManager.BookStatuses.ToList().Where(p => p.Name == "свободна") == null)
            //{

            //}

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("reader") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("reader"));
            }
            if (await roleManager.FindByNameAsync("librarian") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("librarian"));
            }
            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                User admin = new User { UserName = adminLogin, FirstName = adminLogin, SecondName = adminLogin };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
