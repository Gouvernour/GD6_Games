using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BeatPlatform : MonoBehaviour
{
    [SerializeField] float[] Samples = new float[512];
    void Start()
    {
        //StartCoroutine(PrintData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //IEnumerator PrintData()
    //{
    //    AudioSource[] sources = AudioManager.instance.GetComponents<AudioSource>();
    //    foreach
    //    float totalInput = 0;
    //    while(true)
    //    {
    //        yield return null;
    //        foreach(float sample in Samples)
    //        {
    //            totalInput += sample;
    //        }
    //        print(totalInput);
    //        totalInput = 0;
    //    }
    //}
}
