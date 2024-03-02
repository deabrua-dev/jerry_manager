using System.Windows;
using jerry_manager.ViewModel;

namespace jerry_manager.View;

public partial class FilePropertiesView : Window
{
    #region Variables

    private FilePropertiesViewModel m_ViewModel;

    public FilePropertiesViewModel ViewModel
    {
        get => m_ViewModel;
        set => m_ViewModel = value;
    }

    #endregion

    #region Constructors

    public FilePropertiesView()
    {
        InitializeComponent();
        ViewModel = new FilePropertiesViewModel();
        DataContext = ViewModel;
    }

    #endregion

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}
    