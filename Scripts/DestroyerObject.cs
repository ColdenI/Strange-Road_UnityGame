using UnityEngine;
using System.Collections;

public class DestroyerObject : MonoBehaviour
{
    public GameObject ObjectForDestroy;
    public string TextOut = "";
    [SerializeField] private AudioSource DestroySound;
    public bool isDestroyThis = true;
    public bool Flag = true;

    public void DestroyObj()
    {
        Flag = false;
        DestroySound.Play();
        Destroy(ObjectForDestroy);
        if(isDestroyThis) StartCoroutine(Delay());
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
    
}
