using jerry_manager.View;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Core.Services;

class FilePropertiesWindowService : IWindowService
{
    public static void Show(FileSystemObject fileSystemObject)
    {
        var view = new FilePropertiesView();
        view.ViewModel.CurrentFileSystemObject = fileSystemObject;
        view.Show();
    }
}
