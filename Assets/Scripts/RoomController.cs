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
    Dictionary<Vector2Int, RoomInfo> map;
    RoomInfo currentRoom;
    List<Vector2Int> LocationsCreated = new List<Vector2Int>();
    Vector2Int currentlocation = new Vector2Int();

    public static RoomController instance;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }
    void Start()
    {
        currentlocation.x = 1;
        currentlocation.y = 0;
        GetRoom();
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
        bool roomCorrect = false;
        RoomInfo newRoom = GetRoom();
        while(!roomCorrect)
        {
            switch (IncomingDir)
            {
                case direction.Left:
                    if(newRoom.exitDirection != RoomInfo.ExitDirection.Left)
                    {
                        roomCorrect = true;
                    }else
                        newRoom = GetRoom();
                    break;
                case direction.Right:
                    if(newRoom.exitDirection != RoomInfo.ExitDirection.Right)
                    {
                        roomCorrect = true;
                    }
                    else
                        newRoom = GetRoom();
                    break;
                case direction.Up:
                    if(newRoom.exitDirection != RoomInfo.ExitDirection.Up)
                    {
                        roomCorrect = true;
                    }
                    else
                        newRoom = GetRoom();
                    break;
                case direction.Down:
                    if(newRoom.exitDirection != RoomInfo.ExitDirection.Down)
                    {
                        roomCorrect = true;
                    }
                    else
                        newRoom = GetRoom();
                    break;
                default:
                    newRoom = GetRoom();
                    break;
            }
        
        }
    }
    public void LoadedRoom(RoomInfo roomDetails)
    {
        if(roomDetails.exitDirection == RoomInfo.ExitDirection.None || roomDetails.hasExitDirection == RoomInfo.ExitDirection.None)
        {
            //room is valid
            roomDetails.transform.position = new Vector3(currentlocation.x, currentlocation.y, 0);
        }else if(roomDetails.exitDirection != RoomInfo.ExitDirection.None)
        {

        }
    }

    RoomInfo GetRoom()
    {
        SceneManager.LoadScene("Room_1", LoadSceneMode.Additive);
        return null;
    }
}
