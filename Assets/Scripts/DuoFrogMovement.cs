using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoFrogMovement : MonoBehaviour
{
    [SerializeField] public bool Active = false;
    [SerializeField] bool Charging = false;
    [SerializeField] bool falling = false;
    [SerializeField] Rigidbody2D frog;

    [SerializeField] float maxPower = 10;
    [SerializeField] float currCharge = 0;
    [SerializeField] float BuildUpSpeed = 150;

    [SerializeField] GameObject Tounge;
    [SerializeField] GameObject CurrTounge;
    [SerializeField] Animator anim;
    public GameObject OtherFrog;
    void Start()
    {
        //anim.SetBool("Active", true);
        if(Active)
        {
            OtherFrog = GameObject.FindWithTag("Frog2");
            anim.SetBool("Active", true);
        }
        else
        {
            OtherFrog = GameObject.FindWithTag("Frog1");

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
                anim.SetBool("Active", true);
            }
            else
            { 
                frog.bodyType = RigidbodyType2D.Kinematic;
                frog.velocity = Vector3.zero;
                anim.SetBool("Active", false);
            }

        }
        if(!Active && frog.velocity.y == 0)
        {
            //Freeze velocity and Gravity
            anim.SetBool("Active", false);
            frog.gravityScale = 0;
            return;
        }else
        {
            anim.SetBool("Active", true);
        }
        if (Active && frog.velocity.y < 0)
        {
            anim.SetBool("Active", true);
            //anim.SetBool("Grounded", true);
            anim.SetBool("Falling", true);
            //anim.SetBool("Charge", false);
            falling = true;
        }
        if(falling == true && frog.velocity.y == 0)
        {
            anim.SetBool("Grounded", true);
            anim.SetBool("Jump", false);
            falling = false;
        }
        frog.gravityScale = 1;
        if(Input.GetKeyDown(KeyCode.D) && !Charging && frog.velocity.y == 0)
        {
            Charging = true;
            anim.SetBool("Jump", false);
            StartCoroutine(ChargingJump(1));
            anim.SetBool("Charge", true);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(Input.GetKeyDown(KeyCode.A) && !Charging && frog.velocity.y == 0)
        {
            Charging = true;
            anim.SetBool("Jump", false);
            StartCoroutine(ChargingJump(-1));
            anim.SetBool("Charge", true);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if(Input.GetKeyUp(KeyCode.D) && Charging)
        {
            Charging = false;
            anim.SetBool("Jump", true);
            anim.SetBool("Grounded", false);
            anim.SetBool("Charge", false);
        }
        if(Input.GetKeyUp(KeyCode.A) && Charging)
        {
            Charging = false;
            anim.SetBool("Jump", true);
            anim.SetBool("Grounded", false);
            anim.SetBool("Charge", false);
        }
        if(Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Lick", true);
            //anim.SetBool("Charge", false);
            Lick();
        }if(Input.GetMouseButtonUp(0))
        {
            anim.SetBool("Lick", false);
            //anim.SetBool("Charge", false);
            StopLick();
        }
        
        anim.SetBool("Active", true);
    }

    void Lick()
    {
        CurrTounge = Instantiate(Tounge, transform.position, Quaternion.identity);

        CurrTounge.GetComponent<Tounge>().Destination = OtherFrog.transform.position;
    }
    
    void StopLick()
    {
        Destroy(CurrTounge);
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
        Vector2 dir = new Vector2(currCharge * multiplier * 2, (maxPower - currCharge) * 2);
        frog.AddForce(dir);
        currCharge = 0;
    }
}
