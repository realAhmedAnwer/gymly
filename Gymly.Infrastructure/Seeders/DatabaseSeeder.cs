using Gymly.Application.Interfaces.Common;
using Gymly.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Gymly.Infrastructure.Seeders;

public static class DatabaseSeeder
{
    private const string SuperAdminPassword = "SuperAdmin@123";

    public static async Task SeedAsync(GymlyDbContext context, IPasswordHasher passwordHasher)
    {
        var superAdminRole = await context.SystemRoles.FirstOrDefaultAsync(r => r.Name == "Super Admin");
        if (superAdminRole is null)
        {
            superAdminRole = new SystemRole { Name = "Super Admin", Description = "Full system administrator with user management access", IsActive = true };
            context.SystemRoles.Add(superAdminRole);
        }

        var adminRole = await context.SystemRoles.FirstOrDefaultAsync(r => r.Name == "Admin");
        if (adminRole is null)
        {
            adminRole = new SystemRole { Name = "Admin", Description = "Administrator with limited user management access", IsActive = true };
            context.SystemRoles.Add(adminRole);
        }

        var receptionistRole = await context.SystemRoles.FirstOrDefaultAsync(r => r.Name == "Receptionist");
        if (receptionistRole is null)
        {
            receptionistRole = new SystemRole { Name = "Receptionist", Description = "Front desk staff with member and check-in access", IsActive = true };
            context.SystemRoles.Add(receptionistRole);
        }

        await context.SaveChangesAsync();

        if (!await context.SystemUsers.AnyAsync())
        {
            var superAdminUser = new SystemUser
            {
                Username = "superadmin",
                Email = "superadmin@gymly.com",
                FullName = "Super Administrator",
                PasswordHash = passwordHasher.Hash(SuperAdminPassword),
                IsActive = true,
                SystemRoleId = superAdminRole.Id
            };
            context.SystemUsers.Add(superAdminUser);
            await context.SaveChangesAsync();
        }
    }
}
