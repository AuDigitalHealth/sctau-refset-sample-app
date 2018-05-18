using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

namespace CTDemo
{
 /// <summary>
 /// Manages access to the database.
 /// The configuration to establish the connection may be declared in the project properties.
 /// If the specified properties file cannot be found the application will continue with out it (no errors reported).
 ///
 /// The following system properties are required:
 /// ConnectionString (MSQLConnectionString) - Conection to the MYSQL Database
 /// MaxRows (Int) - Limit number of returned rows, less than 0 will return all rows
 /// </summary>

public class DataSource {

    /// <summary>
    /// Creats a connection to the Database
    /// <returns>Return a new instance of a connection to the database</returns>
    /// </summary>
    private static MySqlConnection CreateConnection()
    {
        try {

        MySqlConnection mySqlConnection = new MySqlConnection(GetConnectionString());
        mySqlConnection.Open();
        return mySqlConnection;
        }
        catch (Exception e)
        {
            throw new Exception("Unable to create database connection!", e);
        }
    }

    /// <summary>
    /// Runs a query against the Database, returns a DataTable
    /// <returns>DataTable of Results</returns> 
    /// </summary>
    public static DataTable RunSQLQuery(string query)
    {
        return RunSQLQuery(query,false);
    }

    /// <summary>
    /// Runs a query against the Database results, provides the option to filter results
    ///<param name="query">Sql Query to perform</param>
    ///<param name="limitResults">Specifiy if returned results should be limited</param>
    /// </summary>
    public static DataTable RunSQLQuery(string query, Boolean limitResults)
    {
        if (limitResults)
            return RunSQLQueryWithLimitedResults(query, GetMaxRows());
        else
            return RunSQLQueryWithLimitedResults(query, 0);
    }

    /// <summary>
    /// Run a query against the database to return DataTable.
    /// Resutls may be limited by specifiing a value, 0 returns unlimited Results
    ///<param name="query">Sql Query to perform</param>
    ///<param name="maxRows">Limit the number of returned rows to the value of MaxRows in Project Properties</param>
    /// <returns>Returns a new instance of a connection to the database</returns>
    /// </summary>
    private static DataTable RunSQLQueryWithLimitedResults(string query, int maxRows)
    {
        DataTable outputTable = new DataTable();
        MySqlConnection currentConnection = CreateConnection();

        try
        {
            using (MySqlCommand command = new MySqlCommand(query, currentConnection))
            {
                command.CommandType = CommandType.Text;

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                if (GetMaxRows() > 0)
                    adapter.Fill(0, maxRows, outputTable);
                else
                    adapter.Fill(outputTable);
            }
        }
        catch (Exception e)
        {
            throw new Exception("Unable to execute SQL Query", e);            
        }
        finally
        {
            try
            {
                currentConnection.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Unable to close database connection!", e);
            }
        }

        return outputTable;
    }

    /// <summary>
    /// Get the MaxRows from Project Properties (app.config)
    /// </summary>
    public static int GetMaxRows()
    {
        return Properties.Settings.Default.MaxRows;
    }

    /// <summary>
    /// Get the ConnectionString from Project Properties (app.config)
    /// </summary>
    public static string GetConnectionString()
    {
        return Properties.Settings.Default.ConnectionString;
    }

    /// <summary>
    /// Prints Database details
    /// </summary>
    public static void PrintDatabaseDetails()
    {
        Console.WriteLine("Connecting to database "
            + "//" + GetDetailsFromConnectionString("server")
            + "//" + GetDetailsFromConnectionString("database")
            + " as user '" + GetDetailsFromConnectionString("User Id") + "'");
    }

    /// <summary>
    /// Returns fields from the Connection String
    /// </summary>
    private static string GetDetailsFromConnectionString(string field)
    {
        DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
        builder.ConnectionString = GetConnectionString();
        return (string)builder[field];
    }

}

}
