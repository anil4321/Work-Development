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

/**
 * Created by MAXNIGELNEGRO
 */
public class DBConn {
    
    public DBConn() {
    }
    public Connection connect(String db_connect_str) {
        Connection conn;

        try {
            Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
            conn = DriverManager.getConnection(db_connect_str);


        } catch (Exception e) {
            e.printStackTrace();
            conn = null;

        }
        return conn;
    }


}