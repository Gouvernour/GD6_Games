using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMovement : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [Header("TileMaps")]
    [SerializeField, Tooltip("Tiles that block player from moving")]
    Tilemap Walls;
    [SerializeField, Tooltip("Tiles that are safe to walk on")]
    Tilemap SafeTiles;
    [SerializeField, Tooltip("The ground tiles")]
    Tilemap AlllGround;
    [SerializeField]
    Tilemap DangerTile;
    [SerializeField]
    Tilemap Doors;

    [SerializeField] int wrongTileDmg = 1;
    static public TileMovement instance;
    
    void Awake()
    {
        instance = this;
    }

    public void Move(Vector2 Direction)
    {
        if(CanMove(Direction))
        {
            transform.position += (Vector3)Direction;
            Vector3Int gridPosition = AlllGround.WorldToCell(transform.position);
            if(DangerTile.HasTile(gridPosition))
            {
                Player.instance.TakeDamage(wrongTileDmg);
                Debug.Log("Player took damage");
            }else if(SafeTiles.HasTile(gridPosition))
            {
                Debug.Log("Player hit safe tile");
            }else if (Doors.HasTile(gridPosition))
            {
                MoveToRoom(Direction);
            }
        }
    }

    bool CanMove(Vector2 Direction)
    {
        Vector3Int gridPosition = AlllGround.WorldToCell(transform.position + (Vector3)Direction);
        if(Doors.HasTile(gridPosition))
        {
            return true;
        }
        if (!AlllGround.HasTile(gridPosition) || Walls.HasTile(gridPosition))
            return false;
        return true;
    }

    void MoveToRoom(Vector2 Direction)
    {
        transform.position += (Vector3)Direction;
        Vector3Int gridPosition = AlllGround.WorldToCell(transform.position);

    }
}
