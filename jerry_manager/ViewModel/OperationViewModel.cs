using System;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.ViewModel;

public class OperationViewModel
{
    #region Variables

    private string m_title;

    public string Title
    {
        get => m_title; 
        set => m_title = value;
    }

    #endregion

    #region Constructos

    public OperationViewModel()
    {
        m_title = String.Empty;
    }

    public OperationViewModel(OperationType operationType)
    {
        m_title = GetOperationTitle(operationType);
    }

    #endregion

    #region Methods

    private string GetOperationTitle(OperationType operationType)
    {
        switch (operationType)
        {
            case OperationType.Copy:
                return "Copy to:";
            case OperationType.Move:
                return "Move to:";
            case OperationType.Rename:
                return "Enter the new name:";
            case OperationType.CreateNewFolder:
                return "Enter the folder name:";
            default:
                return "Error!";
        }
    }

    #endregion
}
