using jerry_manager.View;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Core.Services;

public class FilePropertiesWindowService : IWindowService
{
    public static void Show(FileSystemObject fileSystemObject)
    {
        var view = new FilePropertiesView(fileSystemObject);
        view.Show();
    }
}
