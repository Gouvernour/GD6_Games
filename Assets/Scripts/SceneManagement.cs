using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] string daSceneName;
    bool timerStart = false;
    public float timerTime = 2.0f;
    public GameObject pressAny;

    void Update()
    {
        if (Input.anyKey && !timerStart)
        {
            StartCoroutine(timer());
            timerStart = true;
        }
    }
    public void StartGame(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator timer ()
    {
        AudioManager.instance.PlaySound(SoundGroup.AcceptButton);
        pressAny.GetComponent<Animator>().Play("Start");
        yield return new WaitForSeconds(timerTime);
        StartGame(daSceneName);
    }
}
