using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class script_gameController : MonoBehaviour
{
    public static GameObject character;
    script_audioScript[] audioScripts;

    script_controlSoundCinematic mainSoundController;

    static bool finished = false;
    static bool won = false;

    float currentTime = 0f;
    public static float lifeTime;

    [SerializeField] Text count;
   

    // Start is called before the first frame update
    void Awake()
    {
        count.enabled = false;
        lifeTime = 20f;
        mainSoundController = GetComponent<script_controlSoundCinematic>();
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
            mainSoundController.stopSound();
        }

        if(count.enabled) counter();
    }

    void counter()
    {
        currentTime += Time.deltaTime;
        currentTime = currentTime > lifeTime ? lifeTime : currentTime;

        count.text = ((int)(lifeTime - currentTime)).ToString() + "s";


    }



    public static void winGame()
    {
        Debug.Log("You won");
        finished = true;
        won = true;
        character.GetComponent<script_characterController>().blockChar();
    }

    public static void loseGame()
    {
        Debug.Log("You lost");

        finished = true;
    }

    public void startCountDown()
    {
        count.enabled = true;
    }

}
