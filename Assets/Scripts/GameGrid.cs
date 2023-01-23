using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum objectType
{
    Ladder,
    Player,
    Earth,
    Metal
}
[System.Serializable]
public class GridLocation
{
    public string name;
    public objectType type;
    public int blockLife;
    public GameObject objRef;
    public int posY;
    public int posX;
    public Image sprite;
}

public class GameGrid : MonoBehaviour
{
    [SerializeField] GameObject LayoutGroup;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject EmptyObject;
    [SerializeField] int worldSizeX, worldSizeY;
    
    public List<GridLocation> gridObjects;
    GridLocation player;
    void Start()
    {
        for(int i = 0; i < worldSizeY; i++)
        {
            for(int j = 0; j < worldSizeX; j++)
            {
                if(i == 0 && j == worldSizeX/2)
                {
                    gridObjects.Add(new GridLocation());
                    player = gridObjects[i + j + (i * worldSizeX)];
                    Debug.Log(gridObjects.Count);
                    //player.objRef.transform.SetParent(LayoutGroup.transform);
                    //player.objRef.name = "Player";
                    //player.posX = j;
                    //player.posY = i;
                    //player.type = objectType.Player;
                    gridObjects[i + j + (i * worldSizeX)].objRef = GameObject.Instantiate(PlayerObject);
                    gridObjects[i + j + (i * worldSizeX)].objRef.transform.SetParent(LayoutGroup.transform, false);
                    gridObjects[i + j + (i * worldSizeX)].objRef.name = "Player";
                    gridObjects[i + j + (i * worldSizeX)].posX = j;
                    gridObjects[i + j + (i * worldSizeX)].posY = i;
                    gridObjects[i + j + (i * worldSizeX)].name = "Player";
                    gridObjects[i + j + (i * worldSizeX)].type = objectType.Player;
                }
                else
                {
                    gridObjects.Add(new GridLocation());
                    GridLocation obj = gridObjects[i + j + (i * worldSizeX)];
                    obj.objRef = GameObject.Instantiate(EmptyObject);
                    obj.objRef.transform.SetParent(LayoutGroup.transform);
                    obj.objRef.name = gridObjects.Count.ToString();
                    gridObjects[i + j + (i * worldSizeX)].name = gridObjects.Count.ToString();
                    obj.type = objectType.Ladder;
                    obj.posX = j;
                    obj.posY = i;
                }
            }

        }
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GridLocation SwapObject = gridObjects[2];   //If object is not found player is used as safety net
            int PlayerIndex = player.objRef.transform.GetSiblingIndex();
            int PlayerX = player.posX;
            int SwapIndex;
            if (PlayerIndex < 1)
            {
                SwapIndex = worldSizeX;
            }
            else SwapIndex = PlayerIndex - 2;
            foreach (GridLocation gridPiece in gridObjects)
            {
                if (gridPiece.posX == SwapIndex)
                {
                    SwapObject = gridPiece;
                    break;
                }
            }
            int SwapX = SwapObject.posX;
            player.objRef.transform.SetSiblingIndex(SwapIndex);
            player.posX = SwapX;
            SwapObject.objRef.transform.SetSiblingIndex(PlayerIndex);
            SwapObject.posX = PlayerX;
            SwapObject.objRef.name = "Switched";

        }
    }

    void CheckMove()
    {

    }

    void InitWorld()
    {

    }

    IEnumerator Updateworld()
    {
        yield return null;

    }
}
