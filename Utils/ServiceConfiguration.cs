using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HospitalManagementApp.Services;
using HospitalManagementApp.Data;
using HospitalManagementApp.Logging;
using HospitalManagementApp.Models;
using HospitalManagementApp.Utils;

namespace HospitalManagementApp
{
    public static class ServiceConfiguration
    {
        public static void AddServicesAndDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Register logging and other common services
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddTransient<Validation>();

            // Register your database connection or context
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddSingleton<IDBConnection>(provider => new DBConnection(connectionString));

            // Register application services
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IAdminService, AdminService>();
            services.AddSingleton<IDoctorService, DoctorService>();

            // Register the data models (or any other necessary data structures)
            services.AddSingleton<List<Doctor>>();
            services.AddSingleton<List<Patient>>();
            services.AddSingleton<List<Appointment>>();
            services.AddSingleton<List<Equipment>>();

            // Other services can go here as needed
        }
    }
}
