using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject[] LevelTerrain;
    public Vector3[] PlayerPosition;

    public GameObject Player;
    public GameObject Panel_Loading;

    
    void Start()
    {
        Player.GetComponent<PlayerController>().isMove = false;
        Panel_Loading.SetActive(true);

        LoadScen();

    }

    private void LoadScen()
    {
        int index = PlayerPrefs.GetInt("level", 1) - 1;

        Player.GetComponent<CharacterController>().enabled = false;
        Player.transform.position = PlayerPosition[index];
        Player.GetComponent<CharacterController>().enabled = true;

        Instantiate(LevelTerrain[index], Vector3.zero, Quaternion.identity);
        GetComponent<SettingsScript>().LoadSettings();
        StartCoroutine(LevelStart());
    }

    IEnumerator LevelStart()
    {
        GetComponent<ScenManager>().isMAPause = false;
        yield return new WaitForSeconds(3);
        Player.GetComponent<PlayerController>().isMove = true;
        Panel_Loading.SetActive(false);
        GameObject NewCapterPanel = GameObject.Find("Panel_chapter");
        GameObject Teleport_panel = GameObject.Find("Panel_teleportation");


        GameObject.Find("PictureScreem").SetActive(false);
        NewCapterPanel.SetActive(false);
        Teleport_panel.SetActive(false);

        Player.GetComponent<PlayerController>().CheckPoint = Player.transform.position;
        GetComponent<ScenManager>().isMAPause = true;
    }

    void Update()
    {
        
    }
}
