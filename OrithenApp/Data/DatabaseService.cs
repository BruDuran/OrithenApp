using System.IO;
using Microsoft.Data.Sqlite;
using OrithenApp.Models;

namespace OrithenApp.Data;

public class DatabaseService
{
    private readonly string _dbPath;

    public DatabaseService(string? dbPath = null)
    {
        _dbPath = dbPath ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orithenapp.db");
        Initialize();
    }

    public string DbPath => _dbPath;

    private void Initialize()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Exercises (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Category TEXT,
                Material TEXT,
                Description TEXT
            );

            CREATE TABLE IF NOT EXISTS SessionTemplates (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Category TEXT,
                Warmup TEXT,
                Notes TEXT,
                CreatedOn TEXT NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Blocks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SessionTemplateId INTEGER NOT NULL,
                Name TEXT NOT NULL,
                Type TEXT,
                Description TEXT,
                DurationMin INTEGER,
                [Order] INTEGER
            );

            CREATE TABLE IF NOT EXISTS Stations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BlockId INTEGER NOT NULL,
                Number INTEGER NOT NULL,
                Material TEXT,
                RotationOrder INTEGER
            );

            CREATE TABLE IF NOT EXISTS StationExercises (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StationId INTEGER NOT NULL,
                ExerciseId INTEGER NOT NULL,
                Series TEXT,
                Repetitions TEXT,
                Time TEXT,
                Notes TEXT,
                [Order] INTEGER
            );

            CREATE TABLE IF NOT EXISTS Equipment (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }

}
