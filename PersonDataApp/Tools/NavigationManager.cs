
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FilonenkoTaskManager.Tools
{
    static class NavigationManager
    {
        internal static NavigationWindow NavigationWindow { get; set; }
        internal static void Show(Page page)
        {
            NavigationWindow.NavigationService.Navigate(page);
        }
    }
}
