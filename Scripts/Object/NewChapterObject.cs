using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class NewChapterObject : MonoBehaviour
{
    private GameObject NewCapterPanel;
    public AudioSource NewCapterSound;
    private UnityEngine.UI.Text TextChapter;
    public string Text;
    private bool isWork = true;
    public bool isViewCutScene = true;

    private CutSceneManager TimelineManager;

    public PlayableAsset Playable;

    private void Start()
    {
        NewCapterPanel = GameObject.Find("Panel_chapter");
        TextChapter = GameObject.Find("TextNewChapter").GetComponent<UnityEngine.UI.Text>();
        TimelineManager = GameObject.Find("TimelineManager").GetComponent<CutSceneManager>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isWork)
        {
            isWork = false;
            if(isViewCutScene) TimelineManager.Start_Cut(Playable);
            StartCoroutine(NewCapter());
        }
    }

    IEnumerator NewCapter()
    {
        NewCapterSound.Play();
        NewCapterPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
        NewCapterPanel.SetActive(true);
        TextChapter.text = Text;
        for (float i = 0; i < 1; i += .05f)
        {
            yield return new WaitForSeconds(.03f);
            NewCapterPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, i);
        }
        yield return new WaitForSeconds(3f);

        for (float i = 1; i > 0; i -= .05f)
        {
            yield return new WaitForSeconds(.03f);
            NewCapterPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, i);
        }

        NewCapterPanel.SetActive(false);
        Destroy(this.gameObject);
    }

}
