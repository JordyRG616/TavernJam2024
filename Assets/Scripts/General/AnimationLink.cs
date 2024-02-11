using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class AnimationLink : MonoBehaviour
{
    public UnityEvent OnEventFired;

    public void FireAnimationEvent()
    {
        OnEventFired.Invoke();
    }
}
