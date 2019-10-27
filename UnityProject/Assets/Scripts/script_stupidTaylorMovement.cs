using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_stupidTaylorMovement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Vector3 dir = Vector3.forward;
    [SerializeField] float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(dir.normalized * speed * Time.deltaTime);
    }
}
