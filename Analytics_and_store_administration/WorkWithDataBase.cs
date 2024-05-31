using System;
using System.IO;
using System.Data.SqlClient;
using ВКР;

public class DatabaseHelper
{
    private SqlConnection sqlConnection;

    public DatabaseHelper()
    {       
        string dbFileName = "Database1.mdf";
        string projectDirectory = GetProjectDirectory();
        string dbDirectory;
        if (!string.IsNullOrEmpty(projectDirectory))
        {
            dbDirectory = projectDirectory;
        }
        else
        {
            dbDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }
        string dbFilePath = Path.Combine(dbDirectory, dbFileName);
        string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbFilePath};Integrated Security=True";
        sqlConnection = new SqlConnection(connectionString);
    }

    private string GetProjectDirectory()
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        while (!string.IsNullOrEmpty(currentDirectory))
        {
            string[] slnFiles = Directory.GetFiles(currentDirectory, "*.sln");
            if (slnFiles.Length > 0)
            {
                return currentDirectory;
            }
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }
        return null; 
    }

    public void OpenConnection()
    {
        if (sqlConnection.State == System.Data.ConnectionState.Closed)
        {
            sqlConnection.Open();
        }
    }

    public void CloseConnection()
    {
        if (sqlConnection.State == System.Data.ConnectionState.Open)
        {
            sqlConnection.Close();
        }
    }

    public SqlConnection GetConnection()
    {
        return sqlConnection;
    }
}