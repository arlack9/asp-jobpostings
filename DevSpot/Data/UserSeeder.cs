﻿using Microsoft.AspNetCore.Identity;
using DevSpot.Constants;
namespace DevSpot.Data;

public class UserSeeder
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
     {

        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        //seed 3 account with spedific roles
        await CreateUserWithRole(userManager, "admin@dwvops1.com","Admin123!",Roles.Admin);
        await CreateUserWithRole(userManager, "admin@dwvops2.com", "Admin123!", Roles.Jobseeker);
        await CreateUserWithRole(userManager, "admin@dwvops3.com", "Admin123!", Roles.Employee);

    }

    public static async Task CreateUserWithRole
        (UserManager<IdentityUser> userManager, 
        string email , 
        string password , 
        string role)
    {
        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new IdentityUser
            {
                Email = email,
                EmailConfirmed = true,
                UserName = email
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                throw new Exception($"Failed creating user with {user.Email}, Errors:{string.Join(",", result.Errors)}");
            }

        }


    }
}
