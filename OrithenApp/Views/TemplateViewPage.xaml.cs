using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Data.Sqlite;
using OrithenApp.Data;

namespace OrithenApp.Views;

public partial class TemplateViewPage : Page
{
    private readonly DatabaseService _db = new();

    private sealed class TemplateSummaryItem
    {
        public int Id { get; init; }
        public string DisplayName { get; init; } = string.Empty;
        public string TemplateName { get; init; } = string.Empty;
        public string ProgramSelection { get; init; } = string.Empty;
        public string MethodologySelection { get; init; } = string.Empty;
        public string WarmupSelection { get; init; } = string.Empty;
        public string Notes { get; init; } = string.Empty;
    }

    public TemplateViewPage()
    {
        InitializeComponent();
        LoadTemplates();
    }

    private void LoadTemplates()
    {
        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Name, Category, Warmup, Notes, CreatedOn FROM SessionTemplates ORDER BY CreatedOn DESC";
        using var reader = command.ExecuteReader();

        var templates = new List<TemplateSummaryItem>();
        while (reader.Read())
        {
            var name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            var notes = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
            templates.Add(new TemplateSummaryItem
            {
                Id = reader.GetInt32(0),
                DisplayName = string.IsNullOrWhiteSpace(name) ? $"Plantilla {reader.GetInt32(0)}" : name,
                TemplateName = name,
                MethodologySelection = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                WarmupSelection = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Notes = notes
            });
        }

        TemplatesList.ItemsSource = templates;
        if (templates.Count > 0)
        {
            TemplatesList.SelectedIndex = 0;
        }
    }

    private void TemplatesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TemplatesList.SelectedItem is not TemplateSummaryItem selected)
        {
            return;
        }

        SelectedTemplateTitle.Text = string.IsNullOrWhiteSpace(selected.TemplateName) ? "Plantilla sense nom" : selected.TemplateName;
        ProgramText.Text = ParseValue(selected.Notes, "Programa") is { } programValue
            ? $"Programa: {programValue}"
            : "Programa: -";
        WarmupText.Text = string.IsNullOrWhiteSpace(selected.WarmupSelection)
            ? "Escalfament: -"
            : $"Escalfament: {selected.WarmupSelection}";
        MethodologyText.Text = string.IsNullOrWhiteSpace(selected.MethodologySelection)
            ? "Metodologia: -"
            : $"Metodologia: {selected.MethodologySelection}";

        BlocksContainer.Children.Clear();
        var blockEntries = ParseBlockEntries(selected.Notes);
        foreach (var blockEntry in blockEntries)
        {
            BlocksContainer.Children.Add(new TextBlock
            {
                Text = $"• {blockEntry}",
                Margin = new Thickness(0, 4, 0, 0),
                TextWrapping = TextWrapping.Wrap
            });
        }

        if (blockEntries.Count == 0)
        {
            BlocksContainer.Children.Add(new TextBlock
            {
                Text = "Sense blocs",
                Margin = new Thickness(0, 4, 0, 0),
                Foreground = Brushes.Gray
            });
        }
    }

    private static string? ParseValue(string notes, string prefix)
    {
        foreach (var part in notes.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (part.StartsWith(prefix + ":", StringComparison.OrdinalIgnoreCase))
            {
                return part.Substring(prefix.Length + 1).Trim();
            }
        }

        return null;
    }

    private static List<string> ParseBlockEntries(string notes)
    {
        var entries = new List<string>();
        foreach (var part in notes.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (part.StartsWith("Bloc ", StringComparison.OrdinalIgnoreCase))
            {
                entries.Add(part);
            }
        }

        return entries;
    }
}
