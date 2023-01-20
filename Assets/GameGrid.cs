using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] GameObject LayoutGroup;
    [SerializeField] GameObject DefaultObject;
    [SerializeField] int worldSizeX, worldSizeY;
    [System.Serializable]
    public enum objectType
    {
        Ladder,
        Player,
        Earth,
    }
    [System.Serializable]
    public struct GridLocation
    {
        public objectType type;
        public int blockLife;
        public GameObject objRef;
        public int posY;
        public int posX;
        public Sprite sprite;
    }
    public List<GridLocation> gridObject;
    void Start()
    {
        for(int i = 0; i < worldSizeX; i++)
        {
            for(int j = 0; j < worldSizeY; j++)
            {
                gridObject.Add(new GridLocation());
                GameObject obj = gridObject[i + j * (i + 1)].objRef;
                obj = GameObject.Instantiate(DefaultObject);
                //obj.transform.parent = LayoutGroup.transform;
                //obj.name = gridObject.Count.ToString();
            }

        }
        GameObject Obj = gridObject[worldSizeX / 2].objRef;
        Obj = DefaultObject;
    }

    
    void Update()
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
