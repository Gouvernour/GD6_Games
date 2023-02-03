using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMovement : MonoBehaviour
{
    Tilemap Walls;
    Tilemap SafeTiles;
    Tilemap AlllGround;
    Tilemap DangerTile;

    Vector2 direction;
    [SerializeField] KeyCode UP;
    [SerializeField] KeyCode DOWN;
    [SerializeField] KeyCode LEFT;
    [SerializeField] KeyCode RIGHT;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(UP))
        {
            direction.Set(0, 1);
            Move(direction);
        }else if(Input.GetKeyDown(DOWN))
        {
            direction.Set(0, -1);
            Move(direction);

        }else if(Input.GetKeyDown(LEFT))
        {
            direction.Set(-1, 0);
            Move(direction);

        }else if(Input.GetKeyDown(RIGHT))
        {
            direction.Set(1, 0);
            Move(direction);
        }
    }

    private void Move(Vector2 Direction)
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
