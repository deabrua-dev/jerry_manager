using System.Windows;
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
        ViewModel = new OperationViewModel();
        DataContext = ViewModel;
    }

    #endregion

    #region Methods

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.OperationStart();
        Close();
    }

    #endregion

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.PathChoose();
    }
}