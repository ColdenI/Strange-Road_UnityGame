using UnityEngine;

public class ListMessageObject : MonoBehaviour
{
    public bool isDestroy = false;
    [TextArea(6, 100)] public string Text = "";
    public int ID = 1;

}
