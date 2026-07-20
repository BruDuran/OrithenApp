using System.Windows;
using OrithenApp.Views;

namespace OrithenApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new ExercisesPage());
    }

    private void OpenExercises(object sender, RoutedEventArgs e) => MainFrame.Navigate(new ExercisesPage());
    private void OpenCreateTemplate(object sender, RoutedEventArgs e) => MainFrame.Navigate(new CreateTemplatePage());
    private void OpenTemplateView(object sender, RoutedEventArgs e) => MainFrame.Navigate(new TemplateViewPage());
}
