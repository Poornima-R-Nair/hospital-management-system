using HospitalManagementApp.Models;
using HospitalManagementApp.Logging;
using HospitalManagementApp.Services;
using HospitalManagementApp.Utils;
using HospitalManagementApp.Data;

using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagementApp.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly ILoggerService _loggerService;
        private readonly IDBConnection _dbConnection;

        private readonly Validation _validator;

        public DoctorService(ILoggerService loggerService, IDBConnection dbConnection, Validation validator)
        {
            _loggerService = loggerService;
            _dbConnection = dbConnection;
            _validator = validator;
        }

        // View all appointments for the doctor
        public void ViewAllAppointments()
        {
            _loggerService.LogInformation("Viewing all appointments for Doctor ID " + CurrentUser.UserId);
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Appointments for Doctor ID {CurrentUser.UserId}:");
            Console.WriteLine("--------------------------------------------------");

            try
            {
                List<Appointment> doctorAppointments = _dbConnection.ViewAppointmentsByDoctor(CurrentUser.UserId);

                if (doctorAppointments.Count > 0)
                {
                    foreach (var appointment in doctorAppointments)
                    {
                        Console.WriteLine($"Appointment ID: {appointment.Id}, Date: {appointment.Date.ToShortDateString()}, Time: {appointment.Time}, Patient Name: {_dbConnection.GetPatientById(appointment.PatientId ?? 0).Name}");
                    }
                }
                else
                {
                    Console.WriteLine("No appointments found for this doctor.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error fetching appointments: " + exception.Message);
            }
        }

        // View a patient’s medical record
        public void ViewPatientMedicalRecord()
        {
            int patientId = -1;
            while (patientId <= 0)
            {
                Console.WriteLine("Enter Patient ID to view medical record: ");
                string input = Console.ReadLine();
                try
                {
                    patientId = _validator.ValidateIntInput(input, "Patient ID");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Please try again.");
                }
            }

            _loggerService.LogInformation($"Viewing medical record for Patient ID {patientId}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Medical Record for Patient ID {patientId}:");
            Console.WriteLine("--------------------------------------------------");

            try
            {
                MedicalRecord medicalRecord = _dbConnection.ViewPatientMedicalRecord(patientId);

                if (medicalRecord != null)
                {
                    Console.WriteLine($"Diagnosis: {medicalRecord.Diagnosis}");
                    Console.WriteLine($"Treatment: {medicalRecord.Treatment}");
                    Console.WriteLine($"Consultation Details: {medicalRecord.ConsultationDetails}");
                    Console.WriteLine($"Consultation Date: {medicalRecord.ConsultationDate.ToShortDateString()}");
                }
                else
                {
                    Console.WriteLine("No medical record found for this patient.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error viewing medical record: " + exception.Message);
            }
        }

        // Add a new medical record for a patient
        public void AddPatientMedicalRecord()
        {
            int patientId = -1;
            while (patientId <= 0)
            {
                Console.WriteLine("Enter Patient ID to add medical record: ");
                string input = Console.ReadLine();
                try
                {
                    patientId = _validator.ValidateIntInput(input, "Patient ID");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Please try again.");
                }
            }

            _loggerService.LogInformation($"Adding medical record for Patient ID {patientId}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Adding Medical Record for Patient ID {patientId}:");
            Console.WriteLine("--------------------------------------------------");

            try
            {
                string diagnosis = "";
                while (string.IsNullOrWhiteSpace(diagnosis))
                {
                    Console.WriteLine("Enter Diagnosis: ");
                    diagnosis = Console.ReadLine();
                    try
                    {
                        diagnosis = _validator.ValidateAlphaInput(diagnosis, "Diagnosis");
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine(exception.Message);
                        continue; // Ask for input again if validation fails
                    }
                }

                string treatment = "";
                while (string.IsNullOrWhiteSpace(treatment))
                {
                    Console.WriteLine("Enter Treatment: ");
                    treatment = Console.ReadLine();
                    try
                    {
                        treatment = _validator.ValidateAlphaInput(treatment, "Treatment");
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine(exception.Message);
                        continue; // Ask for input again if validation fails
                    }
                }

                string consultationDetails = "";
                while (string.IsNullOrWhiteSpace(consultationDetails))
                {
                    Console.WriteLine("Enter Consultation Details: ");
                    consultationDetails = Console.ReadLine();
                    try
                    {
                        consultationDetails = _validator.ValidateAlphaInput(consultationDetails, "Consultation Details");
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine(exception.Message);
                        continue; // Ask for input again if validation fails
                    }
                }

                // Create new medical record
                MedicalRecord newRecord = new MedicalRecord
                {
                    PatientId = patientId,
                    Diagnosis = diagnosis,
                    Treatment = treatment,
                    ConsultationDetails = consultationDetails,
                    ConsultationDate = DateTime.Now
                };

                // Save the new record to the database
                _dbConnection.AddPatientMedicalRecord(newRecord);
                Console.WriteLine("Medical record added successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error adding medical record: " + exception.Message);
            }
        }

        // Update an existing medical record for a patient
        public void UpdatePatientMedicalRecord()
        {
            int patientId = -1;
            while (patientId <= 0)
            {
                Console.WriteLine("Enter Patient ID to update medical record: ");
                string input = Console.ReadLine();
                try
                {
                    patientId = _validator.ValidateIntInput(input, "Patient ID");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Please try again.");
                }
            }

            _loggerService.LogInformation($"Updating medical record for Patient ID {patientId}");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Fetching Medical Records for Patient ID {patientId}:");
            Console.WriteLine("--------------------------------------------------");

            try
            {
                // Fetch all medical records for the patient
                List<MedicalRecord> medicalRecords = _dbConnection.GetMedicalRecordByPatientId(patientId);

                if (medicalRecords.Count > 0)
                {
                    // Display the records to the user
                    Console.WriteLine("Select a Medical Record to update:");
                    for (int i = 0; i < medicalRecords.Count; i++)
                    {
                        MedicalRecord record = medicalRecords[i];
                        Console.WriteLine($"{i + 1}. Record ID: {record.Id}, Diagnosis: {record.Diagnosis}, Date: {record.ConsultationDate.ToShortDateString()}");
                    }

                    int recordChoice = -1;
                    while (recordChoice < 1 || recordChoice > medicalRecords.Count)
                    {
                        Console.WriteLine("Enter the number of the record you want to update: ");
                        string inputChoice = Console.ReadLine();
                        try
                        {
                            recordChoice = _validator.ValidateIntInput(inputChoice, "Record Choice");
                            if (recordChoice < 1 || recordChoice > medicalRecords.Count)
                            {
                                Console.WriteLine($"Please enter a number between 1 and {medicalRecords.Count}");
                            }
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Invalid input. Please try again.");
                        }
                    }

                    // Fetch the selected medical record
                    MedicalRecord medicalRecord = medicalRecords[recordChoice - 1];

                    // Now prompt the user to update fields, validating input
                    string diagnosis = null;
                    while (string.IsNullOrWhiteSpace(diagnosis))
                    {
                        Console.WriteLine($"Current Diagnosis: {medicalRecord.Diagnosis}");
                        Console.WriteLine("Enter new Diagnosis (or press Enter to keep current): ");
                        string inputDiagnosis = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(inputDiagnosis))
                        {
                            // Keep the current value if the user presses Enter
                            diagnosis = medicalRecord.Diagnosis;
                        }
                        else
                        {
                            try
                            {
                                diagnosis = _validator.ValidateAlphaInput(inputDiagnosis, "Diagnosis");
                            }
                            catch (ArgumentException exception)
                            {
                                Console.WriteLine(exception.Message);
                            }
                        }
                    }

                    string treatment = null;
                    while (string.IsNullOrWhiteSpace(treatment))
                    {
                        Console.WriteLine($"Current Treatment: {medicalRecord.Treatment}");
                        Console.WriteLine("Enter new Treatment (or press Enter to keep current): ");
                        string inputTreatment = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(inputTreatment))
                        {
                            treatment = medicalRecord.Treatment;
                        }
                        else
                        {
                            try
                            {
                                treatment = _validator.ValidateAlphaInput(inputTreatment, "Treatment");
                            }
                            catch (ArgumentException exception)
                            {
                                Console.WriteLine(exception.Message);
                            }
                        }
                    }

                    string consultationDetails = null;
                    while (string.IsNullOrWhiteSpace(consultationDetails))
                    {
                        Console.WriteLine($"Current Consultation Details: {medicalRecord.ConsultationDetails}");
                        Console.WriteLine("Enter new Consultation Details (or press Enter to keep current): ");
                        string inputConsultationDetails = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(inputConsultationDetails))
                        {
                            consultationDetails = medicalRecord.ConsultationDetails;
                        }
                        else
                        {
                            try
                            {
                                consultationDetails = _validator.ValidateAlphaInput(inputConsultationDetails, "Consultation Details");
                            }
                            catch (ArgumentException exception)
                            {
                                Console.WriteLine(exception.Message);
                            }
                        }
                    }


                    // Update record in the database
                    medicalRecord.Diagnosis = diagnosis;
                    medicalRecord.Treatment = treatment;
                    medicalRecord.ConsultationDetails = consultationDetails;

                    _dbConnection.UpdatePatientMedicalRecord(medicalRecord);
                    Console.WriteLine("Medical record updated successfully.");
                }
                else
                {
                    Console.WriteLine("No medical records found for this patient.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error updating medical record: " + exception.Message);
            }
        }

    }
}
