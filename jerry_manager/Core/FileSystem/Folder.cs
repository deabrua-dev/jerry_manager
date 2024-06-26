using Aspose.Zip;
using Aspose.Zip.Rar;
using Aspose.Zip.SevenZip;
using System;
using System.IO;

namespace jerry_manager.Core.FileSystem;

public class Folder : FileSystemObject
{
    #region Variables

    public string Size
    {
        get => "<DIR>";
    }

    #endregion

    #region Constructors

    public Folder(DirectoryInfo folderInfo)
    {
        m_Name = "[" + folderInfo.Name + "]";
        m_Path = folderInfo.FullName;
        m_Extension = string.Empty;
        m_DateCreated = folderInfo.CreationTime;
        m_DateModified = folderInfo.LastWriteTime;
        m_SizeInBytes = 0;
        m_ArchivePath = string.Empty;
        m_IsArchived = false;
    }

    public Folder(ArchiveEntry entry, string path)
    {
        var name = entry.Name.Substring(0, entry.Name.Length - 1);
        m_Name = "[" + Operation.ClearName(name) + "]";
        m_Path = (path + "\\" + entry.Name.Substring(0, entry.Name.Length - 1)).Replace("/", "\\");
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = 0;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    public Folder(RarArchiveEntry entry, string path)
    {
        m_Name = "[" + Operation.ClearName(entry.Name) + "]";
        m_Path = (path + "\\" + entry.Name.Substring(0, entry.Name.Length - 1)).Replace("/", "\\");
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = 0;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    public Folder(SevenZipArchiveEntry entry, string path)
    {
        m_Name = "[" + Operation.ClearName(entry.Name) + "]";
        m_Path = (path + "\\" + entry.Name.Substring(0, entry.Name.Length)).Replace("/", "\\");
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = 0;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    #endregion
}