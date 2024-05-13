using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Linq;

namespace jerry_manager.Core.FileSystem;

public static class Operation
{
    #region Methods

    public static void Open(FileSystemObject item)
    {
        ProcessStartInfo psi = new()
        {
            FileName = item.Path,
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    public static void Copy(string destinationPath, List<FileSystemObject> items)
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

    public static void Move(string destinationPath, List<FileSystemObject> items)
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

    public static void Rename(string destinationPath, FileSystemObject item, string newName)
    {
        if (item is null)
        {
            throw new Exception("File is not selected");
        }

        RenameObject(item.Path, destinationPath, newName);
    }

    public static void Delete(List<FileSystemObject> items)
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

    public static void UnPack(string destinationPath, Archive archive)
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

    public static void CreateFolder(string path, string name)
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

    public static void CreateFile(string path, string name)
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

    public static void EditFile(FileSystemObject item)
    {
        Process.Start("notepad.exe", item.Path);
    }

    public static void AttributesChange(FileSystemObject item, FileAttributes attributes)
    {
        if (item is ParentFolder)
        {
            throw new Exception("Attributes changing fatal error.");
        }

        System.IO.File.SetAttributes(item.Path, attributes);
    }

    private static void CopyObject(string path, string destinationPath, bool isParentFolder = false)
    {
        if (path == destinationPath)
        {
            throw new Exception("The destination folder is a subfolder of the source folder.");
        }

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
                    throw new Exception("The destination folder is a subfolder of the source folder.");
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
        var tempArchive = new Aspose.Zip.Archive(archive.Path);
        Application.Current.Dispatcher.Invoke(() => tempArchive.ExtractToDirectory(destinationPath));
    }

    private static void UnPackRarArchive(string destinationPath, Archive archive)
    {
        var tempArchive = new Aspose.Zip.Rar.RarArchive(archive.Path);
        Application.Current.Dispatcher.Invoke(() => tempArchive.ExtractToDirectory(destinationPath));
    }

    private static void UnPack7ZipArchive(string destinationPath, Archive archive)
    {
        var tempArchive = new Aspose.Zip.SevenZip.SevenZipArchive(archive.Path);
        Application.Current.Dispatcher.Invoke(() => tempArchive.ExtractToDirectory(destinationPath));
    }

    public static ImageSource? GetIconImage(FileSystemObject obj)
    {
        if (obj is Folder)
        {
            return new BitmapImage(new Uri(
                @"pack://application:,,,/jerry_manager;component/Images/Default icons/folder-icon-big-256.png",
                UriKind.Absolute));
        }

        if (obj is ParentFolder)
        {
            return new BitmapImage(new Uri(
                @"pack://application:,,,/jerry_manager;component/Images/Default icons/Empty.png",
                UriKind.Absolute));
        }

        var ico = Icon.ExtractAssociatedIcon(obj.Path);
        return Imaging.CreateBitmapSourceFromHIcon(ico!.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }
    
    public static string ClearName(string name)
    {
        int length = name.LastIndexOf("/", StringComparison.Ordinal);
        return length < 1 ? name : name.Substring(length).Replace("/", "");
    }

    public static long GetDirectorySize(string path)
    {
        var directory = new DirectoryInfo(path);
        return directory.GetFiles("*", new EnumerationOptions
        {
            AttributesToSkip = FileAttributes.System,
            RecurseSubdirectories = true
        }).Sum(i => i.Length);
    }

    public static long GetSizeOnDisk(FileSystemObject file)
    {
        if (file is Folder)
        {
            var directory = new DirectoryInfo(file.Path);
            return directory.GetFiles("*", new EnumerationOptions
            {
                AttributesToSkip = FileAttributes.System,
                RecurseSubdirectories = true
            }).Sum(i => CalculateFileSizeOnDisk(i.FullName));
        }
        else
        {
            return CalculateFileSizeOnDisk(file.Path);
        }
    }

    // Get From https://stackoverflow.com/questions/48670600/c-sharp-net-core-get-file-size-on-disk-cross-platform-solution
    private static long CalculateFileSizeOnDisk(string file)
    {
        var info = new FileInfo(file);
        uint dummy, sectorsPerCluster, bytesPerSector;
        int result = GetDiskFreeSpaceW(info.Directory.Root.FullName, out sectorsPerCluster, out bytesPerSector,
            out dummy, out dummy);
        if (result == 0) throw new Win32Exception();
        uint clusterSize = sectorsPerCluster * bytesPerSector;
        uint hosize;
        uint losize = GetCompressedFileSizeW(file, out hosize);
        var size = (long)hosize << 32 | losize;
        return (size + clusterSize - 1) / clusterSize * clusterSize;
    }

    [DllImport("kernel32.dll")]
    static extern uint GetCompressedFileSizeW([In, MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
        [Out, MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh);

    [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
    static extern int GetDiskFreeSpaceW([In, MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName,
        out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters,
        out uint lpTotalNumberOfClusters);

    #endregion
}