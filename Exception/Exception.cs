using System;

namespace HospitalManagementApp.Exceptions
{
    public class HospitalManagementException : Exception
    {
        public HospitalManagementException(string message) : base(message) { }
    }
}