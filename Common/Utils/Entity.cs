﻿using System;
using System.Collections.Generic;



public delegate void Callback();

public delegate void Callback<T>(T arg1);

public delegate void Callback<T, U>(T arg1, U arg2);

public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

public abstract class Entity
{

    private readonly Dictionary<Enum, Delegate> _table = new Dictionary<Enum, Delegate>();

    //多种AddDelegate，每一种附带不同数量参数
    public void AddListener(Enum p_type, Callback pListener)
    {
        _table[p_type] = _table.ContainsKey(p_type) ? Delegate.Combine(_table[p_type], pListener) : pListener;
    }

    public void AddListener<T>(Enum p_type, Callback<T> pListener)
    {
        _table[p_type] = _table.ContainsKey(p_type) ? Delegate.Combine(_table[p_type], pListener) : pListener;
    }

    public void AddListener<T, U>(Enum p_type, Callback<T, U> pListener)
    {
        _table[p_type] = _table.ContainsKey(p_type) ? Delegate.Combine(_table[p_type], pListener) : pListener;
    }

    public void AddListener<T, U, V>(Enum p_type, Callback<T, U, V> pListener)
    {
        _table[p_type] = _table.ContainsKey(p_type) ? Delegate.Combine(_table[p_type], pListener) : pListener;
    }

    public void RemoveListener(Enum pType, Callback pListener)
    {
        if (_table.ContainsKey(pType))
        {
            Delegate d = Delegate.Remove(_table[pType], pListener);
            if (d == null) _table.Remove(pType);
            else _table[pType] = d;
        }
    }

    public void RemoveListener<T>(Enum pType, Callback<T> pListener)
    {
        if (_table.ContainsKey(pType))
        {
            Delegate d = Delegate.Remove(_table[pType], pListener);
            if (d == null) _table.Remove(pType);
            else _table[pType] = d;
        }
    }

    public void RemoveListener<T, TU>(Enum pType, Callback<T, TU> pListener)
    {
        if (_table.ContainsKey(pType))
        {
            Delegate d = Delegate.Remove(_table[pType], pListener);
            if (d == null) _table.Remove(pType);
            else _table[pType] = d;
        }
    }

    public void RemoveListener<T, TU, TV>(Enum pType, Callback<T, TU, TV> pListener)
    {
        if (_table.ContainsKey(pType))
        {
            Delegate d = Delegate.Remove(_table[pType], pListener);
            if (d == null) _table.Remove(pType);
            else _table[pType] = d;
        }
    }

    /// <summary>
    ///     Clear all listeners of a given type.
    /// </summary>
    /// <param name="p_type">Enum that describes the event being listened.</param>
    public void RemoveAllListeners(Enum p_type)
    {
        if (_table.ContainsKey(p_type))
        {
            Delegate[] dList = _table[p_type].GetInvocationList();
            for (int i = 0; i < dList.Length; i++)
            {
                Delegate.Remove(_table[p_type], dList[i]);
            }
            _table.Remove(p_type);
        }
    }

    public void RemoveAllListeners()
    {
        IEnumerator<Enum> keyEnum = _table.Keys.GetEnumerator();
        Enum[] enums = new Enum[_table.Count];
        int i = 0;
        while (keyEnum.MoveNext())
        {
            enums[i] = keyEnum.Current;
        }
        for (int j = enums.Length - 1; j >= 0; j--)
        {
            RemoveAllListeners(enums[j]);
        }
    }


    public void Dispatch(Enum eventType)
    {
        if (!_table.ContainsKey(eventType)) return;

        Delegate delega = _table[eventType];

        if (delega != null)
        {
            var callback = delega as Callback;
            if (callback != null) callback();
        }
    }

    public void Dispatch<T>(Enum pType, T arg1)
    {
        if (!_table.ContainsKey(pType)) return;

        Delegate delega = _table[pType];

        if (delega != null)
        {
            var callback = delega as Callback<T>;
            if (callback != null) callback(arg1);
        }
    }

    public void Dispatch<T, TU>(Enum pType, T arg1, TU arg2)
    {
        if (!_table.ContainsKey(pType)) return;

        Delegate delega = _table[pType];

        if (delega != null)
        {
            var callback = delega as Callback<T, TU>;
            if (callback != null) callback(arg1, arg2);
        }
    }

    public void Dispatch<T, TU, TV>(Enum pType, T arg1, TU arg2, TV arg3)
    {
        if (!_table.ContainsKey(pType)) return;

        Delegate delega = _table[pType];

        if (delega != null)
        {
            var callback = delega as Callback<T, TU, TV>;
            if (callback != null) callback(arg1, arg2, arg3);
        }
    }

}

/// <summary>
/// 全局消息控制器
/// </summary>
public class GlobalEntity : Entity
{
    private static GlobalEntity _instance;
    public static GlobalEntity GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GlobalEntity();
        }
        return _instance;
    }

    public GlobalEntity()
    {

    }
}
