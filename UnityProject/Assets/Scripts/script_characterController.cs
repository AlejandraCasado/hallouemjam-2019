using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_characterController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeedMultiplier = 100f;
    [SerializeField] float moveMaxSpeed = 5f;
    [Header("Cam")]
    [SerializeField] float rotateXSpeedMultiplier = 100f;
    [SerializeField] float rotateYSpeedMultiplier = 100f;
    [SerializeField] float maxAngle = 100;

    Camera cam;
    Rigidbody rb;

    float inp_mouseX;
    float inp_mouseY;
    Vector3 inp_move = Vector3.zero;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        inputCheck();
    }

    private void FixedUpdate()
    {
        moveCharacter();
        rotateCam();
    }

    void inputCheck()
    {
        inp_mouseX = Input.GetAxis("Mouse X");
        inp_mouseY = Input.GetAxis("Mouse Y");
        inp_move.x = Input.GetAxis("Horizontal");
        inp_move.z = Input.GetAxis("Vertical");


        Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
        Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
        inp_move = inp_move.x * right + inp_move.z * forward;

        //Debug.Log("mouseX = " + inp_mouseX + ", mouseY = " + inp_mouseY + ", moveX = " + inp_move.x + ", moveZ = " + inp_move.z);
    }

    void moveCharacter()
    {
        //MOVE
        rb.velocity += inp_move * moveSpeedMultiplier * Time.fixedDeltaTime;

        //CLAMP
        Vector3 spd = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
        spd = spd.magnitude > moveMaxSpeed ? spd.normalized * moveMaxSpeed : spd;
        rb.velocity = new Vector3(spd.x, rb.velocity.y, spd.z);
    }

    void rotateCam()
    {
        //MOUSE X
        float angleX = inp_mouseX * rotateXSpeedMultiplier * Time.fixedDeltaTime;
        transform.RotateAround(transform.position, Vector3.up, angleX);

        //MOUSE Y
        float angleY = inp_mouseY * rotateYSpeedMultiplier * Time.fixedDeltaTime;
        cam.transform.RotateAround(cam.transform.position, cam.transform.right, angleY);

        //CLAMP
        

        Vector3 from = transform.forward.normalized;
        Vector3 to = cam.transform.forward;
        
        float actualAngleY = Vector3.SignedAngle(from, to, -transform.right.normalized);
        //Debug.Log("actual cam angle = " + actualAngleY);
        
        if(actualAngleY > maxAngle / 2f)
        {
            //Debug.Log("sa pasao por arriba");
            float aux = actualAngleY - (maxAngle / 2f);
            cam.transform.RotateAround(cam.transform.position, cam.transform.right, aux);
        } else if(actualAngleY < -maxAngle / 2f)
        {
            //Debug.Log("sa pasao por debajo");
            float aux = actualAngleY + (maxAngle / 2f);
            cam.transform.RotateAround(cam.transform.position, cam.transform.right, aux);
        }
    }
}
