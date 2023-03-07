using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BeatPlatform : MonoBehaviour
{
    float[] Samples;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PrintData()
    {
        yield return null;
        AudioListener.GetOutputData(Samples, AudioManager.instance.MusicMixer.GetInstanceID());
    }
}
