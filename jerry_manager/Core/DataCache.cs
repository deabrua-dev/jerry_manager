using jerry_manager.View;

namespace jerry_manager.Core;

public static class DataCache
{
    private static FileExplorerView activeView;

    public static FileExplorerView ActiveView
    {
        get => activeView;
        set => activeView = value;
    }
    
}