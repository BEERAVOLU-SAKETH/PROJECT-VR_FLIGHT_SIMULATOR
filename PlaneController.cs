using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float throttleIncrement = 0.1f;
    public float maxThrottle = 200f;
    public float responsiveness = 10f;
    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;
    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void HandleInputs()
    {
        /*roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");*/

        roll = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        pitch = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        yaw = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        throttle = Mathf.Clamp(throttle + OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y * throttleIncrement, 0f, 100f);

        //Handle throttle value being sure to clamp it between 0 and 100.
        if (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.Button.One)) throttle += throttleIncrement;
        else if (Input.GetKey(KeyCode.LeftControl) || OVRInput.Get(OVRInput.Button.Two)) throttle -= throttleIncrement;
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInputs();
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrottle * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(transform.forward * roll * responseModifier);
    }
}
