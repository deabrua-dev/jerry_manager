using System.Windows;
using jerry_manager.Core;
using jerry_manager.ViewModel;

namespace jerry_manager.View;

public partial class MainWindowView : Window
{
    #region Variables

    private MainWindowViewModel m_ViewModel;

    public MainWindowViewModel ViewModel
    {
        get => m_ViewModel;
        set => m_ViewModel = value;
    }

    #endregion

    #region Constructors

    public MainWindowView()
    {
        InitializeComponent();
        m_ViewModel = new MainWindowViewModel();
        DataContext = m_ViewModel;
        DataCache.ActiveView = LeftExplorer.ViewModel;
        DataCache.NotActiveView = RightExplorer.ViewModel;
        LeftExplorer.IsSelected = true;
        RightExplorer.IsSelected = false;
    }

    #endregion

    #region Methods

    private void RightExplorer_OnGotFocus(object sender, RoutedEventArgs e)
    {
        DataCache.ActiveView = RightExplorer.ViewModel;
        DataCache.NotActiveView = LeftExplorer.ViewModel;
        RightExplorer.IsSelected = true;
        LeftExplorer.IsSelected = false;
    }

    private void LeftExplorer_OnGotFocus(object sender, RoutedEventArgs e)
    {
        DataCache.ActiveView = LeftExplorer.ViewModel;
        DataCache.NotActiveView = RightExplorer.ViewModel;
        LeftExplorer.IsSelected = true;
        RightExplorer.IsSelected = false;
    }

    #endregion
}