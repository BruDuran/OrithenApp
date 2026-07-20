using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using OrithenApp.Data;

namespace OrithenApp.Views;

public partial class ExercisesPage : Page
{
    private readonly DatabaseService _db = new();
    private readonly string[] _categories = ["Programa", "Escalfament", "Metodologia", "Repeticions", "Exercicis"];
    private readonly DataTemplate _exerciseItemTemplate;

    private sealed class ExerciseDisplayItem
    {
        public int Id { get; init; }
        public string DisplayText { get; init; } = string.Empty;
    }

    private sealed class ExerciseCategoryGroup
    {
        public string Category { get; init; } = string.Empty;
        public ObservableCollection<ExerciseDisplayItem> Items { get; init; } = new();
    }

    public ExercisesPage()
    {
        InitializeComponent();
        _exerciseItemTemplate = (DataTemplate)Resources["ExerciseItemTemplate"];
        CategoryComboBox.SelectedIndex = 0;
        LoadExercises();
    }

    private void AddExercise(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TypeBox.Text))
        {
            MessageBox.Show("Introduce el tipo antes de guardar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Programa";

        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Exercises (Name, Category, Material, Description) VALUES (@name, @category, @material, @description);";
        command.Parameters.AddWithValue("@name", TypeBox.Text.Trim());
        command.Parameters.AddWithValue("@category", category);
        command.Parameters.AddWithValue("@material", string.Empty);
        command.Parameters.AddWithValue("@description", string.Empty);
        command.ExecuteNonQuery();

        TypeBox.Clear();
        CategoryComboBox.SelectedIndex = 0;
        LoadExercises();
    }

    private void DeleteExercise(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is not int id)
        {
            return;
        }

        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Exercises WHERE Id = @id;";
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();

        LoadExercises();
    }

    private void LoadExercises()
    {
        var groups = _categories
            .Select(category => new ExerciseCategoryGroup
            {
                Category = category,
                Items = new ObservableCollection<ExerciseDisplayItem>()
            })
            .ToList();

        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Category, Material FROM Exercises ORDER BY Category, Name";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var category = reader.IsDBNull(2) ? "Programa" : reader.GetString(2);
            var group = groups.FirstOrDefault(g => string.Equals(g.Category, category, StringComparison.OrdinalIgnoreCase));
            if (group is null)
            {
                continue;
            }

            group.Items.Add(new ExerciseDisplayItem
            {
                Id = reader.GetInt32(0),
                DisplayText = reader.GetString(1)
            });
        }

        PopulateCategoryTabs(groups);
    }

    private void PopulateCategoryTabs(IReadOnlyList<ExerciseCategoryGroup> groups)
    {
        CategoryTabs.Items.Clear();

        foreach (var group in groups)
        {
            var tabItem = new TabItem
            {
                Header = group.Category,
                Content = new ListBox
                {
                    ItemsSource = group.Items,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    Margin = new Thickness(0, 8, 0, 0),
                    ItemTemplate = _exerciseItemTemplate
                }
            };

            CategoryTabs.Items.Add(tabItem);
        }
    }
}
