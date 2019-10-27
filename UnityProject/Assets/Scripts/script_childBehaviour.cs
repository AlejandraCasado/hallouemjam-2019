using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum childState {idle, walk, run, die, checkMask, showFace};

public class script_childBehaviour : MonoBehaviour
{
    bool alreadyInit = false;

    const string trigger_idle = "idle";
    const string trigger_walk = "walk";
    const string trigger_run = "run";
    const string trigger_die = "die";
    const string trigger_checkMask = "checkMask";
    const string trigger_showFace = "showFace";
    Animator anim;
    Rigidbody rb;
    public childState state = childState.idle;
    [Header("PROPERTIES")]
    [SerializeField] Transform targetForCam;
    /*[SerializeField] */float lifeTime/* = 2f*/;
    [SerializeField] float idlePCT = 0.3f;
    [HideInInspector] public bool asthmatic = false;
    [HideInInspector] public bool saved = false;

    [Header("MOVEMENT")]
    [SerializeField] float speedMultiplier = 30f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] float pctMaxSpeedWalk = 0.5f;
    [SerializeField] float runningTime = 5f;

    [Header("CHANGE DIR")]
    [SerializeField] float pointToDirForce = 3f;
    [SerializeField] float minTimeToChangeDir = 0f;
    [SerializeField] float rangeTimeToChangeDir = 1f;
    Vector3 direction = Vector3.zero;


    //CHECKING
    [HideInInspector] public bool checkedMask = false;
    float time = 0f;
    float positionCheckTime = 0f;
    float angleToRot = 0f;

    //TAYLOR
    int tayLayer;

    //CHAOS
    [Header("Chaos")]
    [SerializeField] float chaosTime = 3f;
    [SerializeField] SphereCollider chaosCollider;
    [SerializeField] float chaosChance = 0.5f;
    int chaosLayer;
    
   

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    public void init()
    {
        if (!alreadyInit)
        {
            lifeTime = script_gameController.lifeTime;
            alreadyInit = true;
            tayLayer = LayerMask.NameToLayer("taylor");
            chaosLayer = LayerMask.NameToLayer("chaos");
            //Debug.Log("child generated");
            anim = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            if (state != childState.idle) changeState(state);
            StartCoroutine("changeDir");

            direction = Vector3.right;

            positionCheckTime = script_gameController.character.GetComponent<script_characterController>().pointAtChildTime;
            chooseRandomDir();

            if (chaosCollider) chaosCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.I)) changeState(childState.idle);
        //if (Input.GetKeyDown(KeyCode.O)) changeState(childState.walk);
        //if (Input.GetKeyDown(KeyCode.P)) changeState(childState.run);
        //if (Input.GetKeyDown(KeyCode.L)) changeState(childState.checkMask);

        //behave();

    }

    private void FixedUpdate()
    {
        behave();
    }

    private void LateUpdate()
    {
        clampSpeed();
        //if(state != childState.idle && state != childState.die) transform.LookAt(transform.position + direction.normalized, Vector3.up);
    }

    public void changeState(childState s, bool runType = false) {
        if(state != childState.die)
        {
            state = s;
            switch (s)
            {
                case childState.idle:
                    rb.isKinematic = false;
                    anim.SetTrigger(trigger_idle);
                    break;

                case childState.walk:
                    rb.isKinematic = false;
                    anim.SetTrigger(trigger_walk);
                    break;

                case childState.run:
                    startRunning(runType);
                    rb.isKinematic = false;
                    anim.SetTrigger(trigger_run);
                    break;
                case childState.die:
                    rb.isKinematic = true;
                    anim.SetTrigger(trigger_die);
                    break;
                case childState.checkMask:
                    checkedMask = true;
                    rb.isKinematic = true;
                    time = 0f;
                    Vector3 from = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
                    Vector3 to = Vector3.ProjectOnPlane(script_gameController.character.transform.position - transform.position, Vector3.up).normalized;
                    angleToRot = Vector3.SignedAngle(from, to, Vector3.up);
                    anim.SetTrigger(trigger_checkMask);
                    break;
                case childState.showFace:
                    rb.isKinematic = true;
                    anim.SetTrigger(trigger_showFace);
                    break;
            }
            //Debug.Log(s);
        }
    }

    void behave()
    {
        switch (state)
        {
            case childState.idle:
                behaveIdle();
                break;

            case childState.walk:
                behaveWalk();
                break;

            case childState.run:
                behaveRun();
                break;
            case childState.checkMask:
                behaveCheckMask();
                break;
        }
    }

    void behaveIdle()
    {
        directionToCharacter();
        pointToDir();
    }

    void behaveWalk()
    {
        //Debug.Log("walking");
        rb.AddForce(direction * Time.fixedDeltaTime * speedMultiplier);
        pointToDir();
    }

    void behaveRun()
    {
        //Debug.Log("running");
        rb.AddForce(direction * Time.fixedDeltaTime * speedMultiplier);
        pointToDir();
    }

    public void startRunning(bool generator = false)
    {
        StartCoroutine("stopRunning");
        directionToCharacter(true);

        if(generator && chaosCollider)
        {
            chaosCollider.enabled = true;
            StartCoroutine("stopChaos");
        }
    }

    IEnumerator stopChaos()
    {
        yield return new WaitForSeconds(chaosTime);
        chaosCollider.enabled = false;
    }

    /*void behaveDie()
    {

    }*/

    void behaveCheckMask()
    {
        //Debug.Log("character is at " + script_gameController.character.transform.position);
        time += Time.fixedDeltaTime;

        transform.RotateAround(transform.position, Vector3.up, angleToRot * Time.fixedDeltaTime / positionCheckTime);

        if (time > positionCheckTime) changeState(childState.showFace);
        //Debug.Log("checking .");
    }

    

    IEnumerator changeDir()
    {
        float time = minTimeToChangeDir + Random.Range(0, rangeTimeToChangeDir);
        yield return new WaitForSeconds(time);
        chooseRandomDir();
        StartCoroutine("changeDir");
    }

    void chooseRandomDir()
    {
        if(!(state == childState.idle))
            direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    void directionToCharacter(bool not = false)
    {
        direction = Vector3.ProjectOnPlane(script_gameController.character.transform.position - transform.position, Vector3.up).normalized;
        if (not) direction = -direction;
    }

    void clampSpeed()
    {
        Vector3 vel = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
        float max = state == childState.walk ? max = maxSpeed * pctMaxSpeedWalk : maxSpeed;
        vel = vel.magnitude > max ? vel.normalized * max : vel;

        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
    }

    public void setAsthmatic()
    {
        asthmatic = true;
        if (asthmatic) transform.name = "child asthmatic";
        StartCoroutine("dieCountDown");
    }

    IEnumerator dieCountDown()
    {
        yield return new WaitForSeconds(lifeTime);
        if (!saved)
        {
            script_gameController.character.GetComponent<script_characterController>().changeState(charState.block);
            script_mainCameraController mcc = script_gameController.character.GetComponentInChildren<script_mainCameraController>();
            mcc.targetTransform = targetForCam;
            mcc.changeState(camState.transition);
            changeState(childState.die);
            Debug.Log("dead");
        }
        
    }

    IEnumerator stopRunning()
    {
        yield return new WaitForSeconds(runningTime);
        if (state == childState.run) randomIdleOrWalk();
    }

    void pointToDir()
    {
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        //Debug.Log(angle);
        //if (Mathf.Abs(angle) < 5f) transform.LookAt(transform.position + direction);
        //else transform.RotateAround(transform.position, Vector3.up, angle * Time.fixedDeltaTime * pointToDirForce);
        rb.AddTorque(new Vector3(0f, angle * Time.fixedDeltaTime * pointToDirForce, 0f));
    }


    void randomIdleOrWalk()
    {
        float a = Random.Range(0f, 1f);
        if (a > idlePCT) changeState(childState.walk);
        else changeState(childState.idle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if((state == childState.walk || state == childState.run) && other.gameObject.layer == tayLayer)
        {

            if (Random.Range(0f, 1f) < chaosChance && !asthmatic)
            {
                changeState(childState.idle);
            }
        } else if ((state == childState.idle || state == childState.walk) && other.gameObject.layer == chaosLayer)
        {

            //Debug.Log("chaos activated");
            changeState(childState.run);
            
        }
    }
}
