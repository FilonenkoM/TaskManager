using System.Windows;
using System.Windows.Threading;
using FilonenkoTaskManager.Tools;

namespace FilonenkoTaskManager
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SharedData.ReloadSource.Cancel();
            SharedData.RecountSource.Cancel();
        }
    }
}
