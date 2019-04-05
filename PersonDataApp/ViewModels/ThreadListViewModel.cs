
using System.Collections.ObjectModel;
using System.Diagnostics;
using FilonenkoTaskManager.Tools;

namespace FilonenkoTaskManager.ViewModels
{
    class ThreadListViewModel : BaseViewModel
    {
        public ObservableCollection<ProcessThread> Threads => SharedData.CurrentThreads;
    }
}
