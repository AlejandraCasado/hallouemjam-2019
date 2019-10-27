using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] script_temporalText interactiveText;
    [SerializeField] Text finishText;
    [SerializeField] string winMessage = "You won!";
    [SerializeField] string loseMessage = "You lost...";
    float timeToReload = 3f;

    // Start is called before the first frame update
    void Awake()
    {
        finishText.enabled = false;
        count.enabled = false;
        lifeTime = 100f;
        mainSoundController = GetComponent<script_controlSoundCinematic>();
        character = GameObject.FindGameObjectWithTag("character");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioScripts = GetComponents<script_audioScript>();
        won = false;
    }

    private void Start()
    {
        string[] data = new string[2];

        data[0] = "Have fun in the Halloween Party Taylor!";
        data[1] = "Wait! Your inhaler fell down";

        interactiveText.showInfo(3f, data);
    }



    private void Update()
    {
        if (finished)
        {
            finished = false;
            if (won) audioScripts[0].playSound();
            else audioScripts[1].playSound();
            mainSoundController.stopSound();


            //FINISH TEXT
            finishText.enabled = true;
            if (won) finishText.text = winMessage;
            else finishText.text = loseMessage;

            StartCoroutine("endGame");
        }

        if(count.enabled) counter();
    }

    void counter()
    {
        currentTime += Time.deltaTime;
        currentTime = currentTime > lifeTime ? lifeTime : currentTime;

        count.text = ("Taylor's life: " + (int)(lifeTime - currentTime)).ToString() + "s";


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

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(timeToReload);
        Application.LoadLevel("Menu");
    }


    public void writeInfoInScreen(string i)
    {
        string[] inf = new string[1];
        inf[0] = i;
        interactiveText.showInfo(3f, inf);
    }

}
