using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float Speed = 4f;                            // скорость
    [SerializeField] private float SpeedRunning = 7f;                     // скорость бега
    [SerializeField] public  float JumpSpeed = 6f;                        // скорость прыжка
    [SerializeField] private float Gravity = 20f;                         // гравитация
    [SerializeField] private float SpeedRecovery_Stamina = 0.1f;          // скорость восстановления выносливости
    [SerializeField] private float SpeedRecovery_Live = 0.1f;             // скорость восстановления здоровья
    [SerializeField] private float SpeedSpending_Run = 0.08f;             // скорость траты выносливости при ходьбе
    [SerializeField] private float SpeedSpending_Running = 0.12f;         // скорость траты выносливости при беге
    [SerializeField] private float SpeedSpending_UsingAxe = 3f;        // скорость траты выносливости при рубке дерева
    [SerializeField] private float SpeedSpending_UsingPicaxe = 3f;        // 
    [SerializeField] private float Delay_UsingAxe = 1.5f;                   // задержка при рубке дерева
    [SerializeField] private float MinDiatanceToGoal = 3f;                   // задержка при рубке дерева
    [SerializeField] private float Coeff_OfLogOfTree = .7f;                   // задержка при рубке дерева
    [SerializeField] private float Coeff_OfLogOfStone = .7f;
    [SerializeField] private float Delay_UsingPicaxe = 1.5f;
    [SerializeField] private float TimeActivePotion = 20f;          // длительность работы зелья


    public TMP_Text help_lable;     // объект для отображения подсказок
    public GameObject Aim;      // объект для прицела
    public GameObject Aim_;      // объект для прицела 1
    public Image StaminaBar;    // объект для отображения выносливости
    public Image LiveBar;       // объект для отображения здоровья
    public GameObject ListTextPanel;      // объект для бумаги с текстом
    public GameObject GamePanel;      // объект для UI игрока
    public GameObject QestPanel;      // объект для UI игрока
    public GameObject Inventar_panel; // щбъект инвентарь
    public GameObject GameOver_panel;
    public Text TextListMessage;    // объект UI для отображеня текста с листков
    public Text TextQuestName;    // объект UI для отображеня текста названия квеста
    public Text TextQuestText;    // объект UI для отображеня текста текст квеста
    public Light Lantern;       // объект фонарик
    public Animator PlayerItemToolAnimator;
    public ScrollRect ListMassageView;
    public GameObject BlootEffect;
    public GameObject UI_Effect_potion_speed;
    public GameObject UI_Effect_potion_zombie;
    public GameObject UI_Effect_potion_power;
    public GameObject UI_Effect_potion_jump;
    public GameObject ZombiePotionEffectPanel;

    //fire panel
    public GameObject Fire_panel;
    public Slider fire_p_slider;  // 
    public Button fire_p_butt_fire; // если есть спички
    public Button fire_p_butt_off;
    public Text fire_p_text_stat;
    public Text fire_p_text_count_wood;


    // предметы
    public bool Axe = false;
    public bool Picaxe = false;
    public int Meat = 0;
    public int Stone = 0;
    public int OreRed = 0;
    public int OreGreen = 0;
    public int OreBlue = 0;
    public int Wood = 0;
    public int Bucket = 0;
    public int TNT = 0;
    public int Rope = 0;
    public int Matchbox = 0;
    public int GlassBottle = 0;
    public int MushroomYellow = 0;
    public int MushroomRed = 0;
    public int FriedMeat = 0;
    public int PotionZombie = 0;
    public int PotionSpeed = 0;
    public int PotionPower = 0;
    public int PotionJump = 0;
    public int PotionLive = 0;
    public bool Knife = false;
    public List<string> Keys = new List<string>();
    public bool Fox = false;

    public bool PotionZombieAct = false;
    public bool PotionSpeedAct = false;
    public bool PotionPowerAct = false;
    public bool PotionJumpAct = false;


    // звуки
    public AudioSource event_Sound;
    public AudioSource axe_Sound;
    public AudioSource damage_Sound;
    public AudioSource click_Sound;
    public AudioSource low_Sound;
    public AudioSource gameOver_Sound;
    public AudioSource picaxe_Sound;
    public AudioSource drinkPotion_Sound;
    public AudioSource eatMeat_Sound;


    [SerializeField] private int QuestIndex = 0;
    private bool isRunning = false;
    private bool isUsingAxe = true;
    private bool isQuest_2 = false;
    private bool isQuest_3 = false;
    private bool isMake = true;
    public float Stamina = 100f;
    public float Live = 100f;
    public bool isMove = true;
    private bool isDEMOS = false;
    public bool isMOveLader = false;
    public Vector3 CheckPoint;
    private int Coeff_DamagePower = 1;
    public Vector3 moveDir = Vector3.zero;
    public CharacterController controller;
    private ScenManager ScenManager;
    private FireObject NowFireObj;

    // инвентарь
    public GameObject
        I_Axe,
        I_Wood,
        I_Bucket,
        I_TNT,
        I_Matchbox,
        I_Knife,
        I_Rope,
        I_Picaxe,
        I_Stone,
        I_OreRed,
        I_OreGreen,
        I_OreBlue,
        I_MushroomRed,
        I_MushroomYellow,
        I_GlassBottle,
        I_PotionZombie,
        I_PotionSpeed,
        I_PotionPower,
        I_PotionJump,
        I_PotionLive,
        I_FriedMeat,
        I_Meat;
    public Text
        IT_Wood,
        IT_Meat,
        IT_TNT,
        IT_Stone,
        IT_OreRed,
        IT_OreGreen,
        IT_OreBlue,
        IT_Rope,
        IT_Matchbox,
        IT_MushroomRed,
        IT_MushroomYellow,
        IT_GlassBottle,
        IT_PotionZombie,
        IT_PotionSpeed,
        IT_PotionPower,
        IT_PotionJump,
        IT_PotionLive,
        IT_FriedMeat,
        IT_Bucket;

    private void ChechDemosMode()
    {
        if(PlayerPrefs.GetInt("demos", 0) == 1)
        {
            isDEMOS = true;

            Axe = true;
            Picaxe = true;
            Meat = 9999;
            Stone = 9999;
            OreRed = 9999;
            OreGreen = 9999;
            OreBlue = 9999;
            Wood = 9999;
            Bucket = 9999;
            TNT = 9999;
            Rope = 9999;
            Matchbox = 9999;
            MushroomYellow = 9999;
            MushroomRed = 9999;
            GlassBottle = 9999;
            PotionSpeed = 9999;
            PotionZombie = 9999;
            PotionLive = 9999;
            PotionPower = 9999;
            PotionJump = 9999;
            FriedMeat = 9999;
            Knife = true;

            SpeedRunning = 12f;
            JumpSpeed = 15f;
            SpeedRecovery_Stamina = 10f;
            SpeedRecovery_Live = 10f;
            Delay_UsingPicaxe = .1f;
            Delay_UsingAxe =.1f;
            SpeedSpending_Running = .01f;     
            SpeedSpending_UsingAxe = .1f;        
            SpeedSpending_UsingPicaxe = .1f;
        }
    }


    void Start()
    {
        controller = GetComponent<CharacterController>();
        ScenManager = GameObject.Find("ScenManager").GetComponent<ScenManager>();
        Lantern.gameObject.SetActive(false);

        Inventar_Updata();
        SetQuest(QuestIndex);
        ChechDemosMode();
        Update_UI_Effect_potion();
    }

    public void CloseInventar()
    {
        GamePanel.SetActive(true);
        Inventar_panel.SetActive(false);
        ScenManager.CursorUnVisible();
        click_Sound.Play();
        Time.timeScale = 1f;
    }

    public void CloseFirePanel()
    {
        Fire_panel.SetActive(false);
        click_Sound.Play();
        ScenManager.CursorUnVisible();
        NowFireObj = null;
    }

    public void OpenFirePanel()
    {
        Fire_panel.SetActive(true);
        click_Sound.Play();
        ScenManager.CursorVisible();
        fire_p_slider.maxValue = Wood;
        fire_p_slider.value = 0;
        if (Matchbox > 0 && !NowFireObj.isWork && NowFireObj.fuil > 0) fire_p_butt_fire.interactable = true;
        else fire_p_butt_fire.interactable = false;
        if (NowFireObj.isWork && Bucket > 0) fire_p_butt_off.interactable = true;
        else fire_p_butt_off.interactable = false;
        {
            string str = "Сейчас: ";
            if (NowFireObj.isWork) str += "горит";
            else str += "не горит";
            str += "\nБревен: " + NowFireObj.fuil.ToString();
            fire_p_text_stat.text = str;
        }


    }

    public void fire_slider_event()
    {
        fire_p_text_count_wood.text = fire_p_slider.value.ToString();
    }

    public void fire_button_add_wood()
    {
        if (NowFireObj == null) return;
        click_Sound.Play();
        NowFireObj.fuil += Convert.ToInt32(fire_p_slider.value);
        Wood -= Convert.ToInt32(fire_p_slider.value);
        CloseFirePanel();
    }

    public void fire_button_set_work(bool set)
    {
        if (NowFireObj == null) return;
        click_Sound.Play();

        if (set)
        {
            Matchbox--;
            NowFireObj.StartFire();
        }
        else
        {
            Bucket--;
            NowFireObj.FireStop();
        }
        CloseFirePanel();
    }

    public void fire_button_make_meat()
    {
        if (NowFireObj.isWork && NowFireObj.fuil >= 5 && Meat >= 1)
        {
            NowFireObj.fuil -= 3;
            FriedMeat++;
            Meat--;
            event_Sound.Play();
            CloseFirePanel();
        }
    }

    public void fire_button_make_potion_speed()
    {
        if (NowFireObj.isWork && NowFireObj.fuil >= 5 && GlassBottle >= 1 && MushroomYellow >= 2 && Bucket >= 1)
        {
            NowFireObj.fuil -= 3;
            PotionSpeed++;
            MushroomYellow -= 2;
            GlassBottle--;
            Bucket--;
            event_Sound.Play();
            CloseFirePanel();
        }
    }

    public void fire_button_make_potion_zombie()
    {
        if (NowFireObj.isWork && NowFireObj.fuil >= 5 && GlassBottle >= 1 && MushroomRed >= 3 && Bucket >= 1)
        {
            NowFireObj.fuil -= 3;
            PotionZombie++;
            MushroomRed -= 3;
            GlassBottle--;
            Bucket--;
            event_Sound.Play();
            CloseFirePanel();
        }
    }

    public void fire_button_make_potion_jump()
    {
        if (NowFireObj.isWork && NowFireObj.fuil >= 5 && GlassBottle >= 1 && OreBlue >= 3 && Bucket >= 1)
        {
            NowFireObj.fuil -= 3;
            PotionJump++;
            OreBlue -= 3;
            GlassBottle--;
            Bucket--;
            event_Sound.Play();
            CloseFirePanel();
        }
    }

    public void fire_button_make_potion_power()
    {
        if (NowFireObj.isWork && NowFireObj.fuil >= 5 && GlassBottle >= 1 && OreRed >= 5 && Bucket >= 1)
        {
            NowFireObj.fuil -= 3;
            PotionPower++;
            OreRed -= 5;
            GlassBottle--;
            Bucket--;
            event_Sound.Play();
            CloseFirePanel();
        }
    }

    public void fire_button_make_potion_live()
    {
        if (NowFireObj.isWork && NowFireObj.fuil >= 5 && GlassBottle >= 1 && OreGreen >= 2 && Bucket >= 1)
        {
            NowFireObj.fuil -= 3;
            PotionLive++;
            OreGreen -= 2;
            GlassBottle--;
            Bucket--;
            event_Sound.Play();
            CloseFirePanel();
        }
    }

    public void GameOver()
    {
        // gameOver_Sound.Play();       
        ScenManager.CursorVisible();
        GameOver_panel.SetActive(true);
        //Time.timeScale = 0f;
    }

    void Update()
    {
        // логика смерти
        if(Live <= 1)
        {
            GameOver();
            GameObject.Find("TextGameOverThen").GetComponent<UnityEngine.UI.Text>().text = "Прийдется начать заново...";
        }

        // Логика бега
        if (Input.GetKeyDown(KeyCode.Tab)) isRunning = !isRunning;

        // логика фонарика
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lantern.gameObject.SetActive(!Lantern.isActiveAndEnabled);
            click_Sound.Play();
        }

        // логика квест панели
        if (Input.GetKeyDown(KeyCode.Q) && GamePanel.activeSelf) 
        {
            QestPanel.SetActive(!QestPanel.activeSelf);
            click_Sound.Play();
        }

        /*
        // логика инвентарь панели
        if (Input.GetKeyDown(KeyCode.E) && !Inventar_panel.activeSelf && GamePanel.activeSelf && !Fire_panel.activeSelf && !GameObject.Find("TimelineManager").GetComponent<CutSceneManager>().flag)
        {
            Inventar_Updata();
            GamePanel.SetActive(false);
            Inventar_panel.SetActive(true);
            ScenManager.CursorVisible();
            click_Sound.Play();
            Time.timeScale = 0f;
        }
        */
        

        // отслежисание для надписи
        RaycastHit hit;
        Ray ray = new Ray(Aim_.transform.position, Aim.transform.position - Aim_.transform.position);
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject != Aim.gameObject && Vector3.Distance(Aim_.transform.position, hit.collider.gameObject.transform.position) < MinDiatanceToGoal)
            {
                if (hit.collider.GetComponent<StaminaPotion>()) help_lable.text = "Нажмите F чтобы использовать";
                else if (hit.collider.GetComponent<AxeItem>() && !Axe && (isQuest_2 || PlayerPrefs.GetInt("level") != 1)) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<TreeObject>() && Axe && isUsingAxe) help_lable.text = "Нажмите F чтобы рубить";
                else if (hit.collider.GetComponent<FoxController>() && !hit.collider.GetComponent<FoxController>().isFrendly) help_lable.text = "Нажмите F чтобы приручить";
                else if (hit.collider.GetComponent<ListMessageObject>()) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<Key_1_Object>()) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<DoorObject>() && (Keys.Contains(hit.collider.GetComponent<DoorObject>().Key) || !hit.collider.GetComponent<DoorObject>().isNeedKey)) help_lable.text = "Нажмите F чтобы открыть";
                else if (hit.collider.GetComponent<DoorObject>() && !Keys.Contains(hit.collider.GetComponent<DoorObject>().Key)) help_lable.text = "Нужен ключ";
                else if (hit.collider.GetComponent<DoorVertObject>() && !hit.collider.GetComponent<DoorVertObject>().isAutomatic && (Keys.Contains(hit.collider.GetComponent<DoorVertObject>().key) || !hit.collider.GetComponent<DoorVertObject>().isNeedKey) && !hit.collider.GetComponent<DoorVertObject>().isOpen) help_lable.text = "Нажмите F чтобы открыть";
                else if (hit.collider.GetComponent<DoorVertObject>() && !hit.collider.GetComponent<DoorVertObject>().isAutomatic && !Keys.Contains(hit.collider.GetComponent<DoorVertObject>().key) && !hit.collider.GetComponent<DoorVertObject>().isOpen) help_lable.text = "Нужен ключ";
                else if (hit.collider.GetComponent<DoorVertObject>() && hit.collider.GetComponent<DoorVertObject>().isAutomatic && !hit.collider.GetComponent<DoorVertObject>().isOpen) help_lable.text = "Нужен рычаг";
                else if (hit.collider.GetComponent<MeatObject>()) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<FoxController>() && hit.collider.GetComponent<FoxController>().isFrendly && hit.collider.GetComponent<FoxController>().mode == FoxController.Mode.IdleAndSit) help_lable.text = "Нажмите F чтобы - за мной";
                else if (hit.collider.GetComponent<FoxController>() && hit.collider.GetComponent<FoxController>().isFrendly && hit.collider.GetComponent<FoxController>().mode == FoxController.Mode.Idle) help_lable.text = "Нажмите F чтобы - за мной";
                else if (hit.collider.GetComponent<FoxController>() && hit.collider.GetComponent<FoxController>().isFrendly && hit.collider.GetComponent<FoxController>().mode == FoxController.Mode.Sit) help_lable.text = "Нажмите F чтобы - за мной";
                else if (hit.collider.GetComponent<FoxController>() && hit.collider.GetComponent<FoxController>().isFrendly && hit.collider.GetComponent<FoxController>().mode == FoxController.Mode.Harassment) help_lable.text = "Нажмите F чтобы - сидеть";
                else if (hit.collider.GetComponent<LivePotion>()) help_lable.text = "Нажмите F чтобы использовать";
                else if (hit.collider.GetComponent<RockBarier_1>() && hit.collider.GetComponent<RockBarier_1>().isActive && TNT > 0) help_lable.text = "Нажмите F чтобы взорвать";
                else if (hit.collider.GetComponent<RockBarier_1>() && hit.collider.GetComponent<RockBarier_1>().isActive && TNT <= 0) help_lable.text = "Нужен динамит";
                else if (hit.collider.GetComponent<MatchboxObject>()) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<TNTObject>()) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<KnifeObject>() && !Knife) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<PitObject>() && hit.collider.GetComponent<PitObject>().isWork && isMake) help_lable.text = "Нажмите F чтобы набрать воды";
                else if (hit.collider.GetComponent<PitObject>() && !hit.collider.GetComponent<PitObject>().isWork && Knife && Rope > 0) help_lable.text = "Нажмите F чтобы починить";
                else if (hit.collider.GetComponent<PitObject>() && !hit.collider.GetComponent<PitObject>().isWork && !Knife) help_lable.text = "Нужен нож";
                else if (hit.collider.GetComponent<PitObject>() && !hit.collider.GetComponent<PitObject>().isWork && Knife && Rope <= 0) help_lable.text = "Нужна верёвка";
                else if (hit.collider.GetComponent<RopeObject>()) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<FireObject>()) help_lable.text = "Нажмите F чтобы использовать";
                else if (hit.collider.GetComponent<WardrobeDoorObject>() && (Keys.Contains(hit.collider.GetComponent<WardrobeDoorObject>().Key) || !hit.collider.GetComponent<WardrobeDoorObject>().isNeedKey)) help_lable.text = "Нажмите F чтобы открыть";
                else if (hit.collider.GetComponent<WardrobeDoorObject>() && !Keys.Contains(hit.collider.GetComponent<WardrobeDoorObject>().Key)) help_lable.text = "Нужен ключ";
                else if (hit.collider.GetComponent<WoodObject>()) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<BearController>() && hit.collider.GetComponent<BearController>().Live > 0 && (Axe || Knife)) help_lable.text = "Нажмите F чтобы атаковать";
                else if (hit.collider.GetComponent<BearController>() && hit.collider.GetComponent<BearController>().Live > 0 && !(Axe || Knife)) help_lable.text = "Нужен топор или нож";
                else if (hit.collider.GetComponent<ZombieScript>() && hit.collider.GetComponent<ZombieScript>().Live > 0 && (Axe || Knife)) help_lable.text = "Нажмите F чтобы атаковать";
                else if (hit.collider.GetComponent<ZombieScript>() && hit.collider.GetComponent<ZombieScript>().Live > 0 && !(Axe || Knife)) help_lable.text = "Нужен топор или нож";
                else if (hit.collider.GetComponent<TrolController>() && hit.collider.GetComponent<TrolController>().Live > 0 && (Axe || Knife)) help_lable.text = "Нажмите F чтобы атаковать";
                else if (hit.collider.GetComponent<TrolController>() && hit.collider.GetComponent<TrolController>().Live > 0 && !(Axe || Knife)) help_lable.text = "Нужен топор или нож";
                else if (hit.collider.GetComponent<EndLevelObject>()) help_lable.text = "Нажмите F чтобы ЗАВЕРШИТЬ УРОВЕНЬ";
                else if (hit.collider.GetComponent<PicaxeObject>() && !Picaxe) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<LempObject>() && hit.collider.GetComponent<LempObject>().isWork) help_lable.text = "Нажмите F чтобы потушить";
                else if (hit.collider.GetComponent<LempObject>() && !hit.collider.GetComponent<LempObject>().isWork && (Matchbox > 0)) help_lable.text = "Нажмите F чтобы зажечь";
                else if (hit.collider.GetComponent<LempObject>() && !hit.collider.GetComponent<LempObject>().isWork && !(Matchbox > 0)) help_lable.text = "Нужны спички";
                else if (hit.collider.GetComponent<OrgStoneObject>() && isMake && Picaxe) help_lable.text = "Нажмите F чтобы копать";
                else if (hit.collider.GetComponent<DestroyerObject>() && hit.collider.GetComponent<DestroyerObject>().Flag) help_lable.text = "Нажмите F чтобы " + hit.collider.GetComponent<DestroyerObject>().TextOut;
                else if (hit.collider.GetComponent<SystemPowerDestrotyObj>() && hit.collider.GetComponent<SystemPowerDestrotyObj>().flag && (OreRed >= 10 && OreBlue >= 20 && OreGreen >= 5)) help_lable.text = "Нажмите F чтобы отключить";
                else if (hit.collider.GetComponent<SystemPowerDestrotyObj>() && hit.collider.GetComponent<SystemPowerDestrotyObj>().flag && !(OreRed >= 10 && OreBlue >= 20 && OreGreen >= 5)) help_lable.text = "Нужны кристалы";
                else if (hit.collider.GetComponent<SystemPowerDestrotyObj>() && !hit.collider.GetComponent<SystemPowerDestrotyObj>().flag) help_lable.text = "Система отключена";
                else if (hit.collider.GetComponent<SunTemple.Door>() && !hit.collider.GetComponent<SunTemple.Door>().IsLocked) help_lable.text = "Нажмите F чтобы открыть";
                else if (hit.collider.GetComponent<LeverForDoorObject>() && !hit.collider.GetComponent<LeverForDoorObject>().isOff) help_lable.text = "Нажмите F чтобы открыть";
                else if (hit.collider.GetComponent<MushroomRed>() && Knife) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<MushroomYellowObject>() && Knife) help_lable.text = "Нажмите F чтобы подобрать";
                else if ((hit.collider.GetComponent<MushroomRed>() || hit.collider.GetComponent<MushroomYellowObject>()) && !Knife) help_lable.text = "Нужен нож";
                else if (hit.collider.GetComponent<GlassBottleObject>()) help_lable.text = "Нажмите F чтобы подобрать";
                else if (hit.collider.GetComponent<PotionItemObject>()) help_lable.text = "Нажмите F чтобы подобрать";

                //else help_lable.text = hit.collider.name.ToString();
                else help_lable.text = "";
            }
            else help_lable.text = "";
        }
        else help_lable.text = "";

        if(isDEMOS) help_lable.text += "\nDEMOS mode - on";
    }

    IEnumerator TimerUsingAxeReboot()
    {
        yield return new WaitForSeconds(Delay_UsingAxe);
        isUsingAxe = true;
    }

    IEnumerator TimerMake(float delay)
    {
        yield return new WaitForSeconds(delay);
        isMake = true;
    }

    private void Move()
    {
        // Движение по поверхности
        if (controller.isGrounded)
        {

            if (!isMove)
            {
                moveDir = new Vector3(0, 0, 0);
                moveDir = transform.TransformDirection(moveDir);
            }
            else
            {
                moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                moveDir = transform.TransformDirection(moveDir);
            }

            if (isRunning) moveDir *= SpeedRunning; // бег
            else moveDir *= Speed; // ходьба

        }

        if (isRunning && Input.GetAxis("Horizontal") != 0) isRunning = false;
        if (isRunning && Input.GetAxis("Vertical") < 0) isRunning = false;

        if (Stamina <= 5) low_Sound.Play();

        // Прыжок
        if (Input.GetKey(KeyCode.Space) && controller.isGrounded && Stamina >= 5 && isMove)
        {
            moveDir.y = JumpSpeed;
            Stamina -= 5;
        }

        // Гравитация
        moveDir.y -= Gravity * Time.deltaTime;


        // Передвижение 
        if (Stamina > 5)
        {
            controller.Move(moveDir * Time.deltaTime);
            if (isRunning) Stamina -= SpeedSpending_Running; // бег
            else Stamina -= SpeedSpending_Run; // ходьба

        }

        if (transform.position.y < -1000)
        {
            Live -= 100.1f;
            controller.enabled = false;
        }
    }

    void FixedUpdate()
    {
        Move();
        Update_UI_Effect_potion();

        // работа с UI
        StaminaBar.fillAmount = Stamina / 100f;
        LiveBar.fillAmount = Live / 100f;


        // востановление здоровья
        if (Live < 100) Live += SpeedRecovery_Live;
        if (Live > 100) Live = 100;


        // востановление выносливости
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && Stamina < 100)
        {
            Stamina += SpeedRecovery_Stamina;
            isRunning = false;
        }
        if (Stamina > 100) Stamina = 100;


        // Прицеливание
        if ((Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Mouse0)) && isMove)
        {        
            RaycastHit hit;//сюда запишется инфо о пересечении луча, если оно будет        
            Ray ray = new Ray(Aim_.transform.position, Aim.transform.position - Aim_.transform.position); //сам луч, начинается от позиции этого объекта и направлен в сторону цели      
            Physics.Raycast(ray, out hit);  //пускаем луч         
            if (hit.collider != null) //если луч с чем-то пересёкся, то..
            {
                //если луч не попал в цель
                if (hit.collider.gameObject != Aim.gameObject && Vector3.Distance(Aim_.transform.position, hit.collider.gameObject.transform.position) < MinDiatanceToGoal)
                {
                    //Debug.Log(hit.collider.name);
                    if (hit.collider.GetComponent<StaminaPotion>())
                    {
                        event_Sound.Play();
                        Stamina += hit.collider.GetComponent<StaminaPotion>().PowerItem;               
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<AxeItem>())
                    {
                        if (PlayerPrefs.GetInt("level") == 1)
                        {
                            if (isQuest_2)
                            {
                                event_Sound.Play();
                                Axe = true;
                                Inventar_Updata();
                                ViewListMessage("Вы нашли топор.\nНарубите немного дерева.\n\nНовое задание.Немного дерева.Срубить 3 дерева.");
                                isQuest_2 = false;
                                SetQuest(2);
                                Destroy(hit.collider.gameObject);
                            }
                        }
                        else
                        {
                            event_Sound.Play();
                            Axe = true;
                            Destroy(hit.collider.gameObject);
                        }

                    }
                    else if (hit.collider.GetComponent<PicaxeObject>() && isMake)
                    {
                        Picaxe = true;
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                        StartCoroutine(TimerMake(.4f));
                    }
                    else if (hit.collider.GetComponent<TreeObject>())
                    {
                        if (Axe && (Stamina > SpeedSpending_UsingAxe + 1) && isUsingAxe) {
                            axe_Sound.Play();                 
                            Stamina -= SpeedSpending_UsingAxe;
                            isUsingAxe = false;
                            PlayerItemToolAnimator.SetTrigger("axe");
                            hit.collider.gameObject.transform.localScale *= .98f;
                            if (hit.collider.gameObject.transform.localScale.y < (hit.collider.gameObject.GetComponent<TreeObject>().ScaleDefault.y * Coeff_OfLogOfTree)) 
                            {
                                Wood+=4;
                                Inventar_Updata();
                                if (Wood == 12 && isQuest_2)
                                {
                                    ViewListMessage("Замечательно. Можно развести костер.\nНо нужно найти место...");
                                    SetQuest(0);
                                    event_Sound.Play();
                                    isQuest_2 = false;
                                }
                                Destroy(hit.collider.gameObject); 
                            }
                            StartCoroutine(TimerUsingAxeReboot());
                        }
                    }
                    else if (hit.collider.GetComponent<FoxController>())
                    {
                        // если враг
                        if (!hit.collider.gameObject.GetComponent<FoxController>().isFrendly && isMake)
                        {
                            if (Meat >= 1)
                            {
                                Meat--;
                                Fox = true;
                                event_Sound.Play();
                                hit.collider.gameObject.GetComponent<FoxController>().Effect();
                                isMake = false;
                                StartCoroutine(TimerMake(.5f));
                                ViewListMessage("Замечательно. ");
                                SetQuest(0);

                            }
                            else
                            {
                                if (isQuest_3)
                                {
                                    isQuest_3 = false;
                                    ViewListMessage("Замечательно. \nЯ не знаю почему, но лису я назвал Вайт. \nНо зверь не очень дружелюбен… \nДумаю стоит разыскать что-то съедобное для лисы.");
                                    SetQuest(4);
                                    event_Sound.Play();
                                }
                                damage_Sound.Play();
                                hit.collider.gameObject.GetComponent<FoxController>().Dan_3_Atack();
                                Live -= hit.collider.gameObject.GetComponent<FoxController>().Damage;
                                isMake = false;
                                StartCoroutine(TimerMake(.5f));
                            }

                        }
                        else if (isMake)
                        {
                            isMake = false;
                            
                            if (hit.collider.gameObject.GetComponent<FoxController>().mode == FoxController.Mode.Harassment) hit.collider.gameObject.GetComponent<FoxController>().mode = FoxController.Mode.Sit;
                            else if (hit.collider.gameObject.GetComponent<FoxController>().mode == FoxController.Mode.Sit) hit.collider.gameObject.GetComponent<FoxController>().mode = FoxController.Mode.Harassment;
                            else if (hit.collider.gameObject.GetComponent<FoxController>().mode == FoxController.Mode.Idle) hit.collider.gameObject.GetComponent<FoxController>().mode = FoxController.Mode.Harassment;
                            else if (hit.collider.gameObject.GetComponent<FoxController>().mode == FoxController.Mode.IdleAndSit) hit.collider.gameObject.GetComponent<FoxController>().mode = FoxController.Mode.Harassment;
                            
                            StartCoroutine(TimerMake(.5f));
                        }

                    }
                    else if (hit.collider.GetComponent<ListMessageObject>())
                    {
                        ViewListMessage(hit.collider.GetComponent<ListMessageObject>().Text);
                        EventListM(hit.collider.GetComponent<ListMessageObject>().ID);
                        event_Sound.Play();
                        if(hit.collider.GetComponent<ListMessageObject>().isDestroy) Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<Key_1_Object>())
                    {
                        Keys.Add(hit.collider.GetComponent<Key_1_Object>().Key);
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<DoorObject>())
                    {
                        if (hit.collider.GetComponent<DoorObject>().isNeedKey && isMake)
                        {
                            if (Keys.Contains(hit.collider.GetComponent<DoorObject>().Key))
                            {
                                hit.collider.GetComponent<DoorObject>().Invers();
                                isMake = false;
                                StartCoroutine(TimerMake(1f));
                            }
                        }
                        else if (isMake)
                        {
                            
                            hit.collider.GetComponent<DoorObject>().Invers();
                            isMake = false;
                            StartCoroutine(TimerMake(1.5f));
                        }

                    }
                    else if (hit.collider.GetComponent<DoorVertObject>())
                    {
                        if (hit.collider.GetComponent<DoorVertObject>().isNeedKey && isMake && !hit.collider.GetComponent<DoorVertObject>().isOpen && !hit.collider.GetComponent<DoorVertObject>().isAutomatic)
                        {
                            if (Keys.Contains(hit.collider.GetComponent<DoorVertObject>().key))
                            {
                                hit.collider.GetComponent<DoorVertObject>().Open();
                                isMake = false;
                                StartCoroutine(TimerMake(1.5f));
                            }
                        }
                        else if (isMake && !hit.collider.GetComponent<DoorVertObject>().isOpen && !hit.collider.GetComponent<DoorVertObject>().isAutomatic)
                        {

                            hit.collider.GetComponent<DoorVertObject>().Open();
                            isMake = false;
                            StartCoroutine(TimerMake(1.5f));
                        }

                    }
                    else if (hit.collider.GetComponent<MeatObject>())
                    {
                        Meat++;
                        Inventar_Updata();
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<LivePotion>())
                    {
                        Live += hit.collider.GetComponent<LivePotion>().Live;
                        if (Live > 100) Live = 100;
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<RockBarier_1>())
                    {
                        if (hit.collider.GetComponent<RockBarier_1>().isActive && TNT > 0)
                        {
                            TNT--;
                            hit.collider.GetComponent<RockBarier_1>().StartEfect();
                        }
                    }
                    else if (hit.collider.GetComponent<TNTObject>())
                    {
                        TNT++;
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<MatchboxObject>())
                    {
                        Matchbox+= hit.collider.GetComponent<MatchboxObject>().CountMatches;
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<RopeObject>())
                    {
                        Rope++;
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<KnifeObject>())
                    {
                        Knife = true;
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<PitObject>())
                    {
                        if (hit.collider.GetComponent<PitObject>().isWork && isMake)
                        {
                            Bucket++;
                            isMake = false;
                            event_Sound.Play();
                            StartCoroutine(TimerMake(3f));
                        }
                        else if (Knife && Rope > 0 && isMake)
                        {    
                            event_Sound.Play();
                            Rope--;
                            hit.collider.GetComponent<PitObject>().isWork = true;
                            isMake = false;
                            StartCoroutine(TimerMake(1f));
                        }   
                    }
                    else if (hit.collider.GetComponent<FireObject>())
                    {
                        NowFireObj = hit.collider.GetComponent<FireObject>();
                        OpenFirePanel();
                    }
                    else if (hit.collider.GetComponent<WardrobeDoorObject>())
                    {
                        if (hit.collider.GetComponent<WardrobeDoorObject>().isNeedKey && isMake)
                        {
                            if (Keys.Contains(hit.collider.GetComponent<WardrobeDoorObject>().Key))
                            {
                                hit.collider.GetComponent<WardrobeDoorObject>().SetDoor(!hit.collider.GetComponent<WardrobeDoorObject>().isOpen);
                                isMake = false;
                                StartCoroutine(TimerMake(.5f));
                            }
                        }
                        else if (isMake)
                        {

                            hit.collider.GetComponent<WardrobeDoorObject>().SetDoor(!hit.collider.GetComponent<WardrobeDoorObject>().isOpen);
                            isMake = false;
                            StartCoroutine(TimerMake(.5f));
                        }

                    }
                    else if (hit.collider.GetComponent<WoodObject>())
                    {
                        Wood += hit.collider.GetComponent<WoodObject>().CountWood;
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<BearController>())
                    {
                        if (Axe || Knife)
                        {
                            if(hit.collider.GetComponent<BearController>().Live > 0 && isMake)
                            {
                                hit.collider.GetComponent<BearController>().Live -= hit.collider.GetComponent<BearController>().DamageForBear * Coeff_DamagePower;
                                hit.collider.GetComponent<BearController>().Attack_s[UnityEngine.Random.Range(0, hit.collider.GetComponent<BearController>().Attack_s.Length)].Play();
                                isMake = false;
                                StartCoroutine(TimerMake(2f));

                                if (Axe && !Knife) PlayerItemToolAnimator.SetTrigger("axe");
                                if (!Axe && Knife) PlayerItemToolAnimator.SetTrigger("knife");
                                if (Axe && Knife && UnityEngine.Random.Range(1, 3) == 1) PlayerItemToolAnimator.SetTrigger("knife");
                                else if (Axe && Knife) PlayerItemToolAnimator.SetTrigger("axe");
                            }

                        }
                    }
                    else if (hit.collider.GetComponent<ZombieScript>())
                    {
                        if (Axe || Knife)
                        {
                            if (hit.collider.GetComponent<ZombieScript>().Live > 0 && isMake)
                            {
                                if (Axe && !Knife) PlayerItemToolAnimator.SetTrigger("axe");
                                if (!Axe && Knife) PlayerItemToolAnimator.SetTrigger("knife");
                                if (Axe && Knife && UnityEngine.Random.Range(1, 3) == 1) PlayerItemToolAnimator.SetTrigger("knife");
                                else if (Axe && Knife) PlayerItemToolAnimator.SetTrigger("axe");

                                hit.collider.GetComponent<ZombieScript>().Live -= hit.collider.GetComponent<ZombieScript>().DamageForZombie * Coeff_DamagePower;
                                hit.collider.GetComponent<ZombieScript>().Attack_s[UnityEngine.Random.Range(0, hit.collider.GetComponent<ZombieScript>().Attack_s.Length)].Play();
                                isMake = false;
                                Vector3 pos = hit.collider.gameObject.transform.position;
                                pos.y += 1.8f;
                                pos.x += .5f;
                                StartCoroutine(WaitAndDestroy(Instantiate(BlootEffect, pos, Quaternion.identity)));
                                if (PotionZombieAct) PotionZombieAct = false;
                                StartCoroutine(TimerMake(2f));
                            }

                        }
                    }
                    else if (hit.collider.GetComponent<TrolController>())
                    {
                        if (Axe || Knife)
                        {
                            if (hit.collider.GetComponent<TrolController>().Live > 0 && isMake)
                            {
                                if (Axe && !Knife) PlayerItemToolAnimator.SetTrigger("axe");
                                if (!Axe && Knife) PlayerItemToolAnimator.SetTrigger("knife");
                                if (Axe && Knife && UnityEngine.Random.Range(1, 3) == 1) PlayerItemToolAnimator.SetTrigger("knife");
                                else if (Axe && Knife) PlayerItemToolAnimator.SetTrigger("axe");

                                hit.collider.GetComponent<TrolController>().Live -= hit.collider.GetComponent<TrolController>().DamageForBear * Coeff_DamagePower;
                                hit.collider.GetComponent<TrolController>().Attack_s[UnityEngine.Random.Range(0, hit.collider.GetComponent<TrolController>().Attack_s.Length)].Play();
                                hit.collider.GetComponent<TrolController>().TakeDamage();
                                isMake = false;
                                Vector3 pos = hit.collider.gameObject.transform.position;
                                pos.y += 1.8f;
                                pos.x += .5f;
                                StartCoroutine(WaitAndDestroy(Instantiate(BlootEffect, pos, Quaternion.identity)));
                                if (PotionZombieAct) PotionZombieAct = false;
                                StartCoroutine(TimerMake(2f));
                            }

                        }
                    }
                    else if (hit.collider.GetComponent<EndLevelObject>())
                    {
                        ScenManager.CursorVisible();
                        hit.collider.GetComponent<EndLevelObject>().EndLevel();
                    }
                    else if (hit.collider.GetComponent<LempObject>() && isMake)
                    {
                        if (hit.collider.GetComponent<LempObject>().isWork)
                        {
                            hit.collider.GetComponent<LempObject>().Stop_L();
                            isMake = false;
                            StartCoroutine(TimerMake(.4f));
                        }
                        else if (Matchbox > 0)
                        {
                            hit.collider.GetComponent<LempObject>().Start_L();
                            Matchbox--;
                            isMake = false;
                            StartCoroutine(TimerMake(.4f));
                        }
                        
                    }
                    else if (hit.collider.GetComponent<OrgStoneObject>() && isMake && Picaxe && Stamina > SpeedSpending_UsingPicaxe)
                    {
                        isMake = false;
                        picaxe_Sound.Play();
                        Stamina -= SpeedSpending_UsingPicaxe;
                        PlayerItemToolAnimator.SetTrigger("pickaxe");

                        hit.collider.gameObject.transform.localScale *= hit.collider.gameObject.GetComponent<OrgStoneObject>().PowerDestroy;
                        if (hit.collider.gameObject.transform.localScale.y < (hit.collider.gameObject.GetComponent<OrgStoneObject>().ScaleDefault.y * Coeff_OfLogOfStone))
                        {
                            switch (hit.collider.gameObject.GetComponent<OrgStoneObject>().ResType)
                            {
                                case OrgStoneObject.Resourse.Stone: Stone += hit.collider.gameObject.GetComponent<OrgStoneObject>().CountRes; break;
                                case OrgStoneObject.Resourse.RedOrg: OreRed += hit.collider.gameObject.GetComponent<OrgStoneObject>().CountRes; break;
                                case OrgStoneObject.Resourse.BlueOrg: OreBlue += hit.collider.gameObject.GetComponent<OrgStoneObject>().CountRes; break;
                                case OrgStoneObject.Resourse.GreenOrg: OreGreen += hit.collider.gameObject.GetComponent<OrgStoneObject>().CountRes; break;
                                case OrgStoneObject.Resourse.Random: 
                                    switch(UnityEngine.Random.Range(0, 3))
                                    {
                                        case 0: Stone += hit.collider.gameObject.GetComponent<OrgStoneObject>().CountRes; break;
                                        case 1: OreRed += hit.collider.gameObject.GetComponent<OrgStoneObject>().CountRes; break;
                                        case 2: OreBlue += hit.collider.gameObject.GetComponent<OrgStoneObject>().CountRes; break;
                                        case 3: OreGreen += hit.collider.gameObject.GetComponent<OrgStoneObject>().CountRes; break;
                                    }    
                                break;
                            }

                            Destroy(hit.collider.gameObject);
                        }

                        StartCoroutine(TimerMake(Delay_UsingPicaxe));
                    }
                    else if (hit.collider.GetComponent<DestroyerObject>() && hit.collider.GetComponent<DestroyerObject>().Flag)
                    {
                        hit.collider.GetComponent<DestroyerObject>().DestroyObj();
                    }
                    else if (hit.collider.GetComponent<SystemPowerDestrotyObj>() && (OreRed >= 30 && OreBlue >= 50 && OreGreen >= 15))
                    {
                        if (hit.collider.GetComponent<SystemPowerDestrotyObj>().flag)
                        {
                            hit.collider.GetComponent<SystemPowerDestrotyObj>().Active();
                            event_Sound.Play();
                            OreBlue -= 50;
                            OreGreen -= 15;
                            OreRed -= 30;
                        }
                    }
                    else if (hit.collider.GetComponent<LeverForDoorObject>() && !hit.collider.GetComponent<LeverForDoorObject>().isOff)
                    {
                        hit.collider.GetComponent<LeverForDoorObject>().StartOn();
                    }
                    else if (hit.collider.GetComponent<MushroomRed>() && Knife)
                    {
                        MushroomRed += hit.collider.GetComponent<MushroomRed>().Count;
                        PlayerItemToolAnimator.SetTrigger("knife");
                        Destroy(hit.collider.gameObject);
                        event_Sound.Play();
                    }
                    else if (hit.collider.GetComponent<MushroomYellowObject>() && Knife)
                    {
                        MushroomYellow += hit.collider.GetComponent<MushroomYellowObject>().Count;
                        PlayerItemToolAnimator.SetTrigger("knife");
                        Destroy(hit.collider.gameObject);
                        event_Sound.Play();
                    }
                    else if (hit.collider.GetComponent<GlassBottleObject>())
                    {
                        GlassBottle++;
                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<PotionItemObject>())
                    {
                        switch (hit.collider.GetComponent<PotionItemObject>().typePotion)
                        {
                            case PotionItemObject.TypePotionItem.ZombiePotion: PotionZombie++; break;
                            case PotionItemObject.TypePotionItem.SpeedPotion: PotionSpeed++; break;
                            case PotionItemObject.TypePotionItem.LivePotion: PotionLive++; break;
                            case PotionItemObject.TypePotionItem.JumpPotion: PotionJump++; break;
                            case PotionItemObject.TypePotionItem.PowerPotion: PotionPower++; break;
                        }

                        event_Sound.Play();
                        Destroy(hit.collider.gameObject);
                    }

                }
                else
                {
                   // Debug.Log("Слишком далеко");
                }
                //  Debug.DrawLine(ray.origin, hit.point, Color.red);
            }
        }

        

    }

    private void ViewListMessage(string text)
    {
        ListMassageView.verticalNormalizedPosition = 1f;
        ScenManager.CursorVisible();

        GamePanel.SetActive(false);
        ListTextPanel.SetActive(true);
        //sound
        TextListMessage.text = text;
    }

    private void EventListM(int index)
    {
        switch (index)
        {
            case 1:
                SetQuest(1);
                isQuest_2 = true;
                // sound
                break;
            case 2:
                SetQuest(3);
                isQuest_3 = true;
                // sound
                break;
            case 3:
                SetQuest(5);
                break;
            case 4:
                SetQuest(0);
                break;
            case 5:
                SetQuest(6);
                break;
            case 6:
                SetQuest(7);
                break;
            case 7:
                SetQuest(8);
                break;
            case 8:
                SetQuest(9);
                break;
            case 9:
                SetQuest(10);
                break;
            case 10:
                SetQuest(0);
                break;
            case 11:
                SetQuest(11);
                break;
            case 12:
                SetQuest(12);
                break;

        }
    }

    private void SetQuest(int index)
    {
        QestPanel.SetActive(true);
        QuestIndex = index;
        switch (index)
        {
            case 0:
                TextQuestName.text = "У вас нет заданий.";
                TextQuestText.text = "";
                break;
            case 1:
                TextQuestName.text = "Найти что-то полезное.";
                TextQuestText.text = "Я проснулся ночью в лесу, ничего не помню…\nМне нужны вещи... Нужно найти топор.";
                break;
            case 2:
                TextQuestName.text = "Немного дерева";
                TextQuestText.text = "Вы нашли топор. Нарубите немного дерева.\nСрубить 3 дерева.";
                isQuest_2 = true;
                break;
            case 3:
                TextQuestName.text = "Найди его.";
                TextQuestText.text = "Интересно в этом лесу есть животные?\nНайди 'Вайта'...";
                break;
            case 4:
                TextQuestName.text = "Что-то вкусное.";
                TextQuestText.text = "Нужно найти что-то вкусное для лисы. \n(может где-то есть домик с едой?)\nПодсказка: ключ рядом с замком...";
                break;
            case 5:
                TextQuestName.text = "Динамит!";
                TextQuestText.text = "Выход завален. Нужен динамит…\nНайти динамит.";
                break;
            case 6:
                TextQuestName.text = "Медведь";
                TextQuestText.text = "Убить медведя - найти выход";
                break;
            case 7:
                TextQuestName.text = "На свободу";
                TextQuestText.text = "Найти выход из шахт.";
                break;
            case 8:
                TextQuestName.text = "Тайна долины - 3";
                TextQuestText.text = "Найти пульт управления. Отключить защиту.\nПодсказка: вход- от шахты вправо (до упора), влево (до упора), на двери замок!";
                break;
            case 9:
                TextQuestName.text = "Тайна долины";
                TextQuestText.text = "Найти Василия.\nПодсказка: он уходил в сторону холмов...";
                break;
            case 10:
                TextQuestName.text = "Тайна долины - 2";
                TextQuestText.text = "Найти тайный лагерь Василия.\nПодсказка:  от холмов туда идет тропа...";
                break;
            case 11:
                TextQuestName.text = "Открыть ворота";
                TextQuestText.text = "Убить медведя, подобрать ключ, открыть ворота.";
                break;
            case 12:
                TextQuestName.text = "Выход из города";
                TextQuestText.text = "Найти ключ от выхода.\nПодсказка: он может быть в церкви.";
                break;
        }
    }

    public void Inventar_Updata()
    {
        I_Axe.SetActive(Axe);
        I_Knife.SetActive(Knife);
        I_Picaxe.SetActive(Picaxe);

        if (Wood > 0)
        {
            IT_Wood.text = Wood.ToString();
            I_Wood.SetActive(true);
        }
        else I_Wood.SetActive(false);

        if (Bucket > 0)
        {
            IT_Bucket.text = Bucket.ToString();
            I_Bucket.SetActive(true);
        }
        else I_Bucket.SetActive(false);

        if (Meat > 0)
        {
            IT_Meat.text = Meat.ToString();
            I_Meat.SetActive(true);
        }
        else I_Meat.SetActive(false);

        if (TNT > 0)
        {
            IT_TNT.text = TNT.ToString();
            I_TNT.SetActive(true);
        }
        else I_TNT.SetActive(false);

        if (Matchbox > 0)
        {
            IT_Matchbox.text = Matchbox.ToString();
            I_Matchbox.SetActive(true);
        }
        else I_Matchbox.SetActive(false);

        if (Rope > 0)
        {
            IT_Rope.text = Rope.ToString();
            I_Rope.SetActive(true);
        }
        else I_Rope.SetActive(false);

        if (Stone > 0)
        {
            IT_Stone.text = Stone.ToString();
            I_Stone.SetActive(true);
        }
        else I_Stone.SetActive(false);

        if (OreBlue > 0)
        {
            IT_OreBlue.text = OreBlue.ToString();
            I_OreBlue.SetActive(true);
        }
        else I_OreBlue.SetActive(false);

        if (OreGreen > 0)
        {
            IT_OreGreen.text = OreGreen.ToString();
            I_OreGreen.SetActive(true);
        }
        else I_OreGreen.SetActive(false);

        if (OreRed > 0)
        {
            IT_OreRed.text = OreRed.ToString();
            I_OreRed.SetActive(true);
        }
        else I_OreRed.SetActive(false);

        if (PotionZombie > 0)
        {
            IT_PotionZombie.text = PotionZombie.ToString();
            I_PotionZombie.SetActive(true);
        }
        else I_PotionZombie.SetActive(false);

        if (PotionSpeed > 0)
        {
            IT_PotionSpeed.text = PotionSpeed.ToString();
            I_PotionSpeed.SetActive(true);
        }
        else I_PotionSpeed.SetActive(false);

        if (PotionPower > 0)
        {
            IT_PotionPower.text = PotionPower.ToString();
            I_PotionPower.SetActive(true);
        }
        else I_PotionPower.SetActive(false);

        if (PotionLive > 0)
        {
            IT_PotionLive.text = PotionLive.ToString();
            I_PotionLive.SetActive(true);
        }
        else I_PotionLive.SetActive(false);

        if (PotionJump > 0)
        {
            IT_PotionJump.text = PotionJump.ToString();
            I_PotionJump.SetActive(true);
        }
        else I_PotionJump.SetActive(false);

        if (FriedMeat > 0)
        {
            IT_FriedMeat.text = FriedMeat.ToString();
            I_FriedMeat.SetActive(true);
        }
        else I_FriedMeat.SetActive(false);

        if (MushroomRed > 0)
        {
            IT_MushroomRed.text = MushroomRed.ToString();
            I_MushroomRed.SetActive(true);
        }
        else I_MushroomRed.SetActive(false);

        if (MushroomYellow > 0)
        {
            IT_MushroomYellow.text = MushroomYellow.ToString();
            I_MushroomYellow.SetActive(true);
        }
        else I_MushroomYellow.SetActive(false);

        if (GlassBottle > 0)
        {
            IT_GlassBottle.text = GlassBottle.ToString();
            I_GlassBottle.SetActive(true);
        }
        else I_GlassBottle.SetActive(false);

    }

    private void Update_UI_Effect_potion()
    {
        UI_Effect_potion_speed.SetActive(PotionSpeedAct);
        UI_Effect_potion_zombie.SetActive(PotionZombieAct);
        ZombiePotionEffectPanel.SetActive(PotionZombieAct);
        UI_Effect_potion_power.SetActive(PotionPowerAct);
        UI_Effect_potion_jump.SetActive(PotionJumpAct);
    }

    public void EAT_FriedMeat()
    {
        if(Live <= 75)
        {
            FriedMeat--;
            Live += 25;
            CloseInventar();
            Update_UI_Effect_potion();
            eatMeat_Sound.Play();
        }
    }
    public void EAT_PotionZombie()
    {
        if (!PotionZombieAct)
        {
            PotionZombieAct = true;
            GlassBottle++;
            PotionZombie--;
            CloseInventar();
            Update_UI_Effect_potion();
            drinkPotion_Sound.Play();
            StartCoroutine(DelayPotionZombie());
        }
    }
    public void EAT_PotionSpeed()
    {
        if (!PotionSpeedAct)
        {
            PotionSpeedAct = true;
            Speed *= 1.5f;
            SpeedRunning *= 1.5f;
            GlassBottle++;
            PotionSpeed--;
            CloseInventar();
            Update_UI_Effect_potion();
            drinkPotion_Sound.Play();
            StartCoroutine(DelayPotionSpeed());
        }  
    }
    public void EAT_PotionPower()
    {
        if (!PotionPowerAct)
        {
            PotionPowerAct = true;
            Delay_UsingAxe *= .5f;
            Delay_UsingPicaxe *= 5f;
            Coeff_DamagePower = 2;
            GlassBottle++;
            PotionPower--;
            CloseInventar();
            Update_UI_Effect_potion();
            drinkPotion_Sound.Play();
            StartCoroutine(DelayPotionPower());
        }
    }
    public void EAT_PotionJump()
    {
        if (!PotionJumpAct)
        {
            PotionJumpAct = true;
            JumpSpeed *= 1.5f;
            GlassBottle++;
            PotionJump--;
            CloseInventar();
            Update_UI_Effect_potion();
            drinkPotion_Sound.Play();
            StartCoroutine(DelayPotionJump());
        }
    }
    public void EAT_PotionLive()
    {
        if (Live <= 75)
        {
            PotionLive--;
            Live += 25;
            CloseInventar();
            Update_UI_Effect_potion();
            drinkPotion_Sound.Play();
        }
    }

    private IEnumerator DelayPotionSpeed()
    {
        yield return new WaitForSeconds(TimeActivePotion);
        PotionSpeedAct = false;
        Speed /= 1.5f;
        SpeedRunning /= 1.5f;
        Update_UI_Effect_potion();
    }
    private IEnumerator DelayPotionZombie()
    {
        yield return new WaitForSeconds(TimeActivePotion);
        PotionZombieAct = false;
        Update_UI_Effect_potion();
    }
    private IEnumerator DelayPotionPower()
    {
        yield return new WaitForSeconds(TimeActivePotion);
        Delay_UsingAxe /= .5f;
        Delay_UsingPicaxe /= 5f;
        Coeff_DamagePower = 1;
        PotionPowerAct = false;
        Update_UI_Effect_potion();
    }
    private IEnumerator DelayPotionJump()
    {
        yield return new WaitForSeconds(TimeActivePotion);
        PotionJumpAct = false;
        JumpSpeed /= 1.5f;
        Update_UI_Effect_potion();
    }

    public IEnumerator WaitAndDestroy(GameObject obj, float time = 1f)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }

}
