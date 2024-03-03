using System;
using System.Windows;
using jerry_manager.Core.FileSystem;
using jerry_manager.ViewModel;

namespace jerry_manager.View;

public partial class OperationView : Window
{
    #region Variables

    private OperationViewModel m_ViewModel;

    public OperationViewModel ViewModel
    {
        get => m_ViewModel;
        set => m_ViewModel = value;
    }

    #endregion

    #region Constructors

    public OperationView()
    {
        InitializeComponent();
        m_ViewModel = new OperationViewModel();
        DataContext = ViewModel;
    }

    public OperationView(OperationType operationType)
    {
        InitializeComponent();
        m_ViewModel = new OperationViewModel
        {
            OperationType = operationType
        };
        if (operationType is OperationType.Rename)
        {
            PathChooseButton.IsEnabled = false;
        }

        DataContext = ViewModel;
    }

    #endregion

    #region Methods

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        try
        {
            ViewModel.PathChoose();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

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
            ViewModel.OperationStart();
            Close();
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }

    #endregion
}