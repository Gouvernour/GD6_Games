using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundGrid : MonoBehaviour
{
    public GameObject Ladder;
    public GameObject Empty;
    int worldSizeY = 9;
    int worldSizeX = 16;
    void Start()
    {
        for(int i = 0; i < worldSizeY; i++)
        {
            for(int j = 0; j < worldSizeX; j++)
            {
                if(i == 0)
                {
                    GameObject obj = GameObject.Instantiate(Empty);
                    obj.transform.SetParent(transform);
                    obj.GetComponent<Image>().enabled = false;
                }
                else if(j % 2 == 0)
                {
                    GameObject obj = GameObject.Instantiate(Ladder);
                    obj.transform.SetParent(transform);
                }else
                {
                    GameObject obj = GameObject.Instantiate(Empty);
                    obj.transform.SetParent(transform);
                    obj.GetComponent<Image>().enabled = false;
                }
            }
        }
    }
}
