using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoFrogMovement : MonoBehaviour
{
    [SerializeField] bool Active = false;
    [SerializeField] bool Charging = false;
    [SerializeField] Rigidbody2D frog;

    [SerializeField] float maxPower = 10;
    [SerializeField] float currCharge = 0;
    [SerializeField] float BuildUpSpeed = 150;
    void Start()
    {
        if(!Active)
        {
            //frog.gravityScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Active = !Active;
            if(Active)
            {
                frog.bodyType = RigidbodyType2D.Dynamic;

            }
            else
            { 
                frog.bodyType = RigidbodyType2D.Kinematic;
                frog.velocity = Vector3.zero;
            }

        }
        if(!Active && frog.velocity.y == 0)
        {
            //Freeze velocity and Gravity
            frog.gravityScale = 0;
            return;
        }
        frog.gravityScale = 1;
        if(Input.GetKeyDown(KeyCode.D) && !Charging && frog.velocity.y == 0)
        {
            Charging = true;
            StartCoroutine(ChargingJump(1));
        }
        else if(Input.GetKeyDown(KeyCode.A) && !Charging && frog.velocity.y == 0)
        {
            Charging = true;
            StartCoroutine(ChargingJump(-1));
        }
        if(Input.GetKeyUp(KeyCode.D) && Charging)
        {
            Charging = false;
        }
        if(Input.GetKeyUp(KeyCode.A) && Charging)
        {
            Charging = false;
        }
    }

    IEnumerator ChargingJump(int multiplier)
    {
        while (Charging)
        {
            yield return null;
            if(currCharge < maxPower)
            {
                currCharge += Time.deltaTime * 100;
            }
        }
        if (currCharge < 15)
            currCharge = 0;
        if (currCharge > 100)
            currCharge -= 70;
        Vector2 dir = new Vector2(currCharge * multiplier * 2, maxPower - currCharge);
        frog.AddForce(dir);
        currCharge = 0;
    }
}