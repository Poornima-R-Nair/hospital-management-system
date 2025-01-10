using HospitalManagementApp.Logging;
using HospitalManagementApp.Models;
using HospitalManagementApp.Utils;
using HospitalManagementApp.Exceptions;
using HospitalManagementApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagementApp.Services
{
    public class AdminService : IAdminService
    {
        private readonly ILoggerService _loggerService;
        private readonly IDBConnection _dbConnection;

        private readonly Validation _validator;

        public AdminService(ILoggerService loggerService, IDBConnection dbConnection, Validation validator)
        {
            _loggerService = loggerService;
            _dbConnection = dbConnection;
            _validator = validator;
        }

        // Methods for Doctor Management (Start)
        public void ViewAllDoctors()
        {
            _loggerService.LogInformation("Viewing all doctors");

            try
            {
                // Fetch all doctors from the database
                List<Doctor> doctors = _dbConnection.ViewAllDoctors();

                if (doctors == null)
                {
                    throw new InvalidOperationException("Failed to retrieve doctors from database");
                }

                if (doctors.Any())
                {
                    foreach (var doctor in doctors)
                    {
                        Console.WriteLine($"Doctor ID: {doctor.Id}, Name: {doctor.Name}, Specialization: {doctor.Specialization}");
                    }
                }
                else
                {
                    Console.WriteLine("No doctors are currently available in the system.");
                }
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Error fetching doctors: {exception.Message}");
                Console.WriteLine("Error fetching doctors.");
            }
        }

        public void AddDoctor()
        {
            _loggerService.LogInformation("Adding a new doctor");
            Console.WriteLine("Enter the details for the new doctor:");
            Console.WriteLine("-------------------------------------");

            string name = string.Empty;
            string specialization = string.Empty;
            string email = string.Empty;
            string username = string.Empty;
            string password = string.Empty;

            try
            {
                // Validate name
                bool nameValid = false;
                while (!nameValid)
                {
                    Console.Write("Enter the name of the doctor: ");
                    try
                    {
                        name = _validator.ValidateNameInput(Console.ReadLine() ?? string.Empty, "Name");
                        nameValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate specialization
                bool specializationValid = false;
                while (!specializationValid)
                {
                    Console.Write("Enter the specialization of the doctor: ");
                    try
                    {
                        specialization = _validator.ValidateSpecialization(Console.ReadLine() ?? string.Empty);
                        specializationValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate email
                bool emailValid = false;
                while (!emailValid)
                {
                    Console.Write("Enter the email of the doctor: ");
                    try
                    {
                        email = _validator.ValidateEmail(Console.ReadLine() ?? string.Empty);
                        emailValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate username
                bool usernameValid = false;
                while (!usernameValid)
                {
                    Console.Write("Enter the username of the doctor: ");
                    try
                    {
                        username = _validator.ValidateUsername(Console.ReadLine() ?? string.Empty);
                        usernameValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate password
                bool passwordValid = false;
                while (!passwordValid)
                {
                    Console.Write("Enter the password of the doctor: ");
                    try
                    {
                        password = _validator.ValidatePassword(Console.ReadLine() ?? string.Empty);
                        passwordValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Create a new Doctor object
                Doctor newDoctor = new Doctor
                {
                    Name = name,
                    Specialization = specialization,
                    Email = email,
                    Username = username,
                    Password = password
                };

                // Add doctor to the DBConnection
                _dbConnection.AddDoctor(newDoctor);

                Console.WriteLine("Doctor added successfully!");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred while adding the doctor: {exception.Message}");
            }
        }

        public void UpdateDoctor()
        {
            _loggerService.LogInformation("Updating a doctor");
            Console.WriteLine("Enter the ID of the doctor you want to update:");
            int doctorId = _validator.ValidateIntInput(Console.ReadLine() ?? string.Empty, "Doctor ID");

            if (doctorId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(doctorId), "Doctor ID must be greater than zero");
            }

            try
            {
                // Fetch doctor from the DBConnection
                List<Doctor> doctors = _dbConnection.ViewAllDoctors();
                Doctor doctorToUpdate = doctors.FirstOrDefault(d => d.Id == doctorId);

                if (doctorToUpdate == null)
                {
                    Console.WriteLine("Invalid doctor ID. Please enter a valid ID.");
                    return;
                }

                // Validate name (leave blank to keep current)
                bool nameValid = false;
                while (!nameValid)
                {
                    Console.WriteLine("\nEnter new name (leave blank to keep current):");
                    var name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        nameValid = true; // No change if left blank
                    }
                    else
                    {
                        try
                        {
                            name = _validator.ValidateNameInput(name, "Name");
                            doctorToUpdate.Name = name;
                            nameValid = true;
                        }
                        catch (ArgumentException exception)
                        {
                            Console.WriteLine($"Validation Error: {exception.Message}");
                        }
                    }
                }

                // Validate specialization (leave blank to keep current)
                bool specializationValid = false;
                while (!specializationValid)
                {
                    Console.WriteLine("\nEnter new specialization (leave blank to keep current):");
                    var specialization = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(specialization))
                    {
                        specializationValid = true; // No change if left blank
                    }
                    else
                    {
                        try
                        {
                            specialization = _validator.ValidateSpecialization(specialization);
                            doctorToUpdate.Specialization = specialization;
                            specializationValid = true;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Validation Error: {ex.Message}");
                        }
                    }
                }

                // Validate email (leave blank to keep current)
                bool emailValid = false;
                while (!emailValid)
                {
                    Console.WriteLine("\nEnter new email (leave blank to keep current):");
                    var email = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        emailValid = true; // No change if left blank
                    }
                    else
                    {
                        try
                        {
                            email = _validator.ValidateEmail(email);
                            doctorToUpdate.Email = email;
                            emailValid = true;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Validation Error: {ex.Message}");
                        }
                    }
                }

                // Validate username (leave blank to keep current)
                bool usernameValid = false;
                while (!usernameValid)
                {
                    Console.WriteLine("\nEnter new username (leave blank to keep current):");
                    var username = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(username))
                    {
                        usernameValid = true; // No change if left blank
                    }
                    else
                    {
                        if (doctors.Any(d => d.Username == username && d.Id != doctorId))
                        {
                            Console.WriteLine("Username already exists. Please choose a different username.");
                            continue; // Stay in the loop if username is invalid
                        }
                        doctorToUpdate.Username = username;
                        usernameValid = true;
                    }
                }

                // Validate password (leave blank to keep current)
                bool passwordValid = false;
                while (!passwordValid)
                {
                    Console.WriteLine("\nEnter new password (leave blank to keep current):");
                    var password = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        passwordValid = true; // No change if left blank
                    }
                    else
                    {
                        try
                        {
                            password = _validator.ValidatePassword(password);
                            doctorToUpdate.Password = password;
                            passwordValid = true;
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Validation Error: {ex.Message}");
                        }
                    }
                }

                // Update doctor in the DBConnection
                _dbConnection.UpdateDoctor(doctorToUpdate);

                _loggerService.LogInformation($"Edited Doctor: {doctorToUpdate.Name}, Specialization: {doctorToUpdate.Specialization}");
                Console.WriteLine("Doctor information updated successfully.");
            }
            catch (Exception ex)
            {
                _loggerService.LogError($"Error updating doctor: {ex.Message}");
                Console.WriteLine("Error updating doctor.");
            }
        }

        public void DeleteDoctor()
        {
            _loggerService.LogInformation("Deleting a doctor");
            try
            {
                Console.WriteLine("Enter the ID of the doctor you want to delete:");
                int doctorId = _validator.ValidateIntInput(Console.ReadLine() ?? string.Empty, "Doctor ID");

                // Check if doctor exists in the DBConnection
                List<Doctor> doctors = _dbConnection.ViewAllDoctors();
                Doctor doctorToDelete = doctors.FirstOrDefault(d => d.Id == doctorId);

                if (doctorToDelete == null)
                {
                    throw new HospitalManagementException($"Doctor with ID {doctorId} not found");
                }

                // Check if doctor has pending appointments in the DBConnection
                // Replace the following line with the actual check for pending appointments.
                List<Appointment> appointments = _dbConnection.GetAllAppointments();
                if (appointments.Any(a => a.DoctorId == doctorId && a.Status == "Pending"))
                {
                    throw new HospitalManagementException("Cannot delete doctor with pending appointments");
                }

                // Delete doctor from the DBConnection
                _dbConnection.DeleteDoctor(doctorId);

                _loggerService.LogInformation($"Doctor {doctorToDelete.Name} deleted successfully");
                Console.WriteLine("Doctor deleted successfully.");
            }
            catch (HospitalManagementException hospitalmanagementexception)
            {
                _loggerService.LogError($"Hospital Management Error: {hospitalmanagementexception.Message}");
                Console.WriteLine($"Error: {hospitalmanagementexception.Message}");
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Unexpected error while deleting doctor: {exception.Message}");
                Console.WriteLine("Failed to delete doctor.");
            }
        }
        // Methods for Doctor Management (End)

        // Methods for Patient Management (Start)
        public void ViewAllPatients()
        {
            _loggerService.LogInformation("Viewing all patients");

            try
            {
                var patients = _dbConnection.GetAllPatients();
                if (patients.Any())
                {
                    foreach (var patient in patients)
                    {
                        Console.WriteLine($"Patient ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}, Email: {patient.Email}");
                    }
                }
                else
                {
                    Console.WriteLine("No patients are currently available in the system.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }
        }

        public void AddPatient()
        {
            _loggerService.LogInformation("Adding a new patient");
            Console.WriteLine("Enter the details for the new patient:");
            Console.WriteLine("-------------------------------------");

            try
            {
                // Validate Name
                bool nameValid = false;
                string name = string.Empty;
                while (!nameValid)
                {
                    Console.Write("Enter the name of the patient: ");
                    try
                    {
                        name = _validator.ValidateNameInput(Console.ReadLine() ?? string.Empty, "Name");
                        nameValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate Age
                bool ageValid = false;
                int age = 0;
                while (!ageValid)
                {
                    Console.Write("Enter the age of the patient: ");
                    string input = Console.ReadLine() ?? string.Empty;
                    if (int.TryParse(input, out age))
                    {
                        try
                        {
                            age = _validator.ValidateAge(age);
                            ageValid = true;
                        }
                        catch (ArgumentException exception)
                        {
                            Console.WriteLine($"Validation Error: {exception.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid age.");
                    }
                }

                // Validate Gender
                bool genderValid = false;
                string gender = string.Empty;
                while (!genderValid)
                {
                    Console.Write("Enter the gender of the patient: ");
                    try
                    {
                        gender = _validator.ValidateGender(Console.ReadLine() ?? string.Empty);
                        genderValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate Email
                bool emailValid = false;
                string email = string.Empty;
                while (!emailValid)
                {
                    Console.Write("Enter the email of the patient: ");
                    try
                    {
                        email = _validator.ValidateEmail(Console.ReadLine() ?? string.Empty);
                        emailValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate Address
                bool addressValid = false;
                string address = string.Empty;
                while (!addressValid)
                {
                    Console.Write("Enter the address of the patient: ");
                    try
                    {
                        address = _validator.ValidateStringInput(Console.ReadLine() ?? string.Empty, "Address");
                        addressValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate Date of Birth
                bool dobValid = false;
                DateTime dateOfBirth = DateTime.MinValue;
                while (!dobValid)
                {
                    Console.Write("Enter the date of birth of the patient (yyyy-mm-dd): ");
                    try
                    {
                        dateOfBirth = _validator.ValidateDateTimeInput(Console.ReadLine() ?? string.Empty, "Date of Birth");
                        dobValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate Username
                bool usernameValid = false;
                string username = string.Empty;
                while (!usernameValid)
                {
                    Console.Write("Enter the username of the patient: ");
                    try
                    {
                        username = _validator.ValidateStringInput(Console.ReadLine() ?? string.Empty, "Username");
                        usernameValid = true;
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Create and add the patient to the database
                Patient newPatient = new Patient
                {
                    Name = name,
                    Age = age,
                    Gender = gender,
                    Email = email,
                    Address = address,
                    DateOfBirth = dateOfBirth,
                    Username = username,
                };

                _dbConnection.AddPatient(newPatient);  // Add the patient to the database
                Console.WriteLine("Patient added successfully!");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }
        }

        public void UpdatePatient()
        {
            _loggerService.LogInformation("Updating a patient");
            Console.WriteLine("Enter the ID of the patient you want to update:");
            int patientId = _validator.ValidateIntInput(Console.ReadLine() ?? string.Empty, "Patient ID");

            try
            {
                var patients = _dbConnection.GetAllPatients();
                Patient? patientToUpdate = patients.FirstOrDefault(p => p.Id == patientId);

                if (patientToUpdate == null)
                {
                    Console.WriteLine("Invalid patient ID. Please enter a valid ID.");
                    return;
                }

                Console.WriteLine("\nEnter new name (leave blank to keep current):");
                var name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) patientToUpdate.Name = name;

                Console.WriteLine("\nEnter new age (leave blank to keep current):");
                var ageInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(ageInput)) patientToUpdate.Age = Convert.ToInt32(ageInput);

                Console.WriteLine("\nEnter new gender (leave blank to keep current):");
                var gender = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(gender)) patientToUpdate.Gender = gender;

                Console.WriteLine("\nEnter new email (leave blank to keep current):");
                var email = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(email)) patientToUpdate.Email = email;

                Console.WriteLine("\nEnter new address (leave blank to keep current):");
                var address = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(address)) patientToUpdate.Address = address;

                Console.WriteLine("\nEnter new date of birth (leave blank to keep current):");
                var dateOfBirth = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(dateOfBirth))
                {
                    try
                    {
                        patientToUpdate.DateOfBirth = _validator.ValidateDateTimeInput(dateOfBirth, "Date of Birth");
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Invalid date format. The patient's date of birth was not updated.");
                        return;
                    }
                }

                Console.WriteLine("\nEnter new username (leave blank to keep current):");
                var username = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(username)) patientToUpdate.Username = username;


                _dbConnection.UpdatePatient(patientToUpdate);

                _loggerService.LogInformation($"Edited Patient: {patientToUpdate.Name}, Age: {patientToUpdate.Age}");
                Console.WriteLine("Patient information updated successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }
        }

        public void DeletePatient()
        {
            _loggerService.LogInformation("Deleting a patient");
            try
            {
                Console.WriteLine("Enter the ID of the patient you want to delete:");
                int patientId = _validator.ValidateIntInput(Console.ReadLine() ?? string.Empty, "Patient ID");

                var patients = _dbConnection.GetAllPatients();
                Patient? patientToDelete = patients.FirstOrDefault(p => p.Id == patientId);

                if (patientToDelete == null)
                {
                    Console.WriteLine("Patient with the provided ID not found.");
                    return;
                }

                _dbConnection.DeletePatient(patientId);  // Delete the patient from the database

                _loggerService.LogInformation($"Patient {patientToDelete.Name} deleted successfully");
                Console.WriteLine("Patient deleted successfully.");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message}");
            }
        }
        // Methods for Patient Management (End)


        // Methods for Appointment Management (Start)
        public void ViewAllAppointments()
        {
            _loggerService.LogInformation("Viewing all appointments");

            // Fetch appointments, patients, and doctors from the database
            List<Appointment> appointments = _dbConnection.GetAllAppointments();
            List<Patient> patients = _dbConnection.GetAllPatients();
            List<Doctor> doctors = _dbConnection.ViewAllDoctors();

            if (appointments.Any())
            {
                foreach (var appointment in appointments)
                {
                    // Retrieve patient and doctor details from the fetched lists
                    var patient = patients.FirstOrDefault(p => p.Id == appointment.PatientId);
                    var doctor = doctors.FirstOrDefault(d => d.Id == appointment.DoctorId);

                    // Displaying appointment details
                    Console.WriteLine($"Appointment ID: {appointment.Id}, " + $"Patient: {patient?.Name ?? "Unknown"}, " + $"Doctor: {doctor?.Name ?? "Unknown"}, " + $"Date: {appointment.Date.ToShortDateString()}, " + $"Time: {appointment.Time}, " + $"Status: {appointment.Status}");
                }
            }
            else
            {
                Console.WriteLine("No appointments found.");
            }
        }

        public void AddAppointment()
        {
            _loggerService.LogInformation("Adding a new appointment");
            Console.WriteLine("Enter the details for the new appointment:");

            try
            {
                // Validate Patient ID
                bool patientIdValid = false;
                int patientId = 0;
                while (!patientIdValid)
                {
                    Console.Write("Enter patient ID: ");
                    string input = Console.ReadLine() ?? string.Empty;
                    try
                    {
                        patientId = _validator.ValidateIntInput(input, "Patient ID");
                        patientIdValid = true;  // Exit loop if valid input
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                // Validate Doctor ID
                bool doctorIdValid = false;
                int doctorId = 0;
                while (!doctorIdValid)
                {
                    Console.Write("Enter doctor ID: ");
                    string input = Console.ReadLine() ?? string.Empty;
                    try
                    {
                        doctorId = _validator.ValidateIntInput(input, "Doctor ID");
                        doctorIdValid = true;  // Exit loop if valid input
                    }
                    catch (ArgumentException exception)
                    {
                        Console.WriteLine($"Validation Error: {exception.Message}");
                    }
                }

                var patients = _dbConnection.GetAllPatients();
                var doctors = _dbConnection.ViewAllDoctors();

                var patient = patients.FirstOrDefault(p => p.Id == patientId);
                var doctor = doctors.FirstOrDefault(d => d.Id == doctorId);

                if (patient == null || doctor == null)
                {
                    Console.WriteLine("Invalid patient or doctor ID.");
                    return;
                }

                // Validate Appointment Date
                bool dateValid = false;
                DateTime date = DateTime.MinValue;
                while (!dateValid)
                {
                    Console.Write("Enter appointment date (yyyy-mm-dd): ");
                    string input = Console.ReadLine() ?? string.Empty;
                    if (DateTime.TryParse(input, out date))
                    {
                        try
                        {
                            date = _validator.ValidateAppointmentDate(date);  // Pass the parsed DateTime to ValidateAppointmentDate
                            dateValid = true;  // Exit loop if valid input
                        }
                        catch (ArgumentException exception)
                        {
                            Console.WriteLine($"Validation Error: {exception.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter a valid date (yyyy-mm-dd).");
                    }
                }

                // Validate Appointment Time
                bool timeValid = false;
                TimeSpan appointmentTime = TimeSpan.Zero;
                while (!timeValid)
                {
                    Console.Write("Enter appointment time (HH:mm): ");
                    string timeInput = Console.ReadLine() ?? string.Empty;
                    try
                    {
                        appointmentTime = _validator.ValidateAppointmentTime(TimeSpan.Parse(timeInput));
                        timeValid = true;  // Exit loop if valid input
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid time format. Please enter a valid time in the format HH:mm.");
                    }
                }

                // Create new appointment object
                Appointment newAppointment = new Appointment
                {
                    PatientId = patientId,
                    DoctorId = doctorId,
                    Date = date,
                    Time = appointmentTime,
                    Status = "Scheduled"
                };

                // Insert into database using the AddAppointment method
                _dbConnection.AddAppointment(newAppointment);

                Console.WriteLine("Appointment added successfully!");
            }
            catch (Exception exception)
            {
                throw new HospitalManagementException($"Admin operation failed: {exception.Message}");
            }
        }

        public void UpdateAppointment()
        {
            _loggerService.LogInformation("Updating an appointment");
            Console.WriteLine("Enter the ID of the appointment you want to update:");

            List<Appointment> appointments = _dbConnection.GetAllAppointments();

            // Validate Appointment ID
            bool appointmentIdValid = false;
            int appointmentId = 0;
            while (!appointmentIdValid)
            {
                Console.Write("Enter appointment ID: ");
                string input = Console.ReadLine() ?? string.Empty;
                try
                {
                    appointmentId = _validator.ValidateIntInput(input, "Appointment ID");
                    appointmentIdValid = true;  // Exit loop if valid input
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine($"Validation Error: {exception.Message}");
                }
            }

            var appointmentToUpdate = appointments.FirstOrDefault(a => a.Id == appointmentId);

            if (appointmentToUpdate == null)
            {
                Console.WriteLine("Invalid appointment ID.");
                return;
            }

            // Validate Status
            bool statusValid = false;
            string status = string.Empty;
            while (!statusValid)
            {
                Console.WriteLine("\nEnter new status (leave blank to keep current):");
                status = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(status) || status.Length > 0)
                {
                    statusValid = true;
                }
                else
                {
                    Console.WriteLine("Status cannot be empty.");
                }
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                appointmentToUpdate.Status = status;
            }

            // Update the appointment in the database
            _dbConnection.UpdateAppointment(appointmentToUpdate);

            Console.WriteLine("Appointment information updated successfully.");
        }

        public void DeleteAppointment()
        {
            _loggerService.LogInformation("Deleting an appointment");
            try
            {
                Console.WriteLine("Enter the ID of the appointment you want to delete:");

                List<Appointment> appointments = _dbConnection.GetAllAppointments();

                int appointmentId = _validator.ValidateIntInput(Console.ReadLine() ?? string.Empty, "Appointment ID");
                var appointmentToDelete = appointments.FirstOrDefault(a => a.Id == appointmentId);

                if (appointmentToDelete == null)
                {
                    throw new HospitalManagementException($"Appointment with ID {appointmentId} not found.");
                }

                // Delete the appointment from the database
                _dbConnection.DeleteAppointment(appointmentId);

                Console.WriteLine("Appointment deleted successfully.");
            }
            catch (HospitalManagementException hospitalmanagementexception)
            {
                _loggerService.LogError($"Hospital Management Error: {hospitalmanagementexception.Message}");
                Console.WriteLine($"Error: {hospitalmanagementexception.Message}");
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Unexpected error while deleting appointment: {exception.Message}");
                throw new HospitalManagementException("Failed to delete appointment.");
            }
        }
        // Methods for Appointment Management (End)


        // Methods for Equipment Management (Start)
        public void ViewAllEquipments()
        {
            _loggerService.LogInformation("Viewing all equipment");
            var equipments = _dbConnection.ViewAllEquipments(); // Fetch equipment from the database

            if (equipments == null)
            {
                throw new NullReferenceException("Equipment list cannot be null");
            }

            if (equipments.Any())
            {
                foreach (var equipment in equipments)
                {
                    Console.WriteLine($"Equipment ID: {equipment.Id}, Name: {equipment.Name}, Description: {equipment.Description}, Stock: {equipment.Stock}");
                }
            }
            else
            {
                Console.WriteLine("No equipment found.");
            }
        }

        public void AddEquipment()
        {
            _loggerService.LogInformation("Adding new equipment");
            Console.WriteLine("Enter the details for the new equipment:");

            try
            {
                Console.Write("Enter equipment name: ");
                string? name = _validator.ValidateStringInput(Console.ReadLine() ?? string.Empty, "Equipment Name");

                Console.Write("Enter equipment description: ");
                string? description = _validator.ValidateStringInput(Console.ReadLine() ?? string.Empty, "Equipment Description");

                Console.Write("Enter equipment stock: ");
                string input = Console.ReadLine() ?? string.Empty;

                // Try to parse the input to an integer
                if (int.TryParse(input, out int stock))
                {
                    stock = _validator.ValidateAge(stock);  // Pass the parsed age to the ValidateAge method
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a stock.");
                }

                Equipment newEquipment = new Equipment
                {
                    Name = name,
                    Description = description,
                    Stock = stock // Default to 0 if stock is null
                };

                // Add the equipment to the database
                _dbConnection.AddEquipment(newEquipment);

                Console.WriteLine("Equipment added successfully!");
            }
            catch (Exception exception)
            {
                throw new HospitalManagementException($"Admin operation failed: {exception.Message}");
            }
        }

        public void DeleteEquipment()
        {
            _loggerService.LogInformation("Deleting an equipment");
            try
            {
                Console.WriteLine("Enter the ID of the equipment you want to delete:");

                int equipmentId = _validator.ValidateIntInput(Console.ReadLine() ?? string.Empty, "Equipment ID");

                // Delete the equipment from the database
                _dbConnection.DeleteEquipment(equipmentId);

                Console.WriteLine("Equipment deleted successfully.");
            }
            catch (Exception exception)
            {
                _loggerService.LogError($"Unexpected error while deleting equipment: {exception.Message}");
                throw new HospitalManagementException("Failed to delete equipment.");
            }
        }
    }
}