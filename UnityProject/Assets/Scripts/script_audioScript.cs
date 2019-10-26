using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_audioScript : MonoBehaviour
{
    [HideInInspector]public bool work = false;
    public AudioClip MusicClip; //Esto guarda nuestro sonido
    public AudioSource MusicSource;
    [SerializeField] float timeOffset = 2;
    [SerializeField] float timeRandRange = 2;

    // Start is called before the first frame update
    void Start()
    {
        MusicSource.clip = MusicClip; //Asociamos (o cargamos) nuestro sonido / música
    }

    public void startWorking()
    {
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
        float time = Random.Range(0, timeRandRange) + 2;
        yield return new WaitForSeconds(time);
        playSound();
        StartCoroutine("soundTimer");
    }
}
