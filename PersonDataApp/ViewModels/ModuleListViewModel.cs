
using System.Collections.ObjectModel;
using System.Diagnostics;
using FilonenkoTaskManager.Tools;

namespace FilonenkoTaskManager.ViewModels
{
    class ModuleListViewModel : BaseViewModel
    {
        public ObservableCollection<ProcessModule> Modules => SharedData.CurrentModules;
    }
}

