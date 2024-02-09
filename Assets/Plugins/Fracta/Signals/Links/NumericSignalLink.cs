using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[AddComponentMenu("Signal Links/Numeric Link")]
public class NumericSignalLink : MonoBehaviour
{
    public SignalReference<float> signalReference;
    [Space]
    public UnityEvent<float> linkedCallback;


    private void Start()
    {
        if (signalReference.CreateLink())
        {
            signalReference.Signal += InvokeCallbacks;
        } else
        {
        }
    }

    private void InvokeCallbacks(float value)
    {
        linkedCallback?.Invoke(value);
    }
}
