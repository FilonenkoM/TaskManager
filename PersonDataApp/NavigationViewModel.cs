using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KMA.ProgrammingInCSharp2019.Practice3.LoginControlMVVM;

namespace PersonDataApp
{
    class NavigationViewModel: INotifyPropertyChanged
    {
        public ICommand PersonInfoCommand { get; set;  }
        public ICommand PersonInfoReportCommand { get; set; }
        private object _selectedViewModel;

        public object SelectedViewModel
        {
            get { return _selectedViewModel;  }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NavigationViewModel()
        {
            PersonInfoCommand = new RelayCommand<object>(
                o =>
                {
                    SelectedViewModel = new UserInfoViewModel();
                    // todo switch to main window
                }, o => true);
            PersonInfoReportCommand = new RelayCommand<object>(
                o =>
                {
                    SelectedViewModel = new PersonInfoReportPage();
                    // todo switch to main window
                }, o => true);
        }
    }
}
