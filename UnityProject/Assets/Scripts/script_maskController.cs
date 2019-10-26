using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_maskController : MonoBehaviour
{
    [SerializeField] float lifeTime = 10f;
    [SerializeField] float freeForce = 10f;
    [SerializeField] Vector3 dir = Vector3.up;

    BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
        col.enabled = false;
        
    }

    public void free()
    {
        col.enabled = true;
        Vector3 vec = transform.localToWorldMatrix * dir;
        transform.parent = null;
        Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
        rb.AddForce(freeForce * vec);

        StartCoroutine("vanish");
    }

    IEnumerator vanish()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(transform.gameObject);
    }
}
