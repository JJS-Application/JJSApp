using JJS.Application.Enums;
using JJS.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Org.BouncyCastle.Crypto.Prng.Drbg;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJS.Infrastructure.Identity.Seeds
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if ((await userManager.FindByEmailAsync("superadmin@gmail.com")) == null)
            {
                //Seed Default User
                var defaultUser = new ApplicationUser
                {
                    UserName = "superadmin",
                    Email = "superadmin@gmail.com",
                    FirstName = "Sunil",
                    LastName = "Kumar",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                if (userManager.Users.All(u => u.Id != defaultUser.Id))
                {
                    var user = await userManager.FindByEmailAsync(defaultUser.Email);
                    if (user == null)
                    {
                        await userManager.CreateAsync(defaultUser, "SamsungApp3#!");
                        await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                        await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                        await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                        await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                    }

                }
            }
        }
    }
}
