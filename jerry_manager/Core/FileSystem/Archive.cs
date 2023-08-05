using System;
using System.IO;
using Aspose.Zip;
using Aspose.Zip.Rar;
using Aspose.Zip.SevenZip;

namespace jerry_manager.Core.FileSystem;
public class Archive : FileSystemObject
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

    private ArchiveType m_ArchiveType;

    public ArchiveType ArchiveType
    {
        get => m_ArchiveType;
        set => m_ArchiveType = value;
    }

    #endregion
    
    #region Constructors

    public Archive()
    {
        m_Name = string.Empty;
        m_Path = string.Empty;
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_Attributes = 0;
        m_ArchiveType = GetArchiveType(m_Name);
        m_ArchivePath = string.Empty;
        m_IsArchived = false;
    }

    public Archive(FileInfo fileInfo)
    {
        m_Name = fileInfo.Name;
        m_Path = fileInfo.FullName;
        m_Extension = fileInfo.Extension;
        m_DateCreated = fileInfo.CreationTime;
        m_DateModified = fileInfo.LastWriteTime;
        m_SizeInBytes = (ulong)fileInfo.Length;
        m_Attributes = fileInfo.Attributes;
        m_ArchiveType = GetArchiveType(m_Name);
        m_ArchivePath = string.Empty;
        m_IsArchived = false;
    }
    
    public Archive(FileInfo fileInfo, string archivePath, bool isArchived)
    {
        m_Name = fileInfo.Name;
        m_Path = fileInfo.FullName;
        m_Extension = fileInfo.Extension;
        m_DateCreated = fileInfo.CreationTime;
        m_DateModified = fileInfo.LastWriteTime;
        m_SizeInBytes = (ulong)fileInfo.Length;
        m_Attributes = fileInfo.Attributes;
        m_ArchiveType = GetArchiveType(m_Name);
        m_ArchivePath = archivePath;
        m_IsArchived = isArchived;
    }
    
    public Archive(string name, string path)
    {
        m_Name = name;
        m_Path = path;
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_Attributes = 0;
        m_ArchiveType = GetArchiveType(m_Name);
        m_ArchivePath = string.Empty;
        m_IsArchived = false;
    }
    
    public Archive(ArchiveEntry entry, string path)
    {
        m_Name = "[" + entry.Name.Substring(entry.Name.LastIndexOf('/') + 1) + "]";
        m_Path = path + "\\" + entry.Name;
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = entry.UncompressedSize;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    public Archive(RarArchiveEntry entry, string path)
    {
        m_Name = "[" + entry.Name.Substring(entry.Name.LastIndexOf('/') + 1) + "]";
        m_Path = path + "/" + entry.Name;
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = entry.UncompressedSize;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    public Archive(SevenZipArchiveEntry entry, string path)
    {
        m_Name = "[" + entry.Name.Substring(entry.Name.LastIndexOf('/') + 1) + "]";
        m_Path = path + "/" + entry.Name;
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = entry.ModificationTime;
        m_SizeInBytes = entry.UncompressedSize;
        m_ArchivePath = path;
        m_IsArchived = true;
    }

    #endregion

    #region Methods

    public static ArchiveType GetArchiveType(string name)
    {
        if (name.EndsWith(".zip"))
        {
            return ArchiveType.ZIP;
        } 
        else if (name.EndsWith(".7z"))
        {
            return ArchiveType.SevenZip;
        } 
        else if (name.EndsWith(".rar"))
        {
            return ArchiveType.RAR;
        } 
        else
        {
            return ArchiveType.None;
        }
    }

    #endregion
}