using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;

namespace jerry_manager.Core.FileSystem;

public static class Operation
{

    #region Methods

    public static void Open(FileSystemObject item)
    {
        try
        {
            ProcessStartInfo psi = new()
            {
                FileName = item.Path,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public static void Copy(string destinationPath, List<FileSystemObject> items)
    {
        try
        {
            if (items is null || items.Count <= 0)
            {
                throw new Exception("Files is not selected");
            }

            foreach (var item in items)
            {
                CopyObject(item.Path, destinationPath, true);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public static void Move(string destinationPath, List<FileSystemObject> items)
    {
        try
        {
            if (items is null || items.Count <= 0)
            {
                throw new Exception("Files is not selected");
            }

            foreach (var item in items)
            {
                MoveObject(item.Path, destinationPath);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public static void Rename(string destinationPath, FileSystemObject item, string newName)
    {
        try
        {
            if (item is null)
            {
                throw new Exception("File is not selected");
            }

            RenameObject(item.Path, destinationPath, newName);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public static void Delete(List<FileSystemObject> items)
    {
        try
        {
            if (items is null || items.Count <= 0)
            {
                throw new Exception("Files is not selected");
            }

            foreach (var item in items)
            {
                DeleteObject(item.Path);
            }

            DataCache.NotActiveView.Update();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public static void UnPack(string destinationPath, Archive archive)
    {
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            switch (archive.ArchiveType)
            {
                case ArchiveType.ZIP:
                    UnPackZipArchive(destinationPath, archive);
                    break;
                case ArchiveType.RAR:
                    UnPackRarArchive(destinationPath, archive);
                    break;
                case ArchiveType.SevenZip:
                    UnPack7ZipArchive(destinationPath, archive);
                    break;
                case ArchiveType.None:
                    throw new Exception("Archive read error.");
                default:
                    throw new Exception("File read error.");
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public static void CreateFolder(string path, string name)
    {
        try
        {
            var targetPath = path + "\\" + name;
            if (Directory.Exists(targetPath))
            {
                var counter = 1;
                do
                {
                    counter++;
                } while (Directory.Exists(targetPath + $" ({counter})"));

                targetPath += $" ({counter})";
            }

            Directory.CreateDirectory(targetPath);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public static void CreateFile(string path, string name)
    {
        try
        {
            var index = name.IndexOf(".", StringComparison.Ordinal);
            var fileName = index <= 0 ? name : name.Substring(0, index);
            var extension = index <= 0 ? "" : name.Substring(index, name.Length - index);
            var targetPath = path + "\\" + fileName;
            if (System.IO.File.Exists(targetPath + extension))
            {
                var counter = 1;
                do
                {
                    counter++;
                } while (System.IO.File.Exists(targetPath + $" ({counter})" + extension));

                targetPath += $" ({counter})";
            }

            System.IO.File.Create(targetPath + extension);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public static void EditFile(FileSystemObject item)
    {
        try
        {
            Process.Start("notepad.exe", item.Path);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private static void CopyObject(string path, string destinationPath, bool isParentFolder = false)
    {
        try
        {
            FileAttributes attr = System.IO.File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                string targetDirectoryPath = destinationPath + "\\" + directoryInfo.Name;
                if (!Directory.Exists(targetDirectoryPath))
                {
                    Directory.CreateDirectory(targetDirectoryPath);
                }
                else if (isParentFolder)
                {
                    while (Directory.Exists(targetDirectoryPath))
                    {
                        targetDirectoryPath += " - Copy";
                    }

                    Directory.CreateDirectory(targetDirectoryPath);
                }

                var directories = directoryInfo.GetDirectories();
                var files = directoryInfo.GetFiles();
                foreach (var file in files)
                {
                    string targetFilePath = targetDirectoryPath + "\\" + file.Name;
                    if (!System.IO.File.Exists(targetFilePath))
                    {
                        file.CopyTo(targetFilePath);
                    }
                }

                foreach (var directory in directories)
                {
                    CopyObject(directory.FullName, targetDirectoryPath);
                }
            }
            else
            {
                FileInfo fileInfo = new FileInfo(path);
                string targetFilePath = destinationPath + "\\" + Path.GetFileNameWithoutExtension(fileInfo.FullName);
                while (System.IO.File.Exists(targetFilePath + fileInfo.Extension))
                {
                    targetFilePath += " - Copy";
                }

                fileInfo.CopyTo(targetFilePath + fileInfo.Extension);
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private static void MoveObject(string path, string destinationPath)
    {
        try
        {
            FileAttributes attr = System.IO.File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                if (Directory.Exists(path))
                {
                    string targetDirectoryPath = destinationPath;
                    if (!Directory.Exists(targetDirectoryPath))
                    {
                        Directory.Move(path, targetDirectoryPath);
                    }
                    else
                    {
                        throw new Exception("You cant move folder at this path.");
                    }
                }
            }
            else
            {
                if (System.IO.File.Exists(path))
                {
                    FileInfo fileInfo = new FileInfo(path);
                    string targetFilePath = destinationPath + "\\" + fileInfo.Name;
                    if (!System.IO.File.Exists(targetFilePath))
                    {
                        System.IO.File.Move(path, targetFilePath);
                    }
                    else
                    {
                        throw new Exception("You cant move file at this path.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private static void RenameObject(string path, string destinationPath, string newName)
    {
        try
        {
            FileAttributes attr = System.IO.File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                if (Directory.Exists(path))
                {
                    string targetDirectoryPath = destinationPath + "\\" + newName;
                    if (Directory.Exists(targetDirectoryPath))
                    {
                        throw new Exception("Folder with this name already exist.");
                    }

                    Directory.Move(path, targetDirectoryPath);
                }
            }
            else
            {
                if (System.IO.File.Exists(path))
                {
                    var targetFilePath = destinationPath + "\\" + newName;
                    if (System.IO.File.Exists(targetFilePath))
                    {
                        throw new Exception("File with this name already exist.");
                    }

                    System.IO.File.Move(path, targetFilePath);
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private static void DeleteObject(string path)
    {
        try
        {
            FileAttributes attr = System.IO.File.GetAttributes(path);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            else
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private static void UnPackZipArchive(string destinationPath, Archive archive)
    {
        try
        {
            using (var tempArchive = new Aspose.Zip.Archive(archive.Path))
            {
                App.Current.Dispatcher.Invoke(() => tempArchive.ExtractToDirectory(destinationPath));
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private static void UnPackRarArchive(string destinationPath, Archive archive)
    {
        try
        {
            using (var tempArchive = new Aspose.Zip.Rar.RarArchive(archive.Path))
            {
                App.Current.Dispatcher.Invoke(() => tempArchive.ExtractToDirectory(destinationPath));
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private static void UnPack7ZipArchive(string destinationPath, Archive archive)
    {
        try
        {
            using (var tempArchive = new Aspose.Zip.SevenZip.SevenZipArchive(archive.Path))
            {
                App.Current.Dispatcher.Invoke(() => tempArchive.ExtractToDirectory(destinationPath));
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    #endregion
}