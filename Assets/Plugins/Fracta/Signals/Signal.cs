using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Signal<T> : ISignal
{
    public Type ParameterType => typeof(T);
    public bool Suspended { get; set; }

    private Action<T> callback;


    public void Fire(T value)
    {
        if (Suspended) return;

        callback?.Invoke(value);
    }

    /// <summary>
    /// ESTA FUNÇÃO NÃO ESTÁ IMPLEMENTADA!!
    /// </summary>
    /// <param name="delay"></param>
    public void FireDelayed(T value, float delay)
    {
        //! IMPLEMENTAÇÃO DEPENDE DE ASYNC
    }

    public void Clear()
    {
        callback = null;
    }

    #region Operators
    public static Signal<T> operator+(Signal<T> a, Signal<T> b)
    {
        a.callback += b.callback;
        return a;
    }

    public static Signal<T> operator -(Signal<T> a, Signal<T> b)
    {
        a.callback -= b.callback;
        return a;
    }

    public static Signal<T> operator +(Signal<T> a, Action<T> b)
    {
        a.callback += b;
        return a;
    }

    public static Signal<T> operator -(Signal<T> a, Action<T> b)
    {
        a.callback -= b;
        return a;
    }
    #endregion
}

[Serializable]
public class Signal : ISignal
{
    public Type ParameterType => null;
    public bool Suspended { get; set; }

    private Action callback;


    public void Fire()
    {
        if (Suspended) return;

        callback?.Invoke();
    }

    /// <summary>
    /// ESTA FUNÇÃO NÃO ESTÁ IMPLEMENTADA!!
    /// </summary>
    /// <param name="delay"></param>
    public void FireDelayed(float delay)
    {
        //! IMPLEMENTAÇÃO DEPENDE DE ASYNC
    }

    #region Operators
    public static Signal operator +(Signal a, Signal b)
    {
        a.callback += b.callback;
        return a;
    }

    public static Signal operator -(Signal a, Signal b)
    {
        a.callback -= b.callback;
        return a;
    }

    public static Signal operator +(Signal a, Action b)
    {
        a.callback += b;
        return a;
    }

    public static Signal operator -(Signal a, Action b)
    {
        a.callback -= b;
        return a;
    }
    #endregion
}

public interface ISignal
{
    public Type ParameterType { get; }
    public bool Suspended { get; set; }
}