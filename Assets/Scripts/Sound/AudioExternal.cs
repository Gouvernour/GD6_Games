using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioExternal : MonoBehaviour
{
    public Sound sound;
    public AudioSource source;
    void Start()
    {
        if(source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
        }
        AudioManager.instance.AddToSounds(sound, source);
    }

    private void OnDestroy()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.RemoveFromSounds(sound, source);
    }
}
