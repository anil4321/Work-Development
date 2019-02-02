/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package createvscobol;

import java.io.BufferedReader;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;

/**
 *
 * @author anil4
 */
public class CreateVSCobol {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        // TODO code application logic here
        String fileName="";
        if(args.length > 0){
            fileName = args[0];
        } else {
             System.out.println("Enter an argument for the file name.");
             fileName="testcobol.txt";
             //System.exit(0);
        }
                // The name of the file to open.

        // This will reference one line at a time
        String line = null;

        try {
            // FileReader reads text files in the default encoding.
            FileReader fileReader = 
                new FileReader(fileName);

            // Always wrap FileReader in BufferedReader.
            BufferedReader bufferedReader = 
                new BufferedReader(fileReader);
            String part2="";
            String pad="";
            while((line = bufferedReader.readLine()) != null) {
                int index = line.length()-1;
                
                if (index>6 && index <=72) {
                    part2=line.substring(6,index+1);
                    pad = String.format("%-6s", " ");
                }
                if (index>=72) {
                    part2=line.substring(6,72);
                    pad=String.format("%-6s",line.substring(72,index+1));
                }
                line=pad+part2;
                System.out.println(line);
            }
            // Always close files.
            bufferedReader.close();         
        }
        catch(FileNotFoundException ex) {
            System.out.println(
                "Unable to open file '" + 
                fileName + "'");                
        }
        catch(IOException ex) {
            System.out.println(
                "Error reading file '" 
                + fileName + "'");                  
            // Or we could just do this: 
            // ex.printStackTrace();
        }
    }

}
