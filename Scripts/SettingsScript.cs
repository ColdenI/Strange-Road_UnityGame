using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private OptimizatorManager optimizator;

    public Slider Volume_global;
    public Text TVolume_global;

    public Slider Volume_music;
    public Text TVolume_music;

    public Slider MouseSpeedX;
    public Text TMouseSpeedX;
    public Slider MouseSpeedY;
    public Text TMouseSpeedY;

    public Slider Camera_FOV;
    public Text TCamera_FOV;

    public Slider Ter_MapDraw;
    public Text TTer_MapDraw;

    public Slider Ter_DensityGrass;
    public Text TTer_DensityGrass;

    public Slider Ter_GrassDraw;
    public Text TTer_GrassDraw;

    public Toggle fullScreen;
    public Text ResultScreenResolution;

    public Dropdown QualityVideo;

    public Slider OptiObjDis;
    public Text TOptiObjDis;

    public Slider OptiObjTime;
    public Text TOptiObjTime;

    public Toggle OptiObjDec;

    public Toggle DebrisStat;


    void Start()
    {
        //LoadSettings();
    }

    public void UI_Updata(bool isSlider = true)
    {
        if (isSlider)
        {
            Volume_global.value = AudioListener.volume;
            Volume_music.value = GameObject.Find("Audio Source").GetComponent<AudioSource>().volume;
            MouseSpeedX.value = GetComponent<ScenManager>().MouseLook_Camera.SpeedMoveX;
            MouseSpeedY.value = GetComponent<ScenManager>().MouseLook_Camera.SpeedMoveY;
            Camera_FOV.value = GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView;
            try
            {
                Ter_MapDraw.value = Terrain.activeTerrain.basemapDistance;
                Ter_GrassDraw.value = Terrain.activeTerrain.detailObjectDistance;
                Ter_DensityGrass.value = Terrain.activeTerrain.detailObjectDensity;
            }
            catch (Exception e) { print(e); }
            QualityVideo.value = PlayerPrefs.GetInt("qual_set", 3);

            if (optimizator != null)
            {
                OptiObjDis.value = optimizator.distance;
                OptiObjTime.value = optimizator.TimeUpdate;
            }


        }

        TVolume_global.text = Convert.ToInt32(AudioListener.volume * 100).ToString();
        TVolume_music.text = Convert.ToInt32(GameObject.Find("Audio Source").GetComponent<AudioSource>().volume * 100).ToString();
        TMouseSpeedX.text = Convert.ToInt32(GetComponent<ScenManager>().MouseLook_Camera.SpeedMoveX).ToString();
        TMouseSpeedY.text = Convert.ToInt32(GetComponent<ScenManager>().MouseLook_Camera.SpeedMoveY).ToString();
        TCamera_FOV.text = Convert.ToInt32(GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView).ToString();
        try
        {
            if (optimizator != null)
            {
                TOptiObjDis.text = Convert.ToInt32(optimizator.distance).ToString();
                TOptiObjTime.text = (Convert.ToInt32(optimizator.TimeUpdate * 10) / 10f).ToString();
                OptiObjDec.isOn = optimizator.isDecorait;
            }

            TTer_MapDraw.text = Convert.ToInt32(Terrain.activeTerrain.basemapDistance).ToString();
            TTer_GrassDraw.text = Convert.ToInt32(Terrain.activeTerrain.detailObjectDistance).ToString();
            TTer_DensityGrass.text = (Convert.ToInt32(Terrain.activeTerrain.detailObjectDensity * 100) / 10f).ToString();
        }
        catch (Exception e) { print(e); }



    }

    public void LoadSettings()
    {
        //print("Load settings");
        if (GameObject.Find("OptManager") != null) optimizator = GameObject.Find("OptManager").GetComponent<OptimizatorManager>();
        else
        {
            optimizator = null;
            OptiObjDis.interactable = false;
            OptiObjTime.interactable = false;
            OptiObjDec.interactable = false;
        }


        AudioListener.volume = PlayerPrefs.GetFloat("volume_glob", 1f);
        GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume_music", 1f);

        GetComponent<ScenManager>().MouseLook_Camera.SpeedMoveX = PlayerPrefs.GetFloat("mouse_x", 4f);
        GetComponent<ScenManager>().MouseLook_Player.SpeedMoveX = PlayerPrefs.GetFloat("mouse_x", 4f);
        GetComponent<ScenManager>().MouseLook_Camera.SpeedMoveY = PlayerPrefs.GetFloat("mouse_y", 4f);
        GetComponent<ScenManager>().MouseLook_Player.SpeedMoveY = PlayerPrefs.GetFloat("mouse_y", 4f);

        GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView = PlayerPrefs.GetFloat("camera_fov", 60f);

        foreach(Terrain i in Terrain.activeTerrains) i.basemapDistance = PlayerPrefs.GetFloat("ter_md", 100f);
        foreach (Terrain i in Terrain.activeTerrains) i.detailObjectDistance = PlayerPrefs.GetFloat("ter_gd", 80f); // дальность травы
        foreach (Terrain i in Terrain.activeTerrains) i.detailObjectDensity = PlayerPrefs.GetFloat("ter_dg", .8f); // плотность травы

        bool fullScr = true;
        if (PlayerPrefs.GetInt("full_scr", 1) == 0) fullScr = false;
        SetScreenSettings(PlayerPrefs.GetString("scr_res","1366x768"), fullScr);
        ResultScreenResolution.text = PlayerPrefs.GetString("scr_res", "1366x768");
        fullScreen.isOn = fullScr;

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qual_set", 3), true);

        if(optimizator != null)
        {
            optimizator.distance = PlayerPrefs.GetFloat("opti_obj_dis", 100f);
            optimizator.TimeUpdate = PlayerPrefs.GetFloat("opti_obj_time", 2.4f);
            optimizator.isDecorait = IntToBool(PlayerPrefs.GetInt("opti_obj_dec", 1));
        }

        DebrisStat.isOn = IntToBool(PlayerPrefs.GetInt("gen_debri", 1));
        

        UI_Updata(true);
    }

    public static bool IntToBool(int val)
    {
        if (val == 0) return false;
        else return true;
    }

    public static int BoolToInt(bool val)
    {
        if (val) return 1;
        else return 0;
    }

    public void Set_DebrisStat()
    {
        PlayerPrefs.SetInt("gen_debri", BoolToInt(DebrisStat.isOn));
        PlayerPrefs.Save();
    }

    private void __set_postProcessing(bool val)
    {
        
    }

    public void Set_OptiObjDec()
    {
        optimizator.isDecorait = OptiObjDec.isOn;
        optimizator.DecorUpdata();
        PlayerPrefs.SetInt("opti_obj_dec", BoolToInt(optimizator.isDecorait));
        PlayerPrefs.Save();
        UI_Updata(false);
    }

    public void SetQuality()
    {
        if (QualityVideo.value < QualityVideo.options.Count)
        {
            QualitySettings.SetQualityLevel(QualityVideo.value, true);
            PlayerPrefs.SetInt("qual_set", QualityVideo.value);
            PlayerPrefs.Save();
        }
    }

    public void SetScreenResolutionAndIsFullScreen()
    {
        SetScreenSettings(ResultScreenResolution.text, fullScreen.isOn);
        if (fullScreen.isOn) PlayerPrefs.SetInt("full_scr", 1);
        else PlayerPrefs.SetInt("full_scr", 0);
        PlayerPrefs.SetString("scr_res", ResultScreenResolution.text);
        PlayerPrefs.Save();
    }

    private void SetScreenSettings(string resolution, bool fullScreen_)
    {
        Screen.SetResolution(Convert.ToInt32(resolution.Split('x')[0]), Convert.ToInt32(resolution.Split('x')[1]), fullScreen_);
    }

    public void Set_TerDensityGrass()
    {
        PlayerPrefs.SetFloat("ter_dg", Ter_DensityGrass.value);
        PlayerPrefs.Save();
        foreach (Terrain i in Terrain.activeTerrains) i.detailObjectDensity = Ter_DensityGrass.value;
        UI_Updata(false);
    }

    public void Set_OptiObjDis()
    {
        optimizator.distance = OptiObjDis.value;
        PlayerPrefs.SetFloat("opti_obj_dis", optimizator.distance);
        PlayerPrefs.Save();
        UI_Updata(false);
    }

    public void Set_OptiObjTime()
    {
        optimizator.TimeUpdate = OptiObjTime.value;
        PlayerPrefs.SetFloat("opti_obj_time", optimizator.TimeUpdate);
        PlayerPrefs.Save();
        UI_Updata(false);
    }

    public void Set_TerGrassDraw()
    {
        PlayerPrefs.SetFloat("ter_gd", Ter_GrassDraw.value);
        PlayerPrefs.Save();
        foreach (Terrain i in Terrain.activeTerrains) i.detailObjectDistance = Ter_GrassDraw.value;
        UI_Updata(false);
    }

    public void Set_TerMapDraw()
    {
        PlayerPrefs.SetFloat("ter_md", Ter_MapDraw.value);
        PlayerPrefs.Save();
        foreach (Terrain i in Terrain.activeTerrains) i.basemapDistance = Ter_MapDraw.value;
        UI_Updata(false);
    }

    public void Set_VolumeGlobal()
    {
        PlayerPrefs.SetFloat("volume_glob", Volume_global.value);
        PlayerPrefs.Save();
        AudioListener.volume = Volume_global.value;
        UI_Updata(false);
    }

    public void Set_VolumeMusic()
    {
        PlayerPrefs.SetFloat("volume_music", Volume_music.value);
        PlayerPrefs.Save();
        GameObject.Find("Audio Source").GetComponent<AudioSource>().volume = Volume_music.value;
        UI_Updata(false);
    }

    public void Set_Camera_FOV()
    {
        PlayerPrefs.SetFloat("camera_fov", Camera_FOV.value);
        PlayerPrefs.Save();
        GameObject.Find("Main Camera").GetComponent<Camera>().fieldOfView = Camera_FOV.value;
        UI_Updata(false);
    }

    public void Set_MouseSpeedX()
    {
        PlayerPrefs.SetFloat("mouse_x", MouseSpeedX.value);
        PlayerPrefs.Save();
        GetComponent<ScenManager>().MouseLook_Camera.SpeedMoveX = MouseSpeedX.value;
        GetComponent<ScenManager>().MouseLook_Player.SpeedMoveX = MouseSpeedX.value;
        UI_Updata(false);
    }

    public void Set_MouseSpeedY()
    {
        PlayerPrefs.SetFloat("mouse_y", MouseSpeedY.value);
        PlayerPrefs.Save();
        GetComponent<ScenManager>().MouseLook_Camera.SpeedMoveY = MouseSpeedY.value;
        GetComponent<ScenManager>().MouseLook_Player.SpeedMoveY = MouseSpeedY.value;
        UI_Updata(false);
    }




}
