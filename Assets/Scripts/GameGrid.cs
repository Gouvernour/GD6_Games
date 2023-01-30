using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    PitchVersion currentPitch = PitchVersion.First;
    [SerializeField] int currentLevel = 1;
    [SerializeField] int LowestLevel = 1;
    bool DoneNewRow = false;

    [Header("Score")]
    int Score = 0;
    [SerializeField] TextMeshProUGUI ScoreTxt;
    int HighScore = 0;
    [SerializeField] TextMeshProUGUI HighScoreTxt;

    [Header("Audio")]
    [SerializeField] AudioManager audio;

    [Header("Grid")]
    [SerializeField] GameObject LayoutGroup;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject EmptyObject;
    [SerializeField] int worldSizeX, worldSizeY;
    [SerializeField] List<GridLocation> gridObjects;

    [Header("Sprites")]
    [SerializeField] Sprite DirtBlockPlayer;
    [SerializeField] Sprite DirtDugPlayer;
    [SerializeField] Sprite DirtBlockEnemy;
    [SerializeField] Sprite DirtDugEnemy;
    [SerializeField] Sprite SteelBlockPlayer;
    [SerializeField] Sprite SteelBlockEnemy;
    [SerializeField] Sprite MinerDig;
    [SerializeField] Sprite MinerClimb;
    [SerializeField] Sprite MinerDead;
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
        if (Saves.instance != null)
            HighScore = Saves.instance.DiggerHighScore;
        audio = AudioManager.instance;
        StartCoroutine(PregameSound());
        StartCoroutine(UpdateEnemies());
        for (int i = 0; i < worldSizeY; i++)
        {
            for (int j = 0; j < worldSizeX; j++)
            {
                if (i == 0)
                {
                    gridObjects.Add(new GridLocation());
                    GridLocation obj = gridObjects[j + (i * worldSizeX)];
                    obj.objRef = GameObject.Instantiate(EmptyObject);
                    obj.objRef.transform.SetParent(transform);
                    obj.objRef.GetComponent<Image>().enabled = false;
                }
                else if (i == 1 && j == worldSizeX / 2)
                {
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
                    if (i > 1 && i % 2 == 0 && j % 2 == 0)
                    {
                        obj.blockLife = 2;
                        obj.objRef.GetComponent<Image>().sprite = DirtBlockPlayer;
                        obj.objRef.GetComponent<Image>().preserveAspect = true;
                        obj.type = objectType.Earth;
                    }
                    else if (i > 1 && i % 2 == 0 && j % 2 != 0)
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

    bool IntroPlaying = false;
    public bool PlayerDead = false;
    void Update()
    {
        ScoreTxt.text = "Score: " + Score;
        HighScoreTxt.text = "Hi Score: " + HighScore;
        if (IntroPlaying)
            return;
        if (!PlayerDead)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                audio.PlaySound(SoundGroup.Move, currentPitch);
                //Debug.Log(gridObjects.Count.ToString());
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
                GridLocation EnemyCheck = gridObjects.Find(x => x.objRef.transform == transform.GetChild(SwapIndex + 1));

                if(EnemyCheck.type == objectType.Enemy)
                {
                    KillPlayer();
                }

                PLAYER.objRef.transform.SetSiblingIndex(SwapIndex);
                int swapX = SwapObject.posX;
                PLAYER.posX = swapX;
                SwapObject.posX = PlayerX;
                SwapObject.objRef.transform.SetSiblingIndex(PlayerIndex);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                audio.PlaySound(SoundGroup.Move, currentPitch);
                //Debug.Log(gridObjects.Count.ToString());
                GridLocation PLAYER = gridObjects.Find(x => x.type == objectType.Player);
                int PlayerIndex = PLAYER.objRef.transform.GetSiblingIndex();
                int PlayerX = PLAYER.posX;
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
                GridLocation EnemyCheck = gridObjects.Find(x => x.objRef.transform == transform.GetChild(SwapIndex - 1));

                if(EnemyCheck.type == objectType.Enemy)
                {
                    KillPlayer();
                }

                PLAYER.objRef.transform.SetSiblingIndex(SwapIndex);
                int swapX = SwapObject.posX;
                PLAYER.posX = swapX;
                SwapObject.posX = PlayerX;
                SwapObject.objRef.transform.SetSiblingIndex(PlayerIndex);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                int index = Player.objRef.transform.GetSiblingIndex();
                int swapIndex = index + worldSizeX * 2;
                int objectCheckIndex = index + worldSizeX;
                int PlayerX = Player.posX;
                GridLocation SwapObject;
                GridLocation Dirt = gridObjects.Find(x => x.objRef.transform == transform.GetChild(objectCheckIndex));

                if (Dirt.type == objectType.Ladder)
                {
                    if (swapIndex! > 127)
                    {
                        Debug.Log("Create new Row");
                        NewRow();
                        DoneNewRow = true;
                        swapIndex -= 32;
                        index -= 32;
                        SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(swapIndex));
                        audio.PlaySound(SoundGroup.NextLevel);
                        Score += 10;
                    }
                    else
                    {
                        SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(swapIndex));
                        if(DoneNewRow)
                            currentLevel++;
                    }
                    Player.objRef.transform.SetSiblingIndex(swapIndex);
                    int swapX = SwapObject.posX;
                    Player.posX = swapX;
                    Player.posY += 2;
                    SwapObject.posX = PlayerX;
                    SwapObject.posY -= 2;
                    SwapObject.objRef.transform.SetSiblingIndex(index);
                    if(!DoneNewRow)
                    {
                        currentLevel++;
                        if(currentLevel > LowestLevel)
                        {
                            Score += 10;
                            LowestLevel = currentLevel;
                            Debug.Log(currentLevel + " : " + LowestLevel);
                            audio.PlaySound(SoundGroup.NextLevel);

                        }
                        else
                        {
                            audio.PlaySound(SoundGroup.Move, currentPitch);
                            switch (currentLevel)
                            {
                                case 1:
                                    currentPitch = PitchVersion.First;
                                    break;
                                case 2:
                                    currentPitch = PitchVersion.Second;
                                    break;
                                case 3:
                                    currentPitch = PitchVersion.Third;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    else
                    {
                        audio.PlaySound(SoundGroup.Move, currentPitch);
                        switch (currentLevel)
                        {
                            case 1:
                                currentPitch = PitchVersion.First;
                                break;
                            case 2:
                                currentPitch = PitchVersion.Second;
                                break;
                            case 3:
                                currentPitch = PitchVersion.Third;
                                break;
                            default:
                                break;
                        }
                    }

                }
                else if (Dirt.type == objectType.Earth)
                {
                    audio.PlaySound(SoundGroup.Dig);
                    Dirt.blockLife--;
                    GridLocation ConnectedDirt = gridObjects.Find(x => x.objRef.transform == transform.GetChild(objectCheckIndex + 1));
                    if (Dirt.blockLife <= 0)
                    {
                        Dirt.type = objectType.Ladder;
                        Dirt.objRef.GetComponentInParent<Image>().enabled = false;
                        ConnectedDirt.type = objectType.Ladder;
                        ConnectedDirt.objRef.GetComponentInParent<Image>().enabled = false;
                    }else
                    {
                        Dirt.objRef.GetComponentInParent<Image>().sprite = DirtDugPlayer;
                        ConnectedDirt.objRef.GetComponentInParent<Image>().sprite = DirtDugEnemy;
                    }
                }
                else if (Dirt.type == objectType.Metal)
                    audio.PlaySound(SoundGroup.CantDig);

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (Player.posY == 1)
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
                        audio.PlaySound(SoundGroup.Move, currentPitch);
                        currentLevel--;
                        switch (currentLevel)
                        {
                            case 1:
                                currentPitch = PitchVersion.First;
                                break;
                            case 2:
                                currentPitch = PitchVersion.Second;
                                break;
                            case 3:
                                currentPitch = PitchVersion.Third;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (Dirt.type == objectType.Earth)
                    {
                        return;
                    }
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
                GridLocation removeObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(16));
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
                if (j % 2 != 0)
                {
                    if (i % 2 == 0)
                    {
                        obj.blockLife = 2;
                        SpawnSteel = CreateMetal();
                        if (SpawnSteel)
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

                    }
                    else if (i % 2 != 0)
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

                }
                else if (j % 2 == 0 && i == 1 || (j % 2 == 0 && i == worldSizeX - 1))
                {
                    if (i == 1)
                    {
                        if (SpawnEnemy(true))
                        {
                            obj.type = objectType.Enemy;
                            obj.objRef.GetComponent<Image>().sprite = LandEnemy;
                            obj.objRef.GetComponent<Image>().preserveAspect = true;
                            obj.moveleft = false;
                            obj.posX = i;
                            obj.posY = 7;
                        }
                        else
                            obj.objRef.GetComponentInParent<Image>().enabled = false;
                    }
                    else if (i == worldSizeX - 1)
                    {
                        if (SpawnEnemy(false))
                        {
                            obj.type = objectType.Enemy;
                            obj.objRef.GetComponent<Image>().sprite = FlyEnemy;
                            obj.objRef.GetComponent<Image>().preserveAspect = true;
                            obj.moveleft = true;
                            obj.posX = i;
                            obj.posY = 7;
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
        if (MetalSpawnChance > MaxMetalSpawnChance)
            MetalSpawnChance = MaxMetalSpawnChance;

        EnemyChance *= EnemyChanceMultiplier;
        if (EnemyChance > MaxEnemyChance)
            EnemyChance = MaxEnemyChance;
    }

    bool CreateMetal()
    {
        if (Random.Range(0, 100) + MetalSpawnChance >= 100f && MaxMetalDirt > spawnedMetal)
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
        yield return new WaitForSeconds(EnemyUpdateSpeed);
        bool PlayerDead = false;
        foreach (GridLocation Enemy in gridObjects)
        {
            if(Enemy.type == objectType.Enemy)
            {
                if (Enemy.moveleft)
                {
                    audio.PlaySound(SoundGroup.Enemy2Move);
                    int EnemyIndex = Enemy.objRef.transform.GetSiblingIndex();
                    int EnemyX = Enemy.posX;
                    int EnemyY = Enemy.posY;
                    int SwapIndex;
                    int SwapX;
                    
                    if (EnemyX <= 1)
                    {
                        Enemy.type = objectType.Ladder;
                        Enemy.objRef.GetComponentInParent<Image>().enabled = false;
                        SwapIndex = 0;
                    }
                    else
                    {
                        SwapX = EnemyX - 2;
                        SwapIndex = EnemyIndex - 2;
                    }
                    GridLocation PlayerCheck = gridObjects.Find(x => x.objRef.transform == transform.GetChild(SwapIndex+1));
                    if(PlayerCheck.type == objectType.Player)
                    {
                        KillPlayer();
                        PlayerDead = true;
                    }

                    GridLocation SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(SwapIndex));

                    Enemy.objRef.transform.SetSiblingIndex(SwapIndex);
                    int swapX = SwapObject.posX;
                    Enemy.posX = swapX;
                    SwapObject.posX = EnemyX;
                    SwapObject.objRef.transform.SetSiblingIndex(EnemyIndex);
                }
                else
                {
                    audio.PlaySound(SoundGroup.Enemy1Move);
                    int EnemyIndex = Enemy.objRef.transform.GetSiblingIndex();
                    int EnemyX = Enemy.posX;
                    int EnemyY = Enemy.posY;
                    int SwapIndex;
                    int SwapX;

                    if (EnemyX >= worldSizeX - 1)
                    {
                        Enemy.type = objectType.Ladder;
                        Enemy.objRef.GetComponentInParent<Image>().enabled = false;
                        SwapIndex = EnemyX;
                    }
                    else
                    {
                        SwapX = EnemyX + 2;
                        SwapIndex = EnemyIndex + 2;
                    }
                    GridLocation PlayerCheck = gridObjects.Find(x => x.objRef.transform == transform.GetChild(SwapIndex - 1));
                    if (PlayerCheck.type == objectType.Player)
                    {
                        KillPlayer();
                        PlayerDead = true;
                    }

                    GridLocation SwapObject = gridObjects.Find(x => x.objRef.transform == transform.GetChild(SwapIndex));

                    Enemy.objRef.transform.SetSiblingIndex(SwapIndex);
                    int swapX = SwapObject.posX;
                    Enemy.posX = swapX;
                    SwapObject.posX = EnemyX;
                    SwapObject.objRef.transform.SetSiblingIndex(EnemyIndex);
                }
            }
        }
        if(!PlayerDead)
            StartCoroutine(UpdateEnemies());
        else
        {
            StopCoroutine(UpdateEnemies());
        }

    }

    void KillPlayer()
    {
        audio.PlaySound(SoundGroup.Die);
        PlayerDead = true;
        Player.objRef.GetComponent<Image>().sprite = MinerDead;
        StopAllCoroutines();
        StartCoroutine(FinishGame());
        
    }

    IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(1f);
        if (Score > HighScore)
        {
            HighScore = Score;
            if(Saves.instance != null)
            {
                Saves.instance.DiggerHighScore = HighScore;
                Saves.instance.Save();

            }
            audio.PlaySound(SoundGroup.HighScore);
        }
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }
    IEnumerator PregameSound()
    {
        IntroPlaying = true;
        yield return new WaitForSeconds(.3f);
        audio.PlayMusic("Intro");
        yield return new WaitForSeconds(1.5f);
        IntroPlaying = false;
    }
}

