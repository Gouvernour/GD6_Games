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
    public Sprite sprite;
}

public class GameGrid : MonoBehaviour
{
    [SerializeField] GameObject LayoutGroup;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject EmptyObject;
    [SerializeField] int worldSizeX, worldSizeY;
    [SerializeField] Sprite DirtBlock;
    [SerializeField] Sprite SteelBlock;
    [SerializeField] Sprite MinerDig;
    [SerializeField] Sprite MinerClimb;
    GridLocation Player;
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
                    GridLocation obj = gridObjects[j + (i * worldSizeX)];
                    obj.objRef = GameObject.Instantiate(PlayerObject);
                    obj.objRef.transform.SetParent(LayoutGroup.transform, false);
                    obj.objRef.name = "Player";
                    obj.posX = j + 1;
                    obj.posY = i;
                    obj.name = "Player";
                    obj.type = objectType.Player;
                    Player = obj;
                }
                else
                {
                    gridObjects.Add(new GridLocation());
                    GridLocation obj = gridObjects[j + (i * worldSizeX)];
                    obj.objRef = GameObject.Instantiate(EmptyObject);
                    obj.objRef.transform.SetParent(LayoutGroup.transform);
                    obj.objRef.name = gridObjects.Count.ToString();
                    obj.name = gridObjects.Count.ToString();
                    obj.type = objectType.Ladder;
                    obj.posX = j;
                    obj.posY = i;
                    if(i > 0 && i%2!=0 && j%2==0)
                    {
                        obj.blockLife = 2;
                        obj.objRef.GetComponent<Image>().sprite = DirtBlock;
                        obj.type = objectType.Earth;
                    }else
                    {
                        obj.objRef.GetComponentInParent<Image>().enabled = false;
                    }
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
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
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
        else if (Input.GetKeyDown (KeyCode.DownArrow))
        {
            int index = Player.objRef.transform.GetSiblingIndex();
            int swapIndex = index + worldSizeX * 2;
            int objectCheckIndex = index + worldSizeX;
            int PlayerX = Player.posX;
            GridLocation SwapObject;
            if (swapIndex !> 127)
            {
                Debug.Log("Create new Row");
                NewRow();
                swapIndex -= 32;
                index -= 32;
                SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(swapIndex));

            }
            else
            {
                SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(swapIndex));

            }
            GridLocation Dirt = gridObjects.Find(x => x.objRef.transform == transform.GetChild(objectCheckIndex));

            if(Dirt.type == objectType.Ladder)
            {
                Player.objRef.transform.SetSiblingIndex(swapIndex);
                int swapX = SwapObject.posX;
                Player.posX = swapX;
                Player.posY += 2;
                SwapObject.posX = PlayerX;
                SwapObject.posY -= 2;
                SwapObject.objRef.transform.SetSiblingIndex(index);
            }
            else if(Dirt.type == objectType.Earth)
            {
                Dirt.blockLife--;
                if(Dirt.blockLife <= 0)
                {
                    Dirt.type = objectType.Ladder;
                    Dirt.objRef.GetComponentInParent<Image>().enabled = false;
                }
            }

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Player.posY == 0)
                return;
            else
            {
                int index = Player.objRef.transform.GetSiblingIndex();
                int swapIndex = index - worldSizeX * 2;
                int objectCheckIndex = index - worldSizeX;
                int PlayerX = Player.posX;
                GridLocation SwapObject;
                SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(swapIndex));
                GridLocation Dirt = gridObjects.Find(x => x.objRef.transform == transform.GetChild(objectCheckIndex));

                if (Dirt.type == objectType.Ladder)
                {
                    Player.objRef.transform.SetSiblingIndex(swapIndex);
                    int swapX = SwapObject.posX;
                    Player.posX = swapX;
                    SwapObject.posX = PlayerX;
                    SwapObject.objRef.transform.SetSiblingIndex(index);
                }
                else if (Dirt.type == objectType.Earth)
                {
                    return;
                }
            }
        }
    }

    void NewRow()
    {
        foreach (GridLocation gridPiece in gridObjects)
        {
            gridPiece.posY -= 2;
        }
        for (int j = 0; j < 2; j++)
        {
            for (int i = 0; i < 16; i++)
            {
                gridObjects.Add(new GridLocation());
                GridLocation removeObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(0));
                removeObject.objRef.transform.SetParent(null);
                gridObjects.Remove(removeObject);
                Destroy(removeObject.objRef);
                GridLocation obj = gridObjects[gridObjects.Count - 1];
                obj.objRef = GameObject.Instantiate(EmptyObject);
                obj.objRef.transform.SetParent(LayoutGroup.transform);
                obj.objRef.name = gridObjects.Count.ToString();
                obj.name = gridObjects.Count.ToString();
                obj.type = objectType.Ladder;
                obj.posX = i;
                obj.posY = j + 6;
                if (j > 0 && j % 2 != 0 && i % 2 == 0)
                {
                    obj.blockLife = 2;
                    obj.objRef.GetComponent<Image>().sprite = DirtBlock;
                    obj.type = objectType.Earth;
                }
                else
                {
                    obj.objRef.GetComponentInParent<Image>().enabled = false;
                }
            }
        }
    }

    IEnumerator Updateworld()
    {
        yield return null;

    }
}
