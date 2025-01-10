using System;
using HospitalManagementApp.Models;
using System.Data.SqlClient;
using System.Data;

namespace HospitalManagementApp.Data
{
    class DBConnection : IDBConnection
    {
        private readonly string _connectionString;

        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Doctor> ViewAllDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            string storedProcedureName = "GetAllDoctors";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(storedProcedureName, connection);
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        doctors.Add(new Doctor
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Specialization = reader["Specialization"].ToString(),
                            Email = reader["Email"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                        });
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error fetching doctors: " + exception.Message);
                }
            }

            return doctors;
        }


        public void AddDoctor(Doctor doctor)
        {
            string storedProcedureName = "AddDoctor";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(storedProcedureName, connection);
                command.CommandType = CommandType.StoredProcedure;

                // Adding parameters
                command.Parameters.AddWithValue("@Name", doctor.Name);
                command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                command.Parameters.AddWithValue("@Email", doctor.Email);
                command.Parameters.AddWithValue("@Username", doctor.Username);
                command.Parameters.AddWithValue("@Password", doctor.Password);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Doctor added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add doctor.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }


        public void UpdateDoctor(Doctor doctor)
        {
            string storedProcedureName = "UpdateDoctor";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(storedProcedureName, connection);
                command.CommandType = CommandType.StoredProcedure;

                // Add parameters
                command.Parameters.AddWithValue("@Id", doctor.Id);
                command.Parameters.AddWithValue("@Name", doctor.Name);
                command.Parameters.AddWithValue("@Specialization", doctor.Specialization);
                command.Parameters.AddWithValue("@Email", doctor.Email);
                command.Parameters.AddWithValue("@Username", doctor.Username);
                command.Parameters.AddWithValue("@Password", doctor.Password);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Doctor updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update doctor.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }


        public void DeleteDoctor(int doctorId)
        {
            string procedureName = "DeleteDoctor";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@DoctorId", doctorId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Doctor deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete doctor.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }


        // ---------------------------------------
        // ---------------PATIENT-----------------
        // ---------------------------------------
        public List<Patient> GetAllPatients()
        {
            List<Patient> patients = new List<Patient>();
            string procedureName = "GetAllPatients";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        patients.Add(new Patient
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            Username = reader["Username"].ToString(),
                            Address = reader["Address"].ToString(),
                            Gender = reader["Gender"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        });
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error fetching patients: " + exception.Message);
                }
            }

            return patients;
        }

        // Still this stored procedure
        public void AddPatient(Patient patient)
        {
            string procedureName = "AddPatient";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(procedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Name", patient.Name);
                command.Parameters.AddWithValue("@Email", patient.Email);
                command.Parameters.AddWithValue("@Username", patient.Username);
                command.Parameters.AddWithValue("@Address", patient.Address);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@Age", patient.Age);
                command.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Patient added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add patient.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }


        public void UpdatePatient(Patient patient)
        {
            string query = @"UPDATE Patients 
                     SET Name = @Name, Email = @Email, Address = @Address, Gender = @Gender, 
                         Age = @Age, DateOfBirth = @DateOfBirth, Username = @Username
                     WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", patient.Id);
                command.Parameters.AddWithValue("@Name", patient.Name);
                command.Parameters.AddWithValue("@Email", patient.Email);
                command.Parameters.AddWithValue("@Address", patient.Address);
                command.Parameters.AddWithValue("@Username", patient.Username);
                command.Parameters.AddWithValue("@Gender", patient.Gender);
                command.Parameters.AddWithValue("@Age", patient.Age);
                command.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Patient updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update patient.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        public void DeletePatient(int patientId)
        {
            string query = "DELETE FROM Patients WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", patientId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Patient deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete patient.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        // ---------------------------------------
        // -------------APPOINTMENT---------------
        // ---------------------------------------

        public List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            string query = "SELECT Id, PatientId, DoctorId, Date, Time, Status FROM Appointments"; // Assuming Appointments table

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PatientId = reader["PatientId"] != DBNull.Value ? Convert.ToInt32(reader["PatientId"]) : (int?)null,
                            DoctorId = reader["DoctorId"] != DBNull.Value ? Convert.ToInt32(reader["DoctorId"]) : (int?)null,
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = reader["Time"] != DBNull.Value ? (TimeSpan)reader["Time"] : TimeSpan.Zero,
                            Status = reader["Status"]?.ToString(),
                        });
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error fetching appointments: " + exception.Message);
                }
            }

            return appointments;
        }

        public void AddAppointment(Appointment appointment)
        {
            string query = @"INSERT INTO Appointments (PatientId, DoctorId, Date, Time, Status) 
                     VALUES (@PatientId, @DoctorId, @Date, @Time, @Status)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PatientId", (object?)appointment.PatientId ?? DBNull.Value);
                command.Parameters.AddWithValue("@DoctorId", (object?)appointment.DoctorId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Date", appointment.Date);
                command.Parameters.AddWithValue("@Time", appointment.Time);
                command.Parameters.AddWithValue("@Status", (object?)appointment.Status ?? DBNull.Value);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Appointment added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add appointment.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        public void UpdateAppointment(Appointment appointment)
        {
            string query = @"UPDATE Appointments 
                         SET PatientId = @PatientId, DoctorId = @DoctorId, Date = @Date, Time = @Time, Status = @Status
                         WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", appointment.Id);
                command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                command.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                command.Parameters.AddWithValue("@Date", appointment.Date);
                command.Parameters.AddWithValue("@Time", appointment.Time);
                command.Parameters.AddWithValue("@Status", appointment.Status);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Appointment updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update appointment.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        public void DeleteAppointment(int appointmentId)
        {
            string query = "DELETE FROM Appointments WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", appointmentId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Appointment deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete appointment.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        // ---------------------------------------
        // -------------EQUIPMENTS---------------
        // ---------------------------------------

        public List<Equipment> ViewAllEquipments()
        {
            List<Equipment> equipments = new List<Equipment>();
            string query = "SELECT Id,Name,Description,Stock FROM Equipments"; // Assuming Equipments table

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        equipments.Add(new Equipment
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Stock = Convert.ToInt32(reader["Stock"]),
                        });
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error fetching equipments: " + exception.Message);
                }
            }

            return equipments;
        }

        public void AddEquipment(Equipment equipment)
        {
            string query = @"INSERT INTO Equipments (Name, Description, Stock) 
                         VALUES (@Name, @Description, @Stock)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Name", equipment.Name);
                command.Parameters.AddWithValue("@Description", equipment.Description);
                command.Parameters.AddWithValue("@Stock", equipment.Stock);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Equipment added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add equipment.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        public void DeleteEquipment(int equipmentId)
        {
            string query = "DELETE FROM Equipments WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", equipmentId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Equipment deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete equipment.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        // ---------------------------------------
        // -------------Doctor Service-----------------
        // ---------------------------------------
        public List<Appointment> ViewAppointmentsByDoctor(int doctorId)
        {
            List<Appointment> appointments = new List<Appointment>();
            string query = @"
        SELECT 
            Id, PatientId, DoctorId, Date, Time, Status
        FROM 
            Appointments
        WHERE 
            DoctorId = @DoctorId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DoctorId", doctorId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            DoctorId = Convert.ToInt32(reader["DoctorId"]),
                            Date = Convert.ToDateTime(reader["Date"]),
                            Time = (TimeSpan)reader["Time"],
                            Status = reader["Status"].ToString()
                        });
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error fetching appointments: " + exception.Message);
                }
            }

            return appointments; // Return the list of appointments
        }

        public MedicalRecord ViewPatientMedicalRecord(int patientId)
        {
            MedicalRecord medicalRecord = null;
            string query = @"SELECT Id, PatientId, Diagnosis, Treatment, ConsultationDetails, ConsultationDate 
                     FROM MedicalRecords
                     WHERE PatientId = @PatientId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PatientId", patientId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        medicalRecord = new MedicalRecord
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            Diagnosis = reader["Diagnosis"].ToString(),
                            Treatment = reader["Treatment"].ToString(),
                            ConsultationDetails = reader["ConsultationDetails"].ToString(),
                            ConsultationDate = Convert.ToDateTime(reader["ConsultationDate"])
                        };
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error fetching medical record: " + exception.Message);
                }
            }

            return medicalRecord;
        }

        public void AddPatientMedicalRecord(MedicalRecord record)
        {
            string query = @"INSERT INTO MedicalRecords (PatientId, Diagnosis, Treatment, ConsultationDetails, ConsultationDate) 
                     VALUES (@PatientId, @Diagnosis, @Treatment, @ConsultationDetails, @ConsultationDate)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PatientId", record.PatientId);
                command.Parameters.AddWithValue("@Diagnosis", record.Diagnosis);
                command.Parameters.AddWithValue("@Treatment", record.Treatment);
                command.Parameters.AddWithValue("@ConsultationDetails", record.ConsultationDetails);
                command.Parameters.AddWithValue("@ConsultationDate", record.ConsultationDate);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Medical record added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add medical record.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        public void UpdatePatientMedicalRecord(MedicalRecord record)
        {
            string query = @"UPDATE MedicalRecords 
                     SET Diagnosis = @Diagnosis, Treatment = @Treatment, ConsultationDetails = @ConsultationDetails, ConsultationDate = @ConsultationDate
                     WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", record.Id);
                command.Parameters.AddWithValue("@Diagnosis", record.Diagnosis);
                command.Parameters.AddWithValue("@Treatment", record.Treatment);
                command.Parameters.AddWithValue("@ConsultationDetails", record.ConsultationDetails);
                command.Parameters.AddWithValue("@ConsultationDate", record.ConsultationDate);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Medical record updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update medical record.");
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error: " + exception.Message);
                }
            }
        }

        public List<MedicalRecord> GetMedicalRecordByPatientId(int patientId)
        {
            List<MedicalRecord> medicalRecords = new List<MedicalRecord>();

            string query = @"SELECT Id, PatientId, Diagnosis, Treatment, ConsultationDetails, ConsultationDate 
                     FROM MedicalRecords
                     WHERE PatientId = @PatientId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PatientId", patientId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        MedicalRecord medicalRecord = new MedicalRecord
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            PatientId = Convert.ToInt32(reader["PatientId"]),
                            Diagnosis = reader["Diagnosis"].ToString(),
                            Treatment = reader["Treatment"].ToString(),
                            ConsultationDetails = reader["ConsultationDetails"].ToString(),
                            ConsultationDate = Convert.ToDateTime(reader["ConsultationDate"])
                        };
                        medicalRecords.Add(medicalRecord);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error fetching medical records: " + exception.Message);
                }
            }

            return medicalRecords;
        }

        public Patient GetPatientById(int patientId)
        {
            Patient patient = null;

            string query = "SELECT Id, Name, Email, Username, Address, Gender, Age, DateOfBirth FROM Patients WHERE Id = @PatientId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PatientId", patientId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    patient = new Patient
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"] as string,
                        Email = reader["Email"] as string,
                        Username = reader["Username"] as string,
                        Address = reader["Address"] as string,
                        Gender = reader["Gender"] as string,
                        Age = Convert.ToInt32(reader["Age"]),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"])
                    };
                }
            }

            return patient;
        }

        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            List<Appointment> appointments = new List<Appointment>();

            string query = "SELECT Id, PatientId, DoctorId, Date, Time, Status FROM Appointments WHERE PatientId = @PatientId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PatientId", patientId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    appointments.Add(new Appointment
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        PatientId = reader["PatientId"] as int?,
                        DoctorId = reader["DoctorId"] as int?,
                        Date = Convert.ToDateTime(reader["Date"]),
                        Time = TimeSpan.Parse(reader["Time"].ToString()),
                        Status = reader["Status"] as string
                    });
                }
            }

            return appointments;
        }

        public List<MedicalRecord> GetMedicalRecordsByPatientId(int patientId)
        {
            List<MedicalRecord> medicalRecords = new List<MedicalRecord>();
            string query = "SELECT Id, PatientId, Diagnosis, Treatment, ConsultationDetails, ConsultationDate FROM MedicalRecords WHERE PatientId = @PatientId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PatientId", patientId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    medicalRecords.Add(new MedicalRecord
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        PatientId = Convert.ToInt32(reader["PatientId"]),
                        Diagnosis = reader["Diagnosis"].ToString(),
                        Treatment = reader["Treatment"].ToString(),
                        ConsultationDetails = reader["ConsultationDetails"].ToString(),
                        ConsultationDate = Convert.ToDateTime(reader["ConsultationDate"])
                    });
                }
            }

            return medicalRecords;
        }

        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            string query = "SELECT Id, Name, Specialization, Email, Username FROM Doctors";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new Doctor
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Specialization = reader["Specialization"].ToString()
                    });
                }
            }

            return doctors;
        }

    }
}