using HospitalManagementApp.Models;

namespace HospitalManagementApp.Data
{
    public interface IDBConnection
    {
        List<Doctor> ViewAllDoctors();
        List<Patient> GetAllPatients();
        List<Appointment> ViewAppointmentsByDoctor(int doctorId);
        List<Equipment> ViewAllEquipments();
        List<Appointment> GetAllAppointments();
        void AddDoctor(Doctor doctor);
        void AddPatient(Patient patient);
        void AddAppointment(Appointment appointment);
        void AddEquipment(Equipment equipment);
        void UpdateDoctor(Doctor doctor);
        void UpdatePatient(Patient patient);
        void UpdateAppointment(Appointment appointment);
        void DeleteDoctor(int doctorId);
        void DeletePatient(int patientId);
        void DeleteAppointment(int appointmentId);
        void DeleteEquipment(int equipmentId);
        MedicalRecord ViewPatientMedicalRecord(int patientId);
        void AddPatientMedicalRecord(MedicalRecord record);
        void UpdatePatientMedicalRecord(MedicalRecord record);
        Patient GetPatientById(int patientId);
        List<Appointment> GetAppointmentsByPatientId(int patientId);
        List<MedicalRecord> GetMedicalRecordsByPatientId(int patientId);
        List<Doctor> GetAllDoctors();
        List<MedicalRecord> GetMedicalRecordByPatientId(int patientId);
    }
}