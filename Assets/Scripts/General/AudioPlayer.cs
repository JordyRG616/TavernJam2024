using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private AudioManager audioManager;


    private void Start()
    {
        audioManager = GameMaster.GetManager<AudioManager>();
    }

    public void Play()
    {
        audioManager.PlaySfx(clip);
    }
}
