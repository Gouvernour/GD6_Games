using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int MAXHP = 5;
    [SerializeField] int HP = 5;
    static public Player instance;
    public HealthBar healthBar;

    void Start()
    {
        healthBar.SetMaxHealth(MAXHP);
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
            Destroy(gameObject);
        HP = MAXHP;
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        healthBar.SetHealth(HP);
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
