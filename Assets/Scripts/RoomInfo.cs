using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [System.Serializable]
    public enum doortype
    {
        NotExit,
        HasExit,
        IsExit
    }

    [System.Serializable]
    public enum ExitDirection
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    [SerializeField] doortype Left, Right, Up, Down;
    [SerializeField] public ExitDirection exitDirection;
    [SerializeField] ExitDirection hasExitDirection;

    int X;  //World coordinate location
    int Y;  //World coordinate location
}
