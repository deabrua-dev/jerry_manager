using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using jerry_manager.ViewModel;

namespace jerry_manager.View;

public partial class FileExplorerView : UserControl
{
    #region Variables

    private FileExplorerViewModel m_ViewModel;
    public FileExplorerViewModel ViewModel
    {
        get => m_ViewModel;
        set => m_ViewModel = value;
    }

    #endregion

    #region Constructors

    public FileExplorerView()
    {
        InitializeComponent();
        m_ViewModel = new FileExplorerViewModel();
        DataContext = ViewModel;
    }

    #endregion

    #region Methods

    private void FileObjectsListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        m_ViewModel.DoubleClick();
    }

    private void FileObjectsListView_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        var width = FileObjectsListView.ActualWidth;
        GridView.Columns[0].Width = width * 0.412;
        GridView.Columns[1].Width = width * 0.15;
        GridView.Columns[2].Width = width * 0.20;
        GridView.Columns[3].Width = width * 0.18;
    }

    #endregion
}