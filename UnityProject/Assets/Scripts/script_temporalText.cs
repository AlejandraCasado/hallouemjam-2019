using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class script_temporalText : MonoBehaviour
{
    float time;
    Text text;
    int index = 0;
    int maxIndex = 0;
    string[] data;

    bool showing = false;

    private void Awake()
    {
        text = GetComponent<Text>();
        text.enabled = false;

        //setActiveForXSeconds(5f, "xd");
    }

    public void showInfo(float showTime, string[] info)
    {
        if (!showing)
        {
            showing = true;
            data = info;
            text.enabled = true;
            time = showTime;
            maxIndex = info.Length;
            index = 0;
            writeInfo(info[index]);
        }
    }

    void writeInfo(string info)
    {
        text.text = info;
        StartCoroutine("waitingTime");
    }

    /*public void setActiveForXSeconds(float x, string[] info)
    {
        text.enabled = true;
        time = x;
        text.text = info[index];
        StartCoroutine("endVisibility");
    }*/

    /*IEnumerator endVisibility()
    {
        yield return new WaitForSeconds(time);
        text.enabled = false;
    }*/

    IEnumerator waitingTime()
    {
        yield return new WaitForSeconds(time);
        index++;
        if(index < maxIndex)
        {
            writeInfo(data[index]);
        } else
        {
            text.enabled = false;
            showing = false;
        }
    }
}
