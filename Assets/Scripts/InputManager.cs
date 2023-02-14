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
    bool timeStarted = false;
    public bool Dead = false;
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
        if (Dead)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Dead = false;
                Player.instance.Regenerate(10000);
                RoomController.instance.ReloadGame();
            }
            return;
        }
        if (Input.GetKeyDown(UP))
        {
            if(!timeStarted)
            {
                timeStarted = true;
                RoomController.instance.StartTime();
            }else
            {
                RoomController.instance.ResetTime();
            }
            direction.Set(0, 1);
            TileMovement.instance.Move(direction);
        }
        else if (Input.GetKeyDown(DOWN))
        {
            if (!timeStarted)
            {
                timeStarted = true;
                RoomController.instance.StartTime();
            }
            else
            {
                RoomController.instance.ResetTime();
            }
            direction.Set(0, -1);
            TileMovement.instance.Move(direction);

        }
        else if (Input.GetKeyDown(LEFT))
        {
            if (!timeStarted)
            {
                timeStarted = true;
                RoomController.instance.StartTime();
            }
            else
            {
                RoomController.instance.ResetTime();
            }
            direction.Set(-1, 0);
            TileMovement.instance.Move(direction);

        }
        else if (Input.GetKeyDown(RIGHT))
        {
            if (!timeStarted)
            {
                timeStarted = true;
                RoomController.instance.StartTime();
            }
            else
            {
                RoomController.instance.ResetTime();
            }
            direction.Set(1, 0);
            TileMovement.instance.Move(direction);
        }
    }
}
