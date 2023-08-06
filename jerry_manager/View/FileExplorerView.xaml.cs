using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

    private bool m_Selected;
    public bool IsSelected
    {
        get => m_Selected;
        set
        {
            m_Selected = value;
            if (m_Selected)
            {
                FileObjectsListView.BorderBrush = Brushes.Red;
            } 
            else
            {
                FileObjectsListView.BorderBrush = Brushes.Black;
            }
        }
    }

    private Style ListViewStyle { get; set; }

    #endregion

    #region Constructors

    public FileExplorerView()
    {
        InitializeComponent();
        m_ViewModel = new FileExplorerViewModel();
        DataContext = ViewModel;
        FileObjectsListView.BorderThickness = new Thickness(2.0);
        IsSelected = false;
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