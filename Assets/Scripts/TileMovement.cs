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
    public Tilemap Walls;
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
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        cam = Camera.main;
        transform.SetParent(null);
        AudioManager.instance.PlaySound(SoundGroup.Misc);
    }

    public void Move(Vector2 Direction)
    {
        if(CanMove(Direction))
        {
            AudioManager.instance.PlaySound(SoundGroup.Move);
            transform.position += (Vector3)Direction;
            Vector3Int gridPosition = AlllGround.WorldToCell(transform.position);
            if(DangerTile.HasTile(gridPosition))
            {
                Player.instance.TakeDamage(wrongTileDmg);
                Debug.Log("Player took damage");
            }else if(SafeTiles.HasTile(gridPosition))
            {
                Debug.Log("Player hit safe tile");
            }if (Doors.HasTile(gridPosition))
            {
                Debug.Log("Go to next room");
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
        switch (Direction)
        {
            case Vector2 v when v.Equals(Vector2.up):
                cam.transform.position += cam.orthographicSize * (Vector3)Direction * 2;
                break;
            case Vector2 v when v.Equals(Vector2.left):
                cam.transform.position += (Vector3)new Vector2(-Walls.size.x, 0);
                break;
            case Vector2 v when v.Equals(Vector2.right):
                cam.transform.position += (Vector3)new Vector2(Walls.size.x, 0);
                break;
            case Vector2 v when v.Equals(Vector2.down):
                cam.transform.position += cam.orthographicSize * (Vector3)Direction * 2;
                break;
            default:
                break;
        }
        

    }
}
