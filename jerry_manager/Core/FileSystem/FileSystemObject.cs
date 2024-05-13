using System;
using System.IO;
using System.Windows.Media;

namespace jerry_manager.Core.FileSystem;

public abstract class FileSystemObject
{
    #region Variables

    protected string m_Name { get; set; }
    protected string m_Path { get; set; }
    protected string m_Extension { get; set; }
    protected DateTime m_DateCreated { get; set; }
    protected DateTime m_DateModified { get; set; }
    protected DateTime m_DateAccessed { get; set; }
    protected ulong m_SizeInBytes { get; set; }
    protected ulong m_TotalSizeInBytes { get; set; }
    protected FileAttributes m_Attributes { get; set; }
    protected bool m_IsArchived { get; set; }
    protected string m_ArchivePath { get; set; }
    protected bool m_IsSelected { get; set; }

    public string DateLastModified => m_DateModified.ToString("dd/MM/yyyy");
    public string DateLastAccessed => m_DateAccessed.ToString("dd/MM/yyyy");
    public DateTime DateCreated => m_DateCreated;
    public DateTime DateModified => m_DateModified;
    public DateTime DateAccessed => m_DateAccessed;

    public string Name
    {
        get => m_Name;
        set => m_Name = value;
    }

    public string Path
    {
        get => m_Path;
        set => m_Path = value;
    }

    public string Extension
    {
        get => m_Extension;
        set => m_Extension = value;
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

    public ulong SizeInBytes => m_SizeInBytes;
    public string SizeOnDisk => "";
    public FileAttributes Attributes => m_Attributes;

    public bool IsArchived
    {
        get => m_IsArchived;
        set => m_IsArchived = value;
    }

    public string ArchivePath
    {
        get => m_ArchivePath;
        set => m_ArchivePath = value;
    }

    public bool IsSelected
    {
        get => m_IsSelected;
        set => m_IsSelected = value;
    }

    #endregion
}