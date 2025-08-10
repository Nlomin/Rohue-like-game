using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private List<IObserver> _observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Equals(observer);
    }

    public void NotifyObserver()
    {
        _observers.ForEach((_observer) => { _observer.OnNotify(); }); 
    }
}
public interface IObserver
{
    public void OnNotify(); 
}