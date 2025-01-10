namespace HospitalManagementApp.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Diagnosis { get; set; }
        public string Treatment { get; set; }
        public string ConsultationDetails { get; set; }
        public DateTime ConsultationDate { get; set; }
    }
}
