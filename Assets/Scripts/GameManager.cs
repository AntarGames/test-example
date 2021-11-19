using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0;
    }
    public static void StartGame()
    {
        Time.timeScale = 1;
    }
    public static void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
