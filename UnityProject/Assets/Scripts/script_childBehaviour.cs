using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum charState {idle, walk, run, die, checkMask};

public class script_childBehaviour : MonoBehaviour
{
    const string trigger_idle = "idle";
    const string trigger_walk = "walk";
    const string trigger_run = "run";
    const string trigger_die = "die";
    const string trigger_checkMask = "checkMask";
    Animator anim;
    Rigidbody rb;
    charState state = charState.idle;
    [Header("PROPERTIES")]
    public bool asthmatic = false;
    [SerializeField] float lifeTime = 2f;

    [Header("MOVEMENT")]
    [SerializeField] float speedMultiplier = 30f;
    [SerializeField] float maxSpeed = 20f;
    [SerializeField] float pctMaxSpeedWalk = 0.5f;

    [Header("CHANGE DIR")]
    Vector3 direction = Vector3.zero;
    [SerializeField] float minTimeToChangeDir = 0f;
    [SerializeField] float rangeTimeToChangeDir = 1f;


    // Start is called before the first frame update
    void Awake()
    {
        //Debug.Log("child generated");
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        changeState(state);
        StartCoroutine("changeDir");

        direction = Vector3.right;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) changeState(charState.idle);
        if (Input.GetKeyDown(KeyCode.O)) changeState(charState.walk);
        if (Input.GetKeyDown(KeyCode.P)) changeState(charState.run);
        if (Input.GetKeyDown(KeyCode.L)) changeState(charState.checkMask);

        //behave();

    }

    private void FixedUpdate()
    {
        behave();
    }

    private void LateUpdate()
    {
        clampSpeed();
        if(state != charState.idle && state != charState.die) transform.LookAt(transform.position + direction.normalized, Vector3.up);
    }

    void changeState(charState s) {
        if(state != charState.die)
        {
            state = s;
            switch (s)
            {
                case charState.idle:
                    anim.SetTrigger(trigger_idle);
                    break;

                case charState.walk:
                    anim.SetTrigger(trigger_walk);
                    break;

                case charState.run:
                    anim.SetTrigger(trigger_run);
                    break;
                case charState.die:
                    anim.SetTrigger(trigger_die);
                    break;
                case charState.checkMask:
                    anim.SetTrigger(trigger_checkMask);
                    break;
            }
            //Debug.Log(s);
        }
    }

    void behave()
    {
        switch (state)
        {
            /*case charState.idle:
                behaveIdle();
                break;*/

            case charState.walk:
                behaveWalk();
                break;

            case charState.run:
                behaveRun();
                break;
            case charState.checkMask:
                behaveCheckMask();
                break;
        }
    }

    /*void behaveIdle()
    {
        //Debug.Log("idling");

    }*/

    void behaveWalk()
    {
        //Debug.Log("walking");
        rb.AddForce(direction * Time.fixedDeltaTime * speedMultiplier);
    }

    void behaveRun()
    {
        //Debug.Log("running");
        rb.AddForce(direction * Time.fixedDeltaTime * speedMultiplier);
    }

    /*void behaveDie()
    {

    }*/

    void behaveCheckMask()
    {
        Debug.Log("character is at " + script_gameController.character.transform.position);
    }

    IEnumerator changeDir()
    {
        float time = minTimeToChangeDir + Random.Range(0, rangeTimeToChangeDir);
        yield return new WaitForSeconds(time);
        direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        StartCoroutine("changeDir");
    }

    void clampSpeed()
    {
        Vector3 vel = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
        float max = state == charState.walk ? max = maxSpeed * pctMaxSpeedWalk : maxSpeed;
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
        changeState(charState.die);
        Debug.Log("dead");
    }

}
