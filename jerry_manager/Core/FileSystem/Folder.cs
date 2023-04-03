using System;
using System.IO;
using Aspose.Zip;

namespace jerry_manager.Core;

public class Folder : FileSystemObject
{
    #region Variables

    public String DateLastModified
    {
        get => m_Name == "[...]" ? String.Empty : m_DateModified.ToString("dd/MM/yyyy");
    }
    
    public String Size
    {
        get => m_Name == "[...]" ? String.Empty : "<DIR>";
    }
    
    #endregion
    
    #region Constructors

    public Folder()
    {
        m_Name = String.Empty;
        m_Path = String.Empty;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }

    public Folder(DirectoryInfo folderInfo)
    {
        m_Name = "[" + folderInfo.Name + "]";
        m_Path = folderInfo.FullName;
        m_Extension = String.Empty;
        m_DateCreated = folderInfo.CreationTime;
        m_DateModified = folderInfo.LastWriteTime;
        m_SizeInBytes = 0;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }
    
    public Folder(DirectoryInfo folderInfo, String archivePath, Boolean isArchived)
    {
        m_Name = "[" + folderInfo.Name + "]";
        m_Path = folderInfo.FullName;
        m_Extension = String.Empty;
        m_DateCreated = folderInfo.CreationTime;
        m_DateModified = folderInfo.LastWriteTime;
        m_SizeInBytes = 0;
        m_ArchivePath = archivePath;
        m_IsArchived = isArchived;
    }
    
    public Folder(String name, String path)
    {
        m_Name = name;
        m_Path = path;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }

    public Folder(ArchiveEntry entry, String path)
    {
        var name = entry.Name.Substring(0, entry.Name.Length - 1);
        m_Name = "[" + name + "]";
        m_Path = path + "/" + name;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = 0;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    #endregion
}