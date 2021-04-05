using JJS.Application.Enums;
using JJS.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JJS.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            if (!await roleManager.RoleExistsAsync(Roles.SuperAdmin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Admin.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Moderator.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));

            if (!await roleManager.RoleExistsAsync(Roles.Basic.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}
