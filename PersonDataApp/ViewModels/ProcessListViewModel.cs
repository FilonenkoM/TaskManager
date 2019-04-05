using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using FilonenkoTaskManager.Tools;
using FilonenkoTaskManager.Views;

namespace FilonenkoTaskManager.ViewModels
{
    class ProcessListViewModel : BaseViewModel
    {
        public ProcessGridRowViewModel SelectedProcess { get; set; }
        public ObservableCollectionExtension<ProcessGridRowViewModel> Processes => SharedData.Processes;

        private bool _loaded = SharedData.Processes.Count != 0;

        private bool Loaded
        {
            set
            {
                _loaded = value;
                OnPropertyChanged("InitialProcessLoad");
                OnPropertyChanged("InitialProcessLoaded");
            }
        }

        public Visibility InitialProcessLoad => _loaded ? Visibility.Hidden : Visibility.Visible;
        public Visibility InitialProcessLoaded => _loaded ? Visibility.Visible : Visibility.Hidden;

        public ProcessListViewModel()
        {
            var processReloader = new Thread(Reload);
            var processRecounter = new Thread(Recount);
            processReloader.Start();
            processRecounter.Start();
        }

        private void MergeProcess(List<Process> next)
        {
            //Processes.RemoveAll(p => next.All(np => p.Id != np.Id));
            List<ProcessGridRowViewModel> processesToRemove = new List<ProcessGridRowViewModel>();
            foreach (var process in Processes)
                if (next.All(p => p.Id != process.Id))
                    processesToRemove.Add(process);
            List<ProcessGridRowViewModel> processesToAdd = new List<ProcessGridRowViewModel>();
            foreach (var process in next)
                if (Processes.All(p => p.Id != process.Id))
                    processesToAdd.Add(new ProcessGridRowViewModel(process));
            foreach (var process in processesToAdd)
                Application.Current.Dispatcher.Invoke(() => Processes.Add(process));
            foreach (var process in processesToRemove)
                Application.Current.Dispatcher.Invoke(() => Processes.Remove(process));
        }

        private void Reload()
        {
            while (! SharedData.ReloadToken.IsCancellationRequested)
            {
                MergeProcess(Process.GetProcesses().ToList());
                Processes.UpdateCollection();
                Loaded = true;
                Thread.Sleep(5000);
            }
        }

        private void Recount()
        {
            while (! SharedData.RecountToken.IsCancellationRequested)
            {
                try
                {
                    foreach (var process in Processes) process.Recount();
                    Processes.UpdateCollection();
                    OnPropertyChanged("Processes");
                }
                catch (InvalidOperationException)
                {
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        #region Commands

        private RelayCommand<object> _showModulesCommand;
        public RelayCommand<Object> ShowModulesCommand
        {
            get
            {
                return _showModulesCommand ?? (_showModulesCommand = new RelayCommand<object>(
                           o =>
                           {
                               SharedData.CurrentModules = SelectedProcess.Modules;
                               NavigationManager.Show(new ModuleListPage());
                           }, o => SelectedProcess != null && SelectedProcess.Modules.Count != 0));
            }
        }

        private RelayCommand<object> _showThreadCommand;
        public RelayCommand<Object> ShowThreadsCommand
        {
            get
            {
                return _showThreadCommand ?? (_showThreadCommand = new RelayCommand<object>(
                           o =>
                           {
                               SharedData.CurrentThreads = SelectedProcess.Threads;
                               NavigationManager.Show(new ThreadListPage());
                           }, o => SelectedProcess != null && SelectedProcess.Threads.Count != 0));
            }
        }
        private RelayCommand<object> _openFolderCommand;
        public RelayCommand<Object> OpenFolderCommand
        {
            get
            {
                return _openFolderCommand ?? (_openFolderCommand = new RelayCommand<object>(
                           o =>
                           {
                               try
                               {
                                   Process.Start("explorer.exe",
                                       SelectedProcess.Path.Remove(SelectedProcess.Path.LastIndexOf('\\')));
                               }
                               catch (Exception)
                               {
                                   MessageBox.Show("ccess denied");
                               }
                           }, o => SelectedProcess != null && !SelectedProcess.Path.Equals("Access Denied")));
            }
        }

        private RelayCommand<object> _killProcessCommand;
        public RelayCommand<Object> KillProcessCommand
        {
            get
            {
                return _killProcessCommand ?? (_killProcessCommand = new RelayCommand<object>(
                           o =>
                           {
                               try
                               {
                                   SelectedProcess.Kill();
                                   Application.Current.Dispatcher.Invoke(() => Processes.Remove(SelectedProcess));
                               }
                               catch (Exception)
                               {
                                   MessageBox.Show("Access denied");
                               }
                           }, o => SelectedProcess != null && !SelectedProcess.Path.Equals("Access Denied")));
            }
        }
        #endregion
    }
}
