using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using jerry_manager.Core.FileSystem;
using jerry_manager.Model;

namespace jerry_manager.ViewModel;

public class FilePropertiesViewModel
{
    #region Variables

    private FilePropertiesModel m_Model;

    private FileSystemObject m_CurrentFileSystemObject;

    public FileSystemObject CurrentFileSystemObject
    {
        get => m_CurrentFileSystemObject;
        set
        {
            m_CurrentFileSystemObject = value;
            m_FileName = m_CurrentFileSystemObject.Name;
            m_FileIsReadOnly = (m_CurrentFileSystemObject.Attributes & FileAttributes.ReadOnly) != 0;
            m_FileIsHidden = (m_CurrentFileSystemObject.Attributes & FileAttributes.Hidden) != 0;
        }
    }

    public ImageSource? IconToView => m_Model.GetIconImage(CurrentFileSystemObject);

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

    public string FileType => m_CurrentFileSystemObject.Extension;
    public string FileLocation => m_CurrentFileSystemObject.Path.Replace(m_CurrentFileSystemObject.Name, "");
    public string FileSize => m_CurrentFileSystemObject.SizeInBytes.ToString("N0", new CultureInfo("uk-UA"));

    public string FileSizeOnDisk => m_Model.GetFileSizeOnDisk(m_CurrentFileSystemObject.Path)
        .ToString("N0", new CultureInfo("uk-UA"));

    public string FileDateCreated =>
        m_CurrentFileSystemObject.DateCreated.DayOfWeek + ", " +
        m_CurrentFileSystemObject.DateCreated.ToString(new CultureInfo("uk-UA"));

    public string FileDateModified =>
        m_CurrentFileSystemObject.DateModified.DayOfWeek + ", " +
        m_CurrentFileSystemObject.DateModified.ToString(new CultureInfo("uk-UA"));

    public string FileDateAccessed =>
        m_CurrentFileSystemObject.DateAccessed.DayOfWeek + ", " +
        m_CurrentFileSystemObject.DateAccessed.ToString(new CultureInfo("uk-UA"));


    private bool m_FileIsReadOnly;

    public bool FileIsReadOnly
    {
        get => m_FileIsReadOnly;
        set
        {
            m_FileIsReadOnly = value;
            OnPropertyChanged("FileIsReadOnly");
        }
    }

    private bool m_FileIsHidden;

    public bool FileIsHidden
    {
        get => m_FileIsHidden;
        set
        {
            m_FileIsHidden = value;
            OnPropertyChanged("FileIsHidden");
        }
    }

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

    #region Methods

    public void CheckChanges()
    {
        if (!m_FileName.Equals(m_CurrentFileSystemObject.Name))
        {
            m_Model.RenameFile(m_CurrentFileSystemObject, m_FileName);
        }

        if (!m_FileIsReadOnly.Equals((m_CurrentFileSystemObject.Attributes & FileAttributes.ReadOnly) != 0))
        {
            var attributes = m_CurrentFileSystemObject.Attributes;
            if (m_FileIsReadOnly)
            {
                m_Model.ChangeAttributes(m_CurrentFileSystemObject, attributes | FileAttributes.ReadOnly);
            }
            else
            {
                attributes &= ~FileAttributes.ReadOnly;
                m_Model.ChangeAttributes(m_CurrentFileSystemObject, attributes);
            }
        }

        if (!m_FileIsHidden.Equals((m_CurrentFileSystemObject.Attributes & FileAttributes.Hidden) != 0))
        {
            var attributes = m_CurrentFileSystemObject.Attributes;
            if (m_FileIsHidden)
            {
                m_Model.ChangeAttributes(m_CurrentFileSystemObject, attributes | FileAttributes.Hidden);
            }
            else
            {
                attributes &= ~FileAttributes.Hidden;
                m_Model.ChangeAttributes(m_CurrentFileSystemObject, attributes);
            }
        }
    }

    #endregion
}