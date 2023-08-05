using System;
using System.Xml.Linq;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.ViewModel;

public class OperationViewModel
{
    #region Variables

    private OperationType m_operationType;
    public OperationType OperationType
    {
        get => m_operationType;
        set
        {
            m_operationType = value;
        }
    }
    public string OperationName
    {
        get
        {
            switch (m_operationType)
            {
                case OperationType.Copy:
                    return "Copy to";
                case OperationType.Move:
                    return "Move to:";
                case OperationType.Rename:
                    return "Enter a new folder name:";
                case OperationType.CreateFolder:
                    return "Enter a new folder name:";
                case OperationType.UnPack:
                    return "Unpack to:";
                default:
                    throw new Exception("Fatal error.");
            }
        }
    }

    public string FolderName { get; set; }

    #endregion

    #region Constructors

    public OperationViewModel()
    {
        FolderName = string.Empty;
    }

    #endregion

    #region Methhods

    public void OperationStart()
    {
        Operation.Copy(DataCache.NotActiveView.CurrentPath, DataCache.ActiveView.SelectedFileObjects);
        Operation.Move(DataCache.NotActiveView.CurrentPath, DataCache.ActiveView.SelectedFileObjects);
        Operation.Rename(DataCache.ActiveView.CurrentPath, DataCache.ActiveView.SelectedFileObject, "TempName");
        Operation.CreateFolder(DataCache.ActiveView.CurrentPath, FolderName);
        Operation.Delete(DataCache.ActiveView.SelectedFileObjects);
        Operation.UnPack((Archive)DataCache.ActiveView.SelectedFileObject);
    }

    #endregion
}
