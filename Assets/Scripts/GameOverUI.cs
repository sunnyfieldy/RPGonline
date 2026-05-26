using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("BUTTON CLICKED");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
