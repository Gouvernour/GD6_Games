using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeClimbCamera : MonoBehaviour
{
    public static TreeClimbCamera instance;
    [SerializeField] Camera m_Camera;
    [SerializeField] List<float> beats = new List<float>();
    [SerializeField] List<float> BPMs = new List<float>();
    [SerializeField] Vector3 StartPosition = new Vector3();
    float HeightIncrease = 1.4f;
    float CurrentHeight = 0;
    float Height = -1.95f;
    float Delay = 57.35f;
    [SerializeField] GameObject tree;
    bool showingTree = false;
    bool started = false;
    [SerializeField] GameObject Acorn;
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
        instance = this;
        showingTree = false;
        started = false;
    }

    private void Update()
    {
        if (Input.anyKey && !started)
        {
            started = true;
            showingTree = true;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(started && showingTree)
        {
            m_Camera.transform.position = Vector3.MoveTowards(transform.position, StartPosition, 30 * Time.deltaTime);
        }
        if(m_Camera.transform.position == StartPosition)
        {
            showingTree = false;
            BeatSequenzer.instance.WatchingIntro = false;
        }
        if(BeatSequenzer.instance.beatTimes.Count != 0 && beats.Count == 0)
        {
            foreach (float time in BeatSequenzer.instance.beatTimes)
                beats.Add(time);
            float timeDiff = 0;
            float currentTime = 0;
            for (int i = 0; i < beats.Count; i++)
            {
                beats[i] -= 0.3f;
                timeDiff = beats[i] - currentTime;
                //print(timeDiff);
                currentTime = beats[i];

                GameObject corn = Instantiate(Acorn, transform.position + new Vector3(0, beats[i] * 5 + (i*HeightIncrease) + HeightIncrease + Height, 0), Quaternion.identity);
                corn.transform.position = new Vector3(corn.transform.position.x, corn.transform.position.y, 0);
            }
            float CurrTime = 0 - beats[0];
            for (int i = 0; i < beats.Count-1; i++)
            {
                CurrTime = beats[i];
                BPMs.Add((60 / (beats[i+1] - CurrTime)));
            }
        }
        if(beats.Count > 0)
        {
            m_Camera.transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, CurrentHeight, -10), 5 * Time.deltaTime);
            //tree.transform.position = Vector3.MoveTowards(tree.transform.position, new Vector3(0, CurrentHeight, 0), 5 * Time.deltaTime);

        }
        //tree.transform.localScale = new Vector3(tree.transform.localScale.x, tree.transform.localScale.y + (5 * Time.deltaTime), tree.transform.localScale.z);
    }

    public void SetNewBeat()
    {
        CurrentHeight += HeightIncrease;
    }
}
