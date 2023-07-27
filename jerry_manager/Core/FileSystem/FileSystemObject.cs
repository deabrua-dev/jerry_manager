using System;
using System.IO;

namespace jerry_manager.Core.FileSystem;

public abstract class FileSystemObject
{
    protected string m_Name { get; set; }
    protected string m_Path { get; set; }
    protected string m_Extension { get; set; }
    protected DateTime m_DateCreated { get; set; }
    protected DateTime m_DateModified { get; set; }
    protected UInt64 m_SizeInBytes { get; set; }
    protected UInt64 m_TotalSizeInBytes { get; set; }
    protected FileAttributes m_Attributes { get; set; }
    protected bool m_IsArchived { get; set; }
    protected string m_ArchivePath { get; set; }
    protected bool m_IsSelected { get; set; }
    
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
    public string DateLastModified
    {
        get => m_DateModified.ToString("dd/MM/yyyy");
    }
    public string Size
    {
        get => m_SizeInBytes.ToString();
    }
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
}