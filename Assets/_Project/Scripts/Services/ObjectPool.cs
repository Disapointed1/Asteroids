using System;
using System.Collections.Generic;

public class ObjectPool<T> where T : IPoolable
{
    private readonly List<T> _availableObjects = new();
    private readonly Func<T> _factoryMethod;
    private readonly List<T> _inUseObjects = new();


    public ObjectPool(Func<T> factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

    public IReadOnlyList<T> InUseObjects => _inUseObjects;

    public T Get()
    {
        if (_availableObjects.Count > 0)
        {
            var item = _availableObjects[_availableObjects.Count - 1];
            _availableObjects.RemoveAt(_availableObjects.Count - 1);
            _inUseObjects.Add(item);
            return item;
        }
        else
        {
            var item = _factoryMethod();
            _inUseObjects.Add(item);
            return item;
        }
    }

    public void Return(T item)
    {
        if (!_inUseObjects.Remove(item))
            return;

        item.OnDespawn();
        _availableObjects.Add(item);
    }

    public void Register(T item)
    {
        if (_inUseObjects.Contains(item) || _availableObjects.Contains(item))
            return;

        _inUseObjects.Add(item);
    }
}