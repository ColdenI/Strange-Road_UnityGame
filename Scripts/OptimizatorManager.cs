using UnityEngine;
using System.Collections;

public class OptimizatorManager : MonoBehaviour
{
    public float distance = 10f;
    public float TimeUpdate = 2.4f;
    public bool isDecorait = true;
    [SerializeField] private Camera[] camers;
    public GameObject[] OptiObject;
    public GameObject[] Decoraits;

    private System.Collections.Generic.List<GameObject> decs;

    private void Start()
    {
        StartCoroutine(Delay());
        decs = new System.Collections.Generic.List<GameObject>();
        foreach (GameObject i in Decoraits) decs.Add(i);

        camers[0] = GameObject.Find("Main Camera").GetComponent<Camera>();
        camers[1] = GameObject.Find("CutCamera").GetComponent<Camera>();
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(TimeUpdate);
        DecorUpdata();
        foreach (GameObject i in OptiObject)
        {
            if(i != null && !decs.Contains(i)) CheckObject(i);
        }

        StartCoroutine(Delay());    
    }

    public void DecorUpdata()
    {
        foreach (GameObject i in Decoraits) if (i != null)
            {
                i.SetActive(isDecorait);
            }
    }

    public void CheckObject(GameObject object_)
    {
        

        bool isFlag = true;
        isFlag = false;
        foreach (Camera i in camers)
        {       
            if (i.enabled)
            {
                if (Vector3.Distance(object_.transform.position, i.gameObject.transform.position) <= distance)
                {
                    isFlag = true;
                }
                if(i.name == "CutCamera" && false)
                {
                    isFlag = true; 
                }
            }
        }

        object_.gameObject.SetActive(isFlag);

        
    }


}
