using System;
using System.IO;

namespace jerry_manager.Core;

public class Drive : FileSystemObject
{
    #region Constructors

    public Drive()
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

    public Drive(DriveInfo driveInfo)
    {
        m_Name = driveInfo.Name;
        m_Path = driveInfo.RootDirectory.FullName;
        m_Extension = String.Empty;
        m_DateCreated = driveInfo.RootDirectory.CreationTime;
        m_DateModified = driveInfo.RootDirectory.LastWriteTime;
        m_SizeInBytes = (UInt64)driveInfo.TotalSize;
        m_ArchivePath = String.Empty;
        m_IsArchived = false;
    }

    #endregion
}