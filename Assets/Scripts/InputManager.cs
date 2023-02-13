using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Vector2 direction;
    [SerializeField] KeyCode UP;
    [SerializeField] KeyCode DOWN;
    [SerializeField] KeyCode LEFT;
    [SerializeField] KeyCode RIGHT;
    public static InputManager instance;
    private InputManager destroythis;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            destroythis = gameObject.GetComponent<InputManager>();
            Destroy(destroythis);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(UP))
        {
            direction.Set(0, 1);
            TileMovement.instance.Move(direction);
        }
        else if (Input.GetKeyDown(DOWN))
        {
            direction.Set(0, -1);
            TileMovement.instance.Move(direction);

        }
        else if (Input.GetKeyDown(LEFT))
        {
            direction.Set(-1, 0);
            TileMovement.instance.Move(direction);

        }
        else if (Input.GetKeyDown(RIGHT))
        {
            direction.Set(1, 0);
            TileMovement.instance.Move(direction);
        }
    }
}
