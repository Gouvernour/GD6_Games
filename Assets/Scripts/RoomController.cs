using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum direction
{
    start,
    Left,
    Right,
    Up,
    Down
}

public class RoomController : MonoBehaviour
{
    [SerializeField]
    MazeSupplementAudio mazeRooms;
    [SerializeField]
    Dictionary<Vector2Int, RoomInfo> map = new Dictionary<Vector2Int, RoomInfo>();
    [SerializeField]
    RoomInfo currentRoom;
    [SerializeField]
    List<RoomInfo> RoomsUnused = new List<RoomInfo>();
    [SerializeField]
    Vector2Int currentlocation = new Vector2Int();

    [SerializeField]
    private List<string> scenes = new List<string>();
    public static RoomController instance;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            currentlocation.x = 0;
            currentlocation.y = 0;
            print(currentlocation);
            mazeRooms = MazeSupplementAudio.instance;
            foreach (string scene in mazeRooms.BirdsRooms)
                scenes.Add(scene);
            foreach (string scene in mazeRooms.WaterRooms)
                scenes.Add(scene);
            foreach (string scene in mazeRooms.WindRooms)
                scenes.Add(scene);
            foreach (string scene in mazeRooms.NoAmbianceRooms)
                scenes.Add(scene);
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //mazeRooms = MazeSupplementAudio.instance;
        //foreach(string scene in mazeRooms.BirdsRooms)
        //    scenes.Add(scene);
        //foreach(string scene in mazeRooms.WaterRooms)
        //    scenes.Add(scene);
        //foreach(string scene in mazeRooms.WindRooms)
        //    scenes.Add(scene);
        //foreach(string scene in mazeRooms.NoAmbianceRooms)
        //    scenes.Add(scene);

    }

    public RoomInfo newRooms(direction IncomingDir)
    {
        switch (IncomingDir)
        {
            case direction.start:
                break;
            case direction.Left:
                currentlocation.x--;
                break;
            case direction.Right:
                currentlocation.x++;
                break;
            case direction.Up:
                currentlocation.y++;
                break;
            case direction.Down:
                currentlocation.y--;
                break;
            default:
                break;
        }
        foreach (KeyValuePair<Vector2Int, RoomInfo> piece in map)
        {
            if(currentlocation == piece.Key)
            {
                currentRoom = piece.Value;
                foreach (string scene in mazeRooms.BirdsRooms)
                {
                    if(scene == piece.Value.sceneName)
                    {
                        AudioManager.instance.PlayMusic("Birds");
                    }
                }
                foreach (string scene in mazeRooms.WaterRooms)
                {
                    if (scene == piece.Value.sceneName)
                    {
                        AudioManager.instance.PlayMusic("Water");
                    }
                }
                foreach (string scene in mazeRooms.WindRooms)
                {
                    if (scene == piece.Value.sceneName)
                    {
                        AudioManager.instance.PlayMusic("Wind");
                    }
                }
                foreach (string scene in mazeRooms.NoAmbianceRooms)
                {
                    if (scene == piece.Value.sceneName)
                    {
                        AudioManager.instance.StopMusic();
                    }
                }
                StartCoroutine(SpawningRooms());
                return piece.Value;
            }
        }
        print("Now this should not have happened");
        return null;
    }
    string AddingScene;
    public void LoadedRoom(RoomInfo roomDetails)
    {
        if(scenes.Count == 0)
        {
            foreach (string scene in mazeRooms.BirdsRooms)
                scenes.Add(scene);
            foreach (string scene in mazeRooms.WaterRooms)
                scenes.Add(scene);
            foreach (string scene in mazeRooms.WindRooms)
                scenes.Add(scene);
            foreach (string scene in mazeRooms.NoAmbianceRooms)
                scenes.Add(scene);
        }
        print(map.Count + "MapCount");
        if(map.Count == 0)
        {
            roomDetails.X = currentlocation.x;
            roomDetails.Y = currentlocation.y;
            roomDetails.sceneName = "Room_1";
            map.Add(currentlocation, roomDetails);
            print(map.Count + "MapCount");
            loadingroom = false;
            currentRoom = roomDetails;
            incomingDir = ExitDirection.None;
            StartCoroutine(SpawningRooms());
        }
        else if(roomDetails.exitDirection == incomingDir || (currentRoom.hasExitDirection == newRoomDirection && roomDetails.exitDirection == ExitDirection.None))
        {
            roomDetails.sceneName = AddingScene;
            roomDetails.transform.position = new Vector3(currentlocation.x * TileMovement.instance.Walls.size.x * 10000, currentlocation.y * TileMovement.instance.Walls.size.y, 0);
            RoomsUnused.Add(roomDetails);
            int index = Random.Range(0, scenes.Count);
            AddingScene = scenes[index];
            SceneManager.LoadScene(scenes[index], LoadSceneMode.Additive);
            return;
        }
        else if(roomDetails.exitDirection != ExitDirection.None && currentRoom.hasExitDirection == newRoomDirection && incomingDir != roomDetails.exitDirection)
        {
            //room is valid
            roomDetails.transform.position = new Vector3(currentlocation.x * TileMovement.instance.Walls.size.x, currentlocation.y * TileMovement.instance.Walls.size.y, 0);
            roomDetails.X = currentlocation.x;
            roomDetails.Y = currentlocation.y;
            roomDetails.sceneName = AddingScene;
            map.Add(currentlocation, roomDetails);
            loadingroom = false;
            //print(currentlocation);
        }
        else if(roomDetails.exitDirection == ExitDirection.None && currentRoom.hasExitDirection != incomingDir && incomingDir != ExitDirection.None)
        {
            roomDetails.transform.position = new Vector3(currentlocation.x * TileMovement.instance.Walls.size.x, currentlocation.y * TileMovement.instance.Walls.size.y, 0);
            roomDetails.X = currentlocation.x;
            roomDetails.Y = currentlocation.y;
            roomDetails.sceneName = AddingScene;
            map.Add(currentlocation, roomDetails);
            loadingroom = false;
        }
        else
        {
            roomDetails.transform.position = new Vector3(currentlocation.x * TileMovement.instance.Walls.size.x, currentlocation.y * TileMovement.instance.Walls.size.y, 0);
            roomDetails.X = currentlocation.x;
            roomDetails.Y = currentlocation.y;
            roomDetails.sceneName = AddingScene;
            map.Add(currentlocation, roomDetails);
            loadingroom = false;
        }
    }

    ExitDirection incomingDir;
    ExitDirection newRoomDirection;
    void GetRoom()
    {
        foreach (KeyValuePair<Vector2Int, RoomInfo> location in map)
        {
            if(location.Key == currentlocation)
            {
                loadingroom = false;
                print("Room Already Exist");
                return;
            }
        }
        string spawnRoom = "";
        if(RoomsUnused.Count > 0)
        {
            foreach (RoomInfo room in RoomsUnused)
            {
                if (room.exitDirection != incomingDir && incomingDir != ExitDirection.None)
                {
                    if(currentRoom.exitDirection != ExitDirection.None)
                    {
                        if(room.hasExitDirection == incomingDir && currentRoom.exitDirection != ExitDirection.None)
                        {
                            spawnRoom = room.sceneName;
                            room.transform.position = new Vector3(currentlocation.x * TileMovement.instance.Walls.size.x, currentlocation.y * TileMovement.instance.Walls.size.y, 0);
                            map.Add(currentlocation, room);
                            room.X = currentlocation.x;
                            room.Y = currentlocation.y;
                            loadingroom = false;
                            return;
                        }

                    }else if(currentRoom.hasExitDirection == incomingDir && room.exitDirection != ExitDirection.None)
                    {
                        spawnRoom = room.sceneName;
                        room.transform.position = new Vector3(currentlocation.x * TileMovement.instance.Walls.size.x, currentlocation.y * TileMovement.instance.Walls.size.y, 0);
                        map.Add(currentlocation, room);
                        room.X = currentlocation.x;
                        room.Y = currentlocation.y;
                        loadingroom = false;
                        return;
                    }
                }else if(room.exitDirection == incomingDir && incomingDir != ExitDirection.None)
                {
                    
                }
                else
                {
                    spawnRoom = room.sceneName;
                    room.transform.position = new Vector3(currentlocation.x * TileMovement.instance.Walls.size.x, currentlocation.y * TileMovement.instance.Walls.size.y, 0);
                    map.Add(currentlocation, room);
                    room.X = currentlocation.x;
                    room.Y = currentlocation.y;
                    loadingroom = false;
                    return;
                }
            }
        }else
        {
            if(scenes.Count == 0)
            {
                foreach (string scene in mazeRooms.BirdsRooms)
                    scenes.Add(scene);
                foreach (string scene in mazeRooms.WaterRooms)
                    scenes.Add(scene);
                foreach (string scene in mazeRooms.WindRooms)
                    scenes.Add(scene);
                foreach (string scene in mazeRooms.NoAmbianceRooms)
                    scenes.Add(scene);
            }
            print(scenes.Count + "sceneCount");
            int index = Random.Range(0, scenes.Count);
            AddingScene = scenes[index];
            SceneManager.LoadScene(scenes[index], LoadSceneMode.Additive);
            print("Loading new Scene");
            return;
        }

    }

    [SerializeField] bool newRoomsCreating = false;
    [SerializeField] bool loadingroom = false;
    [SerializeField] int roomsCreated = 0;
    IEnumerator SpawningRooms()
    {
        Vector2Int currRoomLocation = currentlocation;
        newRoomsCreating = true;
        roomsCreated = 0;
        while (newRoomsCreating)
        {
            while(loadingroom)
            {

                yield return null;
            }
            currentlocation = currRoomLocation;
            switch (roomsCreated)
            {
                case 0:
                    currentlocation.x = currRoomLocation.x + 1;
                    roomsCreated++;
                    loadingroom = true;
                    incomingDir = ExitDirection.Left;
                    newRoomDirection = ExitDirection.Right;
                    GetRoom();
                    break;
                case 1:
                    currentlocation.x = currRoomLocation.x - 1;
                    incomingDir = ExitDirection.Right;
                    newRoomDirection = ExitDirection.Left;
                    roomsCreated++;
                    loadingroom = true;
                    GetRoom();
                    break;
                case 2:
                    currentlocation.y = currRoomLocation.y + 1;
                    incomingDir = ExitDirection.Down;
                    newRoomDirection = ExitDirection.Up;
                    roomsCreated++;
                    loadingroom = true;
                    GetRoom();
                    break;
                case 3:
                    currentlocation.y = currRoomLocation.y - 1;
                    incomingDir = ExitDirection.Up;
                    newRoomDirection = ExitDirection.Down;
                    roomsCreated++;
                    loadingroom = true;
                    GetRoom();
                    break;
                case 4:
                    newRoomsCreating = false;               //When all four rooms exist stop creating rooms
                    currentlocation = currRoomLocation;
                    print("All rooms created");
                    break;
                default:
                    break;
            }
        }
    }

    public void StartTime()
    {
        StartCoroutine(timeRunning());
        StartCoroutine(gameTimer());
    }
    public void ResetTime()
    {
        StopAllCoroutines();
        StartCoroutine(gameTimer());
        StartCoroutine(timeRunning());
    }
    float timeToMove = 7f;
    public IEnumerator timeRunning()
    {
        yield return new WaitForSeconds(timeToMove);
        Player.instance.TakeDamage(1);
    }
    public float Timeleft = 180;
    float RoundTime = 180;
    public IEnumerator gameTimer()
    {
        while(Timeleft > 0)
        {
            Timeleft -= Time.deltaTime;
            print("RunningTimer");
            yield return null;
        }
        //End Game
        Player.instance.ManualWon();
        Timeleft = RoundTime;
        AudioManager.instance.StopSFX(SoundGroup.Misc);
        StopAllCoroutines();
    }

    public void ReloadGame()
    {
        map.Clear();
        RoomsUnused.Clear();
        StopAllCoroutines();
        Timeleft = RoundTime;
        SceneManager.LoadScene("IDNRM", LoadSceneMode.Single);
    }
}
