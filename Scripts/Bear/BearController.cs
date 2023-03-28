using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{

    private Animator animator;
    private CharacterController characterController;
    [SerializeField] private float Gravity = 20f;
    [SerializeField] private float Speed = 20f;
    private Vector3 moveDir = Vector3.zero;
    Coroutine smoothMove = null;
    private GameObject Player;
    private bool isAttack = true;
    [SerializeField] private float TimeReAttack = 3f;
    public int Live = 100;
    private bool isLive = true;
    public UnityEngine.UI.Image LiveBar;
    public UnityEngine.UI.Image LiveBarM;
    public float SpeedRotation = 3.5f;


    public GameObject Bonus;
    public int DamageForBear = 10;
    public float Damage = 20f;
    public int maxDistanceSound = 10;
    public int maxDistanceActiv = 20;

    [SerializeField] private AudioSource Sleep_s;
    public AudioSource[] Attack_s;
    [SerializeField] private AudioSource Aktive_s;


    [SerializeField] private int Mode = 0; // 0 - sleep 1 - harassment 2 - death

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Player = GameObject.Find("Player");

        StartCoroutine(ISMODE());

        Aktive_s.volume = Convert.ToSingle(map(Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, maxDistanceSound, 100, 0)) / 100;
        Sleep_s.volume = Aktive_s.volume;
        foreach (AudioSource i in Attack_s) i.volume = Aktive_s.volume;
    }


    public void Harassment_Start()
    {
        Mode = 1;
        animator.SetBool("Sleep", false);
        animator.SetBool("Run Forward", true);
        characterController.enabled = true;
        Sleep_s.Pause();
        Aktive_s.Play();
    }

    public void Sleep_Start()
    {
        Mode = 0;
        animator.SetBool("Sleep", true);
        animator.SetBool("Run Forward", false);
        characterController.enabled = false;
        Sleep_s.Play();
        Aktive_s.Pause();
    }

    public static long map(long x, long in_min, long in_max, long out_min, long out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public void Attack()
    {
        animator.SetTrigger("Attack3");        
    }

    public void Death()
    {
        isLive = false;
        animator.SetBool("Run Forward", false);
        Mode = 2;
        //characterController.enabled = false;
        animator.SetBool("Sleep", false);
        animator.SetBool("Death", true);
        LiveBar.gameObject.SetActive(false);
        LiveBarM.gameObject.SetActive(false);
        Instantiate(Bonus, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Attack_s[1].Play();
        Sleep_s.Pause();
        Aktive_s.Pause();
        
        StartCoroutine(Player.GetComponent<PlayerController>().WaitAndDestroy(this.gameObject, 120f));
    }

    private void Harassment_Tick()
    {
        // поворот к игроку
        LookSmoothly();
        moveDir = new Vector3(0, 0, 1);
        moveDir = transform.TransformDirection(moveDir);
       // moveDir *= Speed;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Mode == 1 && isAttack)
        {
            Attack();
            other.gameObject.GetComponent<PlayerController>().Live -= Damage;
            other.gameObject.GetComponent<PlayerController>().damage_Sound.Play();
            isAttack = false;
            StartCoroutine(ReAttack());
        }
    }

    IEnumerator ReAttack()
    {
        yield return new WaitForSeconds(TimeReAttack);
        isAttack = true;
    }

    IEnumerator ISMODE()
    {
       if(isLive)
        if (Vector3.Distance(Player.transform.position, transform.position) <= maxDistanceActiv) Harassment_Start();
        else Sleep_Start();

        yield return new WaitForSeconds(7f);

        StartCoroutine(ISMODE());
    }

    void FixedUpdate()
    {
        switch (Mode)
        {
            case 0:break;
            case 1: Harassment_Tick();break;
        }

        if (Live <= 0 && isLive) Death();


        moveDir.y -= Gravity * Time.deltaTime * Speed;
        characterController.Move(moveDir * Time.deltaTime);

        if(!isLive && !characterController.isGrounded)
        {
            moveDir.y -= Gravity * Time.deltaTime * Speed;
            characterController.Move(moveDir * Time.deltaTime);
        }

        if (!isLive && characterController.isGrounded) characterController.enabled = false;



        if (Vector3.Distance(Player.transform.position, transform.position) <= maxDistanceSound)
        {
            Aktive_s.volume = Convert.ToSingle(map(Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, maxDistanceSound, 100, 0)) / 100;
            Sleep_s.volume = Aktive_s.volume;
            foreach (AudioSource i in Attack_s) i.volume = Aktive_s.volume;
            //print(Aktive_s.volume);
        }
        else
        {
            Aktive_s.volume = 0f;
            Sleep_s.volume = Aktive_s.volume;
            foreach (AudioSource i in Attack_s) i.volume = Aktive_s.volume;
        }

        if (Live > 0 && Vector3.Distance(Player.transform.position, transform.position) <= maxDistanceActiv / 2)
        {
            LiveBar.gameObject.SetActive(true);
            LiveBarM.gameObject.SetActive(true);
            LiveBar.fillAmount = Live / 100f;
        }
        else
        {
            LiveBar.gameObject.SetActive(false);
            LiveBarM.gameObject.SetActive(false);
        }
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
        Quaternion newRot = Quaternion.LookRotation(worldPosition - objectToMove.position, objectToMove.TransformDirection(Vector3.up));

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime * SpeedRotation;
            objectToMove.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
    }
}
