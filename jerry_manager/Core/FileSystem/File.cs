using System;
using System.IO;
using Aspose.Zip;
using Aspose.Zip.Rar;
using Aspose.Zip.SevenZip;

namespace jerry_manager.Core.FileSystem;

public class File : FileSystemObject
{
    #region Variables

    public string DateLastModified
    {
        get => m_DateModified.ToString("dd/MM/yyyy");
    }
    
    public string Size
    {
        get
        {
            if (m_SizeInBytes < 1_024)
            {
                return m_SizeInBytes + " B";
            }
            else if (m_SizeInBytes < 1_048_576)
            {
                return Math.Round((Double)m_SizeInBytes / 1_024, 2) + " KB";
            }
            else if (m_SizeInBytes < 1_073_741_824)
            {
                return Math.Round((Double)m_SizeInBytes / 1_048_576, 2) + " MB";
            }
            else if (m_SizeInBytes < 1_099_511_627_776)
            {
                return Math.Round((Double)m_SizeInBytes / 1_073_741_824, 2) + " GB";
            }
            else if (m_SizeInBytes < 1_125_899_906_842_624)
            {
                return Math.Round((Double)m_SizeInBytes / 1_099_511_627_776, 2) + " TB";
            }
            else
            {
                return m_SizeInBytes + " B";
            }
        }
    }
    
    #endregion

    #region Constructors

    public File()
    {
        m_Name = String.Empty;
        m_Path = String.Empty;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_Attributes = 0;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }

    public File(FileInfo fileInfo)
    {
        m_Name = fileInfo.Name;
        m_Path = fileInfo.FullName;
        m_Extension = fileInfo.Extension;
        m_DateCreated = fileInfo.CreationTime;
        m_DateModified = fileInfo.LastWriteTime;
        m_SizeInBytes = (UInt64)fileInfo.Length;
        m_Attributes = fileInfo.Attributes;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }
    
    public File(FileInfo fileInfo, string archivePath, bool isArchived)
    {
        m_Name = fileInfo.Name;
        m_Path = fileInfo.FullName;
        m_Extension = fileInfo.Extension;
        m_DateCreated = fileInfo.CreationTime;
        m_DateModified = fileInfo.LastWriteTime;
        m_SizeInBytes = (UInt64)fileInfo.Length;
        m_Attributes = fileInfo.Attributes;
        m_ArchivePath = archivePath;
        m_IsArchived = isArchived;
    }
    
    public File(string name, string path)
    {
        m_Name = name;
        m_Path = path;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_Attributes = 0;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }
    
    public File(ArchiveEntry entry, string path)
    {
        m_Name = "[" + entry.Name.Substring(entry.Name.LastIndexOf('/') + 1) + "]";
        m_Path = path + "/" + entry.Name.Substring(0, entry.Name.Length - 1); ;
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
        m_Path = path + "/" + entry.Name.Substring(0, entry.Name.Length - 1); ;
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
        m_Path = path + "/" + entry.Name.Substring(0, entry.Name.Length - 1); ;
        m_Extension = String.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = entry.UncompressedSize;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    #endregion
}