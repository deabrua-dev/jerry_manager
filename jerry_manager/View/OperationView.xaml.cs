using jerry_manager.Core.FileSystem;
using jerry_manager.ViewModel;
using System.Windows;

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
        m_ViewModel = new OperationViewModel();
        DataContext = ViewModel;
        ViewModel.Title = operationType.ToString();
    }

    #endregion
}