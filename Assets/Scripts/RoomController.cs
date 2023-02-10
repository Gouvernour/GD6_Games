using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum direction
{
    Left,
    Right,
    Up,
    Down
}

public class RoomController : MonoBehaviour
{
    [SerializeField]
    Scene[] scenes;
    Dictionary<Vector2Int, RoomInfo> map;
    RoomInfo currentRoom;
    List<Vector2Int> LocationsCreated = new List<Vector2Int>();
    Vector2Int currentlocation = new Vector2Int();
    Scene scene;
    void Start()
    {
        currentlocation.x = 0;
        currentlocation.y = 0;
    }

    public void newRooms(RoomInfo curr, direction IncomingDir)
    {
        switch (curr.exitDirection)
        {
            case RoomInfo.ExitDirection.None:
                break;
            case RoomInfo.ExitDirection.Left:
                if(IncomingDir == direction.Left)
                {
                    //Escaped
                }
                break;
            case RoomInfo.ExitDirection.Right:
                if(IncomingDir == direction.Right)
                {
                    //Escaped
                }
                break;
            case RoomInfo.ExitDirection.Up:
                if(IncomingDir == direction.Up)
                {
                    //Escaped
                }
                break;
            case RoomInfo.ExitDirection.Down:
                if(IncomingDir == direction.Down)
                {
                    //Escaped
                }
                break;
            default:
                break;
        }
        bool roomCorrect;
        //while(!roomCorrect)
        //{
        //    RoomInfo newRoom = GetRoom();
        //    switch (IncomingDir)
        //    {
        //        case direction.Left:
        //            if(newRoom.exitDirection != RoomInfo.ExitDirection.Left)
        //            {
        //                roomCorrect = true;
        //            }
        //            break;
        //        case direction.Right:
        //            if(newRoom.exitDirection != RoomInfo.ExitDirection.Right)
        //            {
        //                roomCorrect = true;
        //            }
        //            break;
        //        case direction.Up:
        //            if(newRoom.exitDirection != RoomInfo.ExitDirection.Up)
        //            {
        //                roomCorrect = true;
        //            }
        //            break;
        //        case direction.Down:
        //            if(newRoom.exitDirection != RoomInfo.ExitDirection.Down)
        //            {
        //                roomCorrect = true;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    RoomInfo GetRoom()
    {
        return null;
    }
}
