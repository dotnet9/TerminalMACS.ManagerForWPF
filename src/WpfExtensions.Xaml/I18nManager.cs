using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows;

namespace WpfExtensions.Xaml;

public class CurrentUICultureChangedEventArgs : EventArgs
{
    public CurrentUICultureChangedEventArgs(CultureInfo oldUiCulture, CultureInfo newUiCulture)
    {
        OldUICulture = oldUiCulture;
        NewUICulture = newUiCulture;
    }

    public CultureInfo OldUICulture { get; }

    public CultureInfo NewUICulture { get; }
}

public class I18nManager : INotifyPropertyChanged
{
    private readonly ConcurrentDictionary<string, ResourceManager> _resourceManagerStorage = new();
    private CultureInfo _currentUICulture;

    private I18nManager()
    {
    }

    public static I18nManager Instance { get; } = new();

    public CultureInfo CurrentUICulture
    {
        get => _currentUICulture;
        set
        {
            if (EqualityComparer<CultureInfo>.Default.Equals(_currentUICulture, value)) return;

            OnCurrentUICultureChanged(_currentUICulture, _currentUICulture = value);
            OnPropertyChanged(nameof(CurrentUICulture));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public event EventHandler<CurrentUICultureChangedEventArgs> CurrentUICultureChanged;

    public void Add(ResourceManager resourceManager)
    {
        if (_resourceManagerStorage.ContainsKey(resourceManager.BaseName))
            throw new ArgumentException(
                $"The ResourceManager named {resourceManager.BaseName} already exists, cannot be added repeatedly. ",
                nameof(resourceManager));

        _resourceManagerStorage[resourceManager.BaseName] = resourceManager;
    }

    public object Get(ComponentResourceKey key)
    {
        return GetCurrentResourceManager(key.TypeInTargetAssembly.FullName)?
            .GetObject(key.ResourceId.ToString(), CurrentUICulture) ?? $"<MISSING: {key}>";
    }

    private ResourceManager GetCurrentResourceManager(string key)
    {
        return _resourceManagerStorage.TryGetValue(key, out var value) ? value : null;
    }

    protected virtual void OnCurrentUICultureChanged(CultureInfo oldCulture, CultureInfo newCulture)
    {
        CurrentUICultureChanged?.Invoke(this, new CurrentUICultureChangedEventArgs(oldCulture, newCulture));
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}