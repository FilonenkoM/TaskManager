using FilonenkoTaskManager.Tools;

namespace FilonenkoTaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationManager.NavigationWindow = this;
        }
    }
}
