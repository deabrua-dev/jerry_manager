using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.ViewModel;

public class OperationViewModel : INotifyPropertyChanged
{
    #region Variables

    private OperationType m_operationType;
    public OperationType OperationType
    {
        get => m_operationType;
        set
        {
            m_operationType = value;
            switch (m_operationType)
            {
                case OperationType.Copy:
                    FolderName = DataCache.NotActiveView.CurrentPath;
                    break;
                case OperationType.Move:
                    FolderName = DataCache.NotActiveView.CurrentPath;
                    break;
                case OperationType.Rename:
                    FolderName = DataCache.ActiveView.SelectedFileObject.Name;
                    break;
                case OperationType.CreateFolder:
                    FolderName = "New Folder";
                    break;
                case OperationType.UnPack:
                    FolderName = DataCache.ActiveView.CurrentPath;
                    break;
                default:
                    FolderName = string.Empty;
                    break;
            }
        }
    }
    public string OperationName
    {
        get
        {
            try
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
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return string.Empty;
        }
    }

    private string m_FolderName;

    public string FolderName
    {
        get => m_FolderName;
        set => m_FolderName = value;
    }

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
        Operation.UnPack((Archive)DataCache.ActiveView.SelectedFileObject);
    }

    #endregion

    #region PropertyChangedInterface

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    #endregion
}
