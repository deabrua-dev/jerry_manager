using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;
using jerry_manager.Core.Services;
using jerry_manager.Model;

namespace jerry_manager.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    #region Variables

    private MainWindowModel m_Model { get; set; }
    public string ActivePath
    {
        get => DataCache.ActiveView.CurrentPath + "> ";
    }

    private ICommand openCommand;
    private ICommand editCommand;
    private ICommand copyCommand;
    private ICommand moveCommand;
    private ICommand renameCommand;
    private ICommand newFolderCommand;
    private ICommand newFileCommand;
    private ICommand deleteCommand;
    private ICommand unPackCommand;
    private ICommand quitCommand;
    private ICommand propertiesCommand;

    #endregion

    #region Constructor

    public MainWindowViewModel()
    {
        m_Model = new MainWindowModel();
    }

    #endregion
    
    #region Commands

    public ICommand OpenCommand
    {
        get
        {
            return openCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                        DataCache.ActiveView.SelectedFileObject is not null && (
                        DataCache.ActiveView.SelectedFileObject is File ||
                        DataCache.ActiveView.SelectedFileObject is Archive))
                    {
                        m_Model.Open(DataCache.ActiveView.SelectedFileObject);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }

    public ICommand EditCommand
    {
        get
        {
            return editCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                        DataCache.ActiveView.SelectedFileObject is not null && (
                        DataCache.ActiveView.SelectedFileObject is File ||
                        DataCache.ActiveView.SelectedFileObject is Archive))
                    {
                        m_Model.Edit(DataCache.ActiveView.SelectedFileObject);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    public ICommand CopyCommand
    {
        get
        {
            return copyCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                        DataCache.ActiveView.SelectedFileObjects.Count > 0)
                    {
                        OperationWindowService.Show(OperationType.Copy);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    public ICommand MoveCommand
    {
        get
        {
            return moveCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                        DataCache.ActiveView.SelectedFileObjects.Count > 0)
                    {
                        OperationWindowService.Show(OperationType.Move);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    public ICommand RenameCommand
    {
        get
        {
            return renameCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                        DataCache.ActiveView.SelectedFileObject is not null &&
                        DataCache.ActiveView.SelectedFileObjects.Count == 1)
                    {
                        OperationWindowService.Show(OperationType.Rename);
                    }
                    else if (DataCache.ActiveView.SelectedFileObjects.Count > 1)
                    {
                        throw new Exception("You can rename only one file/folder in time.");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    public ICommand NewFolderCommand
    {
        get
        {
            return newFolderCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null)
                    {
                        OperationWindowService.Show(OperationType.CreateFolder);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    public ICommand NewFileCommand
    {
        get
        {
            return newFileCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null)
                    {
                        OperationWindowService.Show(OperationType.CreateFile);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    public ICommand DeleteCommand
    {
        get
        {
            return deleteCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                        DataCache.ActiveView.SelectedFileObjects.Count > 0)
                    {
                        m_Model.Delete(DataCache.ActiveView.SelectedFileObjects);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    public ICommand UnPackCommand
    {
        get
        {
            return unPackCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                        DataCache.ActiveView.SelectedFileObjects.Count > 0 &&
                        DataCache.ActiveView.SelectedFileObject is Archive)
                    {
                        OperationWindowService.Show(OperationType.UnPack);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    public ICommand QuitCommand
    {
        get
        {
            return quitCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    App.Current.Shutdown();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }
    
    public ICommand PropertiesCommand
    {
        get
        {
            return propertiesCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                        DataCache.ActiveView.SelectedFileObject is not null)
                    {
                        FilePropertiesWindowService.Show(DataCache.ActiveView.SelectedFileObject);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }

    #endregion

    #region PropertyChangedInterface

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    #endregion
}