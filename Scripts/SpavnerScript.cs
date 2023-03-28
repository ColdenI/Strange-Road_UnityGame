using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpavnerScript : MonoBehaviour
{

    public float timeDelay = 10f;
    public int maxCount = 10;
    public bool isOn = true;

    public GameObject prefab;

    private List<GameObject> objects = new List<GameObject>();


    private void Spavn()
    {
        Vector3 pos = new Vector3(0, transform.position.y, 0);
        pos.x = Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2);
        pos.z = Random.Range(transform.position.z - transform.localScale.z / 2, transform.position.z + transform.localScale.z / 2);
        objects.Add(Instantiate(prefab, pos, Quaternion.identity));
    }

    IEnumerator DelaySpavn()
    {
        yield return new WaitForSeconds(timeDelay);
        if (isOn && objects.Count < maxCount)
        {
            Spavn();
        }

        StartCoroutine(DelaySpavn());
    }


    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(DelaySpavn());
    }

}
