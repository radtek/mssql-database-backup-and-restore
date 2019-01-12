using System;
using System.Data.SqlClient;

namespace MSSQLBackupAndRestore
{
    /// <summary>
    /// Database maintenance
    /// </summary>
    public static class Maintenance
    {
        /// <summary>
        /// Export Database to a specified location
        /// </summary>
        /// <param name="locationAndFilename">Location/Filename</param>
        public static void ExportDatabase(string connectionString,string locationAndFilename)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(string.Format("BACKUP DATABASE [{0}] TO DISK = '{1} {2}{3}'", connectionString, locationAndFilename.Replace(".bak", ""), DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"), ".bak"), con);
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        /// <summary>
        /// Imports Database from a specified location
        /// </summary>
        /// <param name="locationAndFilename">Location/Filename</param>
        public static void ImportDatabase(string connectionString, string locationAndFilename)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", connectionString), con);
                SqlCommand command0 = new SqlCommand(string.Format("USE MASTER RESTORE DATABASE {0} FROM DISK= '{1}' WITH REPLACE;", connectionString, locationAndFilename), con);
                SqlCommand command1 = new SqlCommand(string.Format("ALTER DATABASE [{0}] SET MULTI_USER", connectionString), con);
                con.Open();
                command.ExecuteNonQuery();
                command0.ExecuteNonQuery();
                command1.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
