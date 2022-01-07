using UnityEngine;
using Valve.VR;
public class Controls : MonoBehaviour
{
    private bool gyroEnabled;
    private Gyroscope gyro;
    //private GameObject GyroControl;
    private Quaternion rot;
    private Quaternion adjustrot;

    private GUIStyle style;

       private Vector2 trackpad;
    private Vector3 moveDirection;
    private int GroundCount;
    private CapsuleCollider CapCollider;

    public SteamVR_Input_Sources MovementHand;//Set Hand To Get Input From
    public SteamVR_Action_Vector2 TrackpadAction;
    public SteamVR_Action_Boolean JumpAction;
    public float jumpHeight;
    public float MovementSpeed;
    public float Deadzone;//the Deadzone of the trackpad. used to prevent unwanted walking.
    public GameObject Head;
    public GameObject AxisHand;//Hand Controller GameObject
    public PhysicMaterial NoFrictionMaterial;
    public PhysicMaterial FrictionMaterial;
 

    void MovementUpdate()
    {
        updateInput();
        updateCollider();
        moveDirection = Quaternion.AngleAxis(Angle(trackpad) + AxisHand.transform.localRotation.eulerAngles.y, Vector3.up) * Vector3.forward;//get the angle of the touch and correct it for the rotation of the controller
        Rigidbody RBody = GetComponent<Rigidbody>();
        Vector3 velocity = new Vector3(0,0,0);
        if (trackpad.magnitude > Deadzone)
        {//make sure the touch isn't in the deadzone and we aren't going to fast.
            CapCollider.material = NoFrictionMaterial;
            velocity = moveDirection;
            if (JumpAction.GetStateDown(MovementHand) && GroundCount > 0)
            {
                float jumpSpeed = Mathf.Sqrt(2 * jumpHeight * 9.81f);
                RBody.AddForce(0, jumpSpeed, 0, ForceMode.VelocityChange);
            }
            RBody.AddForce(velocity.x*MovementSpeed - RBody.velocity.x, 0, velocity.z*MovementSpeed - RBody.velocity.z, ForceMode.VelocityChange);

            Debug.Log("Velocity" + velocity);
            Debug.Log("Movement Direction:" + moveDirection);
        }
        else if(GroundCount > 0)
        {
            CapCollider.material = FrictionMaterial;
        }
    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.x, p_vector2.y) * Mathf.Rad2Deg;
        }
    }

    private void updateCollider()
    {
        CapCollider.height = Head.transform.localPosition.y;
        CapCollider.center = new Vector3(Head.transform.localPosition.x, Head.transform.localPosition.y / 2, Head.transform.localPosition.z);
    }

    private void updateInput()
    {
        trackpad = TrackpadAction.GetAxis(MovementHand);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GroundCount++;
    }
    private void OnCollisionExit(Collision collision)
    {
        GroundCount--;
    }
    private void Start () 
    {
        CapCollider = GetComponent<CapsuleCollider>();
        Application.targetFrameRate = 144;
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
        MovementUpdate();
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