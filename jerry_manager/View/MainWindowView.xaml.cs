using System.Windows;
using jerry_manager.Core;
using jerry_manager.ViewModel;

namespace jerry_manager.View;

public partial class MainWindowView : Window
{
    private MainWindowViewModel m_ViewModel { get; set; }

    public MainWindowView()
    {
        InitializeComponent();
        m_ViewModel = new MainWindowViewModel();
        DataContext = m_ViewModel;
    }

    private void RightExplorer_OnGotFocus(object sender, RoutedEventArgs e)
    {
        DataCache.ActiveView = RightExplorer;
    }

    private void LeftExplorer_OnGotFocus(object sender, RoutedEventArgs e)
    {
        DataCache.ActiveView = RightExplorer;
    }
}