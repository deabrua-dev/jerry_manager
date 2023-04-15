using System;
using System.IO;
using Aspose.Zip;

namespace jerry_manager.Core;

public class ParentFolder : FileSystemObject
{
    #region Variables

    public String DateLastModified
    {
        get => String.Empty;
    }

    public String Size
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

    #endregion
}