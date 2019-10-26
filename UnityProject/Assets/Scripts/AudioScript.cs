using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

    public AudioClip MusicClip; //Esto guarda nuestro sonido

    public AudioSource MusicSource;

    float time;

    // Start is called before the first frame update
    void Start()
    {
        MusicSource.clip = MusicClip; //Asociamos (o cargamos) nuestro sonido / música
        StartCoroutine("soundTimer");
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine("soundTimer");
    }

    public void  playSound()
    {
        MusicSource.Play();
    }

    IEnumerator soundTimer()
    {
        time = Random.Range(0, 5) + 5;
        yield return new WaitForSeconds(time);
        playSound();
        StartCoroutine("soundTimer");
    }
}
