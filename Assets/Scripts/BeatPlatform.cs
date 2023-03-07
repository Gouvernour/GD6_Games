using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BeatPlatform : MonoBehaviour
{
    [SerializeField] float[] Samples = new float[512];
    float currentPitch = 1;
    [SerializeField] Sound music;
    void Start()
    {
        //StartCoroutine(PrintData());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            if (currentPitch < 0)
                currentPitch = 1;
            else
                currentPitch = -1.5f;
            AudioManager.instance.SetMusicPitch(currentPitch, "TwoOfUs");
        }
    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        float dataSum = 0;
        foreach (float d in data)
            dataSum += d / data.Length;
        if (dataSum < 0)
            dataSum *= -1;
        print(dataSum);
    }


    //IEnumerator PrintData()
    //{
    //    
    //    AudioSource[] sources = AudioManager.instance.GetComponents<AudioSource>();
    //    foreach (AudioSource s in sources)
    //    {
    //        print(s.clip.name);
    //        if(s.clip.name == "Just the Two of Us")
    //            music = s;
    //    }
    //    float totalInput = 0;
    //    while(true)
    //    {
    //        yield return null;
    //        music.outputAudioMixerGroup.audioMixer.GetFloat("TwoOfUs", out totalInput);
    //        //foreach(float sample in Samples)
    //        //{
    //        //    totalInput += sample;
    //        //}
    //        totalInput = 
    //        print(totalInput);
    //        //totalInput = 0;
    //    }
    //    yield return null;
    //}
}
