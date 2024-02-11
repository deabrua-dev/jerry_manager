﻿using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;

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
            string targetDirectoryPath = path + "\\" + name;
            if (Directory.Exists(targetDirectoryPath))
            {
                throw new Exception("Folder with this name already exist.");
            }
            Directory.CreateDirectory(targetDirectoryPath);
        }
        catch(Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
    
    public static void CreateFile(string path, string name)
    {
        try
        {
            string targetPath = path + "\\" + name;
            if (System.IO.File.Exists(targetPath))
            {
                throw new Exception("File with this name already exist.");
            }
            System.IO.File.Create(targetPath);
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
    
    private static void CopyObject(string path, string destinationPath, bool isParentFolder = false)
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