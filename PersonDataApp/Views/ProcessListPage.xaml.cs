
using FilonenkoTaskManager.ViewModels;

namespace FilonenkoTaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для ProcessListPage.xaml
    /// </summary>
    public partial class ProcessListPage 
    {
        public ProcessListPage()
        {
            var viewModel = new ProcessListViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
