using System;
using System.IO;

namespace jerry_manager.Core;

public abstract class FileSystemObject
{
    protected String m_Name { get; set; }
    protected String m_Path { get; set; }
    protected String m_Extension { get; set; }
    protected DateTime m_DateCreated { get; set; }
    protected DateTime m_DateModified { get; set; }
    protected UInt64 m_SizeInBytes { get; set; }
    protected FileAttributes m_Attributes { get; set; }
    protected Boolean m_IsArchived { get; set; }
    protected String m_ArchivePath { get; set; }
    
    
    public String Name
    {
        get => m_Name;
        set => m_Name = value;
    }
    public String Path
    {
        get => m_Path;
        set => m_Path = value;
    }
    public String Extension
    {
        get => m_Extension;
        set => m_Extension = value;
    }
    public String DateLastModified
    {
        get => m_DateModified.ToString("dd/MM/yyyy");
    }
    public String Size
    {
        get => m_SizeInBytes.ToString();
    }
    public Boolean IsArchived
    {
        get => m_IsArchived;
        set => m_IsArchived = value;
    }
    public String ArchivePath
    {
        get => m_ArchivePath;
        set => m_ArchivePath = value;
    }
}