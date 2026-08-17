using System;
using System.Collections.Generic;

public class ObjectPool<T> where T: IPoolable
{
    private readonly List<T> _availableObjects = new List<T>();
    private readonly List<T> _inUseObjects = new List<T>();
    private readonly Func<T> _factoryMethod;

    public IReadOnlyList<T> InUseObjects => _inUseObjects;


    public ObjectPool(Func<T> factoryMethod)
    {
        _factoryMethod = factoryMethod;
    }

    public T Get()
    {
        if  (_availableObjects.Count > 0)
        {
            T item = _availableObjects[_availableObjects.Count - 1];
            _availableObjects.RemoveAt(_availableObjects.Count - 1);
            _inUseObjects.Add(item);
            item.OnSpawn();
            return item;
        }
        else
        {
            T item = _factoryMethod();
            _inUseObjects.Add(item);
            item.OnSpawn();
            return item;
        }
    }

    public void Return(T item)
    {
        _inUseObjects.Remove(item);
        item.OnDespawn();
        _availableObjects.Add(item);
    }
}
