using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("CameraControl/MouseLook")]
public class MouseLook_ : MonoBehaviour
{
    // список для настройки
    public enum RotationAxes { MouseXandY = 0, MouseX = 1, MouseY = 2};
    public RotationAxes axes_ = RotationAxes.MouseXandY;

    public bool isMove = true; // разрешить движение

    // чуствительность мыши
    public float SpeedMoveX = 2f;
    public float SpeedMoveY = 2f;

    // ограничители
    [SerializeField] private float maxX = -360f;
    [SerializeField] private float minX = 360f;
    [SerializeField] private float maxY = -360f;
    [SerializeField] private float minY = 360f;

    /*--------------------------------------------------------------------*/

    // текущий угол
    private float rotationX_ = 0f;
    private float rotationY = 0f;


    Quaternion originalRotation;        // тип вращения


    void Start()
    {
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        originalRotation = transform.localRotation;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }


    void Update()
    {
        if (isMove)
        { 
            if (axes_ == RotationAxes.MouseXandY)
            {
                rotationX_ += Input.GetAxis("Mouse X") * SpeedMoveX;
                rotationY += Input.GetAxis("Mouse Y") * SpeedMoveY;

                rotationX_ = ClampAngle(rotationX_, minX, maxX);
                rotationY = ClampAngle(rotationY, minY, maxY);

                Quaternion xQ = Quaternion.AngleAxis(rotationX_, Vector3.up);
                Quaternion yQ = Quaternion.AngleAxis(-rotationY, Vector3.right);

                transform.localRotation = originalRotation * xQ * yQ;
            }
            else if (axes_ == RotationAxes.MouseX)
            {
                rotationX_ += Input.GetAxis("Mouse X") * SpeedMoveX;
                rotationX_ = ClampAngle(rotationX_, minX, maxX);

                Quaternion xQ = Quaternion.AngleAxis(rotationX_, Vector3.up);

                transform.localRotation = originalRotation * xQ;
            }
            else if (axes_ == RotationAxes.MouseY)
            {
                rotationY += Input.GetAxis("Mouse Y") * SpeedMoveY;
                rotationY = ClampAngle(rotationY, minY, maxY);

                Quaternion yQ = Quaternion.AngleAxis(-rotationY, Vector3.right);

                transform.localRotation = originalRotation * yQ;
            }
        }

    }
}
