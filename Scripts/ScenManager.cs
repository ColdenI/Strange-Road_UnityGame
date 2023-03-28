using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenManager : MonoBehaviour
{

    public PlayerController player;
    public MouseLook_ MouseLook_Player;
    public MouseLook_ MouseLook_Camera;

    public GameObject[] AllUIPanel;
    public GameObject PausePanel;
    public GameObject GamePanel;
    public GameObject MapPanel;
    public GameObject Inventar_panel;

    public GameObject MapImage;
    public Sprite[] MapsSprite;

    public GameObject TextHelp;

    public bool isMAPause = true;


    private bool isPause = false;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        MouseLook_Player = GameObject.Find("Player").GetComponent<MouseLook_>();
        MouseLook_Camera = GameObject.Find("Main Camera").GetComponent<MouseLook_>();

        // GetComponent<SettingsScript>().LoadSettings();
        LoadMapImage();
        CursorUnVisible();
    }

    public void CursorVisible()
    {
        player.isMove = false;
        MouseLook_Camera.isMove = false;
        MouseLook_Player.isMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void CursorUnVisible()
    {
        player.isMove = true;
        MouseLook_Player.isMove = true;
        MouseLook_Camera.isMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Pause()
    {
        CursorVisible();
        Time.timeScale = 0;
        isPause = true;

        foreach (GameObject i in AllUIPanel) i.SetActive(false);
        PausePanel.SetActive(true);
        PausePanel.GetComponent<Animator>().SetTrigger("a");
    }

    public void Play()
    {
        CursorUnVisible();
        Time.timeScale = 1;
        isPause = false;

        foreach (GameObject i in AllUIPanel) i.SetActive(false);
        GamePanel.SetActive(true);
    }

    public void ExitInMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MenuScen");
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isMAPause && (GamePanel.activeSelf || PausePanel.activeSelf) && !GameObject.Find("TimelineManager").GetComponent<CutSceneManager>().flag)
        {
            if (isPause) Play();
            else Pause();

        }

        if((Input.GetKeyDown(KeyCode.Escape)) && isMAPause && (Inventar_panel.activeSelf || MapPanel.activeSelf) && !GameObject.Find("TimelineManager").GetComponent<CutSceneManager>().flag)
        {
            Play();
            player.click_Sound.Play();
        }

        if (Input.GetKeyDown(KeyCode.H) && GamePanel.activeSelf)
        {
            TextHelp.SetActive(!TextHelp.activeSelf);
            player.click_Sound.Play();
        }

        if (Input.GetKeyDown(KeyCode.M) && isMAPause && !Inventar_panel.activeSelf && !GameObject.Find("TimelineManager").GetComponent<CutSceneManager>().flag)
        {
            if (MapPanel.activeSelf)
            {
                Play();
            }
            else
            {
                CursorVisible();
                Time.timeScale = 0;
                GamePanel.SetActive(false);
                MapPanel.SetActive(true);
            }
            player.click_Sound.Play();
            LoadMapImage();
        }

        if (Input.GetKeyDown(KeyCode.E) && isMAPause && !MapPanel.activeSelf && !GameObject.Find("TimelineManager").GetComponent<CutSceneManager>().flag)
        {
            if (Inventar_panel.activeSelf)
            {
                Play();
            }
            else
            {
                CursorVisible();
                player.Inventar_Updata();
                Time.timeScale = 0;
                GamePanel.SetActive(false);
                Inventar_panel.SetActive(true);
            }
            player.click_Sound.Play();
        }

        if (MapPanel.activeSelf)
        {
            if(Input.mouseScrollDelta.y < 0)
            {
                MapImage.GetComponent<UnityEngine.UI.Image>().transform.localScale *= .9f;
            }
            else if (Input.mouseScrollDelta.y > 0 && MapImage.GetComponent<UnityEngine.UI.Image>().transform.localScale.x < 12)
            {
                MapImage.GetComponent<UnityEngine.UI.Image>().transform.localScale *= 1.1f;
            }
        }
    }

    private void LoadMapImage()
    {
        Sprite sprite = MapsSprite[PlayerPrefs.GetInt("level", 1) - 1];

        MapImage.GetComponent<UnityEngine.UI.Image>().sprite = sprite;

        MapImage.GetComponent<UnityEngine.UI.Image>().rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
    }


}