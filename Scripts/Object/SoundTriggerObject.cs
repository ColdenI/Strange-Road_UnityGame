using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundTriggerObject : MonoBehaviour
{
    private AudioSource AudioSource;
    public AudioClip AudioClip;
    public bool _flag = true;
    public bool isReplay = false;

    public Sprite Image;
    public bool isOnPicture = true;
    private Image picture;
    public float TimeDelayPicture = 2f;

    void Start()
    {
        picture = GameObject.Find("PictureScreem").GetComponent<Image>();
        GetComponent<MeshRenderer>().enabled = false;
        AudioSource = GetComponent<AudioSource>();
        AudioSource.clip = AudioClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && ((isReplay && _flag) || !isReplay) && !AudioSource.isPlaying)
        {
            AudioSource.Play();
            if (isOnPicture)
            {
                picture.sprite = Image;
                picture.color = new Color(1, .8f, .8f, 1);
                picture.gameObject.SetActive(true);
                StartCoroutine(PictureOff());
            }
            _flag = false;
        }
    }

    IEnumerator PictureOff()
    {
        yield return new WaitForSeconds(TimeDelayPicture);
        for (float i = 1; i > 0; i -= .05f)
        {
            yield return new WaitForSeconds(.03f);
            picture.color = new Color(1, .8f, .8f, i);
        }
        picture.color = new Color(1, .8f, .8f, 0);
    }
}
