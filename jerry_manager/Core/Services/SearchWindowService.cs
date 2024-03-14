using jerry_manager.View;

namespace jerry_manager.Core.Services;

public class SearchWindowService : IWindowService
{
    public static void Show()
    {
        var view = new SearchView();
        view.Show();
    }
}
