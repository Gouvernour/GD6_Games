using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saves : MonoBehaviour
{
    public int DiggerHighScore;
    public static Saves instance;
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        DiggerHighScore = PlayerPrefs.GetInt("DiggerScore", 0);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("DiggerScore", DiggerHighScore);
    }
}
