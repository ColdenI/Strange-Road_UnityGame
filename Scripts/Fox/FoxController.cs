using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FoxController : MonoBehaviour
{
    [SerializeField] public string Name = "Лиса";                            // Имя
    [SerializeField] private float Gravity = 20f;                         // гравитация
    [SerializeField] public  float Damage = 12f;                          // урон игроку
    [SerializeField] public float MakeDefaultRadius = 4f;               // радиус взаимодействия при Mode = 0,1,2
    [SerializeField] public float MakeHarassmentRadius = 6.6f;              // радиус взаимодействия при Mode = 3
    [SerializeField] public int RangeJamp = 70;                            // частота прыжка при приследовании

    public AudioSource fox_Sound1;
    public AudioSource fox_Sound2;
    public AudioSource fox_Sound3;
    public AudioSource Fox_effect;
    public AudioSource Event_sound;

    private bool isFoxSayFlag = false;
    public float minRandomDelayFoxSay = 6f;
    public float maxRandomDelayFoxSay = 20f;

    public Mode mode = Mode.IdleAndSit;

    [SerializeField] public TMP_Text Name_lable;
    [SerializeField] private TMP_Text Status_lable;


    public bool isFrendly = false;


    private bool _mode_0_flag = true;
    private bool _mode_0_lock = true;
    private float _mode_0_delay = 5f;
    private bool _mode_0_delay_random = true;
    private float _mode_0_delay_min = 5f;
    private float _mode_0_delay_max = 20f;


    private Animator Animator;
    private Vector3 moveDir = Vector3.zero;
    private CharacterController controller;
    Coroutine smoothMove = null;
    private SphereCollider SphereCollider;
    private GameObject Player;
    public GameObject effect;
    private int flag = 0;
    private Vector3 old_Position = new Vector3();


    void Start()
    {
        Status_lable.text = "";
        SphereCollider = GetComponent<SphereCollider>();
        SphereCollider.radius = MakeDefaultRadius;
        Name_lable.text = Name;
        controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
        Player = GameObject.Find("Player");
        StartCoroutine(FoxSayTimer(Random.Range(minRandomDelayFoxSay, maxRandomDelayFoxSay)));
    }

    public void Effect()
    {
        StartCoroutine(Effect_WaitAndDel(Instantiate(effect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), effect.transform.rotation)));
    }



    private void Fox_say()
    {
        if (isFoxSayFlag)
        {
            isFoxSayFlag = false;
            StartCoroutine(FoxSayTimer(Random.Range(minRandomDelayFoxSay, maxRandomDelayFoxSay)));
            int random_I = Random.Range(0, 2);
            if (random_I == 0) fox_Sound1.Play();
            if (random_I == 1) fox_Sound2.Play();
            if (random_I == 2) fox_Sound3.Play();

            if (mode != Mode.Harassment)
            {
                random_I = Random.Range(0, 7);
                if (random_I == 2) Dan_2();
            }
        }
    }

    IEnumerator FoxSayTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFoxSayFlag = true;
    }

    IEnumerator Effect_WaitAndDel(GameObject go)
    {
        Fox_effect.Play();
        yield return new WaitForSeconds(6);
        Dan_1();
        Name = "Вайт";
        Name_lable.text = Name;
        isFrendly = true;
        Event_sound.Play();
        yield return new WaitForSeconds(1.85f);
        Destroy(go);
    }

    private void AI()
    {
        switch (mode)
        {
            case Mode.IdleAndSit:
                SphereCollider.radius = MakeDefaultRadius;
                if (_mode_0_lock)
                {
                    _mode_0_lock = false;
                    _mode_0_flag = !_mode_0_flag;
                    if (_mode_0_flag) Idle();
                    else Sit();
                    if (_mode_0_delay_random) StartCoroutine(forMode_0_lock(Random.Range(_mode_0_delay_min, _mode_0_delay_max)));
                    else StartCoroutine(forMode_0_lock(_mode_0_delay));
                }
                break;

            case Mode.Idle: SphereCollider.radius = MakeDefaultRadius; Idle(); break;
            case Mode.Sit: SphereCollider.radius = MakeDefaultRadius; Sit(); break;
            case Mode.Harassment:
                SphereCollider.radius = MakeHarassmentRadius;
                Status_lable.text = "";
                Run();


                flag++;
                if (flag >= RangeJamp)
                {
                    //print(Vector3.Distance(old_Position, transform.position));
                    if (Vector3.Distance(old_Position, transform.position) < 2f) Jump();
                    old_Position = transform.position;
                    flag = 0;
                }
                break;
        }


    }



    
    void OnTriggerStay(Collider other)
    {
        // поворот к игроку
        if ((other.tag == "Player") && (mode == Mode.Idle || mode == Mode.Sit || mode == Mode.IdleAndSit || mode == Mode.Harassment))
        {
            Name_lable.gameObject.SetActive(true);
            LookSmoothly();
        }

        if (other.tag == "Player") Name_lable.gameObject.SetActive(true);

        if (other.tag == "Player") Fox_say();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mode == Mode.Harassment && other.tag == "Player")
        {
            Status_lable.text = "";
            mode = Mode.Harassment;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (mode == Mode.Harassment && other.tag == "Player")
        {
            Status_lable.text = "?";
            fox_Sound1.Play();
            mode = Mode.Sit;
            
        }

        if (other.tag == "Player") Name_lable.gameObject.SetActive(false);
    }



    void LookSmoothly()
    {
        float time = 1f;

        Vector3 lookAt = Player.transform.position;
        lookAt.y = transform.position.y;

        //Start new look-at coroutine
        if (smoothMove == null)
            smoothMove = StartCoroutine(LookAtSmoothly(transform, lookAt, time));
        else
        {
            //Stop old one then start new one
            StopCoroutine(smoothMove);
            smoothMove = StartCoroutine(LookAtSmoothly(transform, lookAt, time));
        }
    }

    IEnumerator LookAtSmoothly(Transform objectToMove, Vector3 worldPosition, float duration)
    {
        Quaternion currentRot = objectToMove.rotation;
        Quaternion newRot = Quaternion.LookRotation(worldPosition -
            objectToMove.position, objectToMove.TransformDirection(Vector3.up));

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToMove.rotation =
                Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
    }

    IEnumerator forMode_0_lock (float Delay)
    {
        yield return new WaitForSeconds(Delay);
        _mode_0_lock = true;
    }


    void FixedUpdate()
    {
        AI();
        RotateStatusLable();
        

        moveDir.y -= Gravity * Time.deltaTime;
        Animator.SetBool("jump_fly", !controller.isGrounded);

        controller.Move(moveDir * Time.deltaTime);
    }

    private void RotateStatusLable()
    {
         Status_lable.transform.Rotate(0, 2.0f, 0);

    }

    private void Jump()
    {
        Animator.SetTrigger("jump");
    }

    public void Run()
    {
        Animator.SetInteger("state", 2);
    }

    public void Run_left()
    {
        Animator.SetInteger("state", 3);
    }

    public void Run_right()
    {
        Animator.SetInteger("state", 4);
    }

    public void Sit()
    {
        //SphereCollider.radius = MakeDefaultRadius;
        Animator.SetInteger("state", 0);
    }

    public void Idle()
    {
        SphereCollider.radius = MakeDefaultRadius;
        Animator.SetInteger("state", 1);
    }

    public void Dan_1()
    {
        Animator.SetTrigger("dan_1");
    }

    public void Dan_2()
    {
        Animator.SetTrigger("dan_2");
    }

    public void Dan_3_Atack()
    {
        Animator.SetTrigger("dan_3");
    }

    public enum Mode 
    { 
        IdleAndSit = 0,
        Idle = 1,
        Sit = 2,
        Harassment = 3
        
    };
}
