
using FilonenkoTaskManager.ViewModels;

namespace FilonenkoTaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для ModuleListPage.xaml
    /// </summary>
    public partial class ModuleListPage 
    {
        public ModuleListPage()
        {
            DataContext = new ModuleListViewModel();
            InitializeComponent();
        }
    }
}
