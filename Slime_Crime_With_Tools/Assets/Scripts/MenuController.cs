using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public void CloseApplication()
    {
        Application.Quit();
    }

    public void LoadConnectionScreen()
    {
        SceneManager.LoadScene(1);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }


}