using jerry_manager.View;
using jerry_manager.ViewModel;

namespace jerry_manager.Core;

public static class DataCache
{
    private static FileExplorerViewModel activeView;

    public static FileExplorerViewModel ActiveView
    {
        get => activeView;
        set => activeView = value;
    }
}