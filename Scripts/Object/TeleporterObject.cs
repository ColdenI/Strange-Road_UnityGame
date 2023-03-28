using UnityEngine;
using System.Collections;

public class TeleporterObject : MonoBehaviour
{
    public Vector3 Position;
    private GameObject Teleport_panel;
    public AudioSource TeleportSound;
    public Color color = new Color(1,1,1);

    private void Start()
    {
        Teleport_panel = GameObject.Find("Panel_teleportation");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<CharacterController>().enabled = false;
            StartCoroutine(Teleportation(other));
        }
    }

    IEnumerator Teleportation(Collider other)
    {
        TeleportSound.Play();
        Teleport_panel.GetComponent<UnityEngine.UI.Image>().color = new Color(color.r, color.g, color.b, 0);
        Teleport_panel.SetActive(true);
        for (float i = 0; i < 1; i += .05f)
        {
            yield return new WaitForSeconds(.07f);
            Teleport_panel.GetComponent<UnityEngine.UI.Image>().color = new Color(color.r, color.g, color.b, i);
        }
        Teleport_panel.GetComponent<UnityEngine.UI.Image>().color = new Color(color.r, color.g, color.b, 1);
        yield return new WaitForSeconds(.85f);
        other.gameObject.transform.position = Position;
        other.GetComponent<CharacterController>().enabled = true;
        Teleport_panel.SetActive(false);
    }
    
}
