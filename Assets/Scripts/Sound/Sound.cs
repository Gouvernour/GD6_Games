using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundGroup
{
    Jump,
    Dig,
    CantDig,
    Move,
    NextLevel,
    Die,
    Enemy1Move,
    Enemy2Move,
    Music,
    HighScore,
    MovingButton,
    AcceptButton,
    BackButton
}

public enum PitchVersion
{
    First,
    Second,
    Third,
    Fourth
}

[System.Serializable]
public class Sound
{
    public string name;
    public SoundGroup group;
    public PitchVersion pitch;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float Manualpitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
