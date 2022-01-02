using UnityEngine;
public class Controls : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;
    //private GameObject GyroControl;
    private Quaternion rot;
    private Quaternion adjustrot;

    private GUIStyle style;
    private void Start () 
    {
        Application.targetFrameRate = 120;
        style = new GUIStyle();
        style.fontSize = 96;
        //GyroControl = new GameObject ("Gyro Control");
        //GyroControl.transform.position = transform.position;
        //transform.SetParent (GyroControl.transform);
        gyroEnabled = EnableGyro();
        if (gyroEnabled) adjustrot = Quaternion.Euler(90f, 0f, 0f) * Quaternion.Inverse (gyro.attitude);
    }


    private bool EnableGyro()
    {
    if (SystemInfo.supportsGyroscope) {
            gyro = Input.gyro;
            gyro.enabled = true;
            //GyroControl.transform.rotation = Quaternion.Euler(90f, -90f, 0f);
            rot = new Quaternion(0, 0, 1, 0);
            return true;
    }
    return false;
    }

    private void Update () 
    {
        if (gyroEnabled) 
        {
            if (Input.touchCount == 3) 
            {
                adjustrot = Quaternion.Euler(90f, 0f, 0f) * Quaternion.Inverse (gyro.attitude);
            }
            if (Input.touchCount == 1) {
                transform.Translate(Vector3.forward / 5, transform);
            }
            transform.localRotation = adjustrot * gyro.attitude * rot;
        }
        else {
            transform.Rotate(Input.GetAxis("Mouse Y")*5f, 0,0);
            transform.Rotate(0,Input.GetAxis("Mouse X")*5f,0);
            transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
            if (Input.GetKey(KeyCode.W)) {
                transform.Translate(Vector3.forward / 5, transform);
            }
        }
    }

    private void OnGUI() {
        GUI.Label(new Rect(0,0,100,100), "FPS: " + ((int) (1 / Time.unscaledDeltaTime)).ToString(), style);
    }
}