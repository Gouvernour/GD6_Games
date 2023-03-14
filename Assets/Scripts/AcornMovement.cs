using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornMovement : MonoBehaviour
{
    float speed = 5;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 100000, 0), speed * Time.deltaTime);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
