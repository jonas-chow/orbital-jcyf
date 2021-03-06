using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public Sound[] sounds;
    
    // Use this for initialization
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
            //s.source.pitch = s.pitch;
        }
    }

    void Start ()
    {
        Play("MenuTheme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void SetBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat("BGM", volume);
        Sound[] soundArray = Array.FindAll(sounds, sound => !sound.soundEffect);
        foreach (Sound s in soundArray)
        {
            s.source.volume = volume;
        }
    }

    public void SetSoundEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("SE", volume);
        Sound[] soundArray = Array.FindAll(sounds, sound => sound.soundEffect);
        foreach (Sound s in soundArray)
        {
            s.source.volume = volume;
        }
    }
}
