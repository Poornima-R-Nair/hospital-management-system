namespace HospitalManagementApp.Services
{
    public interface IDoctorService
    {
        void ViewAllAppointments();
        void ViewPatientMedicalRecord();
        void AddPatientMedicalRecord();
        void UpdatePatientMedicalRecord();
    }
}