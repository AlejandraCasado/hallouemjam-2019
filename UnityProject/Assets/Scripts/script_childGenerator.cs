using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_childGenerator : MonoBehaviour
{
    [SerializeField] GameObject generatedObject;

    // Start is called before the first frame update
    void Start()
    {
        spawnManyKids(0);
    }


    public void spawnManyKids(float number)
    {
        for (int i = 0; i < number; i++) spawnKid();
    }

    void spawnKid()
    {
        GameObject obj = Instantiate(generatedObject);

        Vector3 pos = Vector3.zero;

        pos.x = Random.Range(-transform.localScale.x / 2f, +transform.localScale.x / 2f) + transform.position.x;
        pos.z = Random.Range(-transform.localScale.z / 2f, +transform.localScale.z / 2f) + transform.position.z;
        pos.y = transform.position.y;
        

        obj.transform.position = pos;
    }
}
