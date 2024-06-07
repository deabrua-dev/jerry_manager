using jerry_manager.Core.FileSystem;
using jerry_manager.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            FileObjectsListView.BorderBrush = m_Selected ? Brushes.Red : Brushes.Black;
        }
    }

    private bool m_SortDirection;

    public bool SortDirection
    {
        get => m_SortDirection;
        set => m_SortDirection = value;
    }

    private Point m_MousePos;
    private bool m_MultipleChoose;
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
            var mousePosDiff = m_MousePos - e.GetPosition(null);
            if (e.LeftButton == MouseButtonState.Pressed && ViewModel.SelectedFileObjects.Count != 0 &&
                (Math.Abs(mousePosDiff.X) > SystemParameters.MinimumHorizontalDragDistance &&
                 Math.Abs(mousePosDiff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                DataObject dataObject = new DataObject(ViewModel.SelectedFileObjects);
                dataObject.SetData("DragSource", this);
                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    private void FileObjectsListView_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        m_MousePos = e.GetPosition(null);
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (FileObjectsListView.SelectedItems.Count > 1 && !m_MultipleChoose)
            {
                e.Handled = true;
            }
        }
    }

    private void FileObjectsListView_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var source = sender as ListViewItem;
        if (source.IsSelected)
        {
            source.IsSelected = false;
            e.Handled = true;
        }
    }

    private void FileObjectsListView_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.LeftCtrl or Key.LeftShift)
        {
            m_MultipleChoose = true;
        }
    }

    private void FileObjectsListView_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.LeftCtrl or Key.LeftShift)
        {
            m_MultipleChoose = false;
        }
    }

    #endregion
}