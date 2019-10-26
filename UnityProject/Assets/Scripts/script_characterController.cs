using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum charState { free, block, checkMask };

public class script_characterController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeedMultiplier = 100f;
    [SerializeField] float moveMaxSpeed = 5f;
    [Header("Cam")]
    [SerializeField] float rotateXSpeedMultiplier = 100f;
    [SerializeField] float rotateYSpeedMultiplier = 100f;
    [SerializeField] float maxAngle = 100;
    [Header("CHECK CHILD")]
    [SerializeField] float rayDistance = 1f;

    Camera cam;
    Rigidbody rb;

    //input
    float inp_mouseX;
    float inp_mouseY;
    Vector3 inp_move = Vector3.zero;
    bool inp_checkChild;
    bool inp_taylor;

    int layerMask;
    charState state = charState.free;

    [Header("CHILD CHECK")]
    [SerializeField] float childDistance = 1f;
    [SerializeField] float targetCamAngle = 5f;
    public float pointAtChildTime = 0.5f;

    Transform childPursued;
    float time = 0f;

    Vector3 originalPos;
    Vector3 targetPos;
    float anglesToRotateCam;
    float anglesToRotateChar;
    float lastTime = 0f;

    //TAYLOR
    [Header("Taylor")]
    [SerializeField] CapsuleCollider colTay;
    [SerializeField] float taylorTime = 1f;


    //FUNCTIONS
    void Start()
    {
        layerMask = LayerMask.GetMask("child");
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        if (colTay) colTay.enabled = false;
    }

    void Update()
    {
        inputCheck();
    }

    private void FixedUpdate()
    {
        behave();
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

        inp_checkChild = Input.GetButtonDown("Check");
        inp_taylor = Input.GetButtonDown("Taylor");

        //Debug.Log("mouseX = " + inp_mouseX + ", mouseY = " + inp_mouseY + ", moveX = " + inp_move.x + ", moveZ = " + inp_move.z);
    }

    void changeState(charState s)
    {
            state = s;
    }

    void behave()
    {
        switch (state)
        {
            /*case charState.idle:
                behaveIdle();
                break;*/

            case charState.free:
                behaveFree();
                break;

            /*case charState.block:

                break;*/
            case charState.checkMask:
                behaveCheckMask();
                break;
        }
    }

    void behaveFree()
    {
        moveCharacter();
        rotateCam();
        check();
        throwTaylor();
    }

    void behaveCheckMask()
    {
        lastTime = time;
        time += Time.fixedDeltaTime;
        rb.velocity = new Vector3(0f, 0f, 0f);
        float step = script_staticFuncs.smooth0to1(time / pointAtChildTime);

        //POSITION
        Vector3 vec = targetPos - originalPos;
        transform.position = originalPos + step * vec;

        //CAM
        cam.transform.RotateAround(cam.transform.position, -cam.transform.right, anglesToRotateCam * Time.fixedDeltaTime / pointAtChildTime);


        //FORWARD
        transform.RotateAround(transform.position, Vector3.up, anglesToRotateChar * Time.fixedDeltaTime / pointAtChildTime);

        
        if (time > pointAtChildTime) endCheckingKid();
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
    void check()
    {
        Debug.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward.normalized * rayDistance, Color.red);
        if (inp_checkChild && Mathf.Abs(rb.velocity.y) < 0.05f)
        {
            inp_checkChild = false;
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDistance, layerMask))
            {
                if(!hit.transform.GetComponent<script_childBehaviour>().checkedMask) startCheckingKid(hit.transform);

                /*script_childBehaviour child = hit.transform.GetComponent<script_childBehaviour>();
                if (child)
                    if (child.asthmatic) Debug.Log("He is asthmatic");
                Debug.Log("You hit child");*/
                //Destroy(hit.transform.gameObject);
            }
        }
    }

    void startCheckingKid(Transform c)
    {

        changeState(charState.checkMask);
        childPursued = c;
        childPursued.GetComponent<script_childBehaviour>().changeState(childState.checkMask);
        time = 0f;
        lastTime = 0f;
        //originalForwardCam = cam.transform.forward;

        //CAM

        float aux = Vector3.SignedAngle(transform.forward, cam.transform.forward, -transform.right);

        anglesToRotateCam = targetCamAngle - aux;
        //Debug.Log("aux = " + aux + ", angle = " + anglesToRotateCam);


        //FORWARD
        Vector3 from = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 to = Vector3.ProjectOnPlane(childPursued.position - transform.position, Vector3.up).normalized;
        aux = Vector3.SignedAngle(from, to, Vector3.up);
        anglesToRotateChar = aux;
        //Debug.Log(aux);


        //POSITION
        originalPos = transform.position;
        Vector3 childPosXZ = new Vector3(childPursued.position.x, transform.position.y, childPursued.position.z);
        targetPos = childPosXZ + (transform.position - childPosXZ).normalized * childDistance;
    }

    void endCheckingKid()
    {
        changeState(charState.free);
        //pointingAtChild
    }

    void throwTaylor()
    {
        if (colTay)
        {
            if (inp_taylor && !colTay.enabled)
            {
                colTay.enabled = true;
                StartCoroutine("stopRunning");
                Debug.Log("throwTay");
            }
        }
        
    }

    IEnumerator stopRunning()
    {
        yield return new WaitForSeconds(taylorTime);
        if (colTay) colTay.enabled = false;
    }

    //PUBLIC
    //public void blockChar() { changeState(charState.block); }
    //public void setFree() { changeState(charState.free); }
}
