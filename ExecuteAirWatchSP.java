/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package executestoredprocedure;

/**
 *
 * @author anil4
 */
import java.sql.*;
import java.io.*;
import com.microsoft.sqlserver.jdbc.SQLServerCallableStatement;
import com.microsoft.sqlserver.jdbc.SQLServerDataTable;
import com.microsoft.sqlserver.jdbc.SQLServerPreparedStatement;

public class ExecuteAirWatchSP {

    public ExecuteAirWatchSP() {
    }
    public ExecuteAirWatchSP(String filename, String connectionUrl, String queryDB, Boolean colomn, String[] charSep) {
            Connection con = null;
            Statement stmt = null;
            ResultSet rs = null;  
            try {
                DBConn connessione = new DBConn();
                con=connessione.connect(connectionUrl);
                          // Create an in-memory data table.  
                SQLServerDataTable sourceDataTable = new SQLServerDataTable();
                sourceDataTable.addColumnMetadata("PersonType" ,java.sql.Types.NCHAR);
                sourceDataTable.addRow("EM");
                SQLServerPreparedStatement cstmt = (SQLServerPreparedStatement) con.prepareStatement(queryDB);
                cstmt.setStructured(1, "dbo.PersonTypeTable", sourceDataTable);  

                rs = cstmt.executeQuery();
                ExportData2CSV csv = new ExportData2CSV();
                csv.ExportData2CSV(rs,filename,colomn,charSep[0]);
                csv.createFileCsv();
            }
            catch(Exception e) {
                
            }
    }

}
