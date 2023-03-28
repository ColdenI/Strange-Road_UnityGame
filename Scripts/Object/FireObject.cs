using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObject : MonoBehaviour
{

    public GameObject Fire;
    public GameObject fuil_1, fuil_2, fuil_3, fuil_4;
    public AudioSource fire;
    public AudioSource water;
    public AudioSource spichki;

    public int maxDistance = 10;
    private GameObject Player;
    public bool isWork = false;
    public int fuil = 0;

    public float FuilPower = 60f;

    public void StartFire()
    {
        if(fuil > 0)
        {          
            spichki.Play();
            isWork = true;
            StartCoroutine(Delay());
            fire.Play();
            Fire.SetActive(isWork);
            StartCoroutine(TimeFire());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(FuilPower);
        fuil--;
    }

    IEnumerator TimeFire()
    {
        yield return new WaitForSeconds(FuilPower);
        if(fuil > 0 && isWork)
        {
            fuil--;
            fire.Play();
            isWork = true;
            StartCoroutine(TimeFire());
        }
        else
        {
            isWork = false;
            fire.Pause();
            Fire.SetActive(isWork);
        }
    }

    void Start()
    {
        Player = GameObject.Find("Player");
        Fire.SetActive(isWork);
        fire.volume = Convert.ToSingle(map(Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, maxDistance, 100, 0)) / 100;
    }

    public void FireStop()
    {
        fire.Pause();
        water.Play();      
        isWork = false;
        Fire.SetActive(isWork);
    }

    public static long map(long x, long in_min, long in_max, long out_min, long out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }


    void Update()
    {

        if (isWork && Vector3.Distance(Player.transform.position, transform.position) <= maxDistance)
        {
            fire.volume = Convert.ToSingle(map(Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, maxDistance, 100, 0)) / 100;
            //print(Convert.ToSingle(map(Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, maxDistance, 100, 0)) / 100);
        }
        else
        {
            fire.volume = 0f;
        }

        Fire.SetActive(isWork);
        if (fuil > 0)
        {
            fuil_1.SetActive(true);
        }else
        {
            fuil_1.SetActive(false);
            fuil_2.SetActive(false);
            fuil_3.SetActive(false);
            fuil_4.SetActive(false);
        }

        if (fuil > 2)
        {
            fuil_1.SetActive(true);
            fuil_2.SetActive(true);
        }
        else
        {
            fuil_2.SetActive(false);
            fuil_3.SetActive(false);
            fuil_4.SetActive(false);
        }

        if (fuil > 4)
        {
            fuil_1.SetActive(true);
            fuil_2.SetActive(true);
            fuil_3.SetActive(true);
        }
        else
        {
            fuil_3.SetActive(false);
            fuil_4.SetActive(false);
        }

        if (fuil > 6)
        {
            fuil_1.SetActive(true);
            fuil_2.SetActive(true);
            fuil_3.SetActive(true);
            fuil_4.SetActive(true);
        }
        else
        {
            fuil_4.SetActive(false);
        }
    }
}
