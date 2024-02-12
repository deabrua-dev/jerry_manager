using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using jerry_manager.ViewModel;
using jerry_manager.Core.FileSystem;

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

    private bool m_sortDirection;

    public bool SortDirection
    {
        get => m_sortDirection;
        set { m_sortDirection = value; }
    }

    #endregion

    #region Constructors

    public FileExplorerView()
    {
        InitializeComponent();
        m_ViewModel = new FileExplorerViewModel();
        DataContext = ViewModel;
        FileObjectsListView.BorderThickness = new Thickness(2.0);
        IsSelected = false;
        SortDirection = true;
    }

    #endregion

    #region Methods

    private void FileObjectsListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            m_ViewModel.DoubleClick();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private void FileObjectsListView_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        try
        {
            var width = FileObjectsListView.ActualWidth;
            GridView.Columns[0].Width = width * 0.412;
            GridView.Columns[1].Width = width * 0.15;
            GridView.Columns[2].Width = width * 0.20;
            GridView.Columns[3].Width = width * 0.18;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private void FileObjectsListView_ColumnClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            var column = (sender as GridViewColumnHeader);
            var sortBy = column!.Tag.ToString();
            SortDirection = !SortDirection;
            m_ViewModel.OrderListBy(sortBy, SortDirection);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    #endregion

    private void FileObjectsListView_OnDrop(object sender, DragEventArgs e)
    {
        try
        {
            var dropSource = e.Data.GetData("DragSource");
            var droppedItems = e.Data.GetData(typeof(List<FileSystemObject>)) as List<FileSystemObject>;
            if (!ReferenceEquals(dropSource, this) && droppedItems!.Count != 0)
            {
                ViewModel.CopyDropped(droppedItems);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private void FileObjectsListView_OnMouseMove(object sender, MouseEventArgs e)
    {
        try
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is ListViewItem)
            {
                var list = new List<FileSystemObject>();
                foreach (var item in ViewModel.SelectedFileObjects)
                {
                    list.Add(item);
                }

                DataObject dataObject = new DataObject(list);
                dataObject.SetData("DragSource", this);
                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}