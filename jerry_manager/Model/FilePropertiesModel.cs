using System.IO;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.InteropServices;
using jerry_manager.Core;
using jerry_manager.Core.FileSystem;
using System;

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

    public static long GetSizeOnDisk(FileSystemObject file) => Operation.GetSizeOnDisk(file);

    #endregion
}