using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelObject : MonoBehaviour
{

    private int MaxIndexLevel = 3;


    public void EndLevel()
    {
        if (PlayerPrefs.GetInt("level_c") < (PlayerPrefs.GetInt("level", 1) + 1))
        {
            PlayerPrefs.SetInt("level_c", PlayerPrefs.GetInt("level", 1) + 1);
        }
        Time.timeScale = 1;

        if(PlayerPrefs.GetInt("level", 1) == MaxIndexLevel) SceneManager.LoadSceneAsync("Authors");
        else SceneManager.LoadSceneAsync("MenuScen");
    }
}
