using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BeatPlatform : MonoBehaviour
{
    float[] Samples = new float[512];
    float currentPitch = 1;
    [SerializeField] Sound music;
    [SerializeField] GameObject point;
    [SerializeField] List<float> sums = new List<float>();
    void Start()
    {
        //StartCoroutine(PrintData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnAudioFilterRead(float[] data, int channels)
    {
        float dataSum = 0;
        int index = 1;
        sums.Clear();
        foreach (float d in data)
        {
            float sum = 0;
            if (d < 0)
            {
                sum = d * -1;
            }
            else
                sum = d;
            dataSum += sum / data.Length;
            if(index%16 == 0)
            {
                sums.Add(dataSum);
                dataSum = 0;
            }
            index++;
        }
        //if (dataSum < 0)
        //    dataSum *= -1;
        foreach (float dat in sums)
        {
            print(dat);
        }
        //print(dataSum);
        print("End of this tone");
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
