using EmployeesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApp.Extensions
{
    public static class MigrationManager
    {
        public static WebApplication MigrationDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var appContext = scope.ServiceProvider.GetRequiredService<EmployeeContext>())
                {
                    try
                    {
                        if(appContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                        appContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
            return app;
        }
    }
}
