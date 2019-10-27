using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CreateXMLfromDB
{
    public class Employees : ICollection
    {
        public string CollectionName;
        private ArrayList empArray = new ArrayList();

        public Employee this[int index]
        {
            get { return (Employee)empArray[index]; }
        }

        public void CopyTo(Array a, int index)
        {
            empArray.CopyTo(a, index);
        }
        public int Count
        {
            get { return empArray.Count; }
        }
        public object SyncRoot
        {
            get { return this; }
        }
        public bool IsSynchronized
        {
            get { return false; }
        }
        public IEnumerator GetEnumerator()
        {
            return empArray.GetEnumerator();
        }

        public void Add(Employee newEmployee)
        {
            empArray.Add(newEmployee);
        }
        public String toXML()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Employee emp in empArray)
            {
                sb.Append("<Employee>");
                sb.Append("<Employee_ID>"+emp.employeeID+ "</Employee_ID>");
                sb.Append("<First_Name>" + emp.firstName + "</First_Name>");
                sb.Append("<LastName>" + emp.lastName + "</LastName>");
                sb.Append("</Employee>");
            }
            return sb.ToString();
        }
    }
}
