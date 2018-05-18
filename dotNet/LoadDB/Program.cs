using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Diagnostics;

namespace LoadDB
{
    /// <summary>
    /// This program creates the database/indexes then loads the data from the SNOMED CT folder
    /// </summary>

    class Program
    {
        #region Constants
        /// <summary>
        /// A list of required constants 
        /// The configuration settings are declared in the project properties (app.config)
        /// eg.. ConnectionString, DatabaseName, SnomedFolderLocation
        /// </summary>
        public struct Constants{

          public const string SnapshotRefsetContentFolder = "Snapshot\\Refset\\Content";
            
            public static string DatabaseName
            { get { return Properties.Settings.Default.DatabaseName; }}

            public static string SnomedFolderLocation
            { get { return Properties.Settings.Default.SnomedFolderLocation + "RF2Release\\Snapshot"; } }

            public static string ServerConnectionString
            { get { return Properties.Settings.Default.ConnectionString; }}

            public static string DatabaseConnectionString
            { get { return ServerConnectionString + ";database=" + Properties.Settings.Default.DatabaseName; } }

            public static string SqlCreateReferenceSchemaFileLocation
            { get { return SqlFolder + "createReferenceSchema.sql"; }}

            public static string SqlCreateIndexesFileLocation
            { get { return SqlFolder + "createIndexes.sql"; } }

            public static string SqlImportIntoReferenceSchemaFileLocation
            { get { return SqlFolder + "importIntoReferenceSchema.sql"; } }

            // Assumes that the SQL folder lives with in the LoadDB project directory
            private static string SqlFolder
            { get
                { return Environment.CurrentDirectory.Substring(
                    0, 
                    Environment.CurrentDirectory.Length - 
                    (Environment.CurrentDirectory.Length -Environment.CurrentDirectory.IndexOf("LoadDB")))  + 
                    @"LoadDB\SQL\"; 
                }
            }
        }
        #endregion

        #region Database

        /// <summary>
        /// Runs a SQL query against the database 
        ///<param name="query">sql query to perform</param>
        ///<param name="message">messge to the user  - an string.Empty will display no message</param>
        ///<param name="message">connectionString</param>
        /// </summary>
        private static void RunSQLQuery(string query,string message,string connectionString)
        {
            try
            {
                MySqlConnection currentConnection = new MySqlConnection(connectionString);
                currentConnection.Open();
                using (MySqlCommand command = new MySqlCommand(query, currentConnection))
                {
                    command.CommandTimeout = 1000000;
                    command.CommandType = CommandType.Text;
                    MySqlDataReader msqDr = command.ExecuteReader();
                    if (message != string.Empty)
                       Console.WriteLine(string.Format("{0} {1} ",message, msqDr.RecordsAffected));
                }
                currentConnection.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Format the file location string for a SQL query 
        /// </summary>
        public static string FormatFileForSql(string filelocation)
        {
            return "'" + filelocation.Replace("\\", "/") + "'";
        }

        #endregion

        /// <summary>
        /// This program creates the database/indexes then loads the data from the SNOMED CT folder
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Copyright (c) 2018 Australian Digital Health Agency \n");

            // Create Database
            Console.WriteLine(string.Format("Create database '{0}'",Constants.DatabaseName));
            TextReader tr = new StreamReader(Constants.SqlCreateReferenceSchemaFileLocation);
            string query = String.Format(tr.ReadToEnd(), Constants.DatabaseName);
            RunSQLQuery(query, string.Empty, Constants.ServerConnectionString);

            // Create Indexes
            Console.WriteLine("Create indexes");
            tr = new StreamReader(Constants.SqlCreateIndexesFileLocation);
            query = tr.ReadToEnd();
            RunSQLQuery(query, string.Empty, Constants.DatabaseConnectionString);
            
            // Populate data from refset folder
            Console.WriteLine(string.Format("Import into '{0}' from {1}", Constants.DatabaseName, Constants.SnomedFolderLocation));
            tr = new StreamReader(Constants.SqlImportIntoReferenceSchemaFileLocation);
            query = tr.ReadToEnd();

           

            // Test if directory exists
            if (!Directory.Exists(Constants.SnomedFolderLocation))
            {
                Console.WriteLine("Directory " + Constants.SnomedFolderLocation + " does not exist");
                Process.GetCurrentProcess().Kill();
            }
            Console.ReadKey();
            // Get all directories for the Snomed CT folder location
            string[] listOfAllDir = Directory.GetDirectories(Constants.SnomedFolderLocation, "*", SearchOption.AllDirectories);

            // Find all associated files and write them to the database
            foreach (string dir in listOfAllDir)
            {
                // Get all of the files in current directory
                string[] fileList = Directory.GetFiles(dir);

                // Import all of the files in the description refset folder into the concept_refset table
                if (dir.Contains(Constants.SnapshotRefsetContentFolder))
                {
                    foreach (string file in fileList)
                        RunSQLQuery(
                            String.Format(query, FormatFileForSql(file), "concept_refset"),
                            "Import into concept_refset",
                            Constants.DatabaseConnectionString);
                }
                else
                {
                    // Import files 
                    foreach (string file in fileList)
                    {
                        if (file.Contains("sct2_Concept"))
                            RunSQLQuery(
                                String.Format(query, FormatFileForSql(file), "concepts"),
                                "Import into concepts",
                                Constants.DatabaseConnectionString);

                        if (file.Contains("sct2_Description"))
                            RunSQLQuery(
                                String.Format(query, FormatFileForSql(file), "descriptions"),
                                "Import into descriptions",
                                Constants.DatabaseConnectionString);

                        if (file.Contains("sct2_Relationship"))
                            RunSQLQuery(
                                String.Format(query, FormatFileForSql(file), "relationships"),
                                "Import into relationships",
                                Constants.DatabaseConnectionString);

                        if (file.Contains("sct2_Identifier"))
                            RunSQLQuery(
                                String.Format(query, FormatFileForSql(file), "identifiers"),
                                "Import into identifiers",
                                Constants.DatabaseConnectionString);

                        if (file.Contains("der2_cRefset_LanguageSnapshot-en-AU"))
                            RunSQLQuery(
                                String.Format(query, FormatFileForSql(file), "description_refset"),
                                "Import into description_refset",
                                Constants.DatabaseConnectionString);
                    }
                }
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine("Complete");
            Console.ReadKey();
        }
    }
}
