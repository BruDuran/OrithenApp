using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using OrithenApp.Data;

namespace OrithenApp.Views;

public partial class ExercisesPage : Page
{
    private readonly DatabaseService _db = new();

    public ExercisesPage()
    {
        InitializeComponent();
        _db.SeedSampleData();
        LoadExercises();
    }

    private void AddExercise(object sender, RoutedEventArgs e)
    {
        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Exercises (Name, Category, Material, Description) VALUES (@name, @category, @material, @description);";
        command.Parameters.AddWithValue("@name", NameBox.Text);
        command.Parameters.AddWithValue("@category", CategoryBox.Text);
        command.Parameters.AddWithValue("@material", MaterialBox.Text);
        command.Parameters.AddWithValue("@description", "");
        command.ExecuteNonQuery();
        LoadExercises();
    }

    private void LoadExercises()
    {
        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT Name, Category, Material FROM Exercises ORDER BY Id DESC";
        using var reader = command.ExecuteReader();
        var items = new List<string>();
        while (reader.Read())
        {
            items.Add($"{reader.GetString(0)} | {reader.GetString(1)} | {reader.GetString(2)}");
        }
        ExercisesList.ItemsSource = items;
    }
}
