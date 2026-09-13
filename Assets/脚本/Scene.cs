using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void cj0()
    {
        SceneManager.LoadScene(0);
    }
    public void cj1()
    {
        SceneManager.LoadScene(1);
    }
    public void EXIT()
        {
            Application.Quit();
        }
}
