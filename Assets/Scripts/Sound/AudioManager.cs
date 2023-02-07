using System.Collections;
using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup MasterMixer;

    public AudioMixerGroup SFXMixer;
    public AudioMixerGroup MusicMixer;

    public Sound[] Audio;

    public Dictionary<SoundGroup, List<Sound>> Sounds;

    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        Sounds = new Dictionary<SoundGroup, List<Sound>>();
        foreach (Sound a in Audio)
        {
            bool keyExist = Sounds.ContainsKey(a.group);
            if(!keyExist)
            {
                Sounds.Add(a.group, new List<Sound>());
            }
            foreach (KeyValuePair<SoundGroup, List<Sound>> s in Sounds)
            {
                if(a.group == s.Key)
                {
                    Debug.Log("Added sound " + a.name + " to the group " + a.group);
                    s.Value.Add(a);
                    break;
                }
            }
        }
        DontDestroyOnLoad(gameObject);
        foreach (KeyValuePair<SoundGroup, List<Sound>> s in Sounds)
        {
            foreach (Sound a in s.Value)
            {
                a.source = gameObject.AddComponent<AudioSource>();
                a.source.clip = a.clip;
                a.source.volume = a.volume;
                if (a.volume < 0.0001f)
                {
                    Debug.Log("Volume to low");
                    a.volume = 0.0001f;
                    a.source.volume = 0.0001f;
                }
                a.source.pitch = a.Manualpitch;
                a.source.loop = a.loop;

                string _OutputMixer = a.name;
                try
                {
                    if(a.group != SoundGroup.Music)
                    {
                        a.source.outputAudioMixerGroup = SFXMixer.audioMixer.FindMatchingGroups(_OutputMixer)[0];   //Requires submixer with the name of the Audio clip to exist
                        a.source.outputAudioMixerGroup.audioMixer.SetFloat(a.name, Mathf.Log10(a.source.volume) * 20);
                    }else
                    {
                        a.source.outputAudioMixerGroup = MusicMixer.audioMixer.FindMatchingGroups(_OutputMixer)[0];   //Requires submixer with the name of the Audio clip to exist
                        a.source.outputAudioMixerGroup.audioMixer.SetFloat(a.name, Mathf.Log10(a.source.volume) * 20);
                    }
                }
                catch
                {
                    if(a.group != SoundGroup.Music)
                    {
                        Debug.LogWarning("SubMixer of SFXMixer with the name " + a.name + " does not exist");
                        a.source.outputAudioMixerGroup = SFXMixer;
                        Debug.Log("AudioSource " + a.name + " is using the SFXMixer instead");
                    }else
                    {
                        Debug.LogWarning("SubMixer of MusicMixer with the name " + a.name + " does not exist");
                        a.source.outputAudioMixerGroup = MusicMixer;
                        Debug.Log("AudioSource " + a.name + " is using the Musicixer instead");
                    }
                }
            }
            
        }
        
        SetSFXVol(.5f);
        SetMusicVol(.5f);
        SetMasterVol(.5f);
    }


    public void SetSFXVol(float value)  //Settings Change SFX Volume
    {
        foreach (KeyValuePair<SoundGroup, List<Sound>> s in Sounds)
        {
            if (s.Key != SoundGroup.Music)
            {
                foreach (Sound a in s.Value)
                {
                    a.source.outputAudioMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10(value * 2) * 20);

                }
            }
        }
    }

    public void SetMusicVol(float value)    //Settings Change Music Volume
    {
        foreach (KeyValuePair<SoundGroup, List<Sound>> s in Sounds)
        {
            if(s.Key == SoundGroup.Music)
            {
                foreach(Sound a in s.Value)
                {
                    a.source.outputAudioMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(value * 2) * 20);

                }
            }
        }
    }

    public void SetMasterVol(float value)   //Settings Change Master Volume
    {
        MasterMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(value * 2) * 20);
    }

    public void PlaySound(SoundGroup name)
    {
        foreach (KeyValuePair<SoundGroup, List<Sound>> s in Sounds)
        {
            if (name == s.Key)
            {
                if(s.Value.Count > 1)
                {
                    int index = Random.Range(0, s.Value.Count);
                    s.Value[index].source.Play();
                }else if(s.Value.Count == 1)
                {
                    s.Value[0].source.Play();
                }
                else
                    Debug.LogError(name + " have an associated group but have no existing sounds inserted to the group");
                return;
            }
        }
    }

    public void PlaySound(SoundGroup name, PitchVersion pitchRequest)
    {
        List<Sound> pitchedSounds = new List<Sound>();
        foreach (KeyValuePair<SoundGroup, List<Sound>> s in Sounds)
        {
            List<Sound> sounds;
            Sounds.TryGetValue(name, out sounds);
            foreach (Sound sound in sounds)
            {
                if (sound.pitch == pitchRequest)
                {
                    pitchedSounds.Add(sound);
                }
            }
            if (pitchedSounds.Count > 1)
            {
                int index = Random.Range(0, s.Value.Count);
                pitchedSounds[index].source.Play();
            }
            else if (s.Value.Count == 1)
            {
                pitchedSounds[0].source.Play();
            }
            else
                Debug.LogError(name + " have an associated group but have no existing sounds inserted to the group");
            return;
        }
    }
    public void PlayMusic(string name)
    {
        foreach (KeyValuePair<SoundGroup, List<Sound>> s in Sounds)
        {
            if (s.Key == SoundGroup.Music)
            {
                List<Sound> sounds;
                if (Sounds.TryGetValue(SoundGroup.Music, out sounds))
                {
                    foreach (Sound a in sounds)
                    {
                        a.source.Stop();
                    }
                    Sound song = sounds.Find(x => x.name == name);
                    song.source.Play();
                }
            }
        }
        if (Sounds.ContainsKey(SoundGroup.Music))
        {
            List<Sound> sounds;
            if(Sounds.TryGetValue(SoundGroup.Music, out sounds))
            {
                foreach (Sound s in sounds)
                {
                    s.source.Stop();
                }
                Sound song = sounds.Find(x => x.name == name);
                song.source.Play();
            }
        }
    }

    //public void PauseSFX()
    //{
    //    foreach (Sound s in sounds)
    //    {
    //        s.source.Pause();
    //        //Debug.Log("Sound Paused");
    //    }
    //}
    //public void PauseMusic()
    //{
    //    foreach (Sound s in music)
    //    {
    //        s.source.Pause();
    //        //Debug.Log("Sound Paused");
    //    }
    //}
    //
    //public void UnPauseSFX()
    //{
    //    foreach (Sound s in sounds)
    //    {
    //        s.source.UnPause();
    //        //Debug.Log("Sound Resumed");
    //    }
    //}
    //
    //public void UnPauseMusic()
    //{
    //    foreach (Sound s in music)
    //    {
    //        s.source.UnPause();
    //        //Debug.Log("Sound Resumed");
    //    }
    //}
    //
    //public void StopSFX(List<Sound> soundType)
    //{
    //    
    //    foreach (Sound s in soundType)
    //    {
    //        if (s != null)
    //            s.source.Stop();
    //    }
    //}
    //
    //public void StopMusic()
    //{
    //    foreach (Sound s in music)
    //    {
    //        s.source.Stop();
    //        Debug.Log("Sound Stopped");
    //    }
    //}
    //
    //void PlaySound(List<Sound> SoundList)     //name should be the first one in order of the list
    //{
    //    //Sound s = Array.Find(sounds, sound => sound.name == name);
    //    //int index = Array.IndexOf(sounds, s);
    //    //if (s == null)
    //    //{
    //    //    Debug.LogWarning("Sound: " + name + " not found!");
    //    //    return;
    //    //}
    //    //if (index == previousJumpArraySound || s.name == "Steps")
    //    //{
    //    //    Debug.Log("Retrying for non duplicate sound and preventing looping sounds");
    //    //    Debug.Log("Sound name: " + s.name);
    //    //    JumpSoundArray(name);
    //    //    return;
    //    //}
    //    //s.source.Play();
    //    //previousJumpArraySound = index;
    //}
}
