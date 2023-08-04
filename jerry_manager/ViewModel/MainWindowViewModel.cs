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
                if (DataCache.ActiveView is not null &&
                    DataCache.ActiveView.SelectedFileObject is not null)
                {
                    MessageBox.Show("Edit");
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
                if (DataCache.ActiveView is not null &&
                    DataCache.ActiveView.SelectedFileObjects.Count > 0)
                {
                    MessageBox.Show("Copy");
                    Operation.Copy(DataCache.ActiveView.CurrentPath, DataCache.NotActiveView.CurrentPath, DataCache.ActiveView.SelectedFileObjects);
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
                if (DataCache.ActiveView is not null &&
                    DataCache.ActiveView.SelectedFileObjects.Count > 0)
                {
                    MessageBox.Show("Move");
                    Operation.Move(DataCache.ActiveView.CurrentPath, DataCache.NotActiveView.CurrentPath, DataCache.ActiveView.SelectedFileObjects);
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
                if (DataCache.ActiveView is not null &&
                    DataCache.ActiveView.SelectedFileObject is not null)
                {
                    MessageBox.Show("Rename");
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
                if (DataCache.ActiveView is not null)
                {
                    MessageBox.Show("New Folder");
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
                if (DataCache.ActiveView is not null &&
                    DataCache.ActiveView.SelectedFileObjects.Count > 0)
                {
                    MessageBox.Show("Delete");
                    Operation.Delete(DataCache.ActiveView.SelectedFileObjects);
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
                if (DataCache.ActiveView is not null  && 
                    DataCache.ActiveView.SelectedFileObjects.Count > 0 && 
                    DataCache.ActiveView.SelectedFileObject is Archive)
                {
                    MessageBox.Show("Unpack");
                    Operation.UnPack((Archive)DataCache.ActiveView.SelectedFileObject);
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
                App.Current.Shutdown();
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