using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLogosScript : MonoBehaviour
{

    void Start()
    {
        Cursor.visible = false;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(16f);
        Cursor.visible = true;
        SceneManager.LoadSceneAsync("MenuScen");
    }
}
