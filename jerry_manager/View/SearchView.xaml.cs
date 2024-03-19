using System;
using System.Windows;
using System.Windows.Input;
using jerry_manager.ViewModel;

namespace jerry_manager.View;

public partial class SearchView : Window
{
    #region Variables

    private SearchViewModel m_ViewModel;
    public SearchViewModel ViewModel
    {
        get => m_ViewModel;
        set => m_ViewModel = value;
    }

    #endregion

    #region Constructors

    public SearchView()
    {
        InitializeComponent();
        m_ViewModel = new SearchViewModel();
        DataContext = ViewModel;
        Height = 300;
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

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ViewModel.StartSearch();
            Height = 600;
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
    
    private void SearchResultListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
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

    private void SearchResultListView_ColumnClicked(object sender, RoutedEventArgs e)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    #endregion
}