namespace HospitalManagementApp.Utils
{
    public class Validation
    {
        public string ValidateStringInput(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine($"{fieldName} cannot be empty.");
                throw new ArgumentException($"{fieldName} is required.");
            }
            return input;
        }

        public string ValidatePassword(string password)
        {
            // Check if the password is null or less than 8 characters
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                Console.WriteLine("Password must be at least 8 characters long.");
                throw new ArgumentException("Password is too short.");
            }

            // Check if the password contains at least one uppercase letter
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]"))
            {
                Console.WriteLine("Password must contain at least one uppercase letter.");
                throw new ArgumentException("Password must contain at least one uppercase letter.");
            }

            // Check if the password contains at least one number
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"\d"))
            {
                Console.WriteLine("Password must contain at least one number.");
                throw new ArgumentException("Password must contain at least one number.");
            }

            // Check if the password contains at least one special character
            //     if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#$%^&*(),.?\":{ }|<>]")){
            //         Console.WriteLine("Password must contain at least one special character.");
            //     throw new ArgumentException("Password must contain at least one special character.");
            // }

            return password;
        }


        public int ValidateIntInput(string input, string fieldName)
        {
            if (!int.TryParse(input, out int result))
            {
                Console.WriteLine($"{fieldName} should be a valid number.");
                throw new ArgumentException($"{fieldName} must be a valid integer.");
            }
            return result;
        }

        public DateTime ValidateDateTimeInput(string input, string fieldName)
        {
            if (!DateTime.TryParse(input, out DateTime result))
            {
                Console.WriteLine($"{fieldName} should be a valid date and time.");
                throw new ArgumentException($"{fieldName} must be a valid date and time.");
            }
            return result;
        }

        public string ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.");
            }

            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(email))
            {
                throw new ArgumentException("Invalid email format.");
            }
            return email;
        }

        public string ValidateUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username is required.");
            }

            if (username.Length < 4 || username.Length > 20)
            {
                throw new ArgumentException("Username must be between 4 and 20 characters.");
            }

            var usernameRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9_]+$");
            if (!usernameRegex.IsMatch(username))
            {
                throw new ArgumentException("Username can only contain letters, numbers and underscores.");
            }
            return username;
        }

        public string ValidateGender(string gender)
        {
            var validGenders = new[] { "male", "female", "other", "m", "f" };
            if (!validGenders.Contains(gender.ToLower()))
            {
                throw new ArgumentException("Gender must be Male, Female or Other.");
            }
            return gender;
        }

        public int ValidateAge(int age)
        {
            if (age < 0 || age > 120)
            {
                throw new ArgumentException("Age must be between 0 and 120 years.");
            }
            return age;
        }

        public string ValidateSpecialization(string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
            {
                throw new ArgumentException("Specialization is required.");
            }

            if (specialization.Length < 2 || specialization.Length > 50)
            {
                throw new ArgumentException("Specialization must be between 2 and 50 characters.");
            }
            return specialization;
        }

        public DateTime ValidateAppointmentDate(DateTime date)
        {
            if (date.Date < DateTime.Now.Date)
            {
                throw new ArgumentException("Appointment date cannot be in the past.");
            }

            if (date.Date > DateTime.Now.AddYears(1))
            {
                throw new ArgumentException("Appointment cannot be scheduled more than 1 year in advance.");
            }
            return date;
        }

        public TimeSpan ValidateAppointmentTime(TimeSpan time)
        {
            var startTime = new TimeSpan(9, 0, 0);
            var endTime = new TimeSpan(17, 0, 0);

            if (time < startTime || time > endTime)
            {
                throw new ArgumentException("Appointment time must be between 9 AM and 5 PM.");
            }
            return time;
        }

        public int ValidateStock(int stock)
        {
            if (stock < 0)
            {
                throw new ArgumentException("Stock cannot be negative.");
            }

            if (stock > 10000)
            {
                throw new ArgumentException("Stock cannot exceed 10,000 units.");
            }
            return stock;
        }

        public string ValidateNameInput(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException($"{fieldName} cannot be empty.");
            }

            // Check for alphabetic characters only
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
            {
                throw new ArgumentException($"{fieldName} must contain only alphabetic characters.");
            }

            return input;
        }

        public string ValidateAlphaInput(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException($"{fieldName} cannot be empty.");
            }

            // Check for alphabetic characters only
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
            {
                throw new ArgumentException($"{fieldName} must contain only alphabetic characters.");
            }

            return input;
        }

    }
}