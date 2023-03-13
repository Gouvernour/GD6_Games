using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equalizer : MonoBehaviour
{
    float y = 0;
    float x = 0;
    [SerializeField] Vector3 target = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //print("moving");
        transform.localScale = new Vector3(target.x, 1, 1);
    }
    public void SetPoint(float Width, float Height)
    {
        y = Height;
        x = Width;
        if (Width < 3)
            x = 3;
        if(Width > 10)
            x = 10;
        target = new Vector3(x, y);
    }
}
