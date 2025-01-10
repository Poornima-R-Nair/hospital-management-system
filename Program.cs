
// Title : Hospital Management System Application
// Author: Poornima R Nair
// Created at : 22/12/2024
// Updated at : 26/12/2024
// Reviewed by : Sabapathi Shanmugam
// Reviewed at : 23/12/2024

using HospitalManagementApp.Services;
using HospitalManagementApp.Logging;
using HospitalManagementApp.Models;
using HospitalManagementApp.Exceptions;
using HospitalManagementApp.Data;
using HospitalManagementApp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace HospitalManagementApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                var authService = host.Services.GetRequiredService<IAuthService>();
                var doctorService = host.Services.GetRequiredService<IDoctorService>();


                Console.WriteLine("------------------------------------------");
                Console.WriteLine("Welcome to the Hospital Management System!");
                Console.WriteLine("------------------------------------------");

                string? role = null;
                string? username = null;
                bool isRunning = true;

                while (isRunning)
                {
                    try
                    {
                        // If not logged in, prompt for login
                        while (role == null)
                        {
                            Console.WriteLine("Please login to continue:");
                            Console.WriteLine("-------------------------");
                            Console.WriteLine("-------------------------");

                            Console.Write("Enter your username: ");
                            string? enteredUsername = ValidateStringInput(Console.ReadLine() ?? string.Empty, "Username");

                            string? enteredPassword = null;
                            bool isPasswordValid = false;

                            while (!isPasswordValid)
                            {
                                try
                                {
                                    Console.Write("Enter your password: ");
                                    enteredPassword = ValidatePassword(ValidateStringInput(HidePasswordInput(), "Password"));
                                    isPasswordValid = true;
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            Dictionary<string, string>? authResult = authService.Authenticate(enteredUsername, enteredPassword);

                            if (authResult != null)
                            {
                                role = authResult["Role"];
                                username = authResult["Username"];
                                Console.WriteLine($"Login successful! Welcome, {role}!");
                            }
                            else
                            {
                                Console.WriteLine("Invalid username or password. Please try again.");
                            }
                        }

                        // Perform actions based on the user's role
                        switch (role)
                        {
                            case "Admin":
                                AdminMenu(host, ref role);
                                break;
                            case "Doctor":
                                DoctorMenu(host, ref role);
                                break;
                            default:
                                Console.WriteLine("Invalid role.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new HospitalManagementException($"Operation failed: {ex.Message}");
                    }
                }

                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Thank you for using the Hospital Management System!");
                Console.WriteLine("---------------------------------------------------");
            }
            catch (Exception ex)
            {
                throw new HospitalManagementException($"Application startup error: {ex.Message}");
            }
        }

        private static string HidePasswordInput()
        {
            string password = string.Empty;
            while (true)
            {
                var key = Console.ReadKey(true); // true to hide the key
                if (key.Key == ConsoleKey.Enter)
                    break;
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b"); // Backspace behavior
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    password += key.KeyChar;
                    Console.Write('*'); // Show * instead of the actual character
                }
            }
            return password;
        }

        public static void AdminMenu(IHost host, ref string? role)
        {
            var adminService = host.Services.GetRequiredService<IAdminService>();
            bool isRunning = true;

            while (isRunning)
            {
                try
                {
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("Admin Menu");
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("1. View All Patients");
                    Console.WriteLine("2. Add Patient");
                    Console.WriteLine("3. Edit Patient");
                    Console.WriteLine("4. Delete Patient");
                    Console.WriteLine("5. View All Doctors");
                    Console.WriteLine("6. Add Doctor");
                    Console.WriteLine("7. Edit Doctor");
                    Console.WriteLine("8. Delete Doctor");
                    Console.WriteLine("9. Add Appointment");
                    Console.WriteLine("10. View All Appointments");
                    Console.WriteLine("11. Update Appointments");
                    Console.WriteLine("12. Cancel Appointments");
                    Console.WriteLine("13. View All Equipments");
                    Console.WriteLine("14. Add Equipments");
                    Console.WriteLine("15. Delete Equipment");
                    Console.WriteLine("0. Logout");
                    Console.WriteLine("------------------------------------------");

                    Console.Write("Enter your choice: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            adminService.ViewAllPatients();
                            break;
                        case "2":
                            adminService.AddPatient();
                            break;
                        case "3":
                            adminService.UpdatePatient();
                            break;
                        case "4":
                            adminService.DeletePatient();
                            break;
                        case "5":
                            adminService.ViewAllDoctors();
                            break;
                        case "6":
                            adminService.AddDoctor();
                            break;
                        case "7":
                            adminService.UpdateDoctor();
                            break;
                        case "8":
                            adminService.DeleteDoctor();
                            break;
                        case "9":
                            adminService.AddAppointment();
                            break;
                        case "10":
                            adminService.ViewAllAppointments();
                            break;
                        case "11":
                            adminService.UpdateAppointment();
                            break;
                        case "12":
                            adminService.DeleteAppointment();
                            break;
                        case "13":
                            adminService.ViewAllEquipments();
                            break;
                        case "14":
                            adminService.AddEquipment();
                            break;
                        case "15":
                            adminService.DeleteEquipment();
                            break;
                        case "0":
                            isRunning = false;
                            role = null;
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new HospitalManagementException($"Admin operation failed: {ex.Message}");
                }
            }
        }

        public static void DoctorMenu(IHost host, ref string? role)
        {
            var doctorService = host.Services.GetRequiredService<IDoctorService>();
            bool isRunning = true;

            while (isRunning)
            {
                try
                {
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("Doctor Menu");
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("1. View Appointments");
                    Console.WriteLine("2. View Patient Medical Record");
                    Console.WriteLine("3. Add Patient Medical Record");
                    Console.WriteLine("4. Update Patient Medical Record");
                    Console.WriteLine("0. Logout");
                    Console.WriteLine("------------------------------------------");

                    Console.Write("Enter your choice: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            doctorService.ViewAllAppointments();
                            break;
                        case "2":
                            doctorService.ViewPatientMedicalRecord();
                            break;
                        case "3":
                            doctorService.AddPatientMedicalRecord();
                            break;
                        case "4":
                            doctorService.UpdatePatientMedicalRecord();
                            break;
                        case "0":
                            isRunning = false;
                            role = null;
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new HospitalManagementException($"Doctor operation failed: {ex.Message}");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddServicesAndDatabase(context.Configuration);
                });

        public static string ValidateStringInput(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException($"{fieldName} cannot be empty.");
            }

            return input.Trim();
        }

        public static int ValidateIntInput(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException($"{fieldName} cannot be empty.");
            }

            if (!int.TryParse(input, out int result))
            {
                throw new ArgumentException($"{fieldName} must be a valid integer.");
            }

            return result;
        }

        private static string ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                Console.WriteLine("Password must be at least 8 characters long.");
                throw new ArgumentException("Password is too short.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]"))
            {
                Console.WriteLine("Password must contain at least one uppercase letter.");
                throw new ArgumentException("Password must contain at least one uppercase letter.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"\d"))
            {
                Console.WriteLine("Password must contain at least one number.");
                throw new ArgumentException("Password must contain at least one number.");
            }

            return password;
        }
    }
}