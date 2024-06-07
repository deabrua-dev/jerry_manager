using jerry_manager.ViewModel;

namespace jerry_manager.Core;

public static class DataCache
{
    private static FileExplorerViewModel m_ActiveView;

    public static FileExplorerViewModel ActiveView
    {
        get => m_ActiveView;
        set => m_ActiveView = value;
    }

    private static FileExplorerViewModel m_NotActiveView;

    public static FileExplorerViewModel NotActiveView
    {
        get => m_NotActiveView;
        set => m_NotActiveView = value;
    }

    public static void Update()
    {
        ActiveView.CurrentPath = ActiveView.CurrentPath;
        NotActiveView.CurrentPath = NotActiveView.CurrentPath;
    }
}