using System.IO;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.InteropServices;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Model;

public class FilePropertiesModel
{
    #region Methods

    public ImageSource? GetIconImage(FileSystemObject obj) => Operation.GetIconImage(obj);

    public void RenameFile(FileSystemObject item, string fileName)
    {
        Operation.Rename(item.Path.Replace(item.Name, ""), item, fileName);
    }

    public void ChangeAttributes(FileSystemObject item, FileAttributes attributes)
    {
        Operation.AttributesChange(item, attributes);
        DataCache.Update();
    }

    // Get From https://stackoverflow.com/questions/48670600/c-sharp-net-core-get-file-size-on-disk-cross-platform-solution
    public long GetFileSizeOnDisk(string file)
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