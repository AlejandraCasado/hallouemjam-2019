using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_gameController : MonoBehaviour
{
    public static GameObject character;

    // Start is called before the first frame update
    void Awake()
    {
        character = GameObject.FindGameObjectWithTag("character");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
