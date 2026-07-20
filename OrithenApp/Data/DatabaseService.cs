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

    public void SeedSampleData()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        var existing = connection.CreateCommand();
        existing.CommandText = "SELECT COUNT(*) FROM Exercises";
        var count = Convert.ToInt32(existing.ExecuteScalar());
        if (count > 0)
        {
            return;
        }

        var insertExercise = connection.CreateCommand();
        insertExercise.CommandText = @"INSERT INTO Exercises (Name, Category, Material, Description) VALUES (@name, @category, @material, @description);";

        insertExercise.Parameters.AddWithValue("@name", "Snatch");
        insertExercise.Parameters.AddWithValue("@category", "Fuerza");
        insertExercise.Parameters.AddWithValue("@material", "DB");
        insertExercise.Parameters.AddWithValue("@description", "Ejercicio de fuerza");
        insertExercise.ExecuteNonQuery();

        insertExercise.Parameters.Clear();
        insertExercise.Parameters.AddWithValue("@name", "Pull Up");
        insertExercise.Parameters.AddWithValue("@category", "Fuerza");
        insertExercise.Parameters.AddWithValue("@material", "Peso corporal");
        insertExercise.Parameters.AddWithValue("@description", "Tracción");
        insertExercise.ExecuteNonQuery();

        insertExercise.Parameters.Clear();
        insertExercise.Parameters.AddWithValue("@name", "Goblet Squat");
        insertExercise.Parameters.AddWithValue("@category", "Fuerza");
        insertExercise.Parameters.AddWithValue("@material", "KB");
        insertExercise.Parameters.AddWithValue("@description", "Sentadilla con kettlebell");
        insertExercise.ExecuteNonQuery();
    }
}
