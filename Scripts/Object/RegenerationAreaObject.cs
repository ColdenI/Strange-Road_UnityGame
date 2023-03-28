using UnityEngine;

public class RegenerationAreaObject : MonoBehaviour
{
    private PlayerController Player;
    public bool isRegLive = true;
    public bool isRegStamina = true;
    public float PowerReg = 1f;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (isRegLive && Player.Live < 100 - PowerReg) Player.Live += PowerReg;
            if (isRegStamina && Player.Stamina < 100 - PowerReg) Player.Stamina += PowerReg;
        }
    }
}
