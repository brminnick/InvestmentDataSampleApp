using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using AsyncAwaitBestPractices;

namespace InvestmentDataSampleApp
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        readonly WeakEventManager _propertyChangedEventManager = new WeakEventManager();

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => _propertyChangedEventManager.AddEventHandler(value);
            remove => _propertyChangedEventManager.RemoveEventHandler(value);
        }

        protected void SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName] string propertyname = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyname);
        }

        void OnPropertyChanged([CallerMemberName]string name = "") =>
            _propertyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs(name), nameof(INotifyPropertyChanged.PropertyChanged));
    }
}
