using System;
using System.Globalization;
using System.IO;

namespace jerry_manager.Core.FileSystem;

public class Drive : FileSystemObject
{
    #region Variables

    public string TotalSize
    {
        get
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            return m_TotalSizeInBytes.ToString("n", nfi);
        }
    }
    
    public string Size
    {
        get
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            return m_SizeInBytes.ToString("n", nfi);
        }
    }

    #endregion

    #region Constructors

    public Drive()
    {
        m_Name = string.Empty;
        m_Path = string.Empty;
        m_Extension = string.Empty;
        m_DateCreated = DateTime.Now;
        m_DateModified = DateTime.Now;
        m_SizeInBytes = 0;
        m_TotalSizeInBytes = 0;
        m_ArchivePath = string.Empty;
        m_IsArchived = false;
    }

    public Drive(DriveInfo driveInfo)
    {
        m_Name = driveInfo.Name;
        m_Path = driveInfo.RootDirectory.FullName;
        m_Extension = string.Empty;
        m_DateCreated = driveInfo.RootDirectory.CreationTime;
        m_DateModified = driveInfo.RootDirectory.LastWriteTime;
        m_SizeInBytes = (ulong)driveInfo.TotalSize;
        m_TotalSizeInBytes = (ulong)driveInfo.TotalSize;
        m_ArchivePath = string.Empty;
        m_IsArchived = false;
    }

    #endregion
}