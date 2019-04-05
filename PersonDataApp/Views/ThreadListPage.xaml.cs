
using FilonenkoTaskManager.ViewModels;

namespace FilonenkoTaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для ThreadListPage.xaml
    /// </summary>
    public partial class ThreadListPage 
    {
        public ThreadListPage()
        {
            DataContext = new ThreadListViewModel();
            InitializeComponent();
        }
    }
}
