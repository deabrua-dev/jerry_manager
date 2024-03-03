using System.Collections.Generic;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Model;

public class MainWindowModel
{
    #region Methods

    public void Open(FileSystemObject item)
    {
        Operation.Open(item);
    }

    public void Edit(FileSystemObject item)
    {
        Operation.EditFile(item);
    }

    public void Delete(List<FileSystemObject> items)
    {
        Operation.Delete(items);
    }

    #endregion
}