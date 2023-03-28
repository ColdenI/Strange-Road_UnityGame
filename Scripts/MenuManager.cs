using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;

public class MenuManager : MonoBehaviour
{

    public GameObject panel_main;
    public GameObject panel_help;
    public GameObject panel_about;
    public GameObject panel_clear_data;
    public GameObject panel_Console;

    public InputField InputField_console;

    public GameObject panel_LoadLevel;

    public GameObject[] level_panels;

    public AudioSource click_sound;

    public FoxController fox;
    public float FoxRunTime = 3f;

    IEnumerator FoxControll()
    {
        fox.mode = FoxController.Mode.Harassment;
        yield return new WaitForSeconds(FoxRunTime);
        fox.mode = FoxController.Mode.IdleAndSit;
        yield return new WaitForSeconds(1);
        StartCoroutine(FoxAnim());
    }

    IEnumerator FoxAnim()
    {
        yield return new WaitForSeconds(Random.Range(10, 19));
        fox.gameObject.GetComponent<AudioSource>().Play();
        fox.Dan_1();
        StartCoroutine(FoxAnim());

    }

    void Start()
    {
        StartCoroutine(FoxControll());
        LoadData();
        LoadSettings();

    }

    public void Open_help_p()
    {
        panel_main.SetActive(false);
        panel_help.SetActive(true);
        panel_about.SetActive(false);
        click_sound.Play();
    }
    public void Open_about_p()
    {
        panel_main.SetActive(false);
        panel_help.SetActive(false);
        panel_about.SetActive(true);
        click_sound.Play();
    }
    public void Open_main_p()
    {
        panel_main.SetActive(true);
        panel_help.SetActive(false);
        panel_about.SetActive(false);
        click_sound.Play();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && panel_about.activeSelf) Open_main_p();
        if (Input.GetKeyDown(KeyCode.Escape) && panel_help.activeSelf) Open_main_p();

        if (Input.GetKeyDown(KeyCode.F10)) panel_clear_data.SetActive(true);
        if (Input.GetKeyDown(KeyCode.F11) && panel_clear_data.activeSelf) panel_Console.SetActive(true);
    }

    public void LoadLevel(int indexLevel)
    {
        panel_LoadLevel.SetActive(true);
        PlayerPrefs.SetInt("level", indexLevel);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("LevelScen");
    }

    public void LoadSettings()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume_glob", 1f);
        GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume_music", 1f);
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qual_set", 3), true);

        bool fullScr = true;
        if (PlayerPrefs.GetInt("full_scr", 1) == 0) fullScr = false;
        SetScreenSettings(PlayerPrefs.GetString("scr_res", "1366x768"), fullScr);
    }

    private void SetScreenSettings(string resolution, bool fullScreen_)
    {
        Screen.SetResolution(System.Convert.ToInt32(resolution.Split('x')[0]), System.Convert.ToInt32(resolution.Split('x')[1]), fullScreen_);
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Button_Exit();
    }

    public void Button_Exit()
    {
        click_sound.Play();
        Application.Quit();
    }

    private void LoadData()
    {
        int le = PlayerPrefs.GetInt("level_c", 1);

        foreach (GameObject i in level_panels) i.GetComponent<UnityEngine.UI.Button>().interactable = false;

        for(int i = 0; i<level_panels.Length; i++)
        {
            if (i + 1 <= le)
            {
                level_panels[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
            }
        }

    }

    public void ConsoleApply()
    {
        if (InputField_console.text.Contains("/level "))
        {
            PlayerPrefs.SetInt("level_c", System.Convert.ToInt32(InputField_console.text.Split("/level ")[1]));
            PlayerPrefs.Save();
            LoadData();
        }else if (InputField_console.text.Contains("/demos "))
        {
            PlayerPrefs.SetInt("demos", System.Convert.ToInt32(InputField_console.text.Split("/demos ")[1]));
            PlayerPrefs.Save();
        }
    }



}
