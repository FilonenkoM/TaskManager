
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace FilonenkoTaskManager.Tools
{
    class ObservableCollectionExtension<T> : ObservableCollection<T>
    {
        public void UpdateCollection()
        {
            if(System.Windows.Application.Current != null)
            System.Windows.Application.Current.Dispatcher.Invoke(() => OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)));
        }
    }
}
