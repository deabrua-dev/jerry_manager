using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using jerry_manager.Core;

namespace jerry_manager.ViewModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    #region Variables

    

    #endregion

    #region Commands

    private ICommand copyCommand;

    public ICommand CopyCommand
    {
        get
        {
            return copyCommand ??= new RelayCommand(obj =>
            {
                if (DataCache.ActiveView.SelectedFileObjects.Count > 0)
                {
                    MessageBox.Show("Copy");
                }
                else
                {

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

    #endregion

    #region PropertyChangedInterface

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    #endregion
}