using jerry_manager.View;
using jerry_manager.Core.FileSystem;

namespace jerry_manager.Core.Services;

class OperationWindowService : IWindowService
{
    public static void Show(OperationType OperationType)
    {
        var view = new OperationView();
        view.ViewModel.OperationType = OperationType;
        view.Show();
    }
}
