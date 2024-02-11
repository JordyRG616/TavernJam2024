using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerBehaviour
{
    [SerializeField] private AudioConfiguration audioConfiguration;
    [field:SerializeField] public AudioChannel musicChannel { get; private set; }
    [SerializeField] private List<AudioClip> musics = new List<AudioClip>();

    [field:SerializeField] public AudioChannel SfxChannel { get; private set; }
    [SerializeField] private Vector2 musicIntervalRange;

    private WaitForSeconds waitMusicInterval;
    private bool _playingMusic;
    public bool PlayMusic
    {
        get => _playingMusic;
        set
        {
            _playingMusic = value;

            if (_playingMusic == false) StopCoroutine(ManageMusicLoop());
            else StartCoroutine(ManageMusicLoop());
        }
    }


    private void Start()
    {
        musicChannel.volume = audioConfiguration.musicVolume;
        SfxChannel.volume = audioConfiguration.sfxVolume;
        PlayMusic = true;
    }

    public void SetMusicVolume(float volume)
    {
        audioConfiguration.musicVolume = volume;
        musicChannel.volume = audioConfiguration.musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        audioConfiguration.sfxVolume = volume;
        SfxChannel.volume = audioConfiguration.sfxVolume;
    }

    private IEnumerator ManageMusicLoop()
    {
        while(true)
        {
            var rdm = Random.Range(0, musics.Count);
            var music = musics[rdm];

            musicChannel.Play(music);

            yield return new WaitUntil(() => musicChannel.IsPlaying == false);

            var interval = Random.Range(musicIntervalRange.x, musicIntervalRange.y);
            waitMusicInterval = new WaitForSeconds(interval);

            yield return waitMusicInterval;
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        SfxChannel.Play(clip);
    }
}

[System.Serializable]
public class AudioChannel
{
    public AudioSource source;
    public bool musicChannel;

    private float _volume = 1;
    public float volume
    {
        get => _volume;
        set
        {
            _volume = value;
            source.volume = _volume;
        }
    }
    public bool IsPlaying => source.isPlaying;


    public void Play(AudioClip clip)
    {
        if (musicChannel)
        {
            source.clip = clip;
            source.Play();
        }
        else
        {
            source.PlayOneShot(clip);
        }
    }
}