using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[AddComponentMenu("Signal Links/Intenger Link")]
public class IntengerSignalLink : MonoBehaviour
{
    public SignalReference<int> signalReference;
    [Space]
    public UnityEvent<int> linkedCallback;


    private void Start()
    {
        if (signalReference.CreateLink())
        {
            signalReference.Signal += InvokeCallbacks;
        }
        else
        {
        }
    }

    private void InvokeCallbacks(int value)
    {
        linkedCallback?.Invoke(value);
    }
}
