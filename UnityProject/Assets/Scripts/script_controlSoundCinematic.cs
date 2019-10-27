using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_controlSoundCinematic : MonoBehaviour
{
    [SerializeField] AudioSource mainSceneSound;

    [SerializeField] AudioClip next;
    [SerializeField] AudioClip next2;

    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        time = mainSceneSound.clip.length;
        StartCoroutine("playnext");
    }

    

    IEnumerator playnext()
    {
        yield return new WaitForSeconds(time);
        mainSceneSound.clip = next;
        mainSceneSound.Play();
        time = mainSceneSound.clip.length;
        yield return new WaitForSeconds(time);
        mainSceneSound.clip = next2;
        mainSceneSound.loop = true;
        mainSceneSound.Play();
    }
}
