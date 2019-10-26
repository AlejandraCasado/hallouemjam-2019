using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_childGenerator : MonoBehaviour
{
    [SerializeField] GameObject generatedObject;

    // Start is called before the first frame update
    void Start()
    {
        spawnManyKids(1);
    }


    public void spawnManyKids(float number)
    {
        for (int i = 0; i < number; i++)
        {
            
            GameObject obj = spawnKid();
            if(i == 0)
            {
                obj.GetComponent<script_childBehaviour>().setAsthmatic();
            }
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
