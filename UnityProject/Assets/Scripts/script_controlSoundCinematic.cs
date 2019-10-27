using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_controlSoundCinematic : MonoBehaviour
{
    [SerializeField] AudioSource mainSceneSound;

    [SerializeField] AudioClip next;
    [SerializeField] AudioClip next2;
    [SerializeField] AudioClip main;

    float time = 0f;

    bool playMain = false;

    // Start is called before the first frame update
    void Start()
    {
        time = mainSceneSound.clip.length;
        StartCoroutine("playnext");
    }

    

    IEnumerator playnext()
    {
        yield return new WaitForSeconds(time);
        if (playMain)
        {
            mainSceneSound.clip = next;
            mainSceneSound.Play();
            time = mainSceneSound.clip.length;
        }
        yield return new WaitForSeconds(time);
        if (!playMain)
        {
            mainSceneSound.clip = next2;
            mainSceneSound.loop = true;
            mainSceneSound.Play();
        }
    }

    public void playMainTheme()
    {
        playMain = true;
        mainSceneSound.clip = main;
        mainSceneSound.loop = true;
        mainSceneSound.Play();
    }

}
