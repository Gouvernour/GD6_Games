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
    void Start()
    {
        for(int i = 0; i < worldSizeY; i++)
        {
            for(int j = 0; j < worldSizeX; j++)
            {
                if(i == 0 && j == worldSizeX/2)
                {
                    Debug.Log("Half World X = " + j);
                    gridObjects.Add(new GridLocation());
                    //Debug.Log(gridObjects.Count);
                    //player.objRef.transform.SetParent(LayoutGroup.transform);
                    //player.objRef.name = "Player";
                    //player.posX = j;
                    //player.posY = i;
                    //player.type = objectType.Player;
                    gridObjects[j + (i * worldSizeX)].objRef = GameObject.Instantiate(PlayerObject);
                    gridObjects[j + (i * worldSizeX)].objRef.transform.SetParent(LayoutGroup.transform, false);
                    gridObjects[j + (i * worldSizeX)].objRef.name = "Player";
                    gridObjects[j + (i * worldSizeX)].posX = j + 1;
                    gridObjects[j + (i * worldSizeX)].posY = i;
                    gridObjects[j + (i * worldSizeX)].name = "Player";
                    gridObjects[j + (i * worldSizeX)].type = objectType.Player;
                }
                else
                {
                    gridObjects.Add(new GridLocation());
                    GridLocation obj = gridObjects[j + (i * worldSizeX)];
                    obj.objRef = GameObject.Instantiate(EmptyObject);
                    obj.objRef.transform.SetParent(LayoutGroup.transform);
                    obj.objRef.name = gridObjects.Count.ToString();
                    gridObjects[j + (i * worldSizeX)].name = gridObjects.Count.ToString();
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
            Debug.Log(gridObjects.Count.ToString());
            GridLocation PLAYER = gridObjects.Find(x => x.type == objectType.Player);
            int PlayerIndex = PLAYER.objRef.transform.GetSiblingIndex();
            int PlayerX = PLAYER.posX;
            int PlayerY = PLAYER.posY;
            int SwapIndex;
            int SwapX;
            if (PlayerX < 1)
            {
                SwapX = worldSizeX;
                SwapIndex = PlayerIndex + worldSizeX - 2;
            }
            else
            {
                SwapX = PlayerX - 2;
                SwapIndex = PlayerIndex - 2;
            }

            GridLocation SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(SwapIndex));

            PLAYER.objRef.transform.SetSiblingIndex(SwapIndex);
            int swapX = SwapObject.posX;
            PLAYER.posX = swapX;
            SwapObject.posX = PlayerX;
            SwapObject.objRef.transform.SetSiblingIndex(PlayerIndex);
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Debug.Log(gridObjects.Count.ToString());
            GridLocation PLAYER = gridObjects.Find(x => x.type == objectType.Player);
            int PlayerIndex = PLAYER.objRef.transform.GetSiblingIndex();
            int PlayerX = PLAYER.posX;
            Debug.Log(PlayerX);
            int PlayerY = PLAYER.posY;
            int SwapIndex;
            int SwapX;
            if (PlayerX > 12)
            {
                SwapX = 0;
                SwapIndex = PlayerIndex - 14;
            }
            else
            {
                SwapX = PlayerX + 2;
                SwapIndex = PlayerIndex + 2;
            }

            GridLocation SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(SwapIndex));

            PLAYER.objRef.transform.SetSiblingIndex(SwapIndex);
            int swapX = SwapObject.posX;
            PLAYER.posX = swapX;
            SwapObject.posX = PlayerX;
            SwapObject.objRef.transform.SetSiblingIndex(PlayerIndex);
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
