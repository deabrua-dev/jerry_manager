using System;
using System.IO;
using System.Linq;
using Aspose.Zip;
using Aspose.Zip.Rar;
using Aspose.Zip.SevenZip;

namespace jerry_manager.Core;

public class Folder : FileSystemObject
{
    #region Variables

    public String DateLastModified
    {
        get => m_DateModified.ToString("dd/MM/yyyy");
    }
    
    public String Size
    {
        get => "<DIR>";
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

    public Folder(ArchiveEntry entry, String path)
    {
        var name = entry.Name.Substring(0, entry.Name.Length - 1);
        m_Name = "[" + ClearName(name, name.Count(i => i == '/')) + "]";
        m_Path = path + "/" + entry.Name.Substring(0, entry.Name.Length - 1); ;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = 0;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    public Folder(RarArchiveEntry entry, String path)
    {
        m_Name = "[" + ClearName(entry.Name, entry.Name.Count(i => i == '/')) + "]";
        m_Path = path + "/" + entry.Name.Substring(0, entry.Name.Length - 1); ;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = 0;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    public Folder(SevenZipArchiveEntry entry, String path)
    {
        m_Name = "[" + ClearName(entry.Name, entry.Name.Count(i => i == '/')) + "]";
        m_Path = path + "/" + entry.Name.Substring(0, entry.Name.Length); ;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = 0;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    #endregion

    #region Methods

    private string ClearName(string name, int count)
    {
        for (int i = 0; i < count; i++)
        {
            name = name.Substring(name.IndexOf('/'));
        }
        return name.Replace("/", "");
    }

    #endregion
}