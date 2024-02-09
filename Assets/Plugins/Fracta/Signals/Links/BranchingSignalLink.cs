using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[AddComponentMenu("Signal Links/Branching Link")]
public class BranchingSignalLink : MonoBehaviour
{
    public SignalReference<bool> signalReference;
    [Space]
    public UnityEvent<bool> linkedCallback;


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

    private void InvokeCallbacks(bool value)
    {
        linkedCallback?.Invoke(value);
    }
}
