using System;
using System.IO;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Model;

public class FilePropertiesModel
{
    #region Methods
    
    public ImageSource GetIconImage(FileSystemObject obj)
    {
        if (obj is Folder)
        {
            return new BitmapImage(new Uri(@"pack://application:,,,/jerry_manager;component/Images/Default icons/folder-icon-big-256.png", UriKind.Absolute));
        }
        var ico = Icon.ExtractAssociatedIcon(obj.Path);
        return Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }

    public long GetFileSizeOnDisk(string file)
    {
        var info = new FileInfo(file);
        uint dummy, sectorsPerCluster, bytesPerSector;
        int result = GetDiskFreeSpaceW(info.Directory.Root.FullName, out sectorsPerCluster, out bytesPerSector, out dummy, out dummy);
        if (result == 0) throw new Win32Exception();
        uint clusterSize = sectorsPerCluster * bytesPerSector;
        uint hosize;
        uint losize = GetCompressedFileSizeW(file, out hosize);
        var size = (long)hosize << 32 | losize;
        return ((size + clusterSize - 1) / clusterSize * clusterSize);
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