using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using jerry_manager.Core.FileSystem;
using jerry_manager.Model;

namespace jerry_manager.ViewModel;

public class SearchViewModel
{
    #region Variables

    private SearchModel m_Model { get; set; }
    public ObservableCollection<FileSystemObject> Items { get; set; }
    private FileSystemObject m_SelectedObject;

    public FileSystemObject SelectedFileObject
    {
        get => m_SelectedObject;
        set
        {
            m_SelectedObject = value;
            OnPropertyChanged("SelectedObject");
        }
    }

    private bool m_IsArchive;

    public bool IsArchive
    {
        get => m_IsArchive;
        set
        {
            m_IsArchive = value;
            OnPropertyChanged("IsArchive");
        }
    }

    private bool m_IsReadOnly;

    public bool IsReadOnly
    {
        get => m_IsReadOnly;
        set
        {
            m_IsReadOnly = value;
            OnPropertyChanged("IsReadOnly");
        }
    }

    private bool m_IsHidden;

    public bool IsHidden
    {
        get => m_IsHidden;
        set
        {
            m_IsHidden = value;
            OnPropertyChanged("IsHidden");
        }
    }

    private bool m_IsSystem;

    public bool IsSystem
    {
        get => m_IsSystem;
        set
        {
            m_IsSystem = value;
            OnPropertyChanged("IsSystem");
        }
    }

    private bool m_IsDirectory;

    public bool IsDirectory
    {
        get => m_IsDirectory;
        set
        {
            m_IsDirectory = value;
            OnPropertyChanged("IsDirectory");
        }
    }

    private bool m_IsCompressed;

    public bool IsCompressed
    {
        get => m_IsCompressed;
        set
        {
            m_IsCompressed = value;
            OnPropertyChanged("IsCompressed");
        }
    }

    private bool m_IsEncrypted;

    public bool IsEncrypted
    {
        get => m_IsEncrypted;
        set
        {
            m_IsEncrypted = value;
            OnPropertyChanged("IsEncrypted");
        }
    }

    #endregion

    #region Constructor

    public SearchViewModel()
    {
        m_Model = new SearchModel();
        m_IsArchive = true;
        m_IsReadOnly = true;
        m_IsHidden = true;
        m_IsSystem = true;
        m_IsCompressed = true;
        m_IsEncrypted = true;
        m_IsDirectory = true;
    }

    #endregion

    #region Methods

    public void StartSearch()
    {

    }

    public void DoubleClick()
    {

    }

    #endregion

    #region PropertyChangedInterface

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    #endregion

}