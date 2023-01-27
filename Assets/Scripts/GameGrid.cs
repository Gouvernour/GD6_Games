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
    Metal,
    Enemy
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
    public bool moveleft;
}

public class GameGrid : MonoBehaviour
{
    GridLocation Player;    //Reference for player

    [Header("Grid")]
    [SerializeField] GameObject LayoutGroup;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject EmptyObject;
    [SerializeField] int worldSizeX, worldSizeY;
    [SerializeField] List<GridLocation> gridObjects;

    [Header("Sprites")]
    [SerializeField] Sprite DirtBlockPlayer;
    [SerializeField] Sprite DirtBlockEnemy;
    [SerializeField] Sprite SteelBlockPlayer;
    [SerializeField] Sprite SteelBlockEnemy;
    [SerializeField] Sprite MinerDig;
    [SerializeField] Sprite MinerClimb;
    [SerializeField] Sprite FlyEnemy;
    [SerializeField] Sprite LandEnemy;
    [SerializeField] Sprite BothEnemy;

    [Header("Enemies")]
    [SerializeField, Tooltip("Percentage of initial chance to spawn enemies")] float initialChance;
    [SerializeField, Tooltip("Current chance of spawning new enemy")] float EnemyChance;
    [SerializeField, Tooltip("Multiplier to increase spawn chance per new level")] float EnemyChanceMultiplier;
    [SerializeField, Tooltip("Multiplier to increase spawn chance per new level")] float MaxEnemyChance;
    [SerializeField, Tooltip("Max Allowed enemies")] int MaxActiveEnemies;
    [SerializeField, Tooltip("Currently active enemies on screen")] int ActiveEnemies;

    [Header("Non Diggable dirt")]
    [SerializeField] int MaxMetalDirt;
    int spawnedMetal = 0;
    [SerializeField] float MetalSpawnChance;
    [SerializeField] float MaxMetalSpawnChance = 70;
    [SerializeField] float MetalSpawnMultiplier;
    bool SpawnSteel = false;

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
                        obj.objRef.GetComponent<Image>().sprite = DirtBlockPlayer;
                        obj.objRef.GetComponent<Image>().preserveAspect = true;
                        obj.type = objectType.Earth;
                    }else if(i > 0 && i % 2 != 0 && j % 2 != 0)
                    {
                        obj.objRef.GetComponent<Image>().sprite = DirtBlockEnemy;
                        obj.objRef.GetComponent<Image>().preserveAspect = true;
                    }
                    else
                    {
                        obj.objRef.GetComponent<Image>().enabled = false;
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
            GridLocation Dirt = gridObjects.Find(x => x.objRef.transform == transform.GetChild(objectCheckIndex));

            if(Dirt.type == objectType.Ladder)
            {
                if (swapIndex! > 127)
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
                    GridLocation ConnectedDirt = gridObjects.Find(x => x.objRef.transform == transform.GetChild(objectCheckIndex + 1));
                    ConnectedDirt.type = objectType.Ladder;
                    ConnectedDirt.objRef.GetComponentInParent<Image>().enabled = false;
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
            if (gridPiece.type == objectType.Enemy)
                ActiveEnemies--;
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
                if(j % 2 != 0)
                {
                    if (i % 2 == 0)
                    {
                        obj.blockLife = 2;
                        SpawnSteel = CreateMetal();
                        if(SpawnSteel)
                        {
                            obj.type = objectType.Metal;
                            obj.objRef.GetComponent<Image>().sprite = SteelBlockPlayer;
                            obj.objRef.GetComponent<Image>().preserveAspect = true;
                        }
                        else
                        {
                            obj.type = objectType.Earth;
                            obj.objRef.GetComponent<Image>().sprite = DirtBlockPlayer;
                            obj.objRef.GetComponent<Image>().preserveAspect = true;
                        }

                    }else if(i % 2 != 0)
                    {
                        if (SpawnSteel)
                        {
                            obj.type = objectType.Metal;
                            obj.objRef.GetComponent<Image>().sprite = SteelBlockEnemy;
                            obj.objRef.GetComponent<Image>().preserveAspect = true;
                            SpawnSteel = false;
                        }
                        else
                        {
                            obj.type = objectType.Earth;
                            obj.objRef.GetComponent<Image>().sprite = DirtBlockEnemy;
                            obj.objRef.GetComponent<Image>().preserveAspect = true;
                        }
                    }

                }else if(j % 2 == 0 && i == 1 || (j % 2 == 0 && i == worldSizeX - 1))
                {
                    if (i == 1)
                    {
                        if(SpawnEnemy(true))
                        {
                            obj.type = objectType.Enemy;
                            obj.objRef.GetComponent<Image>().sprite = LandEnemy;
                            obj.objRef.GetComponent<Image>().preserveAspect = true;
                        }
                        else
                            obj.objRef.GetComponentInParent<Image>().enabled = false;
                    }
                    else if (i == worldSizeX - 1)
                    {
                        if(SpawnEnemy(false))
                        {
                            obj.type = objectType.Enemy;
                            obj.objRef.GetComponent<Image>().sprite = FlyEnemy;
                            obj.objRef.GetComponent<Image>().preserveAspect = true;
                        }
                        else
                            obj.objRef.GetComponentInParent<Image>().enabled = false;
                    }
                    else
                        Debug.LogWarning("This should not be called");
                }
                else
                {
                    obj.objRef.GetComponentInParent<Image>().enabled = false;
                }
            }
        }
        spawnedMetal = 0;
        MetalSpawnChance *= MetalSpawnMultiplier;
        if(MetalSpawnChance > MaxMetalSpawnChance)
            MetalSpawnChance = MaxMetalSpawnChance;

        EnemyChance *= EnemyChanceMultiplier;
        if (EnemyChance > MaxEnemyChance)
            EnemyChance = MaxEnemyChance;
    }

    bool CreateMetal()
    {
        if(Random.Range(0, 100) + MetalSpawnChance >= 100f && MaxMetalDirt > spawnedMetal)
        {
            spawnedMetal++;
            return true;
        }
        return false;
    }

    bool SpawnEnemy(bool enemy1)
    {
        if (Random.Range(0, 100) + EnemyChance >= 100f && MaxActiveEnemies > ActiveEnemies)
        {
            ActiveEnemies++;
            
            return true;
        }
        return false;
    }

    List<GridLocation> Enemies;
    float EnemyUpdateSpeed = 1;
    IEnumerator UpdateEnemies()
    {
        
        yield return new WaitForSeconds(1);
        if (Enemies != null)
        {
            foreach(GridLocation Enemy in Enemies)
            {
                if(Enemy.moveleft)
                {
                    Enemies.Remove(Enemy);
                }else
                {

                }
            }
            UpdateEnemies();
        }
        else
            UpdateEnemies();

    }
}
