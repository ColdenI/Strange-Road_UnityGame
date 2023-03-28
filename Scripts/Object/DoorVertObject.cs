using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorVertObject : MonoBehaviour
{
    public bool isOpen = false;
    public string key = "";
    public bool isNeedKey = false;
    private Animator Animator;
    public AudioSource AudioSource;
    public bool isAutomatic = false;

    public long VolumeDist = 10;
    private GameObject Player;

    private void Start()
    {
        Animator = this.gameObject.GetComponent<Animator>();
        Player = GameObject.Find("Player");
    }

    public void Open()
    {
        isOpen = true;
        Animator.SetTrigger("open");
        AudioSource.Play();
    }

    public static long map(long x, long in_min, long in_max, long out_min, long out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= VolumeDist)
        {
            AudioSource.volume = Convert.ToSingle(map(Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, VolumeDist, 100, 0)) / 100;
        }
        else
        {
            AudioSource.volume = 0f;
        }
    }
}

