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

            for (int i = 0; i < beats.Count; i++)
            {
                beats[i] -= 0.3f;
            }
            float CurrTime = beats[0];
            foreach (float beat in beats)
            {
                CurrTime += beat;
                BPMs.Add((60 / (beat- CurrTime)) *4);
            }
        }
        m_Camera.transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, CurrentHeight, -10), 5 * Time.deltaTime);
    }

    public void SetNewBeat()
    {
        CurrentHeight += HeightIncrease;
    }
}
