using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBarier_1 : MonoBehaviour
{
    public int CountDebris = 30;
    public float GravityForse = 20;
    public float TimeDestroyDebris = 60f;

    public GameObject Effect;
    public GameObject Debris;
    private GameObject Player;

    public GameObject Ditanator;
    public GameObject Barier;

    public AudioSource Beep;
    public AudioSource Boom;

    public bool isActive = true;
    private bool isDam = false;
    public float Damage = .1f;
    private List<GameObject> debries = new List<GameObject>();
    public int maxDistanceSound = 25;


    public static long map(long x, long in_min, long in_max, long out_min, long out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public void StartEfect()
    {
        isActive = false;
        StartCoroutine(Start_e());
    }

    IEnumerator Start_e()
    {
        Ditanator.SetActive(true);
        
        for (float i = 1.2f; i > .3f; i -= .1f)
        {
            Beep.Play();
            yield return new WaitForSeconds(i);
        }
        isDam = true;
        Boom.Play();
        //yield return new WaitForSeconds(.3f);
        Effect.SetActive(true);     
        yield return new WaitForSeconds(.5f);
        Destroy(Barier);
        Destroy(Effect);
        Destroy(Ditanator);
        GetComponent<BoxCollider>().enabled = false;
        isDam = false;

        if (SettingsScript.IntToBool(PlayerPrefs.GetInt("gen_debri", 1)))
        {
            for (int i = 0; i < CountDebris; i++)
            {
                GameObject bj = Instantiate(Debris, transform.position, Quaternion.identity);
                debries.Add(bj);
                bj.GetComponent<Rigidbody>().AddForce(transform.up * GravityForse);
            }
        }
        StartCoroutine(TimeDel());
    }

    IEnumerator TimeDel()
    {
        yield return new WaitForSeconds(TimeDestroyDebris);
        foreach (GameObject i in debries) Destroy(i);
        Destroy(this);
    }

    private void Start()
    {
        Ditanator.SetActive(false);
        Player = GameObject.Find("Player");

        Boom.volume = System.Convert.ToSingle(map(System.Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, maxDistanceSound, 100, 0)) / 100;
        Beep.volume = Boom.volume;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && isDam)
        {
            other.GetComponent<PlayerController>().Live -= Damage;
        }

        if (other.gameObject.GetComponent<FoxController>() && isDam)
        {
            Destroy(other.gameObject);
            GameObject.Find("Player").GetComponent<PlayerController>().GameOver();
            GameObject.Find("TextGameOverThen").GetComponent<UnityEngine.UI.Text>().text = "Лиса должна выжить!";
        }

        if ((other.gameObject.GetComponent<LempObject>() || 
            other.gameObject.GetComponent<StaminaPotion>() || 
            other.gameObject.GetComponent<LivePotion>()
            ) && isDam)
        {
            debries.Add(Instantiate(Debris, other.gameObject.transform.position, Quaternion.identity));
            Destroy(other.gameObject);    
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) <= maxDistanceSound)
        {
            Boom.volume = System.Convert.ToSingle(map(System.Convert.ToInt32(Vector3.Distance(Player.transform.position, transform.position)), 2, maxDistanceSound, 100, 0)) / 100;
            Beep.volume = Boom.volume;

        }
    }


}
