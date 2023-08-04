using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;


namespace jerry_manager.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    #region Variables

    public string ActivePath
    {
        get => DataCache.ActiveView.CurrentPath + "> ";
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
                        MessageBox.Show("Edit");
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
                        MessageBox.Show("Copy");
                        Operation.Copy(DataCache.NotActiveView.CurrentPath, DataCache.ActiveView.SelectedFileObjects);
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
                        MessageBox.Show("Move");
                        Operation.Move(DataCache.NotActiveView.CurrentPath, DataCache.ActiveView.SelectedFileObjects);
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
                        MessageBox.Show("Rename");
                        Operation.Rename(DataCache.ActiveView.CurrentPath, DataCache.ActiveView.SelectedFileObject, "TempName");
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
                        MessageBox.Show("New Folder");
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
                        MessageBox.Show("Delete");
                        Operation.Delete(DataCache.ActiveView.SelectedFileObjects);
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
                        MessageBox.Show("Unpack");
                        Operation.UnPack((Archive)DataCache.ActiveView.SelectedFileObject);
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