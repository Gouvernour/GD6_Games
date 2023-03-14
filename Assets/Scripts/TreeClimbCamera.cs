using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeClimbCamera : MonoBehaviour
{
    public static TreeClimbCamera instance;
    [SerializeField] Camera m_Camera;
    [SerializeField] List<float> beats = new List<float>();
    [SerializeField] List<float> BPMs = new List<float>();
    float HeightIncrease = 1.4f;
    float CurrentHeight = -3;
    float Delay = 57.35f;

    [SerializeField] GameObject Acorn;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
        instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(BeatSequenzer.instance.beatTimes != null && beats.Count == 0)
        {
            foreach (float time in BeatSequenzer.instance.beatTimes)
                beats.Add(time);
            float timeDiff = 0;
            float currentTime = 0;
            for (int i = 0; i < beats.Count; i++)
            {
                beats[i] -= 0.3f;
                timeDiff = beats[i] - currentTime;
                print(timeDiff);
                currentTime = beats[i];

                GameObject corn = Instantiate(Acorn, transform.position + new Vector3(0, beats[i] * 3 + (i*HeightIncrease) + HeightIncrease, 0), Quaternion.identity);
                corn.transform.position = new Vector3(corn.transform.position.x, corn.transform.position.y, 0);
            }
            float CurrTime = 0 - beats[0];
            for (int i = 0; i < beats.Count-1; i++)
            {
                CurrTime = beats[i];
                BPMs.Add((60 / (beats[i+1] - CurrTime)));
            }
        }
        m_Camera.transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, CurrentHeight, -10), 5 * Time.deltaTime);
    }

    public void SetNewBeat()
    {
        CurrentHeight += HeightIncrease;
    }
}
