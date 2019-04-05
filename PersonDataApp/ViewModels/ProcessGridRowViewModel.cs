using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using FilonenkoTaskManager.Tools;

namespace FilonenkoTaskManager.ViewModels
{
    class ProcessGridRowViewModel : BaseViewModel
    {
        #region Properties
        public string ProcessName => _process.ProcessName;
        public bool Responding => _process.Responding;
        public int Id => _process.Id;
        public double CpuPercent { get; set; }
        public double RamPercent { get; set; }
        public ObservableCollection<ProcessThread> Threads = new ObservableCollection<ProcessThread>();
        public ObservableCollection<ProcessModule> Modules { get; set; } = new ObservableCollection<ProcessModule>();
        public int ThreadsCount { get; set; }

        private string _user = "";
        public string User
        {
            get
            {
                if (_user.Equals(""))
                {
                    LoadUser();
                }
                return _user;
            }
            set
            {
                _user = value; 
                OnPropertyChanged();
            }
        }
        public string Path { get; set; }
        public DateTime? Time { get; set; }
        public Visibility LoaderVisible { get; set; } = Visibility.Visible;
        #endregion

        private readonly Process _process;
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _ramCounter;
        private readonly Int64 _ramTotal = PerformanceInfo.GetTotalMemoryInMiB() * 10000;
        
        public ProcessGridRowViewModel(Process process)
        {
            _process = process;
            _cpuCounter = new PerformanceCounter("Process", "% Processor Time", _process.ProcessName, true);
            _ramCounter = new PerformanceCounter("Process", "Working Set", _process.ProcessName, true);
            try
            {
                Path = process.MainModule.FileName;
            }
            catch (Exception)
            {
                Path = "Access Denied";
            }
            try
            {
                Time = process.StartTime;
            }
            catch (Exception)
            {
                // ignored
            }

            ThreadsCount = process.Threads.Count;
            RecountThreads();
            LoadModules();
        }

        private async void LoadModules()
        {
            await Task.Run(() =>
            {
                try
                {
                    foreach (ProcessModule module in _process.Modules)
                        Application.Current.Dispatcher.Invoke(() => Modules.Add(module));
                }
                catch (Win32Exception) { }
                catch(InvalidOperationException) { }
            });
            OnPropertyChanged("Modules");
        }
        public async void LoadUser()
        {
            await Task.Run(() =>
            {
                User = UsernameSearcher.GetProcessOwner(Id);
            });
            LoaderVisible = Visibility.Hidden;
            OnPropertyChanged("LoaderVisible");
        }

        public void Recount()
        {
            try
            {
                CpuPercent = _cpuCounter.NextValue() / Environment.ProcessorCount;
                OnPropertyChanged("CpuPercent");
            }
            catch(InvalidOperationException) { }
            try
            {
                RamPercent = _ramCounter.NextValue() / _ramTotal;
                OnPropertyChanged("RamPercent");
            }
            catch (InvalidOperationException) { }
            ThreadsCount = _process.Threads.Count;
        }

        public void Kill()
        {
            _process.Kill();
        }
        public void RecountThreads()
        {
            Threads.Clear();
            foreach (ProcessThread thread in _process.Threads)
                Threads.Add(thread);
        }
    }
}

