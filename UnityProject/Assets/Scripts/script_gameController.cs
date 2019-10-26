using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_gameController : MonoBehaviour
{
    public static GameObject character;
    script_audioScript[] audioScripts;

    static bool finished = false;
    static bool won = false;

    // Start is called before the first frame update
    void Awake()
    {
        character = GameObject.FindGameObjectWithTag("character");
        Cursor.lockState = CursorLockMode.Locked;
        audioScripts = GetComponents<script_audioScript>();
    }

    private void Update()
    {
        if (finished)
        {
            finished = false;
            if (won) audioScripts[0].playSound();
            else audioScripts[1].playSound();

        }
    }

    public static void winGame()
    {
        Debug.Log("You won");
        finished = true;
        won = true;
    }

    public static void loseGame()
    {
        Debug.Log("You lost");
        finished = true;
    }


}
