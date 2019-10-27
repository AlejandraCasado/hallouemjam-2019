using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_closeDoor : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void closeDoor()
    {
        anim.SetTrigger("close");
    }
    public void openDoor()
    {
        anim.SetTrigger("open");
    }
}
