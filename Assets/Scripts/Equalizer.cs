using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equalizer : MonoBehaviour
{
    float y = 0;
    float x = 0;
    Vector3 target = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3.MoveTowards(transform.position, target, 1000 * Time.deltaTime);
    }
    public void SetPoint(float Width, float Height)
    {
        y = Height;
        x = Width;
        target = new Vector3(x, y);
    }
}
