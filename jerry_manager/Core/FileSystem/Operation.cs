using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace jerry_manager.Core.FileSystem;

public class Operation
{
    public static void Copy(string path, string destinationPath, List<FileSystemObject> items)
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
    
    public static void Move(string path, string destinationPath, List<FileSystemObject> items)
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
    
    private static void CopyObject(string path, string destinationPath)
    {
        FileAttributes attr = System.IO.File.GetAttributes(path);
        if (attr.HasFlag(FileAttributes.Directory))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            string targetDirectoryPath = destinationPath + "/" + directoryInfo.Name;
            if (!Directory.Exists(targetDirectoryPath))
            {
                Directory.CreateDirectory(targetDirectoryPath);
            }
            var directories = directoryInfo.GetDirectories();
            var files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                string targetFilePath = destinationPath + "/" + directoryInfo.Name + "/" + file.Name;
                if (!System.IO.File.Exists(targetFilePath))
                {
                    file.CopyTo(targetFilePath);
                }
            }

            foreach (var directory in directories)
            {
                string newDestinationDir = destinationPath + "/" + directoryInfo.Name;
                CopyObject(directory.FullName, newDestinationDir);
            }
        }
        else
        {
            FileInfo fileInfo = new FileInfo(path);
            string targetFilePath = destinationPath + "/" + fileInfo.Name;
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
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
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
                string targetFilePath = destinationPath + "/" + fileInfo.Name;
                if (!System.IO.File.Exists(targetFilePath))
                {
                    System.IO.File.Move(path, targetFilePath);
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
}