using jerry_manager.View;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Core.Services;

public class OperationWindowService : IWindowService
{
    public static void Show(OperationType operationType)
    {
        var view = new OperationView(operationType);
        view.Show();
    }
}
