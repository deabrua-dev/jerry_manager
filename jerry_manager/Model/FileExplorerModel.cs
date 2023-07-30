using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using jerry_manager.Core.FileSystem;
using File = jerry_manager.Core.FileSystem.File;

namespace jerry_manager.Model;

public class FileExplorerModel
{
    #region Variables

    private List<String> archivesFormats = new() { ".7z", ".rar", ".zip" };
    private string m_CurrentPath;
    public string CurrentPath
    {
        get => m_CurrentPath;
        set => m_CurrentPath = value;
    }

    #endregion

    #region Constructors

    public FileExplorerModel()
    {
        
    }

    #endregion
    
    #region Methods
    public void LoadPath(ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        try
        {
            if (!Directory.Exists(CurrentPath))
            {
                CurrentPath = CurrentPath.Substring(0, CurrentPath.LastIndexOf(@"\") + 1);
            }

            if (fileSystemObjects != null)
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());

            var dirInfo = new DirectoryInfo(CurrentPath);
            if (dirInfo.Parent != null)
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new ParentFolder(dirInfo.Parent.ToString())));

            var directoryInfo = new DirectoryInfo(CurrentPath);
            var files = directoryInfo.GetFiles().Where(i => !i.Attributes.HasFlag(FileAttributes.Hidden) &&
                                                            !archivesFormats.Any(j => i.Name.EndsWith(j))).ToArray();
            var directories = directoryInfo.GetDirectories().Where(i => !i.Attributes.HasFlag(FileAttributes.Hidden))
                .ToArray();
            var archives = directoryInfo.GetFiles().Where(i => archivesFormats.Any(j => i.Name.EndsWith(j))).ToArray();
            foreach (var directory in directories)
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new Folder(directory)));
            }
            foreach (var file in files)
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new File(file)));
            }
            foreach (var archive in archives)
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new Archive(archive)));
            }
        }
        catch (UnauthorizedAccessException e)
        {
            MessageBox.Show(e.Message);
            var parentFolderPath = fileSystemObjects.First(i => i is ParentFolder).Path;
            CurrentPath = parentFolderPath;
            this.LoadPath(fileSystemObjects);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void LoadDrives(ObservableCollection<Drive> fileSystemDrives)
    {
        var drives = DriveInfo.GetDrives().Where(i => i.DriveType == DriveType.Fixed).ToArray();
        foreach (var drive in drives)
        {
            App.Current.Dispatcher.Invoke(() => fileSystemDrives.Add(new Drive(drive)));
        }
    }

    public void LoadArchive(FileSystemObject fileObject, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        var selectedFileObject = fileObject as Archive;
        switch (selectedFileObject.ArchiveType)
        {
            case ArchiveType.ZIP:
                Load_ZIP(selectedFileObject, fileSystemObjects);
                break;
            case ArchiveType.RAR:
                Load_RAR(selectedFileObject, fileSystemObjects);
                break;
            case ArchiveType.SevenZip:
                Load_7Zip(selectedFileObject, fileSystemObjects);
                break;
            default:
                throw new FileLoadException("Illegal format type.");
        }
    }

    private void Load_ZIP(FileSystemObject file, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        if (fileSystemObjects.Count != 0)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());
        List<FileSystemObject> temp = new List<FileSystemObject>();
        String cur_path = file.Path + "\\";
        var path = file.Path.Substring(0, file.Path.LastIndexOf(".zip") + 4);
        using (var archive = new Aspose.Zip.Archive(path))
        {
            foreach (var entry in archive.Entries)
            {
                if (entry.IsDirectory)
                {
                    temp.Add(new Folder(entry, path));
                }
                else
                {
                    if (archivesFormats.Any(j => entry.Name.EndsWith(j)))
                    {
                        temp.Add(new Archive(entry, path));
                    }
                    else
                    {
                        temp.Add(new File(entry, path));
                    }
                }  
            }
        }
        temp.RemoveAll(i => CheckPath(i, cur_path));
        var temp_path = file.Path.Substring(0, file.Path.LastIndexOf("\\") + 1);
        if (file.IsArchived && archivesFormats.Any(temp_path.Contains))
        {
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new ParentFolder(temp_path, path, true)));
        }
        else
        {
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new ParentFolder(temp_path)));
        }
        foreach (var item in temp)
        {
            if (item is Folder)
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(item));
            }
        }
        foreach (var item in temp)
        {
            if (!(item is Folder))
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(item));
            }
        }
        temp.Clear();
    }

    private void Load_RAR(FileSystemObject file, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        if (fileSystemObjects.Count != 0)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());
        List<FileSystemObject> temp = new List<FileSystemObject>();
        String cur_path = file.Path + "\\";
        var path = file.Path.Substring(0, file.Path.LastIndexOf(".rar") + 4);
        using (var archive = new Aspose.Zip.Rar.RarArchive(path))
        {
            foreach (var entry in archive.Entries)
            {
                if (entry.IsDirectory)
                {
                    temp.Add(new Folder(entry, path));
                }
                else
                {
                    if (archivesFormats.Any(j => entry.Name.EndsWith(j)))
                    {
                        temp.Add(new Archive(entry, path));
                    }
                    else
                    {
                        temp.Add(new File(entry, path));
                    }
                }
            }
        }
        temp.RemoveAll(i => CheckPath(i, cur_path));
        var temp_path = file.Path.Substring(0, file.Path.LastIndexOf("\\") + 1);
        if (file.IsArchived && archivesFormats.Any(temp_path.Contains))
        {
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new ParentFolder(temp_path, path, true)));
        }
        else
        {
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new ParentFolder(temp_path)));
        }
        foreach (var item in temp)
        {
            if (item is Folder)
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(item));
            }
        }
        foreach (var item in temp)
        {
            if (!(item is Folder))
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(item));
            }
        }
        temp.Clear();
    }

    private void Load_7Zip(FileSystemObject file, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        if (fileSystemObjects.Count != 0)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());
        List<FileSystemObject> temp = new List<FileSystemObject>();
        String cur_path = file.Path + "\\";
        var path = file.Path.Substring(0, file.Path.LastIndexOf(".7z") + 3);
        using (var archive = new Aspose.Zip.SevenZip.SevenZipArchive(path))
        {
            foreach (var entry in archive.Entries)
            {
                if (entry.IsDirectory)
                {
                    temp.Add(new Folder(entry, path));
                }
                else
                {
                    if (archivesFormats.Any(j => entry.Name.EndsWith(j)))
                    {
                        temp.Add(new Archive(entry, path));
                    }
                    else
                    {
                        temp.Add(new File(entry, path));
                    }
                }
            }
        }
        temp.RemoveAll(i => CheckPath(i, cur_path));
        var temp_path = file.Path.Substring(0, file.Path.LastIndexOf("\\") + 1);
        if (file.IsArchived && archivesFormats.Any(temp_path.Contains))
        {
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new ParentFolder(temp_path, path, true)));
        }
        else
        {
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new ParentFolder(temp_path)));
        }
        foreach (var item in temp)
        {
            if (item is Folder)
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(item));
            }
        }
        foreach (var item in temp)
        {
            if (!(item is Folder))
            {
                App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(item));
            }
        }
        temp.Clear();
    }

    public void Execute(FileSystemObject selectedFileObject)
    {
        try
        {
            ProcessStartInfo psi = new();
            psi.FileName = selectedFileObject.Path;
            psi.UseShellExecute = true;
            Process.Start(psi);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void LoadInArchive(FileSystemObject fileObject, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        try
        {
            if (fileObject is File || fileObject is Archive)
            {
                throw new FileLoadException("Unpack first, to open this file.");
            }
            switch (Archive.GetArchiveType(fileObject.ArchivePath))
            {
                case ArchiveType.ZIP:
                    Load_ZIP(fileObject, fileSystemObjects);
                    break;
                case ArchiveType.RAR:
                    Load_RAR(fileObject, fileSystemObjects);
                    break;
                case ArchiveType.SevenZip:
                    Load_7Zip(fileObject, fileSystemObjects);
                    break;
                default:
                    throw new FileLoadException("Illegal format type.");
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
    
    private bool CheckPath(FileSystemObject systemObject, string path)
    {
        string tempPath = systemObject.Path;
        tempPath = tempPath.Replace(path, "");
        return tempPath.Contains("\\");
    }
    #endregion
}