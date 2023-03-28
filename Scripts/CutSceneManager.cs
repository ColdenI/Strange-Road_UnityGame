using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{

    public PlayableDirector PlayableDirector;
    public Camera PlayerCamera;
    public Camera CutCamera;
    public GameObject Player;

    public bool flag = false;


    void Start()
    {
        PlayableDirector = GetComponent<PlayableDirector>();
        PlayerCamera = Camera.main;
        CutCamera.enabled = false;
        GameObject.Find("ScenManager").GetComponent<ScenManager>().isMAPause = false;
        Player = GameObject.Find("Player");
    }

    public void Start_Cut(PlayableAsset playableAsset)
    {
        PlayableDirector.playableAsset = playableAsset;
        PlayableDirector.Play(playableAsset);
        flag = true;
        PlayerCamera.enabled = false;
        GameObject.Find("ScenManager").GetComponent<ScenManager>().isMAPause = true;
        CutCamera.enabled = true;
        Player.GetComponent<PlayerController>().isMove = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && flag)
        {
            flag = false;
            PlayerCamera.enabled = true;
            CutCamera.enabled = false;
            GameObject.Find("ScenManager").GetComponent<ScenManager>().isMAPause = true;
            Player.GetComponent<PlayerController>().isMove = true;
        }

        if(flag && PlayableDirector.state != PlayState.Playing)
        {
            flag = false;
            PlayerCamera.enabled = true;
            CutCamera.enabled = false;
            Player.GetComponent<PlayerController>().isMove = true;
        }
    }
}
