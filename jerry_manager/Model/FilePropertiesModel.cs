using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Model;

public class FilePropertiesModel
{
    public ImageSource GetIconImage(FileSystemObject obj)
    {
        if (obj is Folder)
        {
            return new BitmapImage(new Uri(@"pack://application:,,,/jerry_manager;component/Images/Default icons/folder-icon-big-256.png", UriKind.Absolute));
        }
        var ico = Icon.ExtractAssociatedIcon(obj.Path);
        return Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
    }
}