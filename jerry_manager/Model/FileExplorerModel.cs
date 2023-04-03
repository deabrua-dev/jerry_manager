using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using jerry_manager.Core;
using File = jerry_manager.Core.File;

namespace jerry_manager.Model;

public class FileExplorerModel
{
    private List<String> archivesFormats = new List<String>() { ".7z", ".rar", ".zip" };
    public Boolean LoadPath(ObservableCollection<FileSystemObject> fileSystemObjects, String currentPath)
    {
        if (!Directory.Exists(currentPath))
            return false;

        if (fileSystemObjects != null)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());

        var dirInfo = new DirectoryInfo(currentPath);
        if (dirInfo.Parent != null)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(new Folder("[...]", dirInfo.Parent.ToString())));

        var directoryInfo = new DirectoryInfo(currentPath);
        var files = directoryInfo.GetFiles().Where(i => !i.Attributes.HasFlag(FileAttributes.Hidden) &&
                                                        !archivesFormats.Any(j => i.Name.EndsWith(j))).ToArray();
        var directories = directoryInfo.GetDirectories().Where(i => !i.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
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
        return true;
    }

    public Boolean LoadDrives(ObservableCollection<Drive> fileSystemDrives)
    {
        var drives = DriveInfo.GetDrives().Where(i => i.DriveType == DriveType.Fixed).ToArray();
        foreach (var drive in drives)
        {
            App.Current.Dispatcher.Invoke(() => fileSystemDrives.Add(new Drive(drive)));
        }
        return true;
    }

    public Boolean LoadArchive(FileSystemObject fileObject, ObservableCollection<FileSystemObject> fileSystemObjects, String currentPath)
    {
        var selectedFileObject = fileObject as Archive;
        switch (selectedFileObject.ArchiveType)
        {
            case ArchiveType.ZIP:
                Load_ZIP(selectedFileObject, fileSystemObjects, currentPath);
                break;
            case ArchiveType.RAR:
                break;
            case ArchiveType.SevenZip:
                break;
            default:
                throw new FileLoadException("Illegal format type.");
        }
        return true;
    }

    public void Load_ZIP(Archive fileObject, ObservableCollection<FileSystemObject> fileSystemObjects, String currentPath)
    {
        if (fileSystemObjects != null)
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Clear());
        
        var path = fileObject.Path;
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
            item.Name = item.Name.Substring(item.Name.LastIndexOf('/') + 1);
            App.Current.Dispatcher.Invoke(() => fileSystemObjects.Add(item));
        }
        currentPath = cur_path;
    }

    public void Execute(FileSystemObject selectedFileObject)
    {
        ProcessStartInfo psi = new();
        psi.FileName = selectedFileObject.Path;
        psi.UseShellExecute = true;
        Process.Start(psi);
    }

    public void LoadInArchive(FileSystemObject fileObject, ObservableCollection<FileSystemObject> fileSystemObjects)
    {
        
    }
    
    private bool CheckPath(FileSystemObject systemObject, String path)
    {
        string tempPath = systemObject.Path;
        tempPath = tempPath.Replace(path, "");
        return tempPath.Contains("/");
    }
}