using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using OrithenApp.Data;

namespace OrithenApp.Views;

public partial class CreateTemplatePage : Page
{
    private readonly DatabaseService _db = new();
    private readonly List<WrapPanel> _blockCheckPanelList = new();
    private readonly List<string> _allExerciseNames = new();
    private readonly List<string> _exerciseItems = new();

    public CreateTemplatePage()
    {
        InitializeComponent();
        LoadExerciseOptions();
        AddBlock(null!, null!);
    }

    private void AddBlock(object sender, RoutedEventArgs e)
    {
        var index = _blockCheckPanelList.Count + 1;
        var blockPanel = new StackPanel { Margin = new Thickness(0, 8, 0, 0) };

        var title = new TextBlock
        {
            Text = $"Bloc {index}",
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(0, 0, 0, 4)
        };

        var checkboxPanel = new WrapPanel
        {
            Margin = new Thickness(0, 0, 0, 8)
        };

        foreach (var exercise in _exerciseItems)
        {
            checkboxPanel.Children.Add(new CheckBox
            {
                Content = exercise,
                Margin = new Thickness(0, 0, 8, 4)
            });
        }

        _blockCheckPanelList.Add(checkboxPanel);
        blockPanel.Children.Add(title);
        blockPanel.Children.Add(checkboxPanel);
        BlocksPanel.Children.Add(blockPanel);
    }

    private void LoadExerciseOptions()
    {
        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Name, Category FROM Exercises ORDER BY Category, Name";
        using var reader = command.ExecuteReader();

        var programItems = new List<string>();
        var warmupItems = new List<string>();
        var methodologyItems = new List<string>();
        _allExerciseNames.Clear();
        _exerciseItems.Clear();

        while (reader.Read())
        {
            var name = reader.GetString(0);
            var category = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            _allExerciseNames.Add(name);

            switch (category)
            {
                case "Programa":
                    programItems.Add(name);
                    break;
                case "Escalfament":
                    warmupItems.Add(name);
                    break;
                case "Metodologia":
                    methodologyItems.Add(name);
                    break;
                case "Exercicis":
                    _exerciseItems.Add(name);
                    break;
            }
        }

        ProgramComboBox.ItemsSource = programItems;
        MethodologyComboBox.ItemsSource = methodologyItems;

        WarmupCheckBoxes.Children.Clear();
        foreach (var warmupItem in warmupItems)
        {
            WarmupCheckBoxes.Children.Add(new CheckBox
            {
                Content = warmupItem,
                Margin = new Thickness(0, 0, 0, 4)
            });
        }

        if (ProgramComboBox.Items.Count > 0)
        {
            ProgramComboBox.SelectedIndex = 0;
        }

        if (MethodologyComboBox.Items.Count > 0)
        {
            MethodologyComboBox.SelectedIndex = 0;
        }
    }

    private void SaveTemplate(object sender, RoutedEventArgs e)
    {
        var templateName = string.IsNullOrWhiteSpace(TemplateNameBox.Text)
            ? $"Plantilla {DateTime.Now:yyyyMMddHHmmss}"
            : TemplateNameBox.Text.Trim();
        var programSelection = ProgramComboBox.SelectedItem?.ToString() ?? string.Empty;
        var warmupSelections = string.Join(", ", WarmupCheckBoxes.Children.OfType<CheckBox>().Where(checkBox => checkBox.IsChecked == true).Select(checkBox => checkBox.Content?.ToString()));
        var methodologySelection = MethodologyComboBox.SelectedItem?.ToString() ?? string.Empty;
        var blockParts = _blockCheckPanelList.Select((panel, index) =>
            $"Bloc {index + 1}: {string.Join(", ", panel.Children.OfType<CheckBox>().Where(checkBox => checkBox.IsChecked == true).Select(checkBox => checkBox.Content?.ToString()))}").ToList();
        var notesValue = string.Join(" | ", new[]
        {
            $"Programa: {programSelection}",
            $"Metodologia: {methodologySelection}",
            $"Escalfament: {warmupSelections}"
        }.Concat(blockParts));

        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();

        var templateCommand = connection.CreateCommand();
        templateCommand.CommandText = @"INSERT INTO SessionTemplates (Name, Category, Warmup, Notes, CreatedOn) VALUES (@name, @category, @warmup, @notes, @createdOn);";
        templateCommand.Parameters.AddWithValue("@name", templateName);
        templateCommand.Parameters.AddWithValue("@category", methodologySelection);
        templateCommand.Parameters.AddWithValue("@warmup", warmupSelections);
        templateCommand.Parameters.AddWithValue("@notes", notesValue);
        templateCommand.Parameters.AddWithValue("@createdOn", DateTime.UtcNow.ToString("O"));
        templateCommand.ExecuteNonQuery();

        MessageBox.Show("Plantilla guardada", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
        TemplateNameBox.Clear();
    }
}
