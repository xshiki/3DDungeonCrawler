using UnityEngine.Audio;
using UnityEngine;
using System;

using System.Media;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Sound[] loopSounds;
    public Sound[] bgm;
    public Sound[] footSteps;
    public AudioMixerGroup BGMMixer, SFXMixer;
    private void Awake()
    {
     
   
        foreach(Sound s in sounds)
        {
            s.audiosource = gameObject.AddComponent<AudioSource>();
            s.audiosource.outputAudioMixerGroup = SFXMixer;
            s.audiosource.clip = s.audioClip;
            s.audiosource.loop = s.loop;
        }

        foreach (Sound s in loopSounds)
        {
            s.audiosource = gameObject.AddComponent<AudioSource>();
            s.audiosource.outputAudioMixerGroup = SFXMixer;
            s.audiosource.clip = s.audioClip;
            s.audiosource.loop = s.loop;
            s.audiosource.enabled = false;
            s.audiosource.playOnAwake = true;
            
        }

        foreach (Sound s in bgm)
        {
            s.audiosource = gameObject.AddComponent<AudioSource>();
            s.audiosource.outputAudioMixerGroup = BGMMixer;
            s.audiosource.clip = s.audioClip;
            s.audiosource.loop = s.loop;
        }

        foreach (Sound s in footSteps)
        {
            s.audiosource = gameObject.AddComponent<AudioSource>();
            s.audiosource.outputAudioMixerGroup = SFXMixer;
            s.audiosource.clip = s.audioClip;
            s.audiosource.loop = s.loop;
        }
    }


    //Play AudioClip by name
    public void Play(string name)
    {   
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if(sound == null) {
            Debug.LogWarning("Sound not found!");
            return; }

        sound.audiosource.volume = sound.volume;
        sound.audiosource.pitch = sound.pitch;
        sound.audiosource.Play();

    }
    public void PlayRandomized(string name, float volumeBegin, float volumeStart, float pitchBegin, float pitchEnd)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found!");
            return;
        }

        sound.audiosource.volume = Random.Range(volumeBegin, volumeStart);
        sound.audiosource.pitch = Random.Range(pitchBegin, pitchEnd);
        sound.audiosource.Play();

    }
    public void Play(string name, float volume, float pitch)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found!");
            return;
        }

        sound.audiosource.volume = volume;
        sound.audiosource.pitch = pitch;
        sound.audiosource.Play();

    }


    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.audiosource.Stop();
    }


    //Used for SFX that are played on loop
    public void EnableAudioSource(string name)
    {
        Sound sound = Array.Find(loopSounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found!");
            return;
        }

        sound.audiosource.volume = sound.volume;
        sound.audiosource.pitch = sound.pitch;
        sound.audiosource.enabled = true;
    }

    public void DisableAudioSource(string name)
    {
        Sound sound = Array.Find(loopSounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found!");
            return;
        }

        sound.audiosource.volume = sound.volume;
        sound.audiosource.pitch = sound.pitch;
        sound.audiosource.enabled = false;
    }

    public void PlayFootSteps()
    {
        if (footSteps.Length == 0)
        {
            Debug.LogWarning("No footstep sounds found!");
            return;
        }
        foreach (Sound s in footSteps)
        {
           if(s.audiosource.isPlaying)
            {
                return;
            }
        }
        Debug.Log(footSteps.Length);
        Sound sound = footSteps[Random.Range(0, footSteps.Length)];
        sound.audiosource.pitch = Random.Range(0.9f, 1.05f);
        sound.audiosource.Play();
    }
    public void AnimationEvent_FootStep()
    {
        //Play("Footsteps");
    }
}
