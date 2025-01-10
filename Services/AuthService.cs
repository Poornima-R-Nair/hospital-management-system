using HospitalManagementApp.Data;
using HospitalManagementApp.Models;
using HospitalManagementApp.Utils;

namespace HospitalManagementApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDBConnection _dbConnection;
        private readonly List<Admin> _admins = new List<Admin>
        {
            new Admin { Username = "admin", Password = "Admin123" }
        };

        public AuthService(IDBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public Dictionary<string, string>? Authenticate(string username, string password)
        {
            // Check if the username and password match any admin
            var admin = _admins.FirstOrDefault(a => a.Username == username && a.Password == password);
            if (admin != null)
            {
                CurrentUser.Username = admin.Username;
                CurrentUser.UserId = admin.Id;
                CurrentUser.UserRole = "Admin";

                return new Dictionary<string, string>
                {
                    { "Role", "Admin" },
                    { "Username", admin.Username ?? "" }
                };
            }

            // Check if the username and password match any doctor
            var doctor = _dbConnection.ViewAllDoctors().FirstOrDefault(d => d.Username == username && d.Password == password);
            if (doctor != null)
            {
                CurrentUser.Username = doctor.Username;
                CurrentUser.UserId = doctor.Id;
                CurrentUser.UserRole = "Doctor";

                return new Dictionary<string, string>
                {
                    { "Role", "Doctor" },
                    { "Username", doctor.Username ?? "" }
                };
            }

            // If no match is found, return null
            return null;
        }
    }
}