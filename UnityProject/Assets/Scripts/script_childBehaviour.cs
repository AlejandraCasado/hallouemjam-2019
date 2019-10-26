using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum charState {idle, walk, run};

public class script_childBehaviour : MonoBehaviour
{
    const string trigger_idle = "idle";
    const string trigger_walk = "walk";
    const string trigger_run = "run";
    Animator anim;
    Rigidbody rb;
    charState state = charState.idle;

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

        //behave();

    }

    private void FixedUpdate()
    {
        behave();
    }

    private void LateUpdate()
    {
        clampSpeed();
        transform.LookAt(transform.position + Vector3.ProjectOnPlane(rb.velocity.normalized,Vector3.up), Vector3.up);
    }

    void changeState(charState s) {
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
        }
        //Debug.Log(s);
    }

    void behave()
    {
        switch (state)
        {
            case charState.idle:
                behaveIdle();
                break;

            case charState.walk:
                behaveWalk();
                break;

            case charState.run:
                behaveRun();
                break;
        }
    }

    void behaveIdle()
    {
        //Debug.Log("idling");

    }

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

    IEnumerator changeDir()
    {
        float time = minTimeToChangeDir + Random.Range(0, rangeTimeToChangeDir);
        //Debug.Log(time);
        //Debug.Log("start changeDir");
        yield return new WaitForSeconds(time);
        //Debug.Log("finished changed Dir");

        direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Debug.Log("direction: " + direction);

        StartCoroutine("changeDir");
    }

    void clampSpeed()
    {
        Vector3 vel = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);
        float max = state == charState.walk ? max = maxSpeed * pctMaxSpeedWalk : maxSpeed;
        vel = vel.magnitude > max ? vel.normalized * max : vel;

        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
    }

}
