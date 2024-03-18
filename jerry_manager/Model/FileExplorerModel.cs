using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using jerry_manager.Core.FileSystem;
using File = jerry_manager.Core.FileSystem.File;

namespace jerry_manager.Model;

public class FileExplorerModel
{
    #region Variables

    private List<String> m_ArchiveFormats = new() { ".7z", ".rar", ".zip" };
    private string m_CurrentPath;

    public string CurrentPath
    {
        get => m_CurrentPath;
        set => m_CurrentPath = value;
    }

    public ObservableCollection<FileSystemObject> FileObjects { get; set; }

    #endregion

    #region Constructors

    public FileExplorerModel()
    {
        FileObjects = new();
        m_CurrentPath = string.Empty;
    }

    #endregion

    #region Methods

    public void LoadPath()
    {
        try
        {
            if (!Directory.Exists(CurrentPath))
            {
                CurrentPath = CurrentPath.Substring(0, CurrentPath.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
            }

            if (FileObjects.Count() != 0)
                Application.Current.Dispatcher.Invoke(() => FileObjects.Clear());

            var dirInfo = new DirectoryInfo(CurrentPath);
            if (dirInfo.Parent != null)
                Application.Current.Dispatcher.Invoke(
                    () => FileObjects.Add(new ParentFolder(dirInfo.Parent.ToString())));

            var directoryInfo = new DirectoryInfo(CurrentPath);
            var files = directoryInfo.GetFiles().Where(i => !i.Attributes.HasFlag(FileAttributes.Hidden) &&
                                                            !m_ArchiveFormats.Any(j => i.Name.EndsWith(j))).ToArray();
            var directories = directoryInfo.GetDirectories().Where(i => !i.Attributes.HasFlag(FileAttributes.Hidden))
                .ToArray();
            var archives = directoryInfo.GetFiles().Where(i => m_ArchiveFormats.Any(j => i.Name.EndsWith(j))).ToArray();
            foreach (var directory in directories)
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new Folder(directory)));
            }

            foreach (var file in files)
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new File(file)));
            }

            foreach (var archive in archives)
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new Archive(archive)));
            }
        }
        catch (UnauthorizedAccessException e)
        {
            MessageBox.Show(e.Message);
            var parentFolderPath = FileObjects.First(i => i is ParentFolder).Path;
            CurrentPath = parentFolderPath;
            LoadPath();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void LoadDrives(ObservableCollection<Drive> fileSystemDrives)
    {
        try
        {
            var drives = DriveInfo.GetDrives().Where(i => i.DriveType == DriveType.Fixed).ToArray();
            foreach (var drive in drives)
            {
                Application.Current.Dispatcher.Invoke(() => fileSystemDrives.Add(new Drive(drive)));
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void LoadArchive(FileSystemObject fileObject)
    {
        try
        {
            var selectedFileObject = fileObject as Archive;
            switch (selectedFileObject!.ArchiveType)
            {
                case ArchiveType.ZIP:
                    Load_ZIP(selectedFileObject);
                    break;
                case ArchiveType.RAR:
                    Load_RAR(selectedFileObject);
                    break;
                case ArchiveType.SevenZip:
                    Load_7Zip(selectedFileObject);
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

    private void Load_ZIP(FileSystemObject file)
    {
        try
        {
            if (FileObjects.Count != 0)
                Application.Current.Dispatcher.Invoke(() => FileObjects.Clear());
            List<FileSystemObject> temp = new List<FileSystemObject>();
            String curPath = file.Path + "\\";
            var path = file.Path.Substring(0, file.Path.LastIndexOf(".zip", StringComparison.Ordinal) + 4);
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
                        if (m_ArchiveFormats.Any(j => entry.Name.EndsWith(j)))
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

            temp.RemoveAll(i => CheckPath(i, curPath));
            var tempPath = file.Path.Substring(0, file.Path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            if (file.IsArchived && m_ArchiveFormats.Any(tempPath.Contains))
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new ParentFolder(tempPath, path, true)));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new ParentFolder(tempPath)));
            }

            foreach (var item in temp)
            {
                if (item is Folder)
                {
                    App.Current.Dispatcher.Invoke(() => FileObjects.Add(item));
                }
            }

            foreach (var item in temp)
            {
                if (item is not Folder)
                {
                    Application.Current.Dispatcher.Invoke(() => FileObjects.Add(item));
                }
            }

            temp.Clear();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void Load_RAR(FileSystemObject file)
    {
        try
        {
            if (FileObjects.Count != 0)
                Application.Current.Dispatcher.Invoke(() => FileObjects.Clear());
            List<FileSystemObject> temp = new List<FileSystemObject>();
            String curPath = file.Path + "\\";
            var path = file.Path.Substring(0, file.Path.LastIndexOf(".rar", StringComparison.Ordinal) + 4);
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
                        if (m_ArchiveFormats.Any(j => entry.Name.EndsWith(j)))
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

            temp.RemoveAll(i => CheckPath(i, curPath));
            var tempPath = file.Path.Substring(0, file.Path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            if (file.IsArchived && m_ArchiveFormats.Any(tempPath.Contains))
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new ParentFolder(tempPath, path, true)));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new ParentFolder(tempPath)));
            }

            foreach (var item in temp)
            {
                if (item is Folder)
                {
                    Application.Current.Dispatcher.Invoke(() => FileObjects.Add(item));
                }
            }

            foreach (var item in temp)
            {
                if (item is not Folder)
                {
                    Application.Current.Dispatcher.Invoke(() => FileObjects.Add(item));
                }
            }

            temp.Clear();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void Load_7Zip(FileSystemObject file)
    {
        try
        {
            if (FileObjects.Count != 0)
                Application.Current.Dispatcher.Invoke(() => FileObjects.Clear());
            List<FileSystemObject> temp = new List<FileSystemObject>();
            String curPath = file.Path + "\\";
            var path = file.Path.Substring(0, file.Path.LastIndexOf(".7z", StringComparison.Ordinal) + 3);
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
                        if (m_ArchiveFormats.Any(j => entry.Name.EndsWith(j)))
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

            temp.RemoveAll(i => CheckPath(i, curPath));
            var tempPath = file.Path.Substring(0, file.Path.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            if (file.IsArchived && m_ArchiveFormats.Any(tempPath.Contains))
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new ParentFolder(tempPath, path, true)));
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => FileObjects.Add(new ParentFolder(tempPath)));
            }

            foreach (var item in temp)
            {
                if (item is Folder)
                {
                    Application.Current.Dispatcher.Invoke(() => FileObjects.Add(item));
                }
            }

            foreach (var item in temp)
            {
                if (item is not Folder)
                {
                    Application.Current.Dispatcher.Invoke(() => FileObjects.Add(item));
                }
            }

            temp.Clear();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public void Execute(FileSystemObject selectedFileObject)
    {
        Operation.Open(selectedFileObject);
    }

    public void LoadInArchive(FileSystemObject fileObject)
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
                    Load_ZIP(fileObject);
                    break;
                case ArchiveType.RAR:
                    Load_RAR(fileObject);
                    break;
                case ArchiveType.SevenZip:
                    Load_7Zip(fileObject);
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

    public void CopyObjects(List<FileSystemObject> droppedItems)
    {
        try
        {
            Operation.Copy(m_CurrentPath, droppedItems);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    #endregion
}