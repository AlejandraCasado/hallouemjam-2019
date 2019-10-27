using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_childGenerator : MonoBehaviour
{
    [SerializeField] GameObject generatedObject;
    [SerializeField] float pctWalking = 0.7f;

    // Start is called before the first frame update
    void Start()
    {

        script_maskController[] masks = generatedObject.GetComponentsInChildren<script_maskController>();
        int num1 = (int)Mathf.Floor(Random.Range(0, 3));
        int num2 = (int)Mathf.Floor(Random.Range(0, 3));
        while (num2 == num1) num2 = (int)Mathf.Floor(Random.Range(0, 3));
        Debug.Log(num1 + ", " + num2);
        masks[num1].gameObject.SetActive(false);
        masks[num2].gameObject.SetActive(false);


        /*spawnManyKids(40);*/


    }


    public void spawnManyKids(float number)
    {
        for (int i = 0; i < number; i++)
        {
            
            GameObject obj = spawnKid();

            script_childBehaviour behaviour = obj.GetComponent<script_childBehaviour>();
            behaviour.init();
            if (i == 0)
            {
                behaviour.setAsthmatic();
                obj.GetComponent<script_audioScript>().startWorking();
            }
            else
            {
                script_audioScript a = obj.GetComponent<script_audioScript>();
                AudioSource s = obj.GetComponent<AudioSource>();
                if (a) Destroy(a);
                if (s) Destroy(s);

            }

            if (Random.Range(0f, 1f) < pctWalking) behaviour.changeState(childState.walk);

        }
        Destroy(generatedObject);
    }

    GameObject spawnKid()
    {
        GameObject obj = Instantiate(generatedObject);

        Vector3 pos = Vector3.zero;

        pos.x = Random.Range(-transform.localScale.x / 2f, +transform.localScale.x / 2f) + transform.position.x;
        pos.z = Random.Range(-transform.localScale.z / 2f, +transform.localScale.z / 2f) + transform.position.z;
        pos.y = transform.position.y;
        

        obj.transform.position = pos;
        return obj;
    }
}
