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
    [SerializeField] float multiplier = 1000;
    float playerHeight = 1;
    float HeightIncrease = 1.4f;
    List<float> currentList = new List<float>();
    List<GameObject> points = new List<GameObject>();
    bool started = false;
    bool startedMusic = false;
    void Start()
    {
        //StartCoroutine(PrintData());
        SetPlayerHeight(GameObject.FindGameObjectWithTag("Player").transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (started && !startedMusic)
        {
            print("Music has started");
            BeatSequenzer.instance.hasStarted = true;
            startedMusic = true;
        }
    }
    bool Effectplaying = false;

    public void SetPlayerHeight(float height)
    {
        playerHeight = height;
    }

    public void EqualizeEffect()
    {
        
        Effectplaying = true;
        foreach (GameObject point in points)
            Destroy(point);
        int index = 0;
        currentList = sums;
        float height = playerHeight;
        foreach (float pos in currentList)
        {
            GameObject newPoint = Instantiate(point, new Vector3(0, height), Quaternion.identity);
            newPoint.GetComponent<Equalizer>().SetPoint(pos * multiplier, height);
            points.Add(newPoint);
            height += HeightIncrease;
            if(index == 0)
            {
                newPoint.GetComponent<Collider2D>().isTrigger = false;
                points.Remove(newPoint);
                playerHeight += HeightIncrease;
            }else
            {
                newPoint.SetActive(false);
            }
            index++;
        }
        
        Effectplaying = false;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (Effectplaying)
            return;
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
            if (dataSum > 0)
                started = true;
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
            //print(dat);
        }
        //print(dataSum);
        //print("End of this tone");
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
