using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverForDoorObject : MonoBehaviour
{

    public DoorVertObject[] DoorVertObjects;
    public AudioSource audioSource;
    public bool isAllOrOnly = true; // true - все вместе  false - по одному 
    public float delayTime = 4f;  // задержка при открытии по одному

    public bool isOff = false;

    private Animator Animator;


    void Start()
    {
        Animator = GetComponent<Animator>();
    }
    
    public void StartOn()
    {
        if (!isOff)
        {
            isOff = true;
            audioSource.Play();
            Animator.SetTrigger("on");
            if (isAllOrOnly)
            {
                foreach(DoorVertObject i in DoorVertObjects)
                {
                    if (i.isAutomatic) i.Open();
                }
            }
            else
            {
                for (int i = 0; i < DoorVertObjects.Length; i++)
                {
                    if (DoorVertObjects[i].isAutomatic) StartCoroutine(OpenDoor(DoorVertObjects[i], delayTime * i));
                }
            }
        }

    }

    IEnumerator OpenDoor(DoorVertObject door, float delay)
    {
        yield return new WaitForSeconds(delay);
        door.Open();
    }
}
