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
    public string id;
    public Beat[] beats = new Beat[4];
}

public class BeatSequenzer : MonoBehaviour
{
    [SerializeField] List<Sequence> Sequence;
    [SerializeField] List<string> ID_ORDER;
}
