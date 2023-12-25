using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioMixerGroup group;

    public string Name;

    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume = 1f;
    [Range(.1f, 3f)]
    public float Pitch = 1f;

    public bool PlayOnAwake;
    public bool Loop;

    [HideInInspector]
    public AudioSource audioSource;
}