using AppDBContext;
using DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace DentalClinicAPI.Helpers
{
    public static class DatabaseSeeder
    {
        public static void MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                using (var dbContext = services.GetRequiredService<DentalClinicDBContext>())
                {
                    try
                    {
                        if (dbContext.Database.GetPendingMigrations().Any())
                        {
                            dbContext.Database.Migrate();
                            dbContext.SaveChanges();
                        }
                        if (!dbContext.Set<User>().Any())
                        {
                            dbContext.Set<User>().Add(new User
                            {
                                Username = "admin",
                                Password = "123",
                                FullName = "Admin",
                                Role = Enums.RoleEnum.Doctor,
                                IsActive = true
                            });
                            dbContext.SaveChanges();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Log(ex);
                        throw;
                    }
                }
            }
        }
    }
}
