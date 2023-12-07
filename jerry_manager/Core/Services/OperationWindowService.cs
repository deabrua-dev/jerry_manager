using jerry_manager.View;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Core.Services;

class OperationWindowService : IWindowService
{
    public static void Show(OperationType operationType)
    {
        var view = new OperationView();
        view.ViewModel.OperationType = operationType;
        view.Show();
    }
}
