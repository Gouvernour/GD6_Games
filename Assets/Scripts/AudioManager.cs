using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup MasterMixer;

    public AudioMixerGroup SFXMixer;
    public AudioMixerGroup MusicMixer;

    public Sound[] sounds;
    private int previousJumpArraySound = -1;
    private int[] CrystalJumpArraySound = { -1, -1, -1, -1 };

    public Sound[] music;

    public static AudioManager instance;
    private bool SoundCheckDelay;
    private bool InitialSetSFXVol = true;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            
            s.source.volume = s.volume;
            if (s.volume < 0.0001f)
            {
                Debug.Log("Volume to low");
                s.volume = 0.0001f;
                s.source.volume = 0.0001f;
            }
            //Debug.Log("Sound " + s.name + "'s volume is set to" + s.volume + " " + s.source.volume);
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            string _OutputMixer = s.name;
            try
            {
                s.source.outputAudioMixerGroup = SFXMixer.audioMixer.FindMatchingGroups(_OutputMixer)[0];   //Requires submixer with the name of the Audio clip to exist
                s.source.outputAudioMixerGroup.audioMixer.SetFloat(s.name, Mathf.Log10(s.source.volume) * 20);
            }
            catch
            {
                Debug.LogWarning("SubMixer of SFXMixer with the name " + s.name + " does not exist");
                s.source.outputAudioMixerGroup = SFXMixer;
                Debug.Log("AudioSource " + s.name + " is using the SFXMixer instead");
            }
        }
        
        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            if (s.volume < 0.0001f)
            {
                s.volume = 0.0001f;
                s.source.volume = 0.0001f;
            }
            //Debug.Log("Sound " + s.name + "'s volume is set to" + s.volume + " " + s.source.volume);
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            string _OutputMixer = s.name;
            try
            {
                s.source.outputAudioMixerGroup = MusicMixer.audioMixer.FindMatchingGroups(_OutputMixer)[0];   //Requires submixer with the name of the Audio clip to exist
                s.source.outputAudioMixerGroup.audioMixer.SetFloat(s.name, Mathf.Log10(s.source.volume) * 20);
            }
            catch
            {
                Debug.LogWarning("SubMixer of MusicMixer with the name " + s.name + " does not exist");
                s.source.outputAudioMixerGroup = MusicMixer;
                Debug.Log("AudioSource " + s.name + " is using the MusicMixer instead");
            }
        }
        SetSFXVol(.5f);
        SetMusicVol(.5f);
        SetMasterVol(.5f);
    }


    public void SetSFXVol(float value)
    {
        //foreach (Sound s in sounds)
        //{
        //    s.source.outputAudioMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(value * 2) * 20);
        //}
        //foreach (Sound s in AmeliteVoices)
        //{
        //    s.source.outputAudioMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(value * 2) * 20);
        //}
        //foreach (Sound s in BlobbyVoices)
        //{
        //    s.source.outputAudioMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(value * 2) * 20);
        //}
        //if (!SoundCheckDelay && !InitialSetSFXVol)
        //{
        //    StartCoroutine(SettingsCheckNewSFXVol());
        //}
        //else if (InitialSetSFXVol)
        //    InitialSetSFXVol = false;
    }

    public void SetMusicVol(float value)
    {
        foreach (Sound s in music)
        {
            s.source.outputAudioMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(value * 2) * 20);
        }
    }

    public void SetMasterVol(float value)
    {
        MasterMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(value * 2) * 20);
    }

    public void PlaySFX(string name)
    {
        //if (name == "Jump")
        //{
        //    JumpSoundArray(name);
        //    return;
        //}
        //else if (name == "Break")
        //{
        //    CrystalSoundArray(name);
        //    return;
        //}
        //Sound s = Array.Find(sounds, sound => sound.name == name);
        //if (s == null)
        //{
        //    Debug.LogWarning("Sound: " + name + " not found!");
        //    return;
        //}
        //s.source.Play();
    }

    public void PlayMusic(string name)
    {
        //Sound s = Array.Find(music, music => music.name == name);
        //if (s == null)
        //{
        //    Debug.LogWarning("Sound: " + name + " not found!");
        //    return;
        //}
        //if (name == "Credits")
        //{
        //    s.source.Stop();
        //}
        //s.source.PlayScheduled(.4);
    }

    public void PauseSFX()
    {
        foreach (Sound s in sounds)
        {
            s.source.Pause();
            //Debug.Log("Sound Paused");
        }
    }
    public void PauseMusic()
    {
        foreach (Sound s in music)
        {
            s.source.Pause();
            //Debug.Log("Sound Paused");
        }
    }

    public void UnPauseSFX()
    {
        foreach (Sound s in sounds)
        {
            s.source.UnPause();
            //Debug.Log("Sound Resumed");
        }
    }

    public void UnPauseMusic()
    {
        foreach (Sound s in music)
        {
            s.source.UnPause();
            //Debug.Log("Sound Resumed");
        }
    }

    public void StopSFX(List<Sound> soundType)
    {
        
        foreach (Sound s in soundType)
        {
            if (s != null)
                s.source.Stop();
        }
    }

    public void StopMusic()
    {
        foreach (Sound s in music)
        {
            s.source.Stop();
            Debug.Log("Sound Stopped");
        }
    }

    void PlaySound(List<Sound> SoundList)     //name should be the first one in order of the list
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);
        //int index = Array.IndexOf(sounds, s);
        //if (s == null)
        //{
        //    Debug.LogWarning("Sound: " + name + " not found!");
        //    return;
        //}
        //if (index == previousJumpArraySound || s.name == "Steps")
        //{
        //    Debug.Log("Retrying for non duplicate sound and preventing looping sounds");
        //    Debug.Log("Sound name: " + s.name);
        //    JumpSoundArray(name);
        //    return;
        //}
        //s.source.Play();
        //previousJumpArraySound = index;
    }
}
