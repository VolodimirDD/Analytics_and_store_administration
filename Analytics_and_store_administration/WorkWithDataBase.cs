using System;
using System.IO;
using System.Data.SqlClient;
using ВКР;

public class DatabaseHelper
{
    private SqlConnection sqlConnection;

    public DatabaseHelper()
    {       
        // Получаем имя файла базы данных
        string dbFileName = "Database1.mdf";
        // Пытаемся получить путь к директории проекта (содержащей файл .sln)
        string projectDirectory = GetProjectDirectory();
        // Если projectDirectory пуст, используем путь к директории релизной версии
        string dbDirectory;
        if (!string.IsNullOrEmpty(projectDirectory))
        {
            dbDirectory = projectDirectory;
        }
        else
        {
            dbDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }
        // Формируем полный путь к файлу базы данных
        string dbFilePath = Path.Combine(dbDirectory, dbFileName);
        // Формируем строку подключения с использованием полученного пути
        string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbFilePath};Integrated Security=True";
        // Создаем подключение к базе данных
        sqlConnection = new SqlConnection(connectionString);
    }

    private string GetProjectDirectory()
    {
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        // Проверяем наличие файла .sln в текущей директории и во всех родительских директориях
        while (!string.IsNullOrEmpty(currentDirectory))
        {
            string[] slnFiles = Directory.GetFiles(currentDirectory, "*.sln");
            if (slnFiles.Length > 0)
            {
                return currentDirectory;
            }
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }
        return null; // Если файл .sln не найден, возвращаем null
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