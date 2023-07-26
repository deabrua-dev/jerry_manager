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

    private List<String> archivesFormats = new List<String>() { ".7z", ".rar", ".zip" };
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
                Load_ZIP(selectedFileObject.Path, fileSystemObjects);
                break;
            case ArchiveType.RAR:
                Load_RAR(selectedFileObject.Path, fileSystemObjects);
                break;
            case ArchiveType.SevenZip:
                Load_7Zip(selectedFileObject.Path, fileSystemObjects);
                break;
            default:
                throw new FileLoadException("Illegal format type.");
        }
    }

    private void Load_ZIP(string path, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        if (fileSystemObjects != null)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());
        
        ObservableCollection<FileSystemObject> result = new ObservableCollection<FileSystemObject>();
        List<FileSystemObject> temp = new List<FileSystemObject>();
        String cur_path = path + "/";
        path = path.Substring(0, path.LastIndexOf(".zip") + 4);
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
    }

    private void Load_RAR(string path, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        if (fileSystemObjects != null)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());

        ObservableCollection<FileSystemObject> result = new ObservableCollection<FileSystemObject>();
        List<FileSystemObject> temp = new List<FileSystemObject>();
        String cur_path = path + "/";
        path = path.Substring(0, path.LastIndexOf(".rar") + 4);
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
    }

    private void Load_7Zip(string path, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        if (fileSystemObjects != null)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());

        ObservableCollection<FileSystemObject> result = new ObservableCollection<FileSystemObject>();
        List<FileSystemObject> temp = new List<FileSystemObject>();
        String cur_path = path + "/";
        path = path.Substring(0, path.LastIndexOf(".7z") + 3);
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
        if (fileObject is File || fileObject is Archive)
        {
            return;
        }
        switch (GetArchiveType(fileObject.ArchivePath))
        {
            case ArchiveType.ZIP:
                Load_ZIP(fileObject.Path, fileSystemObjects);
                break;
            case ArchiveType.RAR:
                Load_RAR(fileObject.Path, fileSystemObjects);
                break;
            case ArchiveType.SevenZip:
                Load_7Zip(fileObject.Path, fileSystemObjects);
                break;
            default:
                throw new FileLoadException("Illegal format type.");
        }
    }
    
    private bool CheckPath(FileSystemObject systemObject, string path)
    {
        string tempPath = systemObject.Path;
        tempPath = tempPath.Replace(path, "");
        return tempPath.Contains("/");
    }

    private ArchiveType GetArchiveType(string name)
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