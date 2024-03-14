using System;
using System.Windows;
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
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    #endregion
}