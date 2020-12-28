using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace paperviz
{
    /// <summary>
    /// Borrowed most of this from here:
    /// https://github.com/AndreiMisiukevich/BaseViewModel/blob/master/Lib/BaseViewModel/BaseViewModel.cs
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Lazy<ConcurrentDictionary<Type, object>> _lazyPropertiesMapping = new Lazy<ConcurrentDictionary<Type, object>>(() => new ConcurrentDictionary<Type, object>());
        
        protected Page CurrentPage => Application.Current.MainPage;

        protected Task DisplayAlert(string title, string message, string cancel) =>
            CurrentPage.DisplayAlert(title, message, cancel);
        
        protected T Get<T>(T defaultValue = default(T), [CallerMemberName] string key = null)
            => GetTypeDict<T>().TryGetValue(key ?? string.Empty, out T val)
                ? val
                : defaultValue;
        
        protected bool Set<T>(T value, bool shouldEqual = true, bool shouldRaisePropertyChanged = true, [CallerMemberName] string key = null)
        {
            var typeDict = GetTypeDict<T>();
            if (shouldEqual && typeDict.TryGetValue(key ?? string.Empty, out T oldValue) && EqualityComparer<T>.Default.Equals(oldValue, value))
            {
                return false;
            }
            typeDict[key ?? string.Empty] = value;
            if (shouldRaisePropertyChanged)
            {
                OnPropertyChanged(key);
            }
            return true;
        }        
        protected void OnPropertyChanged([CallerMemberName] string key = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(key));
        private ConcurrentDictionary<string, T> GetTypeDict<T>()
        {
            var type = typeof(T);
            if (!_lazyPropertiesMapping.Value.TryGetValue(type, out object valDict))
            {
                _lazyPropertiesMapping.Value[type] = valDict = new ConcurrentDictionary<string, T>();
            }
            return valDict as ConcurrentDictionary<string, T>;
        }        
    }}