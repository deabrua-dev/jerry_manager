using System;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;
using jerry_manager.Core.Services;

namespace jerry_manager.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    #region Variables

    public string ActivePath
    {
        get => DataCache.ActiveView.CurrentPath + "> ";
    }

    #endregion

    #region Constructors

    public MainWindowViewModel()
    {

    }

    #endregion

    #region Commands

    private ICommand editCommand;

    public ICommand EditCommand
    {
        get
        {
            return editCommand ??= new RelayCommand(obj =>
            {
                try
                {
                    if (DataCache.ActiveView is not null &&
                    DataCache.ActiveView.SelectedFileObject is not null)
                    {
                        
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }

    private ICommand copyCommand;

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

    private ICommand moveCommand;

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

    private ICommand renameCommand;

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

    private ICommand newFolderCommand;

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

    private ICommand deleteCommand;

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
                        OperationWindowService.Show(OperationType.Delete);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            });
        }
    }

    private ICommand unPackCommand;

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

    private ICommand quitCommand;

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

    #endregion

    #region PropertyChangedInterface

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    #endregion
}