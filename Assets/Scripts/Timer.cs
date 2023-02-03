using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float hitTimer = 5;
    [SerializeField] float time;
    [SerializeField] float newRoomDelay = 15;
    [SerializeField] Text timeText;
    [SerializeField] Player;

    private void Update()
    {
        if(time < 0)
        {
            //game over
        }else
            time -= Time.deltaTime;
        timeText.text = time.ToString();
    }
    public void EnterRoom()
    {
        StopAllCoroutines();
        StartCoroutine(NewRoom());
    }

    public void StartTimer()
    {
        StopAllCoroutines();
        StartCoroutine(DamageTick());
    }

    public void ResetHitTime()
    {
        StopAllCoroutines();
        StartCoroutine(DamageTick());
    }

    IEnumerator DamageTick()
    {
        yield return new WaitForSeconds(hitTimer);
        //Take Damage and Restart Timer
        DamageTick();
    }

    IEnumerator NewRoom()
    {
        yield return new WaitForSeconds(newRoomDelay);
        DamageTick();
    }

    void EndGame()
    {

    }
}
