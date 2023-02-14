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
            HP = MAXHP;
        }else
            Destroy(gameObject);
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
        AudioManager.instance.PlaySound(SoundGroup.Dig);
        print("playing Sound");
        healthBar.SetHealth(HP);
        if(HP <= 0)
        {
            //Game Over
            InputManager.instance.Dead = true;
        }
    }

    public void ManualWon()
    {
        Camera.main.enabled = false;
        AudioManager.instance.PlaySound(SoundGroup.CantDig);
        InputManager.instance.Dead = true;
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
