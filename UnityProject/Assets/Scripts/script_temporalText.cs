using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class script_temporalText : MonoBehaviour
{
    float time;
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        text.enabled = false;
    }

    public void setActiveForXSeconds(float x, string info)
    {
        text.enabled = true;
        time = x;
        text.text = info;
    }

    IEnumerator endVisibility()
    {
        yield return new WaitForSeconds(time);
        text.enabled = false;
    }
}
