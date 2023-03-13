using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class Beat
{
    
    public string Takt;
    //int indexEdit = 0;  //for editor manipulation
    public bool One;
    public bool Two;
    public bool Three;
    public bool Four;



    private void InsertArrayitem()
    {
        Beat newSequence = new Beat();
        //beats.Insert(indexEdit, newSequence);
        //newSequence.beats
        //beats.Insert(indexEdit, newSequence);
    }
    
    private void AddBeat()
    {
    
    }
    
    private void SortIntArray()
    {
        //beats.Sort();
    }
    
    private void removeIndex()
    {
    
    }
}

[System.Serializable]
public class Sequence
{
    public string tag;

    public Beat[] beats;
    //[ContextMenuItem("remove IndexEdit index", "removeIndex")]
    //[ContextMenuItem("Insert new item at IndexEdit index", "InsertBeat")]
    //[ContextMenuItem("Insert new item at end", "AddBeat")]
    //[ContextMenuItem("Sort array", "SortIntArray")]
    //[SerializeReference]
    public int id;
    public float S_BPM = 60;

    
}

public class BeatSequenzer : MonoBehaviour
{
    

    [SerializeField] List<Sequence> sequence;
    [SerializeField] List<int> ID_ORDER;

    [SerializeField] float BPM = 92;
    [SerializeField] float RewindSpeed = 2;
    public float Speed = 1;
    BeatPlatform eq;

    private void Start()
    {
        Speed = 60 / BPM;
        SetTiming();
        //Setup beat from sequences
        eq = GameObject.FindGameObjectWithTag("Equalizer").GetComponent<BeatPlatform>();
        AudioManager.instance.PlayMusic("TwoOfUs");
        StartCoroutine(BeatNotes());
    }
    bool isRewinding = false;
    bool stoppedRewinding = false;
    float timeRewind = 0;
    float currentSpeed = 1;
    int totalBeats = 0;
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            if(isRewinding)
            {
                isRewinding = false;
                stoppedRewinding = true;
                currentSpeed = 1;
                AudioManager.instance.SetMusicPitch(currentSpeed, "TwoOfUs");
            }
            else
            {
                isRewinding = true;
                StopAllCoroutines();
                currentSpeed = -RewindSpeed;
                AudioManager.instance.SetMusicPitch(currentSpeed, "TwoOfUs");
            }    
        }
        if(stoppedRewinding)
        {
            Rewind(timeRewind);
            stoppedRewinding = false;
        }
        if(isRewinding)
        {
            timeRewind += Time.deltaTime * RewindSpeed;
        }
    }

    void Rewind(float timeRewinded)
    {
        currentTime -= timeRewinded;
        if(currentTime <= 0)
        {
            currentBeat = 0;
            currentTime = 0;

        }
        int beat = 0;
        foreach (float time in beatTimes)
        {
            if(currentTime < time)
            {
                currentBeat = beat--;
                break;
            }
            beat++;
        }

        timeRewind = 0f;
        StartCoroutine(BeatNotes());
    }

    [SerializeField] List<float> beatTimes = new List<float>();
    void SetTiming()
    {
        float time = 0f;
        foreach (int id in ID_ORDER)
        {
            foreach (Sequence s in sequence)
            {
                if(s.id == id)
                {
                    Speed = 60 / s.S_BPM;
                    foreach (Beat beat in s.beats)
                    {
                        if (beat.One == true)
                        {
                            beatTimes.Add(time);
                            totalBeats++;
                        }
                        time += Speed / 4;
                        if (beat.Two == true)
                        {
                            beatTimes.Add(time);
                            totalBeats++;
                        }
                        time += Speed / 4;
                        if (beat.Three == true)
                        {
                            beatTimes.Add(time);
                            totalBeats++;
                        }
                        time += Speed / 4;
                        if (beat.Four == true)
                        {
                            beatTimes.Add(time);
                            totalBeats++;
                        }
                        time += Speed / 4;
                    }
                }
            }
        }
    }

    public float currentTime = 0;
    public int currentBeat = 0;
    IEnumerator BeatNotes()
    {
        
        while(currentTime <= beatTimes[beatTimes.Count-1])
        {
            yield return new WaitForSeconds(beatTimes[currentBeat] - currentTime);
            currentTime = beatTimes[currentBeat];
            currentBeat++;
            AudioManager.instance.PlaySound(SoundGroup.Misc);
            eq.EqualizeEffect();
            if (currentBeat >= beatTimes.Count)
                break;
        }
        yield return null;
    }

    
}


//if (ID_ORDER.Count == 0 || sequence.Count == 0)
//    break;
//foreach(int id in ID_ORDER)
//{
//    foreach(Sequence s in sequence)
//    {
//        if(s.id == id)
//        {
//            Speed = 60 / s.S_BPM;
//            foreach(Beat beat in s.beats)
//            {
//                if(beat.One == true)
//                {
//                    AudioManager.instance.PlaySound(SoundGroup.Misc);
//                    //Replace with platform instantiation
//                }
//                yield return new WaitForSecondsRealtime(Speed/4);
//                if (beat.Two == true)
//                {
//                    AudioManager.instance.PlaySound(SoundGroup.Misc);
//                    //Replace with platform instantiation
//                }
//                yield return new WaitForSecondsRealtime(Speed/4);
//                if (beat.Three == true)
//                {
//                    AudioManager.instance.PlaySound(SoundGroup.Misc);
//                    //Replace with platform instantiation
//                }
//                yield return new WaitForSecondsRealtime(Speed/4);
//                if (beat.Four == true)
//                {
//                    AudioManager.instance.PlaySound(SoundGroup.Misc);
//                    //Replace with platform instantiation
//                }
//                yield return new WaitForSecondsRealtime(Speed/4);
//            }
//        }
//    }
//}