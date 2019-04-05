
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using FilonenkoTaskManager.ViewModels;

namespace FilonenkoTaskManager.Tools
{
    internal static class SharedData
    {
        internal static ObservableCollection<ProcessModule> CurrentModules = new ObservableCollection<ProcessModule>();
        internal static ObservableCollectionExtension<ProcessGridRowViewModel> Processes =
            new ObservableCollectionExtension<ProcessGridRowViewModel>();
        internal static ObservableCollection<ProcessThread> CurrentThreads = new ObservableCollection<ProcessThread>();

        internal static CancellationTokenSource ReloadSource = new CancellationTokenSource();
        internal static CancellationToken ReloadToken = ReloadSource.Token;
        internal static CancellationTokenSource RecountSource = new CancellationTokenSource();
        internal static CancellationToken RecountToken = RecountSource.Token;
    }
}
