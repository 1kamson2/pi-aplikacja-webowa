using System;
using System.Collections.Generic;
using System.Text;
using Core.History;

namespace Core.Singleton;
/// <summary>
/// Singleton is a base class for implementing the singleton pattern.
/// It ensures that class is properly setup and thread-safe.
/// </summary>
/// <typeparam name="TSelf"></typeparam>

public abstract class Singleton<TSelf>
    where TSelf : Singleton<TSelf>, new()
{
    public Singleton() => Setup();
    protected readonly object _lock = new();
    /// <summary>
    /// Setup is used to setup the function
    /// </summary>
    protected abstract void Setup();
}
