using System.Windows.Controls;
using System.Windows.Input;
using jerry_manager.ViewModel;

namespace jerry_manager.View;

public partial class FileExplorerView : UserControl
{
    private FileExplorerViewModel m_ViewModel { get; set; }
    public FileExplorerView()
    {
        InitializeComponent();
        m_ViewModel = new FileExplorerViewModel();
        DataContext = m_ViewModel;
    }

    private void FileObjectsListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        m_ViewModel.DoubleClick();
    }
}