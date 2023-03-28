using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{

    public float Speed = .1f;

    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().isMOveLader = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            other.gameObject.GetComponent<PlayerController>().moveDir = transform.TransformDirection(other.gameObject.GetComponent<PlayerController>().moveDir);
            //other.gameObject.GetComponent<PlayerController>().Stamina -= .1f;
            other.gameObject.GetComponent<PlayerController>().controller.Move(other.gameObject.GetComponent<PlayerController>().moveDir * Time.deltaTime * Speed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().isMOveLader = false;
        }
    }

    void Update()
    {
        
    }
}
