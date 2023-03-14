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

    private void Start()
    {
        if (instance1 == null)
            instance1 = this;
        else if (instance2 == null)
            instance2 = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(UP))
        {
            Jump();
        }
    }

    public void Jump()
    {
        Body.AddForce(Vector2.up * Force);
    }
}
