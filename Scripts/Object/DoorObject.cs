using UnityEngine;

public class DoorObject : MonoBehaviour
{
    public bool isOpen = false;
    public string Key = "";
    public bool isNeedKey = false;
    public AudioSource door;

    public Animator Animator;

    public void Open()
    {
        isOpen = true;
        Animator.SetBool("IsOpen", isOpen);
        door.Play();
    }

    public void Invers()
    {
        isOpen = !isOpen;
        Animator.SetBool("IsOpen", isOpen);
        door.Play();
    }

    public void Close()
    {
        isOpen = false;
        Animator.SetBool("IsOpen", isOpen);
        door.Play();
    }

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }


}
