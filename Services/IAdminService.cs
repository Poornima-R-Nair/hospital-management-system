namespace HospitalManagementApp.Services
{
    public interface IAdminService
    {
        void ViewAllDoctors();
        void AddDoctor();
        void UpdateDoctor();
        void DeleteDoctor();
        void ViewAllPatients();
        void AddPatient();
        void UpdatePatient();
        void DeletePatient();
        void ViewAllAppointments();
        void AddAppointment();
        void UpdateAppointment();
        void DeleteAppointment();
        void ViewAllEquipments();
        void AddEquipment();

        void DeleteEquipment();
    }
}