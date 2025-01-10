namespace HospitalManagementApp.Services
{
    public interface IAuthService
    {
        Dictionary<string, string>? Authenticate(string username, string password);
    }
}