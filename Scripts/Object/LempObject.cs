using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LempObject : MonoBehaviour
{
    public bool isWork = true;
    [SerializeField] private GameObject light;
    [SerializeField] private GameObject light_;
    private Animator Animator;

    public AudioSource start, stop;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        light.SetActive(isWork);
        light_.SetActive(isWork);
        Animator.enabled = isWork;
    }

    public void Start_L()
    {
        isWork = true;
        light.SetActive(isWork);
        light_.SetActive(isWork);
        Animator.enabled = isWork;
        start.Play();
    }

    public void Stop_L()
    {
        isWork = false;
        light.SetActive(isWork);
        light_.SetActive(isWork);
        Animator.enabled = isWork;
        stop.Play();
    }

}
