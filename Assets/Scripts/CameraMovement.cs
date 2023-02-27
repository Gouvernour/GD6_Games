using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera cam;
    GameObject player1;
    GameObject player2;
    bool p1 = true;
    void Start()
    {
        cam = Camera.main;
        player1 = GameObject.FindGameObjectWithTag("Frog1");
        player2 = GameObject.FindGameObjectWithTag("Frog2");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            p1 = !p1;
        }

        if(p1)
        {
            cam.transform.position = Vector3.MoveTowards(transform.position, new Vector3(player1.transform.position.x, transform.position.y, -10), Time.deltaTime);
        }else
        {
            cam.transform.position = Vector3.MoveTowards(transform.position, new Vector3(player2.transform.position.x, transform.position.y, -10), Time.deltaTime);

        }
    }
}
