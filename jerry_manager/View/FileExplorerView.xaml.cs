using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using jerry_manager.Core;
using jerry_manager.ViewModel;
using Microsoft.VisualBasic;

namespace jerry_manager.View;

public partial class FileExplorerView : UserControl
{
    private FileExplorerViewModel m_ViewModel;
    public FileExplorerViewModel ViewModel
    {
        get => m_ViewModel;
        set => m_ViewModel = value;
    }

    public FileExplorerView()
    {
        InitializeComponent();
        ViewModel = new FileExplorerViewModel();
        DataContext = ViewModel;
    }

    private void FileObjectsListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        m_ViewModel.DoubleClick();
    }

    private void FileObjectsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var items = (sender as ListView).SelectedItems;
        ViewModel.SelectedFileObjects = items.Cast<FileSystemObject>().Where(i => !(i is ParentFolder)).ToList();
    }
}