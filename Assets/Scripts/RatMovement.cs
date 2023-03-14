using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatMovement : MonoBehaviour
{
    public static RatMovement instance1;
    public static RatMovement instance2;
    [SerializeField] KeyCode UP;
    [SerializeField] Rigidbody2D Body;
    [SerializeField] float Force = 100;
    [SerializeField] public Animator anim;
    public Vector3 JumpOrigin = Vector3.zero;

    private void Start()
    {
        if (instance1 == null && UP == KeyCode.W)
            instance1 = this;
        else if (instance2 == null && UP == KeyCode.UpArrow)
            instance2 = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(UP))
        {
            Jump();
            anim.SetBool("jump", true);
            JumpOrigin = Body.transform.position;
        }
        //if(Body.transform.position.y == JumpOrigin.y)
        //    anim.SetBool("jump", false);
    }

    public void Jump()
    {
        Body.AddForce(Vector2.up * Force);
    }
}
