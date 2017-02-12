using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace ObservableBinding
{
    internal class ObserverBinding
    {
        private readonly BindingExpression _bindingExpression;
        private readonly FrameworkElement _frameworkElement;
        private INotifyCollectionChanged _lastNotifyCollectionChanged;
        private INotifyPropertyChanged _dataContext;

        public ObserverBinding(BindingExpression bindingExpression, FrameworkElement frameworkElement)
        {
            _bindingExpression = bindingExpression;
            _frameworkElement = frameworkElement;
            _frameworkElement.Loaded += (sender, eventArgs) => Init();
            _frameworkElement.DataContextChanged += (sender, eventArgs) => Init();
        }

        private void Init()
        {
            DetachEvents();
            AttachEvents();
        }

        private void AttachEvents()
        {
            _dataContext = (INotifyPropertyChanged)_bindingExpression.DataItem;
            if (_dataContext == null)
            {
                return;
            }

            _lastNotifyCollectionChanged = _bindingExpression.GetBoundObject<INotifyCollectionChanged>();
            if (_lastNotifyCollectionChanged == null)
            {
                return;
            }

            _dataContext.PropertyChanged += BindingObserver_PropertyChanged;
            _lastNotifyCollectionChanged.CollectionChanged += NotifyCollectionChangedOnCollectionChanged;
        }

        private void BindingObserver_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _bindingExpression.ResolvedSourcePropertyName)
            {
                DetachEvents();
                AttachEvents();
            }
        }

        private void DetachEvents()
        {
            if (_dataContext == null || _lastNotifyCollectionChanged == null)
            {
                return;
            }

            _dataContext.PropertyChanged -= BindingObserver_PropertyChanged;
            _lastNotifyCollectionChanged.CollectionChanged -= NotifyCollectionChangedOnCollectionChanged;
        }

        private void NotifyCollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            _bindingExpression.UpdateTarget();
        }
    }
}