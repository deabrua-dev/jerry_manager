using System.Windows;
using System.Windows.Input;
using jerry_manager.Core;

namespace jerry_manager.ViewModel;

public class MainWindowViewModel
{
    private ICommand copyCommand;

    public ICommand CopyCommand
    {
        get
        {
            return copyCommand ??= new RelayCommand(obj =>
            {
                MessageBox.Show("Copy");
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
                MessageBox.Show("Move");
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
                MessageBox.Show("Delete");
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
}