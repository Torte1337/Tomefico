using System;

namespace Tomefico.Service;

public static class PathService
{
    private const string DbName = "tomefico.db";

    public static string GetDatabasePath()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
        return dbPath;
    }
    public static string GetPureDatabasePath()
    {
        return FileSystem.AppDataDirectory;
    }
    public static string GetSQLiteConnectionString()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
        return $"Filename={dbPath}";
    }
}
