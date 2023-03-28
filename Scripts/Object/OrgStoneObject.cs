using UnityEngine;

public class OrgStoneObject : MonoBehaviour
{

    public Resourse ResType = Resourse.Stone;
    public Vector3 ScaleDefault;
    public int CountRes = 3;
    public float PowerDestroy = .98f;

    void Start()
    {
        ScaleDefault = transform.localScale;
    }

    public enum Resourse
    {
        Stone = 0,
        RedOrg = 1,
        BlueOrg = 2,
        GreenOrg = 3,
        Random = 4
    }
}
