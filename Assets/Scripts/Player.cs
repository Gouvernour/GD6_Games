using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int MAXHP = 5;
    int HP = 5;
    static public Player instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
            Destroy(gameObject);
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        if(HP <= 0)
        {
            //Game Over
        }
    }

    public void Regenerate(int heal)
    {
        HP += heal;
        if(HP >= MAXHP)
        {
            HP = MAXHP;
        }
    }
}
