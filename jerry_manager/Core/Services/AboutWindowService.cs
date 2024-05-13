using jerry_manager.View;

namespace jerry_manager.Core.Services;

public class AboutWindowService : IWindowService
{
    public static void Show()
    {
        var view = new AboutView();
        view.Show();
    }
}
