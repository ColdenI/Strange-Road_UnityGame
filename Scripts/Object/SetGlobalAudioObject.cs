using UnityEngine;

public class SetGlobalAudioObject : MonoBehaviour
{

    private AudioSource GlobalSourse;
    public AudioClip AudioClip;
    public AudioClip OriginalClip;
    private bool flag = true;
    public bool isSingle = false;

    void Start()
    {      
        GlobalSourse = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        if(OriginalClip == null)
            OriginalClip = GlobalSourse.clip;

        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!isSingle)
            {
                if (flag) GlobalSourse.clip = AudioClip;
                else GlobalSourse.clip = OriginalClip;
                flag = !flag;
            }
            else if (flag)
            {
                flag = false;
                GlobalSourse.clip = AudioClip;
            }

            GlobalSourse.Play();
            
        }
    }
}
