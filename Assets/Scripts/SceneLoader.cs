using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{

    [SerializeField] float timeToWait = 2.0f;
    public void LoadNextScene()
    {
        Debug.Log("clicked");
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex += 1);
    }

    public void LoadSceneByString(string name)
    {
        SceneManager.LoadScene(name);
        if (name.Equals("GameScene"))
        {
            FindObjectOfType<GameSession>().ResetGame();
        }
    }

    public void PlayerDied()
    {
        StartCoroutine(WaitForPlayer());
    }

    private IEnumerator WaitForPlayer()
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene("GameOver");
    }

    public void GameOver()
    {
        Application.Quit();
    }
}
