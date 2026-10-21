using System;
using System.Collections.Generic;
using System.Text;
using Core.History;

namespace Core.Singleton;

public abstract class Singleton<TSelf>
    where TSelf : Singleton<TSelf>, new()
{
    public Singleton() => Setup();
    protected readonly object _lock = new();
    protected abstract void Setup();
}
