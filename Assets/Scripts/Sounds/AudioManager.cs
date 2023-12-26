using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;

    public Sound[] sounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var item in sounds)
        {
            item.audioSource = gameObject.AddComponent<AudioSource>();
            item.audioSource.clip = item.Clip;

            item.audioSource.volume = item.Volume;
            item.audioSource.pitch = item.Pitch;
            item.audioSource.playOnAwake = item.PlayOnAwake;
            item.audioSource.loop = item.Loop;

            item.audioSource.outputAudioMixerGroup = item.group;
        }
    }

    public void Play(string name)
    {
        Sound s = FindSound(name);

        if (s == null || s.audioSource.isPlaying)
            return;
        
        s.audioSource.Play();
    }
    public void Pause(string name)
    {
        Sound s = FindSound(name);

        if (s == null)
            return;

        s.audioSource.Pause();
    }
    public void Stop(string name)
    {
        Sound s = FindSound(name);

        if (s == null)
            return;

        s.audioSource.Stop();
    }

    public void Mute(string name, bool value)
    {
        Sound s = FindSound(name);

        if (s == null)
            return;

        s.audioSource.mute = value;
    }

    public Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound \"" + name + "\" Not Found!");
            return null;
        }
        else
        {
            return s;
        }
    }

    // Set Volume
    public void Set_MasterVolume(float value)
    {
        float volume;
        if (value >= 0.1f) volume = -40 + (value - 0.1f) * (40 / 0.9f);
        else volume = -80 + value * (40 / 0.1f);
        volume = Math.Max(-80, Math.Min(0, volume));

        audioMixer.SetFloat("volume", volume);
    }
    public void Set_MusicVolume(float value)
    {
        float volume;
        if (value >= 0.1f) volume = -40 + (value - 0.1f) * (40 / 0.9f);
        else volume = -80 + value * (40 / 0.1f);
        volume = Math.Max(-80, Math.Min(0, volume));

        audioMixer.SetFloat("musicVolume", volume);
    }
    public void Set_SFXVolume(float value)
    {
        float volume;
        if (value >= 0.1f) volume = -40 + (value - 0.1f) * (40 / 0.9f);
        else volume = -80 + value * (40 / 0.1f);
        volume = Math.Max(-80, Math.Min(0, volume));

        audioMixer.SetFloat("sfxVolume", volume);
    }
    // Get Volume
    float Get_Volume(string groupMix)
    {
        audioMixer.GetFloat(groupMix, out float volume);
        volume = (volume + 80) / 80;
        return volume;
    }
    public float Get_MasterVolume()
    {
        return Get_Volume("volume");
    }
    public float Get_MusicVolume()
    {
        return Get_Volume("musicVolume");
    }
    public float Get_SFXVolume()
    {
        return Get_Volume("sfxVolume");
    }
}