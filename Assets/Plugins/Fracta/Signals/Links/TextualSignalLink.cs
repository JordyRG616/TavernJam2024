using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[AddComponentMenu("Signal Links/Textual Link")]
public class TextualSignalLink : MonoBehaviour
{
    public SignalReference<string> signalReference;
    [Space]
    public UnityEvent<string> linkedCallback;


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

    private void InvokeCallbacks(string value)
    {
        linkedCallback?.Invoke(value);
    }
}
