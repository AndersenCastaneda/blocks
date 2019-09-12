using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip backgroundAudio;
    [SerializeField] private AudioClip winAudio;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayAudioLevel()
    {
        source.PlayOneShot(backgroundAudio);
    }

    public void PLayAudioWin()
    {
        source.Stop();
        source.PlayOneShot(winAudio);
    }

    public void StopAudio()
    {
        source.Stop();
    }
}
