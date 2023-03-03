using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Beat
{
    public bool One;
    public bool Two;
    public bool Three;
    public bool Four;
}

[System.Serializable]
public class Sequence
{
    public string tag;
    public int id;
    public Beat[] beats = new Beat[4];
}

public class BeatSequenzer : MonoBehaviour
{
    [SerializeField] List<Sequence> sequence;
    [SerializeField] List<int> ID_ORDER;

    [SerializeField] float BPM = 92;
    public float Speed = 1;

    private void Start()
    {
        Speed = 60 / BPM;
        StartCoroutine(BeatNotes());
    }

    IEnumerator BeatNotes()
    {
        while(true)
        {
            
            foreach(int id in ID_ORDER)
            {
                foreach(Sequence s in sequence)
                {
                    if(s.id == id)
                    {
                        foreach(Beat beat in s.beats)
                        {
                            if(beat.One == true)
                            {
                                print("One");
                                AudioManager.instance.PlaySound(SoundGroup.Misc);
                            }
                            yield return new WaitForSecondsRealtime(Speed/4);
                            if (beat.Two == true)
                            {
                                AudioManager.instance.PlaySound(SoundGroup.Misc);
                            }
                            yield return new WaitForSecondsRealtime(Speed/4);
                            if (beat.Three == true)
                            {
                                AudioManager.instance.PlaySound(SoundGroup.Misc);
                            }
                            yield return new WaitForSecondsRealtime(Speed/4);
                            if (beat.Four == true)
                            {
                                AudioManager.instance.PlaySound(SoundGroup.Misc);
                            }
                            yield return new WaitForSecondsRealtime(Speed/4);
                        }
                    }
                }
            }

        }
    }
}
