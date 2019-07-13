using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager_S : MonoBehaviour
{
    public Sound_S[] sounds;


    public static AudioManager_S self;


    // Start is called before the first frame update
    void Awake()
    {
        if (self == null)
        {
            self = this;

            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
            GameController_S.maintainedScripts.Add(gameObject);
        }
        else Destroy(gameObject);


        foreach (Sound_S sound in sounds) // Creates audio sources for all sounds in the array
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }


    // ** Methods **
    public void Play(string name)
    {
        Sound_S s = Array.Find(sounds, sound => sound.name == name); // Uses System to find the sound, fast but somewhat slow at runtime
        s.source.Play();
    }


    public void Stop(string name)
    {
        Sound_S s = Array.Find(sounds, sound => sound.name == name); // Uses System to find the sound, fast but somewhat slow at runtime
        s.source.Stop();
    }


    public void StopAllUnnecessaryLoops()
    {
        Sound_S s = Array.Find(sounds, sound => sound.name == "Footstep Loop");
        s.source.Stop();
    }


    // ** Get Methods **
    public static AudioManager_S local()
    {
        return self;
    }
}
