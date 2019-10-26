using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum camState { free, transition, end };

public class script_mainCameraController : MonoBehaviour
{
    /*[HideInInspector]*/camState state = camState.free;
    [SerializeField] float transitionTime = 1f;
    [HideInInspector] public Transform targetTransform;
    float time = 0f;
    Vector3 originalPos;
    Vector3 originalEulerAngles;

    Vector3 targetPos;
    Vector3 targetEulerAngles;

    void Start()
    {
        /*time = 0f;*/
    }

    private void FixedUpdate()
    {
        behave();
    }

    public void changeState(camState s)
    {
        state = s;
        if (state == camState.transition) startTransition();
    }

    void behave()
    {
        switch (state)
        {
            case camState.transition:
                behaveTransition();
                break;

        }
    }

    void behaveTransition()
    {
        Debug.Log("InTransition");
        transform.position = Vector3.Lerp(originalPos, targetPos, time / transitionTime);
        transform.rotation = Quaternion.Euler(Vector3.Lerp(originalEulerAngles, targetEulerAngles, time/transitionTime));
        time += Time.fixedDeltaTime;
        if (time > transitionTime) changeState(camState.end);
    }

    void startTransition()
    {
        //transform.parent = null;
        time = 0f;
        originalPos = transform.position;
        originalEulerAngles = transform.rotation.eulerAngles;

        targetPos = targetTransform.position;
        targetEulerAngles = targetTransform.rotation.eulerAngles;
    }
}
