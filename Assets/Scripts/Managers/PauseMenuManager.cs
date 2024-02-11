using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public Signal<bool> OnPauseTriggered;
    [SerializeField] private KeyCode pauseButton = KeyCode.Escape;

    private bool open;

    private void Update()
    {
        if (Input.GetKeyDown(pauseButton))
        {
            open = !open;
            OnPauseTriggered.Fire(open);
        }
    }
}
