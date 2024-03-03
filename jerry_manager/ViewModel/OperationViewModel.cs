using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ookii.Dialogs.Wpf;
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
                    NamePlaceHolder = DataCache.NotActiveView.CurrentPath;
                    break;
                case OperationType.Move:
                    NamePlaceHolder = DataCache.NotActiveView.CurrentPath;
                    break;
                case OperationType.Rename:
                    NamePlaceHolder = DataCache.ActiveView.SelectedFileObject.Name;

                    break;
                case OperationType.CreateFolder:
                    NamePlaceHolder = "New Folder";
                    break;
                case OperationType.CreateFile:
                    NamePlaceHolder = "File";
                    break;
                case OperationType.UnPack:
                    NamePlaceHolder = DataCache.ActiveView.CurrentPath;
                    break;
                default:
                    NamePlaceHolder = string.Empty;
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
                    case OperationType.CreateFile:
                        return "Enter a new file name:";
                    case OperationType.UnPack:
                        return "Unpack to:";
                    default:
                        throw new Exception("Fatal error.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return string.Empty;
        }
    }

    private string m_namePlaceHolder;

    public string NamePlaceHolder
    {
        get => m_namePlaceHolder;
        set
        {
            m_namePlaceHolder = value;
            OnPropertyChanged("NamePlaceHolder");
        }
    }

    private string m_currentFolderPath;

    public string CurrentFolderPath
    {
        get
        {
            if (m_currentFolderPath == DataCache.ActiveView.CurrentPath)
            {
                return DataCache.ActiveView.CurrentPath;
            }

            return m_currentFolderPath;
        }
        set => m_currentFolderPath = value;
    }

    #endregion

    #region Constructors

    public OperationViewModel()
    {
        CurrentFolderPath = DataCache.ActiveView.CurrentPath;
        NamePlaceHolder = string.Empty;
    }

    #endregion

    #region Methhods

    public bool OperationStart()
    {
        switch (m_operationType)
        {
            case OperationType.Copy:
                Operation.Copy(NamePlaceHolder, DataCache.ActiveView.SelectedFileObjects);
                break;
            case OperationType.Move:
                Operation.Move(NamePlaceHolder, DataCache.ActiveView.SelectedFileObjects);
                break;
            case OperationType.Rename:
                Operation.Rename(CurrentFolderPath, DataCache.ActiveView.SelectedFileObject, NamePlaceHolder);
                break;
            case OperationType.CreateFolder:
                Operation.CreateFolder(CurrentFolderPath, NamePlaceHolder.Replace(CurrentFolderPath, ""));
                break;
            case OperationType.CreateFile:
                Operation.CreateFile(CurrentFolderPath, NamePlaceHolder.Replace(CurrentFolderPath, ""));
                break;
            case OperationType.UnPack:
                Operation.UnPack(NamePlaceHolder, (Archive)DataCache.ActiveView.SelectedFileObject);
                break;
            default:
                NamePlaceHolder = string.Empty;
                break;
        }

        return true;
    }

    public void PathChoose()
    {
        var placeHolder = string.Empty;
        if (m_operationType is OperationType.CreateFolder or OperationType.CreateFile)
        {
            placeHolder = m_operationType is OperationType.CreateFolder ? "New Folder" : "File";
        }

        var fbd = new VistaFolderBrowserDialog
        {
            SelectedPath = DataCache.NotActiveView.CurrentPath + "\\"
        };
        if (fbd.ShowDialog().GetValueOrDefault())
        {
            CurrentFolderPath = fbd.SelectedPath;
            NamePlaceHolder = fbd.SelectedPath + "\\" + placeHolder;
        }
    }

    #endregion

    #region PropertyChangedInterface

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    #endregion
}
