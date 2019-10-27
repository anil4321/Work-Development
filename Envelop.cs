using System;
using System.Collections.Generic;
using System.Text;

namespace CreateXMLfromDB
{
    class Envelop
    {
        private readonly String envolopBodyBegin= @"<?xml version=""1.0"" encoding=""utf-8""?>
<PurchaseOrder xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://www.cpandl.com"">";
        private String groupName;
        private Employees employees;
        private readonly String envolopBodyEnd= @"</PurchaseOrder>";

        public void Show()
        {
            Console.WriteLine(envolopBodyBegin);
            Console.WriteLine(groupName);
            Console.WriteLine(employees.toXML());
            Console.WriteLine(envolopBodyEnd);
        }
    }
}
