using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSupplementAudio : MonoBehaviour
{
    public string[] BirdsRooms;
    public string[] WaterRooms;
    public string[] WindRooms;
    public string[] NoAmbianceRooms;

    public static MazeSupplementAudio instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
