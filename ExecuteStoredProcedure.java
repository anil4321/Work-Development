/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package executestoredprocedure;

import java.sql.*;
import java.io.*;
import com.microsoft.sqlserver.jdbc.SQLServerCallableStatement;
import com.microsoft.sqlserver.jdbc.SQLServerDataTable;
import com.microsoft.sqlserver.jdbc.SQLServerPreparedStatement;

public class ExecuteStoredProcedure {

    public static void main(String[] args) {
        
      String filename =       "C:\\Anil\\TestFile.csv";
      String userDB =         "testjdbc";
      String passDB =         "P@ssword111";
      String[] charSep =        new String[] {";"};
      Boolean isColomn=         true;
      String queryDB =        "{call dbo.GetPerson(?)}";
  
     // Create a variable for the connection string.
      String connectionUrl = 
           "jdbc:sqlserver://localhost:1433;" +
           "databaseName=AdventureWorks2017"+";user="+userDB+";password="+passDB;

        try {
          // Establish the connection.
          File fileTemp = new File(filename);
          if (fileTemp.exists()){ 
                fileTemp.delete();
                System.out.println("---------------DELETE FILE------------" + filename);
          } 
         ExecuteAirWatchSP sp = new ExecuteAirWatchSP(filename, connectionUrl, queryDB, isColomn, charSep);
         if (fileTemp.exists()){ 
            System.out.println("---File created---" + filename);
         }
        }
        // Handle any errors that may have occurred.
        catch (Exception e) {
            e.printStackTrace();
        }
    }
}