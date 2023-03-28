using UnityEngine;

public class WardrobeDoorObject : MonoBehaviour
{
    public bool isNeedKey = false;
    public string Key = "";
    public AudioSource door;
    public GameObject lock_;

    public bool isOpen { private set; get; }
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        lock_.SetActive(isNeedKey);
    }

    public void SetDoor(bool set)
    {
        isOpen = set;
        door.Play();
        animator.SetBool("isOpen", isOpen);
    }
    
}
