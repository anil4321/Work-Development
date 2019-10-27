using System;
using System.Collections.Generic;
using System.Text;

namespace CreateXMLfromDB
{
    public class Employee
    {
        public int employeeID;
        public string firstName;
        public string lastName;
        public Employee() { }
        public Employee(string FirstName, string LastName)
        {
            this.firstName = FirstName;
            this.lastName = LastName;
        }
    }
}
