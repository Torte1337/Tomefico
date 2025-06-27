using System;

namespace Tomefico.Service;

public class PathService
{
    private const string DbName = "tomefico.db";

    public string GetDatabasePath()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);
        return dbPath;
    }
    public string GetSQLiteConnectionString()
    {
        return $"Filename={GetDatabasePath()}";
    }
}
