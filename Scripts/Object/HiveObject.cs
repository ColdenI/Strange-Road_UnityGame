using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveObject : MonoBehaviour
{

    private PlayerController Player;
    [SerializeField] private AudioSource Hive_s;
    public float Damage = 7f;
    public float TimeREDamage = 5f;
    private bool isDam = true;
    public float RadiusDamage = 2f;
    public long MaxDistVolume = 15;



    public static long map(long x, long in_min, long in_max, long out_min, long out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(UpdateVolume());
    }

    IEnumerator UpdateVolume()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= MaxDistVolume)
        {
            Hive_s.volume = Convert.ToSingle(map(Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, MaxDistVolume, 100, 0)) / 100;
        }
        else
        {
            Hive_s.volume = 0f;
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(UpdateVolume());
    }

    IEnumerator DelayAndReDam()
    {
        yield return new WaitForSeconds(TimeREDamage);
        isDam = true;
    }
 
    void FixedUpdate()
    {
        if(isDam && Vector3.Distance(this.transform.position, Player.gameObject.transform.position) <= RadiusDamage)
        {
            isDam = false;
            StartCoroutine(DelayAndReDam());
            Player.Live -= Damage;
            Player.Stamina += Damage;
            Player.damage_Sound.Play();
        }   
    }
}
