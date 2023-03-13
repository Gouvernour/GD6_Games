using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioExternal : MonoBehaviour
{
    public static AudioExternal instance;
    public Sound sound;
    public AudioSource source;
    void Start()
    {
        instance = this;
        if(source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
        AudioManager.instance.AddToSounds(sound, source);
        source.Stop();
    }

    private void OnDestroy()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.RemoveFromSounds(sound, source);
    }
}
