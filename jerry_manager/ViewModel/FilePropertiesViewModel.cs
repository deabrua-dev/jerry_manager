using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using jerry_manager.Core.FileSystem;
using jerry_manager.Model;

namespace jerry_manager.ViewModel;

public class FilePropertiesViewModel
{
    #region Variables

    private FilePropertiesModel m_Model;

    private FileSystemObject m_currentFileSystemObject;
    public FileSystemObject CurrentFileSystemObject
    {
        get => m_currentFileSystemObject;
        set
        {
            m_currentFileSystemObject = value;
            FileName = m_currentFileSystemObject.Name;
        }
    }

    public ImageSource IconToView => m_Model.GetIconImage(CurrentFileSystemObject);

    private string m_FileName;

    public string FileName
    {
        get => m_FileName;
        set
        {
            m_FileName = value;
            OnPropertyChanged("FileName");
        }
    }
    
    public string FileType => m_currentFileSystemObject.Extension;
    public string FileLocation => m_currentFileSystemObject.Path.Replace(m_currentFileSystemObject.Name, "");
    public string FileSize => m_currentFileSystemObject.SizeInBytes.ToString("N0", new CultureInfo("uk-UA"));

    public string FileSizeOnDisk => m_Model.GetFileSizeOnDisk(m_currentFileSystemObject.Path)
        .ToString("N0", new CultureInfo("uk-UA"));

    public string FileDateCreated =>
        m_currentFileSystemObject.DateCreated.DayOfWeek + ", " +
        m_currentFileSystemObject.DateCreated.ToString(new CultureInfo("uk-UA"));

    public string FileDateModified =>
        m_currentFileSystemObject.DateModified.DayOfWeek + ", " +
        m_currentFileSystemObject.DateModified.ToString(new CultureInfo("uk-UA"));
    
    public string FileDateAccessed =>
        m_currentFileSystemObject.DateAccessed.DayOfWeek + ", " +
        m_currentFileSystemObject.DateAccessed.ToString(new CultureInfo("uk-UA"));

    #endregion
    
    #region Constructors

    public FilePropertiesViewModel()
    {
        m_Model = new FilePropertiesModel();
        m_FileName = string.Empty;
    }

    #endregion
    
    #region PropertyChangedInterface
    
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    
    #endregion

    public void CheckChanges()
    {
        if (!FileName.Equals(m_currentFileSystemObject.Name))
        {
            m_Model.RenameFile(m_currentFileSystemObject, FileName);
        }
    }
}