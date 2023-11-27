using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public static void LoadScene(string name, bool async)
    {
        if(async)
        {
            SceneManager.LoadSceneAsync(name);
        }
        else
        {
            SceneManager.LoadScene(name);
        }
    }

    public static void LoadScene(int index, bool async)
    {
        if (async)
        {
            SceneManager.LoadSceneAsync(index);
        }
        else
        {
            SceneManager.LoadScene(index);
        }
    }

    public static void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitted Game...");
    }
}
