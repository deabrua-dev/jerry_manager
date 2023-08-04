using System;

namespace jerry_manager.Core.FileSystem;

public class ParentFolder : FileSystemObject
{
    #region Variables

    public string DateLastModified
    {
        get => String.Empty;
    }

    public string Size
    {
        get => String.Empty;
    }

    #endregion

    #region Constructors

    public ParentFolder()
    {
        m_Name = "[...]";
        m_Path = String.Empty;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }

    public ParentFolder(string path)
    {
        m_Name = "[...]";
        m_Path = path;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }
    
    public ParentFolder(string path, string archivePath, bool isArchived)
    {
        m_Name = "[...]";
        m_Path = path.Substring(0, path.Length - 1);
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_ArchivePath = archivePath;
        m_IsArchived = isArchived;
    }

    #endregion
}