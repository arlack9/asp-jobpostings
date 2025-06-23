using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;
namespace DevSpot.Data;
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            // check Admin role exist
            if (! await roleManager.RoleExistsAsync(Roles.Admin))
            {

                //if not seed admin role
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }


        // check  Employee exist
        if (!await roleManager.RoleExistsAsync(Roles.Employee))
        {

            //if not seed admin role
           await roleManager.CreateAsync(new IdentityRole(Roles.Employee));
        }



        // check Jobseeker role exist
        if (!await roleManager.RoleExistsAsync(Roles.Jobseeker))
        {

            //if not seed admin role
            await roleManager.CreateAsync(new IdentityRole(Roles.Jobseeker));
        }
    }
    }

