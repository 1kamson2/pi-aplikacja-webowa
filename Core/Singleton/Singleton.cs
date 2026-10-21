using System;
using System.Collections.Generic;
using System.Text;
using Core.History;

namespace Core.Singleton;

public abstract class Singleton<TSelf>
    where TSelf : Singleton<TSelf>, new()
{
    protected Singleton() => Setup();

    public static TSelf Instance { get; } = new TSelf();
    protected readonly object _lock = new();
    protected abstract void Setup();
}
