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

    public FilePropertiesModel Model
    {
        get => m_Model;
        set => m_Model = value;
    }

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

    public ImageSource IconToView
    {
        get => m_Model.GetIconImage(CurrentFileSystemObject);
    }

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

    public string Size
    {
        get => m_currentFileSystemObject.SizeInBytes.ToString("N0", new CultureInfo("uk-UA"));
    }
    
    public string SizeOnDisk
    {
        get => m_Model.GetFileSizeOnDisk(m_currentFileSystemObject.Path).ToString("N0", new CultureInfo("uk-UA"));
    }

    #endregion
    
    #region Constructors

    public FilePropertiesViewModel()
    {
        Model = new FilePropertiesModel();
    }

    #endregion
    
    #region PropertyChangedInterface
    
    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    
    #endregion
}