using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMovement : MonoBehaviour
{
    [SerializeField]
    Tilemap Walls;
    [SerializeField]
    Tilemap SafeTiles;
    [SerializeField]
    Tilemap AlllGround;
    [SerializeField]
    Tilemap DangerTile;

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
            Vector3Int gridPosition = AlllGround.WorldToCell(transform.position + (Vector3)Direction);
            if(DangerTile.HasTile(gridPosition))
            {

            }else if(SafeTiles.HasTile(gridPosition))
            {

            }
        }
    }

    bool CanMove(Vector2 Direction)
    {
        Vector3Int gridPosition = AlllGround.WorldToCell(transform.position + (Vector3)Direction);
        if (!AlllGround.HasTile(gridPosition) || Walls.HasTile(gridPosition))
            return false;
        return true;
    }
}
