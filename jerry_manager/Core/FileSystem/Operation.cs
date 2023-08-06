using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using jerry_manager.Core.Exceptions;

namespace jerry_manager.Core.FileSystem;

public class Operation
{
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
                CopyObject(item.Path, destinationPath);
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
                    throw new ArchiveReadException("Archive read error.");
                default:
                    throw new FileReadException("File read error.");
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
            string targetDirectoryPath = path + "\\" + name;
            Directory.CreateDirectory(path);
        }
        catch(Exception e)
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
    
    private static void CopyObject(string path, string destinationPath)
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
            var directories = directoryInfo.GetDirectories();
            var files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                string targetFilePath = destinationPath + "\\" + directoryInfo.Name + "\\" + file.Name;
                if (!System.IO.File.Exists(targetFilePath))
                {
                    file.CopyTo(targetFilePath);
                }
            }

            foreach (var directory in directories)
            {
                string newDestinationDir = destinationPath + "\\" + directoryInfo.Name;
                CopyObject(directory.FullName, newDestinationDir);
            }
        }
        else
        {
            FileInfo fileInfo = new FileInfo(path);
            string targetFilePath = destinationPath + "\\" + fileInfo.Name;
            fileInfo.CopyTo(targetFilePath);
        }
    }

    private static void MoveObject(string path, string destinationPath)
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
            }
        }
    }

    private static void RenameObject(string path, string destinationPath, string newName)
    {
        FileAttributes attr = System.IO.File.GetAttributes(path);
        if (attr.HasFlag(FileAttributes.Directory))
        {
            if (Directory.Exists(path))
            {
                string targetDirectoryPath = destinationPath + "\\" + newName; ;
                if (!Directory.Exists(targetDirectoryPath))
                {
                    Directory.Move(path, targetDirectoryPath);
                }
            }
        }
        else
        {
            if (System.IO.File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                string targetFilePath = destinationPath + "\\" + newName + fileInfo.Extension;
                if (!System.IO.File.Exists(targetFilePath))
                {
                    System.IO.File.Move(path, targetFilePath);
                }
                else
                {
                    throw new Exception("File already exists.");
                }
            }
        }
    }

    private static void DeleteObject(string path)
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

    private static void UnPackZipArchive(string destinationPath, Archive archive)
    {
        using (var temp_archive = new Aspose.Zip.Archive(archive.Path))
        {
            temp_archive.ExtractToDirectory(destinationPath);
        }
    }
    
    private static void UnPackRarArchive(string destinationPath, Archive archive)
    {
        using (var temp_archive = new Aspose.Zip.Rar.RarArchive(archive.Path))
        {
            temp_archive.ExtractToDirectory(destinationPath);
        }
    }
    
    private static void UnPack7ZipArchive(string destinationPath, Archive archive)
    {
        using (var temp_archive = new Aspose.Zip.SevenZip.SevenZipArchive(archive.Path))
        {
            App.Current.Dispatcher.Invoke(() =>temp_archive.ExtractToDirectory(destinationPath));
        }
    }
}