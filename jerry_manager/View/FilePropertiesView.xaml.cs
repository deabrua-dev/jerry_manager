using System;
using System.Windows;
using jerry_manager.Core.FileSystem;
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
        m_ViewModel = new FilePropertiesViewModel();
        DataContext = ViewModel;
    }

    public FilePropertiesView(FileSystemObject fileSystemObject)
    {
        InitializeComponent();
        m_ViewModel = new FilePropertiesViewModel
        {
            CurrentFileSystemObject = fileSystemObject
        };
        DataContext = ViewModel;
    }

    #endregion

    #region Methods

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ViewModel.CheckChanges();
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    #endregion
}
    