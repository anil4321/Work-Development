using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Configuration;
using System.Data;
using System.Xml.Serialization;
namespace CreateXMLfromDB
{
    class Program
    {
        private static OracleConnection _con;
        private const string connectionString = "User Id={0};Password={1};Data Source=XE;";
        private const string OracleDBUser = "hr";
        private const string OracleDBPassword = "hr";
        private static OracleTransaction transaction;

        private void Close()
        {
            if (_con != null)
            {
                _con.Close();
                _con.Dispose();
                _con = null;
            }
        }

        private void InitializeDBConnection()
        {
            try
            {
                _con = new OracleConnection();
                _con.ConnectionString = string.Format(connectionString, OracleDBUser, OracleDBPassword);
                _con.Open();
            }
            catch (OracleException e)
            {
                string errorMessage = "Code: " + e.ErrorCode + "\n" +
                                      "Message: " + e.Message;

                System.Diagnostics.EventLog log = new System.Diagnostics.EventLog();
                log.Source = "My Application";
                log.WriteEntry(errorMessage);
                Console.WriteLine("An exception occurred. Please contact your system administrator.");
            }
        }
        static void Main(string[] args)
        {
            Program ot = new Program();

            ot.InitializeDBConnection();
            string sql = "select employee_id,first_name, last_name from HR.employees where employee_id < "+" 105 and first_name like '%A%'";
            OracleCommand cmd = new OracleCommand(sql, _con);
            OracleDataReader reader = cmd.ExecuteReader();
            Employees empls = new Employees();
            try
            {
                while (reader.Read())
                {
                    Employee empl = new Employee();
                    empl.employeeID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    empl.firstName = reader.IsDBNull(1) ? null: reader.GetString(1);
                    empl.lastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                    //Console.WriteLine(reader.GetValue(0) + " "+ reader.GetValue(1) + "  "+ reader.GetValue(2));
                    empls.Add(empl);
                }
            }
            finally
            {
                reader.Close();
            }
            //ot.Close();
            Console.WriteLine(empls.toXML());


            /*
             * Get the response from the Web Service 
             * Deserialize and create rows in the database
             * 
            */
            String xmlstring = @"   <notes>
                                    <note>
                                    <to>Tove</to>
                                    <from>Jane</from>
                                    <heading>Reminder</heading>
                                    <body>Don't forget me this weekend!</body>
                                    </note>
                                    <note>
                                    <to>Doe</to>
                                    <from>John</from>
                                    <heading>Meeting</heading>
                                    <body>Hello Monday!</body>
                                    </note>
                                    </notes>";
            notes result = Deserialize<notes>(xmlstring);
            foreach (notesNote n in result.note)
            {
                Console.WriteLine(n.body);
            }

            /*
             * Insert CLOB 
             */
            // Start a local transaction
            OracleTransaction transaction = _con.BeginTransaction(IsolationLevel.ReadCommitted);
            string insertquery = "Insert into hr.note (id) VALUES (3)";
            OracleCommand command = _con.CreateCommand();
            command.Transaction = transaction;

            command.CommandText = "INSERT INTO hr.note (id, xmldata) values (:id, :xmldata)";
            try
            {
                
                {
                    byte[] newvalue = System.Text.Encoding.Unicode.GetBytes(xmlstring);
                    OracleClob clob = new OracleClob(_con);
                    clob.Write(newvalue, 0, newvalue.Length);
                    command.Parameters.Add(new OracleParameter("id", 10));
                    command.Parameters.Add(new OracleParameter("xmldata", clob));
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database."); 
                    _con.Close();
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e.ToString());
                Console.WriteLine("Neither record was written to database.");
            }
        }

        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }
    }
}
