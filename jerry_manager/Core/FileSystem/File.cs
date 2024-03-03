using System;
using System.IO;
using Aspose.Zip;
using Aspose.Zip.Rar;
using Aspose.Zip.SevenZip;

namespace jerry_manager.Core.FileSystem;

public class File : FileSystemObject
{
    #region Constructors

    public File(FileInfo fileInfo)
    {
        m_Name = fileInfo.Name;
        m_Path = fileInfo.FullName;
        m_Extension = fileInfo.Extension;
        m_DateCreated = fileInfo.CreationTime;
        m_DateModified = fileInfo.LastWriteTime;
        m_DateAccessed = fileInfo.LastAccessTime;
        m_SizeInBytes = (UInt64)fileInfo.Length;
        m_Attributes = fileInfo.Attributes;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }

    public File(ArchiveEntry entry, string path)
    {
        m_Name = "[" + entry.Name.Substring(entry.Name.LastIndexOf('/') + 1) + "]";
        m_Path = (path + "\\" + entry.Name.Substring(0, entry.Name.Length - 1)).Replace("/", "\\");
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = entry.UncompressedSize;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    public File(RarArchiveEntry entry, string path)
    {
        m_Name = "[" + entry.Name.Substring(entry.Name.LastIndexOf('/') + 1) + "]";
        m_Path = (path + "\\" + entry.Name.Substring(0, entry.Name.Length - 1)).Replace("/", "\\");
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = entry.UncompressedSize;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    public File(SevenZipArchiveEntry entry, string path)
    {
        m_Name = "[" + entry.Name.Substring(entry.Name.LastIndexOf('/') + 1) + "]";
        m_Path = (path + "\\" + entry.Name.Substring(0, entry.Name.Length - 1)).Replace("/", "\\");
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = entry.UncompressedSize;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    #endregion
}