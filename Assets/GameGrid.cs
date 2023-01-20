using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
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
        int posY;
        int posX;
    }
    public GridLocation[][] coordinate;
    public List<GridLocation> gridObject;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    IEnumerator Updateworld()
    {
        yield return null;

    }
}
