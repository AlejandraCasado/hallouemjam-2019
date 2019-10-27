using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_closeDoor : MonoBehaviour
{
    Animator anim;
    BoxCollider bc;

    private void Start()
    {
        bc = transform.parent.GetComponent<BoxCollider>();
        bc.enabled = false;
        anim = GetComponentInChildren<Animator>();
    }

    public void closeDoor()
    {
        anim.SetTrigger("close");
        bc.enabled = true;
    }
    public void openDoor()
    {
        anim.SetTrigger("open");
        bc.enabled = false;
    }
}
