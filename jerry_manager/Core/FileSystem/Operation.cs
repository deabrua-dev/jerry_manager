using System;
using System.Collections.Generic;
using System.IO;

namespace jerry_manager.Core;

public class Operation
{
    public static Boolean Copy(string path, string destinationPath, List<FileSystemObject> items)
    {
        if (items is null || items.Count <= 0)
        {
            return false;
        }
        foreach (var item in items)
        {
            if (item is Folder)
            {
                CopyObject(item.Path, destinationPath);
            }
            else
            {
                FileInfo fileInfo = new FileInfo(item.Path);
                string targetFilePath = destinationPath + "/" + fileInfo.Name;
                fileInfo.CopyTo(targetFilePath);
            }
        }
        return true;
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

    public static Boolean Move(string path, string destinationPath, List<FileSystemObject> items)
    {
        if (items is null || items.Count <= 0)
        {
            return false;
        }
        foreach (var item in items)
        {
            if (item is Folder)
            {
                MoveObject(item.Path, destinationPath);
            }
            else
            {
                MoveObject(item.Path, destinationPath);
            }
        }
        return true;
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
}