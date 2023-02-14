using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public enum ExitDirection
{
    None,
    Left,
    Right,
    Up,
    Down
}
public class RoomInfo : MonoBehaviour
{
    [System.Serializable]
    public enum doortype
    {
        NotExit,
        HasExit,
        IsExit
    }


    [SerializeField] doortype Left, Right, Up, Down;
    [SerializeField] public ExitDirection exitDirection;
    [SerializeField] public ExitDirection hasExitDirection;

    public int X;  //World coordinate location
    public int Y;  //World coordinate location
    public string sceneName;

    [Header("TileMaps")]
    [SerializeField, Tooltip("Tiles that block player from moving")]
    public Tilemap Walls;
    [SerializeField, Tooltip("Tiles that are safe to walk on")]
    public Tilemap SafeTiles;
    [SerializeField, Tooltip("The ground tiles")]
    public Tilemap AlllGround;
    [SerializeField]
    public Tilemap DangerTile;
    [SerializeField]
    public Tilemap Doors;

    private void Start()
    {
        if(RoomController.instance != null)
        {
            RoomController.instance.LoadedRoom(this);
        }
    }

    public void SetTilemaps(Tilemap walls, Tilemap safe, Tilemap ground, Tilemap danger, Tilemap doors)
    {
        Walls = walls;
        SafeTiles = safe;
        AlllGround = ground;
        DangerTile = danger;
        Doors = doors;
    }
}
