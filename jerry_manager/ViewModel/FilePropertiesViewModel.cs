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
        set => m_currentFileSystemObject = value;
    }

    private ImageSource m_IconToView;

    public ImageSource IconToView
    {
        get => m_Model.GetIconImage(CurrentFileSystemObject);
    }

    #endregion
    
    #region Constructors

    public FilePropertiesViewModel()
    {
        Model = new FilePropertiesModel();
    }

    #endregion
}