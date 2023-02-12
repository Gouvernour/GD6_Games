using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateHealthRemoval : MonoBehaviour
{
    public static DuplicateHealthRemoval instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
