using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.Sqlite;
using OrithenApp.Data;

namespace OrithenApp.Views;

public partial class CreateTemplatePage : Page
{
    private readonly DatabaseService _db = new();

    public CreateTemplatePage()
    {
        InitializeComponent();
    }

    private void SaveTemplate(object sender, RoutedEventArgs e)
    {
        using var connection = new SqliteConnection($"Data Source={_db.DbPath}");
        connection.Open();

        var templateCommand = connection.CreateCommand();
        templateCommand.CommandText = @"INSERT INTO SessionTemplates (Name, Category, Warmup, Notes, CreatedOn) VALUES (@name, @category, @warmup, @notes, @createdOn);";
        templateCommand.Parameters.AddWithValue("@name", SessionNameBox.Text);
        templateCommand.Parameters.AddWithValue("@category", "");
        templateCommand.Parameters.AddWithValue("@warmup", WarmupBox.Text);
        templateCommand.Parameters.AddWithValue("@notes", $"Bloque: {BlockNameBox.Text}");
        templateCommand.Parameters.AddWithValue("@createdOn", DateTime.UtcNow.ToString("O"));
        templateCommand.ExecuteNonQuery();

        MessageBox.Show("Plantilla guardada", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
